using System;

namespace WeekPlanner
{
	
	public enum WeekDay {
		Monday,
		Tuesday,
		Wednesday,
		Thursday,
		Friday,
		Saturday,
		Sunday	
	}

	public enum TaskPriority {
		Medium,
		High,
		Low
	}

	public delegate void PriorityChangedHandler (object obj, TaskPriority priority);
	public delegate void StartingHourChangedHandler (object obj, int hour);	
	public delegate void StartingMinuteChangedHandler (object obj, int minute);	
	public class Task {

		public Task ()
		{
		}

		private int _startingHour = 9;
		public int StartingHour {
			get {
				return _startingHour;
			}

			set {
				_startingHour = value;
				if (StartingHourChanged != null)
					StartingHourChanged (this, value);
			}
		}

		private int _endingHour = 10;
		public int EndingHour {
			get {
				return _endingHour;
			}
			set {
				_endingHour = value;
			}
		}

		private int _startingMinute = 0;
		public int StartingMinute {
			get {
				return _startingMinute;
			}
			set {
				_startingMinute = value;
				if (StartingMinuteChanged != null)
					StartingMinuteChanged (this, value);
			}
		}

		private int _endingMinute = 59;
		public int EndingMinute {
			get {
				return _endingMinute;
			}
			set {
				_endingMinute = value;
			}
		}
			

		private TaskPriority _priority = TaskPriority.Medium;
		public TaskPriority Priority {
			get {
				return _priority;
			}
			set {
				_priority = value;
				if (PriorityChanged != null)
					PriorityChanged (this, value);
			}
		}

		private WeekDay _day;
		public WeekDay Day {
			get {
				return _day;
			}
			set {
				_day = value;
			}
		}

		private string _description;
		public string Description {
			get {
				return _description;
			}
			set {
				_description = value;
			}
		}

		public event PriorityChangedHandler PriorityChanged;
		public event StartingHourChangedHandler StartingHourChanged;
		public event StartingMinuteChangedHandler StartingMinuteChanged;
		
		
			
		
	}
}
