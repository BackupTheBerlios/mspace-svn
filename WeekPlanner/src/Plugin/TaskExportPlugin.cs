namespace WeekPlanner
{
		public abstract class TaskExportPlugin : IPlugin
		{
				public string Name {
					get {
							return "Task Exporter";
					}
				}
							
				public string Description {
					get {
							return "Exports the tasks";
					}
				}

				public abstract void PerformAction (IPluginContext context);
		}
}
