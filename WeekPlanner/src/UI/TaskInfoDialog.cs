namespace WeekPlanner
{
	using Glade;
	using Gtk;
	using System;

	public class TaskInfoDialog
	{
		private string gladeFile = "NewTask.glade";
		private XML xml;
		private Task task;
		[Glade.Widget] SpinButton startHourSpin;
		[Glade.Widget] SpinButton endHourSpin;
		[Glade.Widget] SpinButton startMinuteSpin;
		[Glade.Widget] SpinButton endMinuteSpin;
		[Glade.Widget] Dialog newTaskDialog;
		[Glade.Widget] OptionMenu priorityOption;
		[Glade.Widget] OptionMenu dayOption;
		[Glade.Widget] TextView textView;
	
		public TaskInfoDialog ()
		{
			Init ();
		}

		public TaskInfoDialog (Task task)
		{
			this.task = task;
			Init ();
			startHourSpin.Value = task.StartingHour;
			endHourSpin.Value = task.EndingHour;
			startMinuteSpin.Value = task.StartingMinute;
			endMinuteSpin.Value = task.EndingMinute;
			priorityOption.SetHistory ((uint)task.Priority);
			dayOption.SetHistory ((uint)task.Day);
			textView.Buffer.Text = (task.Description == null) ? "" : task.Description;
		}

		private void Init ()
		{
			xml = new XML (null, gladeFile, "newTaskDialog", null);
			xml.Autoconnect (this);
			
		}

		public int EndingHour {
			get {
				return endHourSpin.ValueAsInt;
			}
		}

		public int StartingHour {
			get {
				return startHourSpin.ValueAsInt;
			}
		}

		public int EndingMinute {
			get {
				return endMinuteSpin.ValueAsInt;
			}
		}

		public int StartingMinute {
			get {
				return startMinuteSpin.ValueAsInt;
			}
		}

		public string Day {
			get {
				return ((WeekDay)dayOption.History).ToString ();
			}
		}

		public string Priority {
			get {
				return ((TaskPriority)priorityOption.History).ToString ();
			}
		}

		public string Description {
			get {
				return textView.Buffer.Text;
			}
		}

		public int Run ()
		{
			return newTaskDialog.Run ();
		}

		public void Destroy ()
		{
			newTaskDialog.Destroy ();
		}

	}
}
