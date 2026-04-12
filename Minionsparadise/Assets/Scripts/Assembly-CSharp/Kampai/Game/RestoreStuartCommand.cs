namespace Kampai.Game
{
	public class RestoreStuartCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.SocialEventAvailableSignal socialEventAvailableSignal { get; set; }

		[Inject]
		public global::Kampai.Game.FrolicSignal frolicSignal { get; set; }

		[Inject]
		public global::Kampai.Game.AddMinionToTikiBarSignal tikiSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.StuartCharacter firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.StuartCharacter>(70001);
			if (firstInstanceByDefinitionId == null)
			{
				return;
			}
			global::Kampai.Game.Prestige firstInstanceByDefinitionId2 = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Prestige>(40003);
			if (firstInstanceByDefinitionId2.state != global::Kampai.Game.PrestigeState.Questing)
			{
				frolicSignal.Dispatch(firstInstanceByDefinitionId.ID);
			}
			else
			{
				global::Kampai.Game.TikiBarBuilding byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.TikiBarBuilding>(313);
				if (byInstanceId != null)
				{
					int minionSlotIndex = byInstanceId.GetMinionSlotIndex(firstInstanceByDefinitionId2.Definition.ID);
					if (minionSlotIndex != -1)
					{
						tikiSignal.Dispatch(byInstanceId, firstInstanceByDefinitionId, firstInstanceByDefinitionId2, minionSlotIndex);
					}
				}
			}
			if (firstInstanceByDefinitionId2.CurrentPrestigeLevel >= 1)
			{
				socialEventAvailableSignal.Dispatch();
			}
		}
	}
}
