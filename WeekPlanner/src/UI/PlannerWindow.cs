namespace WeekPlanner
{
	using Glade;
	using Gtk;
	using System;
	using Gnome;
	using System.Collections;

	public class PlannerWindow
	{
		private string gladeFile = "PlannerWindow.glade";
		private Canvas canvas;
		[Glade.Widget] Gtk.Window plannerWindow;
		[Glade.Widget] VBox vbox;
		private TimeTable table;

		/* MEASURES */
		const double width = 910;
		const double height = 471;
		const double taskWidth = 110;
		const double taskHeight = 20;
		const double cellWidth = 113.75;
		const double cellHeight = 33.642;
		public const double minuteLength = 0.56;
		const double hourLength = 32.71;
		const int initialHour = 9;
		
		public PlannerWindow ()
		{
			Init ();
		}

		private void Init ()
		{
			XML xml = new XML (null, gladeFile, "plannerWindow", null);
			xml.Autoconnect (this);
			canvas = new Canvas ();
			canvas.SetSizeRequest ((int)width, (int)height);
			canvas.SetScrollRegion (0, 0, width, height);
			table = new TimeTable (canvas.Root (), 
					new TaskArea (cellWidth, cellHeight, cellWidth * 8, cellHeight * 14, 13, 7));
			vbox.Add (canvas);
			plannerWindow.ShowAll ();
		}

		private void Task2Coordinates (Task task, out double x1, out double x2, out double y1, out double y2)
		{
			x1 = x2 = y1 = y2 = 0;
			x1 = cellWidth + ((int)task.Day * cellWidth);
			x2 = x1 + taskWidth;
			y1 = TaskStartingPoint (task);
			y2 = y1 + TaskHeigth (task);
		}

		private double TaskStartingPoint (Task task)
		{
			double baseHeight = cellHeight;
			double hourCoord = (task.StartingHour - 9) * cellHeight;
			double minuteOffset = task.StartingMinute * minuteLength;
			return hourCoord + minuteOffset + baseHeight;
		}

		private double TaskHeigth (Task task)
		{
			
			int totalHours = task.EndingHour - task.StartingHour;
			int totalMinutes = (task.EndingHour * 60 + task.EndingMinute)
						- (task.StartingHour * 60 + task.StartingMinute);
			return totalMinutes * minuteLength;
		}

		private void CreateTaskFromDialog (TaskInfoDialog dialog)
		{
			Task task = new Task ();
			task.StartingHour = dialog.StartingHour;
			task.StartingMinute = dialog.StartingMinute;
			task.EndingHour = dialog.EndingHour;
			task.EndingMinute = dialog.EndingMinute;
			task.Day = (WeekDay)Enum.Parse (typeof (WeekDay), dialog.Day);
			task.Priority = (TaskPriority)Enum.Parse (typeof (TaskPriority), dialog.Priority);
			task.Description = dialog.Description;
			double x1, x2, y1, y2;
			Task2Coordinates (task, out x1, out x2, out y1, out y2);
			SelectableTask seltask = new SelectableTask (canvas.Root (), task);
			seltask.X1 = x1;
			seltask.Y1 = y1;
			seltask.X2 = x2;
			seltask.Y2 = y2;
		}

		/*
		 * EVENT HANDLING
		 */

		private void OnDeleteEvent (object obj, DeleteEventArgs args)
		{
			Quit ();
		}

		private void OnRemoveClicked (object obj, EventArgs args)
		{
			Dialog dialog = new MessageDialog 
				(plannerWindow, DialogFlags.Modal | DialogFlags.DestroyWithParent,
				 MessageType.Warning, ButtonsType.OkCancel, 
				 "All the selected tasks will be deleted,\nare you sure?");
			if ((ResponseType)dialog.Run () == ResponseType.Ok)
			{
				// Is there any other way for iterating the collection and
				// removing elements at the same time without cloning it?
				ArrayList list = table.TaskList.Clone () as ArrayList;
				foreach (SelectableTask stask in list)
				{
					if (stask.Selected)
					{
						App.CurrentTaskSet.Remove (stask.Task);
						stask.Destroy ();
					}
				}
			}
			dialog.Destroy ();
		}
		private void OnAddClicked (object obj, EventArgs args)
		{
			TaskInfoDialog dialog = new TaskInfoDialog ();
			switch (dialog.Run ())
			{
				case (int)ResponseType.Ok:
					CreateTaskFromDialog (dialog);
					break;
				default:
					break;
			}
			dialog.Destroy ();
		}
		
		private void OnClearClicked (object obj, EventArgs args)
		{
			Dialog dialog = new MessageDialog 
				(plannerWindow, DialogFlags.Modal | DialogFlags.DestroyWithParent,
				 MessageType.Warning, ButtonsType.OkCancel, 
				 "You are going to clear the Timetable,\nare you sure?");
			if ((ResponseType)dialog.Run () == ResponseType.Ok)
			{
				ArrayList list = (ArrayList)table.TaskList.Clone ();
				foreach (SelectableTask stask in list)
				{
					App.CurrentTaskSet.Remove (stask.Task);
					table.TaskList.Clear ();
					stask.Destroy ();
				}
			}
			dialog.Destroy ();

		}

		private void OnQuitClicked (object obj, EventArgs args)
		{
			Quit ();
		}
		
		private void OnSaveClicked (object obj, EventArgs args)
		{
		}

		private void Quit ()
		{
			App.SaveSettings ();
			Application.Quit ();
		}

	}
}
