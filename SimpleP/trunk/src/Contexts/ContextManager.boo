namespace SimpleP

import System
import System.Collections

class ContextManager:

	_contexts as Hashtable = Hashtable ()

	def RegisterContext (context as IProjectContext):
		_contexts.Add (context.Name, context)
		ContextAdded (self, ContextAddedArgs (context)) if ContextAdded

	def GetContext (name as string) as IProjectContext:
		return _contexts[name]

	def GetAllContexts () as ICollection:
		return _contexts.Values

	public event ContextAdded as callable (object, ContextAddedArgs)

class ContextAddedArgs:

	def constructor (context as IProjectContext):
		Context = context

	Context as IProjectContext = null
