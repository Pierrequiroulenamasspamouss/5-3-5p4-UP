namespace Kampai.Game
{
	public class CallMinionCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.Building building { get; set; }

		[Inject(global::Kampai.Game.GameElement.MINION_MANAGER)]
		public global::UnityEngine.GameObject minionManager { get; set; }

		[Inject]
		public global::Kampai.Game.FinishCallMinionSignal finishSignal { get; set; }

		[Inject]
		public global::UnityEngine.GameObject signalSender { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.StartMinionTaskSignal startMinionTaskSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playMinionNoAnimAudioSignal { get; set; }

		[Inject]
		public global::Kampai.Common.DeselectMinionSignal deselectSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.TaskableBuilding taskableBuilding = building as global::Kampai.Game.TaskableBuilding;
			int harvestTimeForTaskableBuilding = BuildingUtil.GetHarvestTimeForTaskableBuilding(taskableBuilding, definitionService);
			if (taskableBuilding == null)
			{
				return;
			}
			global::Kampai.Game.View.MinionManagerView component = minionManager.GetComponent<global::Kampai.Game.View.MinionManagerView>();
			global::Kampai.Game.Minion closestMinionToLocation = component.GetClosestMinionToLocation(building.Location, building is global::Kampai.Game.ResourceBuilding);
			if (closestMinionToLocation == null)
			{
				return;
			}
			if (closestMinionToLocation.State == global::Kampai.Game.MinionState.Selected)
			{
				global::Kampai.Game.DebrisBuilding debrisBuilding = building as global::Kampai.Game.DebrisBuilding;
				if (debrisBuilding == null)
				{
					return;
				}
				deselectSignal.Dispatch(closestMinionToLocation.ID);
			}
			playMinionNoAnimAudioSignal.Dispatch("Play_minion_confirm_pathToBldg_01");
			global::Kampai.Game.View.MinionObject second = component.Get(closestMinionToLocation.ID);
			startMinionTaskSignal.Dispatch(new global::Kampai.Util.Tuple<int, global::Kampai.Game.View.MinionObject, int>(taskableBuilding.ID, second, timeService.CurrentTime()));
			finishSignal.Dispatch(new global::Kampai.Util.Tuple<int, int, global::UnityEngine.GameObject>(closestMinionToLocation.ID, harvestTimeForTaskableBuilding, signalSender));
		}
	}
}
