namespace WeekPlanner
{
	using System;

	[Serializable]
	public class UserConfiguration : AppConfiguration
	{

		public UserConfiguration ()
		{
		}

		public string CurrentTaskSet = "default";
	}
}
