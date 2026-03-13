namespace Kampai.Game.View
{
	public class LeisureBuildingObjectMediator : global::strange.extensions.mediation.impl.EventMediator
	{
		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("LeisureBuildingObjectMediator") as global::Kampai.Util.IKampaiLogger;

		private global::strange.extensions.signal.impl.Signal<global::Kampai.Game.View.CharacterObject, int> addToBuildingSignal;

		[Inject]
		public global::Kampai.Game.View.LeisureBuildingObjectView view { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject(global::Kampai.Game.GameElement.MINION_MANAGER)]
		public global::UnityEngine.GameObject minionManager { get; set; }

		[Inject]
		public global::Kampai.Game.RouteMinionToLeisureSignal routeMinionToLeisureSignal { get; set; }

		[Inject]
		public global::Kampai.Game.TeleportMinionToLeisureSignal teleportMinionToLeisureSignal { get; set; }

		[Inject]
		public global::Kampai.Game.MinionStateChangeSignal minionStateChangeSignal { get; set; }

		[Inject]
		public global::Kampai.Game.RemoveMinionFromLeisureSignal removeMinionFromLeisureSignal { get; set; }

		[Inject]
		public global::Kampai.Game.PrepareLeisureMinionsForPartySignal prepareLeisureMinionsForPartySignal { get; set; }

		[Inject]
		public global::Kampai.Game.RestoreLeisureMinionsFromPartySignal restoreLeisureMinionsFromPartySignal { get; set; }

		[Inject]
		public global::Kampai.Game.StartLeisurePartyPointsSignal startLeisurePartyPointsSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ToggleMinionRendererSignal toggleMinionSignal { get; set; }

		[Inject]
		public global::Kampai.Common.PickControllerModel model { get; set; }

		[Inject]
		public global::Kampai.Game.RelocateCharacterSignal relocateCharacterSignal { get; set; }

		public override void OnRegister()
		{
			addToBuildingSignal = new global::strange.extensions.signal.impl.Signal<global::Kampai.Game.View.CharacterObject, int>();
			routeMinionToLeisureSignal.AddListener(RouteMinionToLeisure);
			removeMinionFromLeisureSignal.AddListener(RemoveMinionFromLeisure);
			teleportMinionToLeisureSignal.AddListener(TeleportMinionToLeisure);
			addToBuildingSignal.AddListener(AddToBuilding);
			prepareLeisureMinionsForPartySignal.AddListener(ReleaseMinionsForParty);
			restoreLeisureMinionsFromPartySignal.AddListener(RestoreMinionsFromParty);
			SetupInjections();
		}

		public override void OnRemove()
		{
			routeMinionToLeisureSignal.RemoveListener(RouteMinionToLeisure);
			removeMinionFromLeisureSignal.RemoveListener(RemoveMinionFromLeisure);
			teleportMinionToLeisureSignal.RemoveListener(TeleportMinionToLeisure);
			addToBuildingSignal.RemoveListener(AddToBuilding);
			prepareLeisureMinionsForPartySignal.RemoveListener(ReleaseMinionsForParty);
			restoreLeisureMinionsFromPartySignal.RemoveListener(RestoreMinionsFromParty);
		}

		private void SetupInjections()
		{
			view.SetupInjections(minionStateChangeSignal, startLeisurePartyPointsSignal, relocateCharacterSignal);
		}

		private void RouteMinionToLeisure(global::Kampai.Game.View.MinionObject minionObject, global::Kampai.Game.View.RouteInstructions routeInfo, int routeIndex)
		{
			if (view.leisureBuilding.ID == routeInfo.TargetBuilding.ID)
			{
				playSFXSignal.Dispatch("Play_whistle_call_01");
				view.PathMinionToLeisureBuilding(minionObject, routeInfo.Path, routeInfo.Rotation, routeIndex, addToBuildingSignal);
			}
		}

		private void AddToBuilding(global::Kampai.Game.View.CharacterObject characterObject, int routeIndex)
		{
			view.AddCharacterToBuildingActions(characterObject, routeIndex);
			bool flag = false;
			if (model.SelectedBuilding.HasValue && view.leisureBuilding.ID == model.SelectedBuilding.Value)
			{
				flag = true;
			}
			if (view.IsGFXFaded() || flag)
			{
				view.FadeMinions(toggleMinionSignal, false);
			}
		}

		private void TeleportMinionToLeisure(global::Kampai.Game.Minion minion)
		{
			if (minion.BuildingID == view.leisureBuilding.ID)
			{
				global::Kampai.Game.View.MinionManagerView component = minionManager.GetComponent<global::Kampai.Game.View.MinionManagerView>();
				global::Kampai.Game.View.MinionObject mo = component.Get(minion.ID);
				int minionRouteIndex = view.leisureBuilding.GetMinionRouteIndex(minion.ID);
				if (minionRouteIndex == -1)
				{
					logger.Error("Minion {0} doesn't exist on this building {1}, but you are still trying to add him.", minion.ID, view.leisureBuilding.ID);
				}
				view.AddCharacterToBuildingActions(mo, minionRouteIndex);
			}
		}

		private void RemoveMinionFromLeisure(int buildingInstanceID)
		{
			if (view.leisureBuilding.ID == buildingInstanceID)
			{
				CleanBuildingState(true);
				view.FreeAllMinions();
			}
		}

		private void ReleaseMinionsForParty()
		{
			timeEventService.RemoveEvent(view.leisureBuilding.ID);
			CleanBuildingState(false);
			view.FreeAllMinions();
		}

		private void RestoreMinionsFromParty()
		{
			global::System.Collections.Generic.IList<int> minionList = view.leisureBuilding.MinionList;
			foreach (int item in minionList)
			{
				global::Kampai.Game.Minion byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Minion>(item);
				byInstanceId.IsInMinionParty = false;
				global::Kampai.Game.View.MinionManagerView component = minionManager.GetComponent<global::Kampai.Game.View.MinionManagerView>();
				global::Kampai.Game.View.MinionObject minionObject = component.Get(byInstanceId.ID);
				minionObject.ClearActionQueue();
				TeleportMinionToLeisure(byInstanceId);
				minionStateChangeSignal.Dispatch(item, global::Kampai.Game.MinionState.Leisure);
			}
		}

		private void CleanBuildingState(bool isTaskComplete)
		{
			foreach (int minion in view.leisureBuilding.MinionList)
			{
				if (!view.IsMinionInBuilding(minion))
				{
					global::Kampai.Game.View.MinionManagerView component = minionManager.GetComponent<global::Kampai.Game.View.MinionManagerView>();
					global::Kampai.Game.View.MinionObject minionObject = component.Get(minion);
					minionObject.ClearActionQueue();
				}
				global::Kampai.Game.Minion byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Minion>(minion);
				if (isTaskComplete)
				{
					byInstanceId.BuildingID = -1;
				}
				minionStateChangeSignal.Dispatch(minion, global::Kampai.Game.MinionState.Idle);
				toggleMinionSignal.Dispatch(minion, true);
			}
			if (isTaskComplete)
			{
				view.leisureBuilding.CleanMinionQueue();
			}
		}
	}
}
