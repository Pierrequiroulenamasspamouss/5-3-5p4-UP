namespace Kampai.Game
{
	public class RestoreMinionPartyCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("RestoreMinionPartyCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Game.IGuestOfHonorService guestService { get; set; }

		[Inject]
		public global::Kampai.UI.View.DisplayDiscoGlobeSignal displayDiscoGlobeSignal { get; set; }

		[Inject]
		public global::Kampai.Game.PreLoadPartyAssetsSignal preloadPartyAssetsSignal { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Game.LoadPartyAssetsSignal loadPartyAssetsSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.MinionParty minionPartyInstance = playerService.GetMinionPartyInstance();
			minionPartyInstance.ResolveBuffStartTime();
			minionPartyInstance.IsPartyHappening = false;
			int buffStartTime = minionPartyInstance.BuffStartTime;
			if (buffStartTime == 0)
			{
				return;
			}
			int currentBuffDuration = guestService.GetCurrentBuffDuration();
			minionPartyInstance.NewBuffStartTime = 0;
			if (buffStartTime + currentBuffDuration > timeService.CurrentTime())
			{
				minionPartyInstance.IsBuffHappening = true;
				global::System.Collections.Generic.List<int> lastGuestsOfHonorPrestigeIDs = minionPartyInstance.lastGuestsOfHonorPrestigeIDs;
				if (lastGuestsOfHonorPrestigeIDs.Count == 1)
				{
					guestService.SelectGuestOfHonor(lastGuestsOfHonorPrestigeIDs[0]);
				}
				else if (lastGuestsOfHonorPrestigeIDs.Count == 2)
				{
					guestService.SelectGuestOfHonor(lastGuestsOfHonorPrestigeIDs[0], lastGuestsOfHonorPrestigeIDs[1]);
				}
				else
				{
					logger.Error("No stored guests of honor");
				}
				global::Kampai.Game.PartyMeterTierDefinition partyMeterTierDefinition = minionPartyInstance.Definition.partyMeterDefinition.Tiers[minionPartyInstance.PartyStartTier];
				guestService.StartBuff(partyMeterTierDefinition.Duration);
				preloadPartyAssetsSignal.Dispatch();
				loadPartyAssetsSignal.Dispatch();
				DisplayDiscoBall(true);
			}
			else
			{
				HandleBuffOver(minionPartyInstance, currentBuffDuration);
			}
		}

		private void HandleBuffOver(global::Kampai.Game.MinionParty minionParty, int buffDuration)
		{
			guestService.StopBuff(buffDuration, minionParty.BuffStartTime);
			minionParty.IsBuffHappening = false;
			minionParty.BuffStartTime = 0;
			DisplayDiscoBall(false);
		}

		private void DisplayDiscoBall(bool display)
		{
			routineRunner.StartCoroutine(DisplayDiscoBallDelayed(display));
		}

		private global::System.Collections.IEnumerator DisplayDiscoBallDelayed(bool display)
		{
			yield return new global::UnityEngine.WaitForEndOfFrame();
			yield return new global::UnityEngine.WaitForEndOfFrame();
			yield return new global::UnityEngine.WaitForEndOfFrame();
			displayDiscoGlobeSignal.Dispatch(display);
		}
	}
}
