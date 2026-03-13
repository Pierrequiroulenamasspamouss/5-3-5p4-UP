namespace Kampai.Game
{
	public class RevealMasterPlanComponentCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int buildingDefinitionId { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		public override void Execute()
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.MasterPlanComponent> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.MasterPlanComponent>();
			foreach (global::Kampai.Game.MasterPlanComponent item in instancesByType)
			{
				if (item.buildingDefID == buildingDefinitionId)
				{
					item.State = global::Kampai.Game.MasterPlanComponentState.Built;
				}
			}
		}
	}
}
