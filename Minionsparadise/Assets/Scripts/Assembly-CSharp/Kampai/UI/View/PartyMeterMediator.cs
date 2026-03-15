namespace Kampai.UI.View
{
	public class PartyMeterMediator : global::strange.extensions.mediation.impl.Mediator
	{
		private global::Kampai.Game.MinionParty minionParty;

		private global::System.Collections.IEnumerator fillMeterCoroutine;

		private global::Kampai.Game.PostStartPartyBuffTimerSignal postStartPartyBuffTimerSignal;

		[Inject]
		public global::Kampai.UI.View.PartyMeterView view { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IGuestOfHonorService guestService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		public override void OnRegister()
		{
			postStartPartyBuffTimerSignal = gameContext.injectionBinder.GetInstance<global::Kampai.Game.PostStartPartyBuffTimerSignal>();
			postStartPartyBuffTimerSignal.AddListener(OnBuffStarted);
			Init();
		}

		public override void OnRemove()
		{
			RemoveCoroutine(fillMeterCoroutine);
			postStartPartyBuffTimerSignal.RemoveListener(OnBuffStarted);
			postStartPartyBuffTimerSignal = null;
		}

		private void Init()
		{
			minionParty = playerService.GetMinionPartyInstance();
			if (minionParty.IsBuffHappening && !minionParty.IsPartyHappening)
			{
				OnBuffStarted();
			}
		}

		private void OnBuffStarted()
		{
			if (guestService.GetBuffRemainingTime(minionParty) > 0)
			{
				ShowTheCooldownbar();
			}
		}

		private void ShowTheCooldownbar()
		{
			if (guestService.PartyShouldProduceBuff())
			{
				DisplayCooldownMeter(true);
				fillMeterCoroutine = AnimMeter();
				StartCoroutine(fillMeterCoroutine);
			}
		}

		private global::System.Collections.IEnumerator AnimMeter()
		{
			for (float buffRemaining = guestService.GetBuffRemainingTime(minionParty); buffRemaining >= 0f; buffRemaining = guestService.GetBuffRemainingTime(minionParty))
			{
				UpdateCountDownText(buffRemaining);
				yield return new global::UnityEngine.WaitForEndOfFrame();
			}
			fillMeterCoroutine = null;
			DisplayCooldownMeter(false);
		}

		private void UpdateCountDownText(float timeRemaining)
		{
			view.UpdateCountDownText(string.Format("{0}", UIUtils.FormatTime(timeRemaining, localizationService)));
		}

		private void DisplayCooldownMeter(bool display)
		{
			view.DisplayCooldownMeter(display);
			if (display)
			{
				float num = guestService.GetBuffRemainingTime(minionParty);
				if (num >= 0f)
				{
					UpdateCountDownText(num);
				}
				else
				{
					view.DisplayCooldownMeter(false);
				}
				view.UpdateBuffText(localizationService.GetString("partyBuffMultiplier", guestService.GetCurrentBuffMultipler()));
				global::Kampai.Game.BuffDefinition recentBuffDefinition = guestService.GetRecentBuffDefinition();
				view.UpdateBuffIcon(recentBuffDefinition.buffSimpleMask);
			}
		}

		private void RemoveCoroutine(global::System.Collections.IEnumerator routine)
		{
			if (routine != null)
			{
				StopCoroutine(routine);
				routine = null;
			}
		}
	}
}
