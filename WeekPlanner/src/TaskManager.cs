namespace WeekPlanner
{
	using System.Collections;
	using System;

	public class TaskManager : IEnumerable
	{

		private ArrayList taskList = new ArrayList ();

		private TaskManager ()
		{
		}

		private static TaskManager _taskManager;
		public static TaskManager Instance
		{
			get {
				if (_taskManager == null)
					_taskManager = new TaskManager ();
				return _taskManager;
			}
		}

		public bool AddTask (Task task)
		{
			if (task != null && !taskList.Contains (task))
			{
				taskList.Add (task);
				return true;
			}
			return false;
		}

		public bool RemoveTask (Task task)
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
