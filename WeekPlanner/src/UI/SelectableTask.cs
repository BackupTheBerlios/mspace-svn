namespace WeekPlanner
{
	using Gnome;
	using System;
	using Gtk;

	public delegate void SelectionChangedHandler (object obj, bool value);

	public class SelectableTask : CanvasRect
	{
		private CanvasWidget infoWidget;
		private CanvasGroup group;
		private Tooltips tooltips;
		private string imgFile = "click.png";
		private Label label;
		private const double extraWidth = 2.0;
		private const double normalWidth = 1.0;
		private const int widgetHeigh = 14;
		private const int widgetWidth = 44;
		private const double taskSpacing = 3;
		private const string highPriorityFillColor = "#FFB2B2";
		private const string highPriorityOutlineColor = "#FF0000";
		private const string midPriorityFillColor = "#B9B9FF";
		private const string midPriorityOutlineColor = "#0000FF";
		private const string lowPriorityFillColor = "#BAFFB8";
		private const string lowPriorityOutlineColor = "#00FF00";
		
		public SelectableTask (CanvasGroup group, Task task) : base (group)
		{
			this.group = group;
			this._task = task;
			Init ();
		}

		private void Init ()
		{
			tooltips = new Tooltips ();
			CapStyle = Gdk.CapStyle.Round;
			JoinStyle = Gdk.JoinStyle.Round;
			if (Task.Priority == TaskPriority.High)
			{
				FillColor = highPriorityFillColor;
				OutlineColor = highPriorityOutlineColor;
			} else if (Task.Priority == TaskPriority.Medium)
			{
				FillColor = midPriorityFillColor;
				OutlineColor = midPriorityOutlineColor;
			} else {
				FillColor = lowPriorityFillColor;
				OutlineColor = lowPriorityOutlineColor;
			}
			SetupInfoWidget ();
			Task.PriorityChanged += OnPriorityChanged;
			Task.StartingHourChanged += OnTimeChanged;
			Task.StartingMinuteChanged += OnTimeChanged;
			WidthUnits = normalWidth;
			CanvasEvent += OnCanvasEvent;
		}

		private void SetupInfoWidget ()
		{
			infoWidget = new CanvasWidget (group);
			infoWidget.Width = widgetWidth;
			infoWidget.Height = widgetHeigh;
			int startingMinute = Task.StartingMinute;
			string minuteText = startingMinute.ToString ();
			if (startingMinute <= 9)
				minuteText = "0" + minuteText;	
			label = new Label ();
			label.UseMarkup = true;
			label.Markup = "<b>" + Task.StartingHour.ToString () + ":" + minuteText + "</b>";
			EventBox ebox = new EventBox ();
			ebox.Show ();
			ebox.Add (label);
			tooltips.SetTip (ebox, Task.Description, Task.Description);
			infoWidget.Widget = ebox;
			label.Show ();
			infoWidget.Show ();
			infoWidget.RaiseToTop ();
		}

		public new double X1 {
			get {
				return base.X1;
			}
			set {
				base.X1 = value + taskSpacing;
				infoWidget.X = value + 4;
			}
		}

		public new double Y1 {
			get {
				return base.Y1;
			}
			set {
				base.Y1 = value + taskSpacing;
				infoWidget.Y = value + 4;
			}
		}
		
		public new double X2 {
			get {
				return base.X2;
			}
			set {
				base.X2 = value - taskSpacing;
			}
		}

		public new double Y2 {
			get {
				return base.Y2;
			}
			set {
				if (value > Y1)
					base.Y2 = value - taskSpacing;
			}
		}

		public event SelectionChangedHandler SelectionChanged;
		private bool _selected;
		public bool Selected {
			get {
				return _selected;
			}
			set {
				_selected = value;
				if (_selected)
					WidthUnits = extraWidth;
				else
					WidthUnits = normalWidth;
				if (SelectionChanged != null)
					SelectionChanged (this, value);
			}
		}

		private Task _task;
		public Task Task {
			get {
				return _task;
			}
		}

		public new void Destroy ()
		{
			base.Destroy ();
			infoWidget.Destroy ();
		}

		/*
		 * PRIVATE
		 */
		
		private void ChangePriority () 
		{
			switch (Task.Priority)
			{
				case TaskPriority.High:
					Task.Priority = TaskPriority.Medium;
					break;
				case TaskPriority.Medium:
					Task.Priority = TaskPriority.Low;
					break;
				case TaskPriority.Low:
					Task.Priority = TaskPriority.High;
					break;
				default:
					break;
			}
			
		}

		private void OnPriorityChanged (object obj, TaskPriority priority)
		{
			switch (priority)
			{
				case TaskPriority.High:
					FillColor = highPriorityFillColor;
					OutlineColor = highPriorityOutlineColor;
					break;
				case TaskPriority.Medium:
					FillColor = midPriorityFillColor;
					OutlineColor = midPriorityOutlineColor;
					break;
				case TaskPriority.Low:
					FillColor = lowPriorityFillColor;
					OutlineColor = lowPriorityOutlineColor;
					break;
				default:
					break;
			}
					
		}

		private void OnTimeChanged (object obj, int unit)
		{
			int startingMinute = Task.StartingMinute;
			string minuteText = startingMinute.ToString ();
			if (startingMinute <= 9)
				minuteText = "0" + minuteText;	
			label.Markup = "<b>" + Task.StartingHour.ToString () + 
				+ ":" + minuteText + "</b>";
		}

		protected void OnCanvasEvent (object obj, CanvasEventArgs args)
		{
			Gdk.EventButton ev = new Gdk.EventButton (args.Event.Handle);	
			if (args.Event.Type == Gdk.EventType.EnterNotify)
				WidthUnits = extraWidth;
			if (args.Event.Type == Gdk.EventType.LeaveNotify)
				if (!Selected)
					WidthUnits = normalWidth;
			if (ev.Type == Gdk.EventType.TwoButtonPress)
			{		
				ReadonlyTaskInfoDialog dialog = new ReadonlyTaskInfoDialog (Task);
				dialog.Run ();
				dialog.Destroy ();
			} 
			else if (ev.Type == Gdk.EventType.ButtonPress)
			{
				switch (ev.Button)
				{
					case 1:
						Selected = !Selected;
						break;
					case 2:
						ChangePriority ();
						break;
					default:
						break;
				}
			}
		}
		
	}
}
