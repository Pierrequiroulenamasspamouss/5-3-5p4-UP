namespace Kampai.Game
{
	public class StartMinionTaskCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("StartMinionTaskCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Util.Tuple<int, global::Kampai.Game.View.MinionObject, int> parameters { get; set; }

		public int buildingID { get; set; }

		public global::Kampai.Game.View.MinionObject minion { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		public int startTime { get; set; }

		[Inject]
		public global::Kampai.Game.StartMinionRouteSignal startMinionRouteSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		[Inject]
		public global::Kampai.Util.PathFinder pathFinder { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.MinionStateChangeSignal minionStateChangeSignal { get; set; }

		[Inject]
		public global::Kampai.Game.BuildingChangeStateSignal changeState { get; set; }

		[Inject]
		public global::Kampai.Game.TeleportMinionToBuildingSignal teleportMinionToBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Common.MinionTaskCompleteSignal taskCompleteSignal { get; set; }

		public override void Execute()
		{
			buildingID = parameters.Item1;
			minion = parameters.Item2;
			startTime = parameters.Item3;
			global::Kampai.Game.View.BuildingManagerView component = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			if (buildingManager == null || component == null)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.CMD_NULL_REF, 0);
			}
			global::Kampai.Game.View.TaskableBuildingObject taskableBuildingObject = component.GetBuildingObject(buildingID) as global::Kampai.Game.View.TaskableBuildingObject;
			object obj;
			if (taskableBuildingObject != null)
			{
				global::Kampai.Game.TaskableBuilding byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.TaskableBuilding>(taskableBuildingObject.ID);
				obj = byInstanceId;
			}
			else
			{
				obj = null;
			}
			global::Kampai.Game.TaskableBuilding taskableBuilding = (global::Kampai.Game.TaskableBuilding)obj;
			if (taskableBuilding != null)
			{
				HandleAddingMinion(taskableBuilding, taskableBuildingObject);
			}
		}

		private void HandleAddingMinion(global::Kampai.Game.TaskableBuilding building, global::Kampai.Game.View.TaskableBuildingObject taskableBuildingObject)
		{
			global::Kampai.Game.BuildingState state = building.State;
			if (state != global::Kampai.Game.BuildingState.Idle && state != global::Kampai.Game.BuildingState.Working && state != global::Kampai.Game.BuildingState.Harvestable)
			{
				return;
			}
			int minionsInBuilding = building.GetMinionsInBuilding();
			int minionSlotsOwned = building.GetMinionSlotsOwned();
			if (minionsInBuilding < minionSlotsOwned)
			{
				int num = HandlePathing(minion.gameObject, taskableBuildingObject, building);
				changeState.Dispatch(building.ID, global::Kampai.Game.BuildingState.Working);
				building.StateStartTime = num;
				building.AddMinion(minion.ID, num);
				if (questService == null)
				{
					logger.Fatal(global::Kampai.Util.FatalCode.CMD_NULL_REF, 2);
				}
				global::Kampai.Game.IQuestService obj = questService;
				int iD = building.Definition.ID;
				obj.UpdateAllQuestsWithQuestStepType(global::Kampai.Game.QuestStepType.MinionTask, global::Kampai.Game.QuestTaskTransition.Complete, null, iD, minion.Level);
			}
		}

		private int HandlePathing(global::UnityEngine.GameObject minionGo, global::Kampai.Game.View.TaskableBuildingObject tbo, global::Kampai.Game.TaskableBuilding building)
		{
			global::UnityEngine.Vector3 position = minionGo.transform.position;
			global::UnityEngine.Vector3 routePosition = tbo.GetRoutePosition(building.GetMinionsInBuilding(), building, position);
			global::UnityEngine.Vector3 routeRotation = tbo.GetRouteRotation(building.GetMinionsInBuilding());
			global::System.Collections.Generic.IList<global::UnityEngine.Vector3> list = pathFinder.FindPath(position, routePosition, 4, true);
			if (list == null)
			{
				global::System.Collections.Generic.List<global::UnityEngine.Vector3> list2 = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>();
				list2.Add(routePosition);
				list = list2;
			}
			global::Kampai.Game.View.RouteInstructions type = new global::Kampai.Game.View.RouteInstructions
			{
				minion = minion,
				Path = list,
				Rotation = routeRotation.y,
				TargetBuilding = building,
				StartTime = startTime
			};
			startMinionRouteSignal.Dispatch(type);
			return SetupMinionState(minion.ID, minionGo, building);
		}

		private int SetupMinionState(int minionID, global::UnityEngine.GameObject minionGo, global::Kampai.Game.TaskableBuilding building)
		{
			global::Kampai.Game.View.MinionObject component = minionGo.GetComponent<global::Kampai.Game.View.MinionObject>();
			float num = component.GetAction<global::Kampai.Game.View.ConstantSpeedPathAction>().Duration();
			int num2 = timeService.CurrentTime();
			global::Kampai.Game.Minion minion = component.GetMinion();
			if (minion == null)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.CMD_NO_SUCH_MINION, "{0}", minionID);
			}
			minion.BuildingID = buildingID;
			if (building is global::Kampai.Game.MignetteBuilding)
			{
				minionStateChangeSignal.Dispatch(minion.ID, global::Kampai.Game.MinionState.PlayingMignette);
				routineRunner.StartCoroutine(WaitThenTeleportMinion(minion.ID));
			}
			else
			{
				minion.TaskDuration = BuildingUtil.GetHarvestTimeForTaskableBuilding(building, definitionService);
				minion.UTCTaskStartTime = num2 + (int)num + 1;
				if (!(building is global::Kampai.Game.DebrisBuilding))
				{
					timeEventService.AddEvent(minion.ID, minion.UTCTaskStartTime, minion.TaskDuration, taskCompleteSignal, (building is global::Kampai.Game.ResourceBuilding) ? global::Kampai.Game.TimeEventType.ProductionBuff : global::Kampai.Game.TimeEventType.Default);
				}
				minionStateChangeSignal.Dispatch(minion.ID, global::Kampai.Game.MinionState.Tasking);
			}
			return num2;
		}

		private global::System.Collections.IEnumerator WaitThenTeleportMinion(int minionID)
		{
			yield return new global::UnityEngine.WaitForSeconds(0.2f);
			teleportMinionToBuildingSignal.Dispatch(minionID);
		}
	}
}
