namespace Red.ProjectManager

import System.Text

class ProjectNode:

	def constructor (name as string):
		Name = name

	public Name as string
	public Childs = []

	def HasChilds () as bool:
		return Childs.Count > 0
		
	override def ToString ():
		builder = StringBuilder ()
		builder.Append ("${Name}:[")
		for node as ProjectNode in Childs:
			builder.Append ("${node.Name},")
		builder.Append ("]")
		return builder.ToString ()
