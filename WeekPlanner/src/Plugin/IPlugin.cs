namespace WeekPlanner
{
		public interface IPlugin
		{
				string Name {get;}
				string Description {get;}
				void PerformAction (IPluginContext context);
		}
}

