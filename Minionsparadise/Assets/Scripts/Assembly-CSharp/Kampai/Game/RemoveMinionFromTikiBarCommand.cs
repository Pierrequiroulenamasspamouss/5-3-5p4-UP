namespace Kampai.Game
{
	public class RemoveMinionFromTikiBarCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.Prestige prestige { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService prestigeService { get; set; }

		[Inject]
		public global::Kampai.Game.MinionStateChangeSignal minionStateChangeSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ReleaseMinionFromTikiBarSignal releaseMinionSignal { get; set; }

		[Inject]
		public global::Kampai.Game.MinionPrestigeCompleteSignal minionPrestigeCompleteSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.GUIServiceQueueEmptySignal guiServiceQueueEmptySignal { get; set; }

		public override void Execute()
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.Instance> instancesByDefinition = playerService.GetInstancesByDefinition<global::Kampai.Game.TikiBarBuildingDefinition>();
			if (instancesByDefinition == null || instancesByDefinition.Count == 0)
			{
				return;
			}
			global::Kampai.Game.TikiBarBuilding tikiBar = instancesByDefinition[0] as global::Kampai.Game.TikiBarBuilding;
			int minionSlotIndex = tikiBar.GetMinionSlotIndex(prestige.Definition.ID);
			if (minionSlotIndex >= 0 && minionSlotIndex < tikiBar.minionQueue.Count)
			{
				tikiBar.minionQueue[minionSlotIndex] = -1;
			}
			global::Kampai.Game.Character byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Character>(prestige.trackedInstanceId);
			releaseMinionSignal.Dispatch(byInstanceId, false);
			if (byInstanceId is global::Kampai.Game.Minion)
			{
				minionStateChangeSignal.Dispatch(byInstanceId.ID, global::Kampai.Game.MinionState.Idle);
			}
			if (prestige.state != global::Kampai.Game.PrestigeState.TaskableWhileQuesting)
			{
				prestigeService.ChangeToPrestigeState(prestige, global::Kampai.Game.PrestigeState.Taskable);
			}
			if (tikiBar.minionQueue.Count > 3)
			{
				int waitingNewPrestigeDefId = tikiBar.minionQueue[3];
				guiServiceQueueEmptySignal.AddOnce(delegate
				{
					global::Kampai.Game.Prestige type = prestigeService.GetPrestige(waitingNewPrestigeDefId);
					tikiBar.minionQueue.Remove(waitingNewPrestigeDefId);
					minionPrestigeCompleteSignal.Dispatch(type);
				});
			}
		}
	}
}
