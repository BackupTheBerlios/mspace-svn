/*
	Copyright (c) 2005 Sergio Rubio, <sergio.rubio@hispalinux.es>

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

namespace SimpleP

import Red.ProjectManager
import System

class Services:

	static _pManager as ProjectManager
	
	public static ProjectManager as ProjectManager:
		get:
			if not _pManager:
				_pManager = Red.ProjectManager.ProjectManager()

			return _pManager

	static _mimeService as MimetypeService
	public static Mimetype as MimetypeService:
		get:
			if not _mimeService:
				_mimeService = MimetypeService ()
			return _mimeService

	static _contextManager as ContextManager
	public static ContextManager as ContextManager:
		get:
			if not _contextManager:
				_contextManager = SimpleP.ContextManager ()
			return _contextManager

	static _statusbar as StatusbarService
	public static Statusbar as StatusbarService:
		get:
			if not _statusbar:
				_statusbar = SimpleP.StatusbarService ()
			return _statusbar

	static _popupMenuService as PopupMenuService
	public static PopupMenu as PopupMenuService:
		get:
			if not _popupMenuService:
				_popupMenuService = PopupMenuService ()
			return _popupMenuService

	static _configService as ConfigurationService
	public static Config as ConfigurationService:
		get:
			if not _configService:
				_configService = ConfigurationService ()
			return _configService
