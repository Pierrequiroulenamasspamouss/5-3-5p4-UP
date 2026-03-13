namespace Kampai.UI.View
{
	public class SkipPartyButtonMediator : global::strange.extensions.mediation.impl.Mediator
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SkipPartyButtonMediator") as global::Kampai.Util.IKampaiLogger;

		private global::System.Collections.IEnumerator skipButtonCooldownMeterCoroutine;

		private global::Kampai.Game.MinionParty party;

		[Inject]
		public global::Kampai.UI.View.SkipPartyButtonView view { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Game.IGuestOfHonorService guestService { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowMinionPartySkipButtonSignal showButtonSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.RevealLevelUpUISignal revealLevelUpSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetGrindCurrencySignal setGrindCurrencySignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetPremiumCurrencySignal setPremiumCurrencySignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetStorageCapacitySignal setStorageSignal { get; set; }

		public override void OnRegister()
		{
			view.SkipButton.ClickedSignal.AddListener(SkipPartyButtonClick);
			showButtonSignal.AddListener(DisplaySkipButton);
			Init();
		}

		public override void OnRemove()
		{
			RemoveCoroutine(skipButtonCooldownMeterCoroutine);
			view.SkipButton.ClickedSignal.RemoveListener(SkipPartyButtonClick);
			showButtonSignal.RemoveListener(DisplaySkipButton);
		}

		private void Init()
		{
			party = playerService.GetMinionPartyInstance();
			view.ShowSkipPartyButtonView(false);
		}

		private void DisplaySkipButton(bool isEnabled)
		{
			view.ShowSkipPartyButtonView(isEnabled);
			if (isEnabled)
			{
				party.PartyPreSkip = false;
				StartSkipCooldownMeter();
			}
		}

		private void SkipPartyButtonClick()
		{
			logger.Debug("Dispatching the EndMinionPartySignal from the skip party button");
			party.PartyPreSkip = true;
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.EndMinionPartySignal>().Dispatch(true);
			setGrindCurrencySignal.Dispatch();
			setPremiumCurrencySignal.Dispatch();
			setStorageSignal.Dispatch();
			revealLevelUpSignal.Dispatch();
			StopSkipCooldownTime();
			DisplaySkipButton(false);
		}

		private void StartSkipCooldownMeter()
		{
			global::Kampai.Game.MinionPartyDefinition definition = party.Definition;
			float num = definition.GetPartyDuration(guestService.PartyShouldProduceBuff());
			num += 3.34f;
			skipButtonCooldownMeterCoroutine = AnimSkipCooldown(num);
			StartCoroutine(skipButtonCooldownMeterCoroutine);
		}

		private global::System.Collections.IEnumerator AnimSkipCooldown(float duration)
		{
			float totalPartyTime = duration;
			while (duration >= 0f)
			{
				view.UpdateSkipMeterTime(duration, totalPartyTime);
				yield return new global::UnityEngine.WaitForEndOfFrame();
				duration -= global::UnityEngine.Time.deltaTime;
			}
			skipButtonCooldownMeterCoroutine = null;
			DisplaySkipButton(false);
		}

		public void StopSkipCooldownTime()
		{
			if (skipButtonCooldownMeterCoroutine != null)
			{
				StopCoroutine(skipButtonCooldownMeterCoroutine);
			}
			skipButtonCooldownMeterCoroutine = null;
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
