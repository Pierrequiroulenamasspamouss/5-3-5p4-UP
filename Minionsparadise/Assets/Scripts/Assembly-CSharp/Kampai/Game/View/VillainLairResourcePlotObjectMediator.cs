namespace Kampai.Game.View
{
	public class VillainLairResourcePlotObjectMediator : global::strange.extensions.mediation.impl.EventMediator
	{
		[Inject]
		public global::Kampai.Game.View.VillainLairResourcePlotObjectView view { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Common.IRandomService randomService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.Game.RouteMinionToLairResourcePlotSignal routeMinionToPlotSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal { get; set; }

		[Inject]
		public global::Kampai.Game.MinionStateChangeSignal minionStateChangeSignal { get; set; }

		[Inject]
		public global::Kampai.Game.RemoveMinionFromLairResourcePlotSignal removeMinionFromLairResourcePlotSignal { get; set; }

		[Inject]
		public global::Kampai.Game.TeleportMinionToLeisureSignal teleportMinionToLeisureSignal { get; set; }

		[Inject]
		public global::Kampai.Util.PathFinder pathfinder { get; set; }

		[Inject(global::Kampai.Game.GameElement.MINION_MANAGER)]
		public global::UnityEngine.GameObject minionManager { get; set; }

		public override void OnRegister()
		{
			routeMinionToPlotSignal.AddListener(RouteMinionToResourcePlot);
			removeMinionFromLairResourcePlotSignal.AddListener(RemoveFromBuilding);
			teleportMinionToLeisureSignal.AddListener(TeleportMinionToResourcePlot);
			view.addToBuildingSignal.AddListener(AddCharacterToBuildingActions);
			view.gagSignal.AddListener(TriggerGagAnimation);
			view.InitializeControllers(randomService, minionStateChangeSignal);
		}

		public override void OnRemove()
		{
			routeMinionToPlotSignal.RemoveListener(RouteMinionToResourcePlot);
			removeMinionFromLairResourcePlotSignal.RemoveListener(RemoveFromBuilding);
			teleportMinionToLeisureSignal.RemoveListener(TeleportMinionToResourcePlot);
			view.addToBuildingSignal.RemoveListener(AddCharacterToBuildingActions);
			view.gagSignal.RemoveListener(TriggerGagAnimation);
		}

		private void RouteMinionToResourcePlot(global::Kampai.Game.View.MinionObject minionObject, int buildingID)
		{
			if (view.resourcePlot.ID == buildingID)
			{
				InitializeGagAnimation();
				playSFXSignal.Dispatch("Play_whistle_call_01");
				view.PathMinionToPlot(minionObject, view.addToBuildingSignal);
			}
		}

		private void TeleportMinionToResourcePlot(global::Kampai.Game.Minion minion)
		{
			if (minion.BuildingID == view.resourcePlot.ID)
			{
				view.resourcePlot.AddMinion(minion.ID, view.resourcePlot.UTCLastTaskingTimeStarted);
				global::Kampai.Game.View.MinionManagerView component = minionManager.GetComponent<global::Kampai.Game.View.MinionManagerView>();
				global::Kampai.Game.View.MinionObject mo = component.Get(minion.ID);
				InitializeGagAnimation();
				view.AddCharacterToBuildingActions(mo, 0);
			}
		}

		private void AddCharacterToBuildingActions(global::Kampai.Game.View.CharacterObject mo, int routeIndex)
		{
			InitializeGagAnimation();
			view.AddCharacterToBuildingActions(mo, routeIndex);
		}

		private void RemoveFromBuilding(int buildingInstanceID)
		{
			if (view.resourcePlot.ID == buildingInstanceID)
			{
				CleanBuildingState();
				global::UnityEngine.Vector3 newPos = pathfinder.RandomPosition(true);
				view.FreeAllMinions(newPos);
			}
		}

		private void CleanBuildingState()
		{
			int minionIDInBuilding = view.resourcePlot.MinionIDInBuilding;
			view.resourcePlot.ClearMinionInBuilding();
			global::Kampai.Game.Minion byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Minion>(minionIDInBuilding);
			if (byInstanceId != null)
			{
				byInstanceId.BuildingID = -1;
				minionStateChangeSignal.Dispatch(minionIDInBuilding, global::Kampai.Game.MinionState.Idle);
			}
		}

		private void InitializeGagAnimation()
		{
			if (view.isGagable && view.resourcePlot.MinionIsTaskedToBuilding())
			{
				int eventTime = randomService.NextInt(view.resourcePlot.Definition.randomGagMin, view.resourcePlot.Definition.randomGagMax);
				timeEventService.AddEvent(view.resourcePlot.MinionIDInBuilding, timeService.CurrentTime(), eventTime, view.gagSignal);
			}
		}

		private void TriggerGagAnimation(int minionId)
		{
			if (minionId == view.resourcePlot.MinionIDInBuilding)
			{
				InitializeGagAnimation();
				view.TriggerGagAnimation();
			}
		}
	}
}
