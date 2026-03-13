namespace Kampai.Game
{
	public class StartPartyBuffTimerCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.EndPartyBuffTimerWithCallbackSignal endPartyBuffTimerWithCallbackSignal { get; set; }

		[Inject]
		public global::Kampai.Game.PostStartPartyBuffTimerSignal postStartPartyBuffTimerSignal { get; set; }

		[Inject]
		public global::Kampai.Game.UnloadPartyAssetsSignal unloadPartyAssetsSignal { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable uiContext { get; set; }

		[Inject]
		public global::Kampai.Game.IGuestOfHonorService guestService { get; set; }

		public override void Execute()
		{
			if (playerService.IsMinionPartyUnlocked())
			{
				global::Kampai.Game.MinionParty minionPartyInstance = playerService.GetMinionPartyInstance();
				if (minionPartyInstance.IsBuffHappening)
				{
					endPartyBuffTimerWithCallbackSignal.Dispatch(new global::Kampai.Util.Boxed<global::System.Action>(StartBuff));
				}
				else if (guestService.PartyShouldProduceBuff())
				{
					StartBuff();
				}
				else
				{
					CleanUpPartyWithoutBuff();
				}
			}
		}

		private void StartBuff()
		{
			global::Kampai.Game.MinionParty minionPartyInstance = playerService.GetMinionPartyInstance();
			int buffStartTime = timeService.CurrentTime();
			global::Kampai.Game.MinionPartyDefinition definition = minionPartyInstance.Definition;
			minionPartyInstance.BuffStartTime = buffStartTime;
			minionPartyInstance.NewBuffStartTime = 0;
			minionPartyInstance.IsBuffHappening = true;
			minionPartyInstance.PartyType = global::Kampai.Game.MinionPartyType.LUAU;
			minionPartyInstance.PartyStartTier = minionPartyInstance.DeterminePartyTier(playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID));
			global::Kampai.Game.PartyMeterTierDefinition partyMeterTierDefinition = definition.partyMeterDefinition.Tiers[minionPartyInstance.PartyStartTier];
			guestService.StartBuff(partyMeterTierDefinition.Duration);
			postStartPartyBuffTimerSignal.Dispatch();
		}

		private void CleanUpPartyWithoutBuff()
		{
			global::System.Collections.Generic.List<global::Kampai.Game.Minion> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.Minion>();
			foreach (global::Kampai.Game.Minion item in instancesByType)
			{
				item.IsInMinionParty = false;
			}
			playerService.UpdateMinionPartyPointValues();
			uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.DisplayDiscoGlobeSignal>().Dispatch(false);
			unloadPartyAssetsSignal.Dispatch();
		}
	}
}
