
	private void AddClicked (object obj, EventArgs args)
	{
	    string desc, url, shortcut;
	    desc = shortcutEntry.Text;
	    if (desc == string.Empty)
		MessageDialog dialog = new MessageDialog (this, 
							    DialogFlags.DestroyWithParent,
							    MessageType.Error,
							    ButtonsType.Close,
							    "Fill all the entries.");
	    store.AppendValues (shortcutEntry.Text, descriptionEntry.Text, urlEntry.Text);
	    Destroy ();
	}
