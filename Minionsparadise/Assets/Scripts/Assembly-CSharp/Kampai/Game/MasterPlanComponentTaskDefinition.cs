namespace Kampai.Game
{
	public class MasterPlanComponentTaskDefinition
	{
		public int requiredItemId { get; set; }

		public uint requiredQuantity { get; set; }

		public bool ShowWayfinder { get; set; }

		public global::Kampai.Game.MasterPlanComponentTaskType Type { get; set; }

		public global::Kampai.Game.MasterPlanComponentTask Build()
		{
			global::Kampai.Game.MasterPlanComponentTask masterPlanComponentTask = new global::Kampai.Game.MasterPlanComponentTask();
			masterPlanComponentTask.Definition = this;
			return masterPlanComponentTask;
		}
	}
}
