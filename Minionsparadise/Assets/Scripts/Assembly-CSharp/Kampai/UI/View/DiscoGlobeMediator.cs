namespace Kampai.UI.View
{
	public class DiscoGlobeMediator : global::strange.extensions.mediation.impl.Mediator
	{
		private global::Kampai.Game.MinionParty minionParty;

		private global::Kampai.Game.StartMinionPartySignal startMinionPartySignal;

		private global::Kampai.Game.PostStartPartyBuffTimerSignal postStartPartyBuffTimerSignal;

		private bool partyIsHappening;

		private bool partyWillProduceBuff;

		[Inject]
		public global::Kampai.UI.View.DiscoGlobeView view { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideItemPopupSignal hideItemPopupSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.UI.View.DisplayCameraControlsSignal displayCameraControlsSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowHUDSignal showHudSignal { get; set; }

		[Inject]
		public global::Kampai.UI.IPositionService positionService { get; set; }

		[Inject]
		public global::Kampai.UI.View.DisplayDisco3DElements displayDisco3DElements { get; set; }

		[Inject]
		public global::Kampai.Game.VillainLairModel villainLairModel { get; set; }

		public override void OnRegister()
		{
			minionParty = playerService.GetMinionPartyInstance();
			displayCameraControlsSignal.AddListener(DisplayPartyControls);
			global::strange.extensions.injector.api.ICrossContextInjectionBinder injectionBinder = gameContext.injectionBinder;
			startMinionPartySignal = injectionBinder.GetInstance<global::Kampai.Game.StartMinionPartySignal>();
			startMinionPartySignal.AddListener(OnPartyStarted);
			postStartPartyBuffTimerSignal = injectionBinder.GetInstance<global::Kampai.Game.PostStartPartyBuffTimerSignal>();
			postStartPartyBuffTimerSignal.AddListener(OnBuffStarted);
			displayDisco3DElements.AddListener(DisplayDisco3DElements);
			showHudSignal.AddListener(OnShowHUDSignal);
			global::Kampai.Game.IGuestOfHonorService instance = gameContext.injectionBinder.GetInstance<global::Kampai.Game.IGuestOfHonorService>();
			partyWillProduceBuff = instance.PartyShouldProduceBuff();
		}

		public void DisplayDiscoGlobe()
		{
			positionService.AddHUDElementToAvoid(view.DiscoGlobeMesh, true);
			if (minionParty.IsBuffHappening && !minionParty.IsPartyHappening)
			{
				OnBuffStarted();
			}
			else
			{
				DisplayEffects(minionParty.IsPartyHappening);
			}
			view.ShowDiscoBallAwesomeness();
		}

		public override void OnRemove()
		{
			startMinionPartySignal.RemoveListener(OnPartyStarted);
			postStartPartyBuffTimerSignal.RemoveListener(OnBuffStarted);
			displayDisco3DElements.RemoveListener(DisplayDisco3DElements);
			startMinionPartySignal = null;
			postStartPartyBuffTimerSignal = null;
			positionService.RemoveHUDElementToAvoid(view.DiscoGlobeMesh);
			hideItemPopupSignal.Dispatch();
			showHudSignal.RemoveListener(OnShowHUDSignal);
			displayCameraControlsSignal.RemoveListener(DisplayPartyControls);
		}

		private void OnPartyStarted()
		{
			DisplayEffects(true);
			partyIsHappening = true;
		}

		private void OnBuffStarted()
		{
			DisplayEffects(false);
			partyIsHappening = false;
		}

		private void DisplayEffects(bool display)
		{
			if (!display)
			{
				DisplayPartyControls(display);
			}
			view.DisplayEffects(display);
		}

		private void DisplayPartyControls(bool isEnabled)
		{
			if (!isEnabled || partyIsHappening)
			{
				view.ShowCameraControlsPanel(isEnabled && partyWillProduceBuff && minionParty.IsPartyHappening);
			}
		}

		private void DisplayDisco3DElements(bool display)
		{
			view.DisplayDisco3DElements(display);
		}

		private void OnShowHUDSignal(bool display)
		{
			if (!partyIsHappening)
			{
				if (display && villainLairModel.currentActiveLair == null)
				{
					positionService.AddHUDElementToAvoid(view.DiscoGlobeMesh, true);
					view.ShowDiscoBallAwesomeness();
				}
				else
				{
					view.RemoveDiscoBallAwesomeness(null);
				}
			}
		}
	}
}
