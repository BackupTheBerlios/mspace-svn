namespace SimpleP

class PopupMenuService:
	
	_fileOperationsMenu as FileOperationsMenu
	public FileOperations as FileOperationsMenu:
		get:
			if not _fileOperationsMenu:
				_fileOperationsMenu = FileOperationsMenu ()
			return _fileOperationsMenu
