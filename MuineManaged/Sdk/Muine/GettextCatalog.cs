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
 *
 * !!! Note that this class has to have the same API as the one
 *     from GNU.Gettext.dll, because otherwise the strings won't
 *     be picked up by update-po.
 */

using System;
using System.Runtime.InteropServices;

public class GettextCatalog
{
	[DllImport ("libmuine")]
	private static extern void intl_init (string package);

	public GettextCatalog (string package)
	{
		intl_init (package);
	}

	[DllImport ("libmuine")]
	private static extern IntPtr intl_get_string (IntPtr str);

	public string GetString (string str)
	{
		IntPtr inptr = Marshal.StringToHGlobalAuto (str);
		IntPtr sptr = intl_get_string (inptr);
		Marshal.FreeHGlobal (inptr);
		if (inptr == sptr)
			return str;
		else    
			return Marshal.PtrToStringAuto (sptr);
	}

	[DllImport ("libmuine")]
	private static extern IntPtr intl_get_plural_string (IntPtr singular,
							     IntPtr plural,
							     int n);

	public string GetPluralString (string singular,
					      string plural,
					      int n)
	{
		IntPtr singptr = Marshal.StringToHGlobalAuto (singular);
		IntPtr plurptr = Marshal.StringToHGlobalAuto (plural);
		IntPtr sptr = intl_get_plural_string (singptr, plurptr, n);
		Marshal.FreeHGlobal (singptr);
		Marshal.FreeHGlobal (plurptr);
		if (sptr == singptr)
			return singular;
		else if (sptr == plurptr)
			return plural;
		else
			return Marshal.PtrToStringAuto (sptr);
	}
}