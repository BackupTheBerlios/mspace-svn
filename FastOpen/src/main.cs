using Gtk;

namespace FastOpen
{
    public class main
    {
	public static void Main (string[] args)
	{
	    Gnome.Program program = new Gnome.Program("FastOpen", "1.0", Gnome.Modules.UI, args);
	    FastOpenWindow fs = new FastOpenWindow ();
	    program.Run ();
	}
    }
}
