namespace WeekPlanner
{
	using System.Collections;
	using System;

	[Serializable]
	public class TaskSet : IEnumerable
	{

		private ArrayList taskList = new ArrayList ();

		public TaskSet (string name)
		{
				Name = name;
		}

		private string _name;
		public string Name {
				get {
						return _name;
				}
				set {
						_name = value;
				}
		}

		public bool Add (Task task)
		{
			if (task != null && !taskList.Contains (task))
			{
				taskList.Add (task);
				return true;
			}
			return false;
		}

		public bool Remove (Task task)
		{
			if (taskList.Contains (task) && task!= null)
			{
				taskList.Remove (task);
				return true;
			}
			return false;
		}

		public IEnumerator GetEnumerator ()
		{
			return taskList.GetEnumerator ();
		}

		
	}
}
