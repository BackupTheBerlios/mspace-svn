namespace WeekPlanner
{

	using Gnome;
	using Gtk;
	using Gdk;
	using System;
	using System.Collections;

	public delegate void TaskDrawnEventHandler (object src, SelectableTask task);

	public class TimeTable : CanvasPixbuf
	{
		private double firstX, firstY, curX, curY;
		private bool holding = false;
		private	SelectableTask stask;
		private TaskArea taskArea;
		private CanvasGroup group;
		private Cell curCell, firstCell;
		public readonly ArrayList TaskList = new ArrayList ();

		public TimeTable (CanvasGroup group, TaskArea taskArea) : base (group)
		{
			this.group = group;
			this.taskArea = taskArea;
			Pixbuf = Gdk.Pixbuf.LoadFromResource ("Coolgrey-squared.png");
			CanvasEvent += OnCanvasEvent;
		}

		public event TaskDrawnEventHandler TaskDrawn;

		private void OnCanvasEvent (object obj, CanvasEventArgs args)
		{
			EventButton ev = new EventButton (args.Event.Handle);
			switch (ev.Type)
			{
				case EventType.ButtonPress:
					if (ev.Button == 1)
					{
						holding= true;
						firstCell = taskArea.GetCellAt (ev.X, ev.Y);
						StartDrawing (ev);
					}
					break;
				case EventType.MotionNotify:
					if (holding && stask != null)
					{
						curCell = taskArea.GetCellAt (ev.X, ev.Y);
						stask.Y2 = ev.Y;
					}
					break;
				case EventType.ButtonRelease:
					holding = false;
					double rounded = RoundDownTask (ev.Y);
					stask.Y2 = rounded;
					PointToTime (firstCell, curCell, stask.Task, firstX, firstY, ev.X, rounded);
					break;
				default:
					break;
			}
		}

		private void StartDrawing (EventButton ev)
		{
			if (ev.X >= taskArea.X1 && ev.X <= taskArea.X2
					&& ev.Y >= taskArea.Y1 && ev.Y <= taskArea.Y2)
			{
				Task task = new Task ();
				stask = new SelectableTask (group, task);
				double minRounded = RoundMinutes (firstCell, ev.Y);
				stask.X1 = firstCell.Width * firstCell.Column;
				firstX = stask.X1;
				stask.Y1 = minRounded;
				firstY = stask.Y1;
				stask.X2 = firstCell.Width * (firstCell.Column + 1);
				stask.Y2 = stask.Y1 + firstCell.Height/4;
				task.StartingHour = firstCell.Row + 8;
				task.StartingMinute = (int)((minRounded - (firstCell.Row * firstCell.Height))/ PlannerWindow.minuteLength);
				task.Priority = TaskPriority.Medium;
				task.Day = (WeekDay)firstCell.Column - 1; //starting on monday = 0
				App.CurrentTaskSet.Add (task);
				TaskList.Add (stask);
			}
		}

		private void PointToTime 
				(Cell firstCell, Cell curCell, Task task, double firstX, double firstY, double curX, double curY)
		{
			task.EndingHour = curCell.Row + 8;
			task.EndingMinute = (int) ((curY - (curCell.Row * curCell.Height)) / PlannerWindow.minuteLength);
			
		}

		private double RoundMinutes (Cell cell, double y)
		{
			double minutes = y - (cell.Row * cell.Height);
			double roundup = 0;
			if ((minutes - cell.Height/2) < 0)
			{
				if ((minutes - cell.Height/4) < 0)
					roundup = 0;
				else
					roundup = cell.Height/4;
			} else {
				if ((minutes - ((cell.Height/4) * 3)) < 0)
					roundup = cell.Height/2;
				else
					roundup = (cell.Height/4) * 3;
			}
			return y - minutes + roundup;
		}

		private double RoundDownTask (double y)
		{
			double taskLength = y - this.firstY;
			int taskMinutes = (int)(taskLength / PlannerWindow.minuteLength);
			int resto = taskMinutes % 15;
			if (resto != 0)
				return y - resto;
			else
				return y;
		}

	}
}
