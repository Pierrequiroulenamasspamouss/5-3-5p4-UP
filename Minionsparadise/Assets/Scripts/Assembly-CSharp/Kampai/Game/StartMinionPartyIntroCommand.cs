namespace Kampai.Game
{
	public class StartMinionPartyIntroCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.TriggerPhilPartyStartSignal triggerPhilPartyStartSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowHUDSignal showHudSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.CameraMoveToCustomPositionSignal customCameraPositionSignal { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable uiContext { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowStoreSignal showStoreSignal { get; set; }

		[Inject]
		public global::Kampai.Game.TeleportMinionsToTownForPartySignal teleportMinionsToTownForPartySignal { get; set; }

		[Inject]
		public global::Kampai.Game.PrepareLeisureMinionsForPartySignal prepareLeisureMinionsForPartySignal { get; set; }

		[Inject]
		public global::Kampai.Game.PrepareTaskingMinionsForPartySignal prepareTaskingMinionsForPartySignal { get; set; }

		[Inject]
		public global::Kampai.Game.AddCharacterToPartyStageSignal addCharacterToPartyStageSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.KillDiscoGlobeSignal killDiscoGlobeSignal { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.Game.IPartyService partyService { get; set; }

		[Inject]
		public global::Kampai.Game.PreLoadPartyAssetsSignal preLoadPartyAssetsSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Game.IGuestOfHonorService guestService { get; set; }

		[Inject]
		public global::Kampai.Game.CheckMinionPartyLevelSignal checkMinionPartyLevelSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		public override void Execute()
		{
			if (timeEventService.HasEventID(80000))
			{
				timeEventService.RushEvent(80000);
			}
			int quantity = (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.XP_ID);
			checkMinionPartyLevelSignal.Dispatch(true);
			global::Kampai.Game.MinionParty minionPartyInstance = playerService.GetMinionPartyInstance();
			minionPartyInstance.IsPartyHappening = true;
			if (guestService.PartyShouldProduceBuff())
			{
				minionPartyInstance.NewBuffStartTime = timeService.CurrentTime() + minionPartyInstance.Definition.GetPartyDuration(true);
				minionPartyInstance.PartyStartTier = minionPartyInstance.DeterminePartyTier(playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID));
				guestService.UpdateAndStoreGuestOfHonorCooldowns();
			}
			prepareLeisureMinionsForPartySignal.Dispatch();
			prepareTaskingMinionsForPartySignal.Dispatch();
			addCharacterToPartyStageSignal.Dispatch();
			showHudSignal.Dispatch(false);
			uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.HideAllWayFindersSignal>().Dispatch();
			showStoreSignal.Dispatch(false);
			killDiscoGlobeSignal.Dispatch();
			preLoadPartyAssetsSignal.Dispatch();
			customCameraPositionSignal.Dispatch(60000, new global::Kampai.Util.Boxed<global::System.Action>(OnPanComplete));
			SendTelemetry(minionPartyInstance, quantity);
		}

		private void OnPanComplete()
		{
			teleportMinionsToTownForPartySignal.Dispatch();
			triggerPhilPartyStartSignal.Dispatch();
		}

		private void SendTelemetry(global::Kampai.Game.MinionParty minionParty, int totalPartyPoints)
		{
			int quantity = (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID);
			global::Kampai.Game.BuffDefinition recentBuffDefinition = guestService.GetRecentBuffDefinition(true);
			string guestOfHonor = ((guestService.CurrentGuestOfHonor == null) ? "Guest Of Honor is null" : guestService.CurrentGuestOfHonor.LocalizedKey);
			bool isInspiredParty = partyService.IsInspirationParty(quantity, minionParty.CurrentPartyIndex);
			telemetryService.Send_Telemetry_EVT_MINION_PARTY_STARTED(totalPartyPoints, (recentBuffDefinition == null) ? "Buff is null" : recentBuffDefinition.buffType.ToString(), guestOfHonor, isInspiredParty);
		}
	}
}
