namespace Kampai.UI.View
{
	public class WayFinderSettings : global::Kampai.UI.View.WorldToGlassUISettings
	{
		public int QuestDefId { get; private set; }

		public bool updatePriority { get; private set; }

		public WayFinderSettings(int trackedId, bool updatePriority = true)
			: base(trackedId)
		{
			this.updatePriority = updatePriority;
		}

		public WayFinderSettings(int questDefId, int trackedId)
			: this(trackedId)
		{
			QuestDefId = questDefId;
		}
	}
}
