namespace Red.ProjectManager
import System.IO


#manager = ProjectManager ()
#project = manager.NewProject ("Test", "testdir", true)
#project.NewFile ("test.boo")
#project.NewFile ("caca.boo")
#project.NewFile ("lola.boo")
#project.NewFile ("melon.boo")
#project.Save ()

#pxml as duck = XmlObject ("testdir/test.pjt")
#print pxml.Name
#for obj as XmlObject in pxml.fileset:
#	print obj

print "Adding project"
ProjectManager.NewProject ("test1", "test1", true).Autosave = true
ProjectManager.CurrentProject.NewFile ("test")
print "Adding project"
ProjectManager.NewProject ("test2", "test2", true).Autosave = true
ProjectManager.CurrentProject.NewFile ("test")
print "Adding project"
ProjectManager.NewProject ("test3", "test3", true).Autosave = true
ProjectManager.CurrentProject.NewFile ("test")
ProjectManager.CurrentProject.NewFile ("test1")
ProjectManager.CurrentProject.RemoveFile (Path.Combine (ProjectManager.CurrentProject.Location, "test"), true)
print ProjectManager.CurrentProject.Name
