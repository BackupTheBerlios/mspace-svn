namespace WeekPlanner
{
	using Gtk;
	using System;
	using System.Configuration;
	
	public class WeekPlannerMain
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			App.Init ();
			foreach (object obj in ConfigurationSettings.AppSettings)
					Console.WriteLine (obj);
			PlannerWindow window = new PlannerWindow ();
			Application.Run ();

		}
	}
}
