namespace WeekPlanner
{
	using Glade;
	using Gtk;
	using System;

	public class ReadonlyTaskInfoDialog
	{
		private string gladeFile = "ReadonlyTaskInfoDialog.glade";
		private XML xml;
		private Task task;
		[Glade.Widget] Label startingMinuteLabel;
		[Glade.Widget] Label startingHourLabel;
		[Glade.Widget] Label endingMinuteLabel;
		[Glade.Widget] Label endingHourLabel;
		[Glade.Widget] Dialog readonlyTaskInfoDialog;
		[Glade.Widget] Label priorityLabel;
		[Glade.Widget] Label dayLabel;
		[Glade.Widget] TextView descriptionTextView;
	
		public ReadonlyTaskInfoDialog ()
		{
			Init ();
		}

		public ReadonlyTaskInfoDialog (Task task)
		{
			this.task = task;
			Init ();
			startingHourLabel.Text = task.StartingHour.ToString ();
			endingHourLabel.Text = task.EndingHour.ToString ();
			startingMinuteLabel.Text = task.StartingMinute.ToString ();
			endingMinuteLabel.Text = task.EndingMinute.ToString ();
			priorityLabel.Text = task.Priority.ToString ();
			dayLabel.Text = task.Day.ToString ();
			descriptionTextView.Buffer.Text = (task.Description == null) ? "" : task.Description;
		}

		private void Init ()
		{
			xml = new XML (null, gladeFile, "readonlyTaskInfoDialog", null);
			xml.Autoconnect (this);
			
		}

		public int Run ()
		{
			return readonlyTaskInfoDialog.Run ();
		}

		public void Destroy ()
		{
			readonlyTaskInfoDialog.Destroy ();
		}

	}
}
