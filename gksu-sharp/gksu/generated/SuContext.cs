// This file was generated by the Gtk# code generator.
// Any changes made will be lost if regenerated.

namespace Gksu {

	using System;
	using System.Collections;
	using System.Runtime.InteropServices;

#region Autogenerated code
	public  class SuContext : GLib.Object {

		~SuContext()
		{
			Dispose();
		}

		[Obsolete]
		protected SuContext(GLib.GType gtype) : base(gtype) {}
		public SuContext(IntPtr raw) : base(raw) {}

		[DllImport("gksu")]
		static extern IntPtr gksu_context_new();

		public SuContext () : base (IntPtr.Zero)
		{
			if (GetType () != typeof (SuContext)) {
				CreateNativeObject (new string [0], new GLib.Value[0]);
				return;
			}
			Raw = gksu_context_new();
		}

		[DllImport("gksu")]
		static extern IntPtr gksu_context_get_password(IntPtr raw);

		[DllImport("gksu")]
		static extern void gksu_context_set_password(IntPtr raw, IntPtr password);

		public string Password { 
			get {
				IntPtr raw_ret = gksu_context_get_password(Handle);
				string ret = GLib.Marshaller.Utf8PtrToString (raw_ret);
				return ret;
			}
			set {
				gksu_context_set_password(Handle, GLib.Marshaller.StringToPtrGStrdup(value));
			}
		}

		[DllImport("gksu")]
		static extern unsafe bool gksu_context_sudo_run(IntPtr raw, out IntPtr error);

		public unsafe bool SudoRun() {
			IntPtr error = IntPtr.Zero;
			bool raw_ret = gksu_context_sudo_run(Handle, out error);
			bool ret = raw_ret;
			if (error != IntPtr.Zero) throw new GLib.GException (error);
			return ret;
		}

		[DllImport("gksu")]
		static extern bool gksu_context_get_ssh_fwd(IntPtr raw);

		[DllImport("gksu")]
		static extern void gksu_context_set_ssh_fwd(IntPtr raw, bool value);

		public bool SshFwd { 
			get {
				bool raw_ret = gksu_context_get_ssh_fwd(Handle);
				bool ret = raw_ret;
				return ret;
			}
			set {
				gksu_context_set_ssh_fwd(Handle, value);
			}
		}

		[DllImport("gksu")]
		static extern bool gksu_context_get_debug(IntPtr raw);

		[DllImport("gksu")]
		static extern void gksu_context_set_debug(IntPtr raw, bool value);

		public bool Debug { 
			get {
				bool raw_ret = gksu_context_get_debug(Handle);
				bool ret = raw_ret;
				return ret;
			}
			set {
				gksu_context_set_debug(Handle, value);
			}
		}

		[DllImport("gksu")]
		static extern IntPtr gksu_context_get_user(IntPtr raw);

		[DllImport("gksu")]
		static extern void gksu_context_set_user(IntPtr raw, IntPtr username);

		public string User { 
			get {
				IntPtr raw_ret = gksu_context_get_user(Handle);
				string ret = GLib.Marshaller.Utf8PtrToString (raw_ret);
				return ret;
			}
			set {
				gksu_context_set_user(Handle, GLib.Marshaller.StringToPtrGStrdup(value));
			}
		}

		[DllImport("gksu")]
		static extern IntPtr gksu_context_get_command(IntPtr raw);

		[DllImport("gksu")]
		static extern void gksu_context_set_command(IntPtr raw, IntPtr command);

		public string Command { 
			get {
				IntPtr raw_ret = gksu_context_get_command(Handle);
				string ret = GLib.Marshaller.Utf8PtrToString (raw_ret);
				return ret;
			}
			set {
				gksu_context_set_command(Handle, GLib.Marshaller.StringToPtrGStrdup(value));
			}
		}

		[DllImport("gksu")]
		static extern bool gksu_context_get_login_shell(IntPtr raw);

		[DllImport("gksu")]
		static extern void gksu_context_set_login_shell(IntPtr raw, bool value);

		public bool LoginShell { 
			get {
				bool raw_ret = gksu_context_get_login_shell(Handle);
				bool ret = raw_ret;
				return ret;
			}
			set {
				gksu_context_set_login_shell(Handle, value);
			}
		}

		[DllImport("gksu")]
		static extern void gksu_context_free(IntPtr raw);

		public void Free() {
			gksu_context_free(Handle);
		}

		[DllImport("gksu")]
		static extern unsafe bool gksu_context_run(IntPtr raw, out IntPtr error);

		public unsafe bool Run() {
			IntPtr error = IntPtr.Zero;
			bool raw_ret = gksu_context_run(Handle, out error);
			bool ret = raw_ret;
			if (error != IntPtr.Zero) throw new GLib.GException (error);
			return ret;
		}

		[DllImport("gksu")]
		static extern IntPtr gksu_context_get_type();

		public static new GLib.GType GType { 
			get {
				IntPtr raw_ret = gksu_context_get_type();
				GLib.GType ret = new GLib.GType(raw_ret);
				return ret;
			}
		}

		[DllImport("gksu")]
		static extern bool gksu_context_get_keep_env(IntPtr raw);

		[DllImport("gksu")]
		static extern void gksu_context_set_keep_env(IntPtr raw, bool value);

		public bool KeepEnv { 
			get {
				bool raw_ret = gksu_context_get_keep_env(Handle);
				bool ret = raw_ret;
				return ret;
			}
			set {
				gksu_context_set_keep_env(Handle, value);
			}
		}


		static SuContext ()
		{
			GtkSharp.GksuSharp.ObjectManager.Initialize ();
		}
#endregion
	}
}