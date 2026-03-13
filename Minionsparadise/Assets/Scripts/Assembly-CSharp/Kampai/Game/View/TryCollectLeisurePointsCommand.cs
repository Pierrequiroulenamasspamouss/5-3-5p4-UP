namespace Kampai.Game.View
{
	public class TryCollectLeisurePointsCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("TryCollectLeisurePointsCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.BuildingChangeStateSignal buildingChangeStateSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateResourceIconCountSignal updateResourceIconCountSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SpawnDooberSignal spawnDooberSignal { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.Game.IMasterPlanService masterPlanService { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideAllWayFindersSignal hideAllWayFindersSignal { get; set; }

		[Inject]
		public global::Kampai.Game.UpdateAdHUDSignal updateAdHUDSignal { get; set; }

		[Inject]
		public global::Kampai.Game.LeisureBuilding building { get; set; }

		public override void Execute()
		{
			if (building == null || building.UTCLastTaskingTimeStarted == 0)
			{
				logger.Warning("Can not collect points for Leisure building");
				return;
			}
			int partyPointsReward = building.Definition.PartyPointsReward;
			playerService.AddXP(partyPointsReward);
			questService.UpdateAllQuestsWithQuestStepType(global::Kampai.Game.QuestStepType.HarvestAnyLeisure);
			masterPlanService.ProcessActiveComponent(global::Kampai.Game.MasterPlanComponentTaskType.EarnPartyPoints, (uint)partyPointsReward, building.Definition.ID);
			masterPlanService.ProcessActiveComponent(global::Kampai.Game.MasterPlanComponentTaskType.EarnLeisurePartyPoints, (uint)partyPointsReward, building.Definition.ID);
			building.UTCLastTaskingTimeStarted = 0;
			telemetryService.Send_Telemetry_EVT_PARTY_POINTS_EARNED(partyPointsReward, building.Definition.LocalizedKey);
			buildingChangeStateSignal.Dispatch(building.ID, global::Kampai.Game.BuildingState.Idle);
			SpawnDoober();
			if (playerService.GetMinionPartyInstance().IsPartyReady)
			{
				hideAllWayFindersSignal.Dispatch();
			}
			updateAdHUDSignal.Dispatch();
		}

		private void SpawnDoober()
		{
			updateResourceIconCountSignal.Dispatch(new global::Kampai.Util.Tuple<int, int>(building.ID, 2), 0);
			global::UnityEngine.Vector3 type = new global::UnityEngine.Vector3(building.Location.x, 0f, building.Location.y);
			spawnDooberSignal.Dispatch(type, global::Kampai.UI.View.DestinationType.XP, 2, true);
		}
	}
}
