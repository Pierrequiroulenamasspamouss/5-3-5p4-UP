namespace Kampai.Game
{
	public class SendMinionToLeisureCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SendMinionToLeisureCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject(global::Kampai.Game.GameElement.MINION_MANAGER)]
		public global::UnityEngine.GameObject minionManager { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		[Inject]
		public global::Kampai.Util.PathFinder pathFinder { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Game.MinionStateChangeSignal minionStateChangeSignal { get; set; }

		[Inject]
		public global::Kampai.Game.BuildingChangeStateSignal buildingStateChangeSignal { get; set; }

		[Inject]
		public global::Kampai.Game.RouteMinionToLeisureSignal routeMinionToLeisureSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playMinionNoAnimAudioSignal { get; set; }

		[Inject]
		public int buildingId { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.View.MinionManagerView component = minionManager.GetComponent<global::Kampai.Game.View.MinionManagerView>();
			global::Kampai.Game.View.BuildingManagerView component2 = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::Kampai.Game.View.LeisureBuildingObjectView leisureBuildingObjectView = component2.GetBuildingObject(buildingId) as global::Kampai.Game.View.LeisureBuildingObjectView;
			global::Kampai.Game.LeisureBuilding leisureBuilding = leisureBuildingObjectView.leisureBuilding;
			if (leisureBuilding == null || leisureBuildingObjectView == null)
			{
				return;
			}
			global::System.Collections.Generic.Queue<int> minionListSortedByDistanceAndState = component.GetMinionListSortedByDistanceAndState(leisureBuildingObjectView.transform.position);
			if (minionListSortedByDistanceAndState.Count >= leisureBuilding.Definition.WorkStations)
			{
				playMinionNoAnimAudioSignal.Dispatch("Play_minion_confirm_pathToBldg_01");
				for (int i = 0; i < leisureBuilding.Definition.WorkStations; i++)
				{
					int objectId = minionListSortedByDistanceAndState.Dequeue();
					global::Kampai.Game.View.MinionObject minionObject = component.Get(objectId);
					HandlePathing(minionObject, leisureBuildingObjectView, leisureBuilding);
				}
				global::Kampai.Game.IQuestService obj = questService;
				int iD = leisureBuilding.Definition.ID;
				obj.UpdateAllQuestsWithQuestStepType(global::Kampai.Game.QuestStepType.Leisure, global::Kampai.Game.QuestTaskTransition.Complete, null, iD);
				questService.UpdateAllQuestsWithQuestStepType(global::Kampai.Game.QuestStepType.PlayAnyLeisure);
				buildingStateChangeSignal.Dispatch(leisureBuilding.ID, global::Kampai.Game.BuildingState.Working);
			}
		}

		private void HandlePathing(global::Kampai.Game.View.MinionObject minionObject, global::Kampai.Game.View.LeisureBuildingObjectView leisureBuildingObject, global::Kampai.Game.LeisureBuilding building)
		{
			global::Kampai.Game.Minion byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Minion>(minionObject.ID);
			if (byInstanceId == null)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.CMD_NO_SUCH_MINION, "{0}", minionObject.ID);
			}
			byInstanceId.BuildingID = buildingId;
			minionStateChangeSignal.Dispatch(byInstanceId.ID, global::Kampai.Game.MinionState.Leisure);
			int minionsInBuilding = building.GetMinionsInBuilding();
			global::UnityEngine.Vector3 position = minionObject.transform.position;
			global::UnityEngine.Vector3 routePosition = leisureBuildingObject.GetRoutePosition(minionsInBuilding, building, position);
			global::UnityEngine.Vector3 routeRotation = leisureBuildingObject.GetRouteRotation(minionsInBuilding);
			global::System.Collections.Generic.IList<global::UnityEngine.Vector3> list = pathFinder.FindPath(position, routePosition, 4, true);
			if (list == null)
			{
				global::System.Collections.Generic.List<global::UnityEngine.Vector3> list2 = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>();
				list2.Add(routePosition);
				list = list2;
			}
			global::Kampai.Game.View.RouteInstructions type = new global::Kampai.Game.View.RouteInstructions
			{
				minion = minionObject,
				Path = list,
				Rotation = routeRotation.y,
				TargetBuilding = building,
				StartTime = timeService.CurrentTime()
			};
			routeMinionToLeisureSignal.Dispatch(minionObject, type, minionsInBuilding);
			float num = minionObject.GetAction<global::Kampai.Game.View.ConstantSpeedPathAction>().Duration();
			building.AddMinion(minionObject.ID, type.StartTime + (int)num + 1);
		}
	}
}
