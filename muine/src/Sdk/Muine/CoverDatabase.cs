/*
 * Copyright (C) 2004 Jorn Baayen <jorn@nl.linux.org>
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License as
 * published by the Free Software Foundation; either version 2 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public
 * License along with this program; if not, write to the
 * Free Software Foundation, Inc., 59 Temple Place - Suite 330,
 * Boston, MA 02111-1307, USA.
 */

using System;
using System.Collections;
using System.IO;
using System.Web;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;

using Gnome;
using Gdk;

public class CoverDatabase 
{
	public static int AlbumCoverSize = 66;

	public Hashtable Covers;

	public Pixbuf DownloadingPixbuf;
	
	public string amazon_locale;

	private bool loaded = false;

	/*** constructor ***/
	private IntPtr dbf;
	private object dbf_box;

	private GnomeProxy proxy;

	[DllImport ("libmuine")]
	private static extern IntPtr db_open (string filename, int version,
	                                      out IntPtr error);
						   
	public CoverDatabase (int version)
	{
		try {
			amazon_locale = (string) AppContext.GConfClient.Get ("/apps/muine/amazon_locale");
		} catch {
			amazon_locale = "us";
		}

		//FIXME
		DirectoryInfo dinfo = new DirectoryInfo ("/home/rubiojr/.gnome2/muine");
		if (!dinfo.Exists) {
			try {
				dinfo.Create ();
			} catch (Exception e) {
				throw e;
			}
		}
		
		string filename = dinfo.FullName + "/covers.db";

		IntPtr error_ptr;

		dbf = db_open (filename, version, out error_ptr);

		if (dbf == IntPtr.Zero)
			throw new Exception (GLib.Marshaller.PtrToStringGFree (error_ptr));

		dbf_box = dbf;

		Covers = new Hashtable ();

		proxy = new GnomeProxy ();

		/* Hack to get the GtkStyle .. */
		Gtk.Label label = new Gtk.Label ("");
		label.EnsureStyle ();
		DownloadingPixbuf = label.RenderIcon ("muine-cover-downloading",
						      StockIcons.AlbumCoverSize, null);
		label.Destroy ();
	}

	/*** loading ***/
	[DllImport ("libmuine")]
	private static extern IntPtr db_unpack_pixbuf (IntPtr p, out IntPtr pixbuf);

	private void DecodeFunc (string key, IntPtr data, IntPtr user_data)
	{
		IntPtr pix_handle;
		
		db_unpack_pixbuf (data, out pix_handle);

		LoadedCover lc = new LoadedCover (key, pix_handle);

		loaded_covers.Enqueue (lc);
	}

	private delegate void DecodeFuncDelegate (string key, IntPtr data, IntPtr user_data);

	[DllImport ("libmuine")]
	private static extern void db_foreach (IntPtr dbf, DecodeFuncDelegate decode_func,
					       IntPtr user_data);

	private bool thread_done;

	private Queue loaded_covers;

	public bool Loading = true;

	public delegate void DoneLoadingHandler ();
	public event DoneLoadingHandler DoneLoading;

	public void Load ()
	{
		if (loaded)
		    return;
		thread_done = false;

		loaded_covers = Queue.Synchronized (new Queue ());

		GLib.Idle.Add (new GLib.IdleHandler (ProcessActionsFromThread));
		
		Thread thread = new Thread (new ThreadStart (LoadThread));
		thread.Priority = ThreadPriority.BelowNormal;
		thread.Start ();
		loaded = true;
	}

	private class LoadedCover {
		public string Key;
		public Pixbuf Pixbuf;

		public LoadedCover (string key, IntPtr pixbuf_ptr) {
			Key = key;

			Pixbuf = new Pixbuf (pixbuf_ptr);
		}
	}

	private bool ProcessActionsFromThread ()
	{
		int counter = 0;
		
		if (loaded_covers.Count > 0) {
			while (loaded_covers.Count > 0 && counter < 10) {
				LoadedCover lc = (LoadedCover) loaded_covers.Dequeue ();
	
				if (!Covers.ContainsKey (lc.Key))
					Covers.Add (lc.Key, lc.Pixbuf);

				Album a = (Album) AppContext.DB.Albums [lc.Key];
				if (a != null) {
					a.CoverImage = lc.Pixbuf;

					AppContext.DB.EmitAlbumChanged (a);
				}

				counter++;
			}

			return true;
		} else if (thread_done) {
			Loading = false;

			if (DoneLoading != null)
				DoneLoading ();
		}
		
		return !thread_done;
	}

	private void LoadThread ()
	{
		lock (dbf_box) {
			db_foreach (dbf, new DecodeFuncDelegate (DecodeFunc), IntPtr.Zero);
		}

		thread_done = true;
	}

	/*** storing ***/
	private Pixbuf BeautifyPixbuf (Pixbuf cover)
	{
		Pixbuf border;

		/* 1px border, so -2 .. */
		int target_size = AlbumCoverSize - 2;

		/* scale the cover image if necessary */
		if (cover.Height > target_size || cover.Width > target_size) {
			int new_width, new_height;

			if (cover.Height > cover.Width) {
				new_width = (int) Math.Round ((double) target_size / (double) cover.Height * cover.Width);
				new_height = target_size;
			} else {
				new_height = (int) Math.Round ((double) target_size / (double) cover.Width * cover.Height);
				new_width = target_size;
			}

			cover = cover.ScaleSimple (new_width, new_height, InterpType.Bilinear);
		}

		/* create the background + black border pixbuf */
		border = new Pixbuf (Colorspace.Rgb, true, 8, cover.Width + 2, cover.Height + 2);
		border.Fill (0x000000ff);
			
		/* put the cover image on the border area */
		cover.CopyArea (0, 0, cover.Width, cover.Height, border, 1, 1);

		/* done */
		return border;
	}

	public Pixbuf DownloadCoverPixbuf (string album_url)
	{
		Pixbuf cover;

		/* read the cover image */
		HttpWebRequest req = (HttpWebRequest) WebRequest.Create (album_url);
		req.UserAgent = "Muine";
		req.KeepAlive = false;
		req.Timeout = 30000; /* Timeout after 30 seconds */
		if (proxy.Use)
			req.Proxy = proxy.Proxy;
			
		WebResponse resp = null;
	
		/* May throw an exception, but we catch it in the calling
		 * function in Song.cs */
		resp = req.GetResponse ();

		Stream s = resp.GetResponseStream ();
	
		cover = new Pixbuf (s);

		resp.Close ();

		/* Trap Amazon 1x1 images */
		if (cover.Height == 1 && cover.Width == 1)
			return null;

		return BeautifyPixbuf (cover);
	}

	public Pixbuf AddCoverLocal (string key, string filename)
	{
		Pixbuf pix;

		try {
			pix = new Pixbuf (filename);
		} catch (Exception e) {
			return null;
		}

		pix = BeautifyPixbuf (pix);

		AddCover (key, pix);

		return pix;
	}

	public Pixbuf AddCoverEmbedded (string key, Pixbuf cover_image)
	{
		Pixbuf pix = BeautifyPixbuf (cover_image);

		AddCover (key, pix);

		return pix;
	}

	public Pixbuf AddCoverDownloading (string key)
	{
		Covers.Add (key, DownloadingPixbuf);

		return DownloadingPixbuf;
	}

	private delegate IntPtr EncodeFuncDelegate (IntPtr handle, out int length);

	[DllImport ("libmuine")]
	private static extern void db_store (IntPtr dbf, string key, bool overwrite,
					     EncodeFuncDelegate encode_func,
					     IntPtr user_data);

	public void AddCover (string key, Pixbuf pix)
	{
		if (pix == null)
			return;

		Covers.Add (key, pix);

		lock (dbf_box) {
			db_store (dbf, key, false,
			          new EncodeFuncDelegate (EncodeFunc), pix.Handle);
		}
	}

	public void ReplaceCover (string key, Pixbuf pix)
	{
		Covers.Remove (key);

		AddCover (key, pix);
	}

	[DllImport ("libmuine")]
	private static extern void db_delete (IntPtr dbf, string key);

	public void RemoveCover (string key)
	{
		lock (dbf_box) {
			db_delete (dbf, key);
		}

		Covers.Remove (key);
	}

	[DllImport ("libmuine")]
        private static extern IntPtr db_pack_start ();
	[DllImport ("libmuine")]
	private static extern void db_pack_pixbuf (IntPtr p, IntPtr pixbuf);
	[DllImport ("libmuine")]
        private static extern IntPtr db_pack_end (IntPtr p, out int length);

	private IntPtr EncodeFunc (IntPtr handle, out int length)
	{
		IntPtr p = db_pack_start ();
		db_pack_pixbuf (p, handle);
		return db_pack_end (p, out length);
	}

	private string SanitizeString (string s)
	{
		s = s.ToLower ();
		s = Regex.Replace (s, "\\(.*\\)", "");
		s = Regex.Replace (s, "\\[.*\\]", "");
		s = s.Replace ("-", " ");
		s = s.Replace ("_", " ");
		s = Regex.Replace (s, " +", " ");

		return s;
	}

	public Pixbuf GetAlbumCoverFromAmazon (Song song)
	{
		AmazonSearchService search_service = new AmazonSearchService ();

		string sane_album_title = SanitizeString (song.Album);

		string [] album_title_array = sane_album_title.Split (' ');
		Array.Sort (album_title_array);

		/* This assumes the right artist is always in Artists [0] */
		string sane_artist = SanitizeString (song.Artists [0]);
		
		/* Prepare for handling multi-page results */
		int total_pages = 1;
		int current_page = 1;
		int max_pages = 2; /* check no more than 2 pages */
		
		/* Create Encapsulated Request */
		ArtistRequest asearch = new ArtistRequest ();
		asearch.devtag = "INSERT DEV TAG HERE";
		asearch.artist = sane_artist;
		asearch.keywords = sane_album_title;
		asearch.type = "heavy";
		asearch.mode = "music";
		asearch.tag = "webservices-20";

		/* Use selected Amazon service */
		switch (amazon_locale) {
		case "uk":
			search_service.Url = "http://soap-eu.amazon.com/onca/soap3";
			asearch.locale = "uk";

			break;
		case "de":
			search_service.Url = "http://soap-eu.amazon.com/onca/soap3";
			asearch.locale = "de";

			break;
		case "jp":
			search_service.Url = "http://soap.amazon.com/onca/soap3";
			asearch.locale = "jp";

			break;
		default:
			search_service.Url = "http://soap.amazon.com/onca/soap3";

			break;
		}

		double best_match_percent = 0.0;
		Pixbuf best_match = null;

		while (current_page <= total_pages && current_page <= max_pages) {
			asearch.page = Convert.ToString (current_page);

			ProductInfo pi;
			
			/* Amazon API requires this .. */
			Thread.Sleep (1000);
		
			/* Web service calls timeout after 30 seconds */
			search_service.Timeout = 30000;
			if (proxy.Use)
				search_service.Proxy = proxy.Proxy;
			
			/* This may throw an exception, we catch it in Song.cs in the calling function */
			pi = search_service.ArtistSearchRequest (asearch);

			int num_results = pi.Details.Length;
			total_pages = Convert.ToInt32 (pi.TotalPages);

			/* Work out how many matches are on this page */
			if (num_results < 1)
				return null;

			for (int i = 0; i < num_results; i++) {
				/* Ignore bracketed text on the result from Amazon */
				string sane_product_name = SanitizeString (pi.Details[i].ProductName);

				/* Compare the two strings statistically */
				string [] product_name_array = sane_product_name.Split (' ');
				Array.Sort (product_name_array);

				int match_count = 0;
				foreach (string s in album_title_array) {
					if (Array.BinarySearch (product_name_array, s) >= 0)
						match_count++;
				}

				double match_percent;
				match_percent = match_count / (double) album_title_array.Length;

				if (match_percent >= 0.6) {
					string url = pi.Details [i].ImageUrlMedium;

					if (url != null && url.Length > 0)
					{
						double forward_match_percent = match_percent;
						double backward_match_percent = 0.0;
						int backward_match_count = 0;

						foreach (string s in product_name_array) {
							if (Array.BinarySearch (album_title_array, s) >= 0)
								backward_match_count++;
						}
						backward_match_percent = backward_match_count / (double) product_name_array.Length;

						double total_match_percent = match_percent + backward_match_percent;
						if (total_match_percent > best_match_percent) {
							Pixbuf pix;
							
							try {
								pix = DownloadCoverPixbuf (url);
								if (pix == null && amazon_locale != "us") {
									// Manipulate the image URL since Amazon sometimes return it wrong :(
									// http://www.amazon.com/gp/browse.html/103-1953981-2427826?node=3434651#misc-image
									url = Regex.Replace (url, "[.]0[0-9][.]", ".01.");
									pix = DownloadCoverPixbuf (url);
								}
							} catch (WebException e) {
								throw e;
							} catch (Exception e) {
								pix = null;
							}

							if (pix != null) {
								best_match_percent = total_match_percent;
								best_match = pix;

								if (best_match_percent == 2.0)
									return best_match;
							}
						}
						// ELSE keep iterating to find a better match
					}
				}
			}

			current_page++;
		}

		return best_match;
	}
}