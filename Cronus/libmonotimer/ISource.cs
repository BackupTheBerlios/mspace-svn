namespace libmonotimer
{
	/* This interface must be implemented by all the kind of Sources working in the application.
	 * For example:
	 * 	A XML source must implement all this functions to work with a local file.
	 * 	A XML-RPC source must implement all this functions in order to work remotely.
	*/
	public interface ISource {
		SProject[] getProjects ();
		Project loadProject ();
		bool saveProject ();
	}
}



