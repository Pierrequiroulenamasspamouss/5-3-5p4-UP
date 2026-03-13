namespace Kampai.Game.View
{
	public class PhilMediator : global::strange.extensions.mediation.impl.Mediator
	{
		private const int cameraMask = 7;

		private bool cameraDisabled;

		private global::Kampai.UI.View.DisplayCameraControlsSignal displayCameraControlsSignal;

		[Inject]
		public global::Kampai.Game.View.PhilView view { get; set; }

		[Inject]
		public global::Kampai.Game.PhilCelebrateSignal celebrateSignal { get; set; }

		[Inject]
		public global::Kampai.Game.PhilGetAttentionSignal getAttentionSignal { get; set; }

		[Inject]
		public global::Kampai.Game.PhilBeginIntroLoopSignal beginIntroLoopSignal { get; set; }

		[Inject]
		public global::Kampai.Game.PhilPlayIntroSignal playIntroSignal { get; set; }

		[Inject]
		public global::Kampai.Game.PhilSignFixedSignal philSignFixedSignal { get; set; }

		[Inject]
		public global::Kampai.Game.PhilSitAtBarSignal sitAtBarSignal { get; set; }

		[Inject]
		public global::Kampai.Game.PhilActivateSignal activateSignal { get; set; }

		[Inject]
		public global::Kampai.Game.AnimatePhilSignal animatePhilSignal { get; set; }

		[Inject]
		public global::Kampai.Game.PhilGoToTikiBarSignal philGoToTikiBarSignal { get; set; }

		[Inject]
		public global::Kampai.Game.PhilEnableTikiBarControllerSignal enableTikiBarControllerSignal { get; set; }

		[Inject]
		public global::Kampai.Game.PhilPlayConfettiSignal philPlayConfettiSignal { get; set; }

		[Inject]
		public global::Kampai.Game.TeleportCharacterToTikiBarSignal teleportCharacterToTikiBarSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::strange.extensions.injector.api.IInjectionBinder injectionBinder { get; set; }

		[Inject]
		public global::Kampai.Game.TikiBarSetAnimParamSignal tikiBarSetAnimParamSignal { get; set; }

		[Inject]
		public global::Kampai.Game.TikiBarResetAnimParamSignal tikiBarResetAnimParamSignal { get; set; }

		[Inject]
		public global::Kampai.Game.PhilGoToStartLocationSignal goToStartLocationSignal { get; set; }

		[Inject]
		public global::Kampai.Game.EnableCameraBehaviourSignal enableCameraBehaviourSignal { get; set; }

		[Inject]
		public global::Kampai.Game.DisableCameraBehaviourSignal disableCameraBehaviourSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CameraAutoMoveToBuildingSignal cameraAutoMoveToBuildingSignal { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable uiContext { get; set; }

		[Inject]
		public global::Kampai.Game.PostMinionPartyEndSignal postMinionPartyEndSignal { get; set; }

		[Inject]
		public global::Kampai.Game.TriggerPhilPartyStartSignal triggerPartyStartSignal { get; set; }

		[Inject]
		public global::Kampai.Game.StartMinionPartySignal startMinionPartySignal { get; set; }

		[Inject]
		public global::Kampai.Game.LoadPartyAssetsSignal loadPartyAssetsSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CameraMoveToCustomPositionSignal customCameraPositionSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CreateDoobersFromGiftBoxSignal createDoobersSignal { get; set; }

		[Inject]
		public global::Kampai.Common.PickControllerModel pickControllerModel { get; set; }

		[Inject]
		public global::Kampai.Game.EndMinionPartySignal endMinionPartySignal { get; set; }

		public override void OnRegister()
		{
			celebrateSignal.AddListener(Celebrate);
			getAttentionSignal.AddListener(GetAttention);
			beginIntroLoopSignal.AddListener(BeginIntroLoop);
			playIntroSignal.AddListener(PlayIntro);
			sitAtBarSignal.AddListener(SitAtBar);
			activateSignal.AddListener(Activate);
			animatePhilSignal.AddListener(AnimatePhil);
			enableTikiBarControllerSignal.AddListener(EnableTikiBarController);
			philGoToTikiBarSignal.AddListener(GotoTikiBar);
			philSignFixedSignal.AddListener(SignFixed);
			philPlayConfettiSignal.AddListener(PlayConFetti);
			view.AnimSignal.AddListener(SendTikiBarSignal);
			view.OnAnimatorParamatersResetSignal.AddListener(ResetTikiBarAnimParams);
			view.partyIntroCompleteSignal.AddListener(PartyIntroFinished);
			goToStartLocationSignal.AddListener(GoToStartLocation);
			endMinionPartySignal.AddListener(SkipParty);
			postMinionPartyEndSignal.AddListener(StopSpinFireStick);
			triggerPartyStartSignal.AddListener(TriggerPartyStart);
			displayCameraControlsSignal = uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.DisplayCameraControlsSignal>();
			displayCameraControlsSignal.AddListener(StartSpinFireStick);
		}

		public override void OnRemove()
		{
			view.AnimSignal.RemoveListener(SendTikiBarSignal);
			view.OnAnimatorParamatersResetSignal.RemoveListener(ResetTikiBarAnimParams);
			view.partyIntroCompleteSignal.RemoveListener(PartyIntroFinished);
			celebrateSignal.RemoveListener(Celebrate);
			getAttentionSignal.RemoveListener(GetAttention);
			beginIntroLoopSignal.RemoveListener(BeginIntroLoop);
			playIntroSignal.RemoveListener(PlayIntro);
			sitAtBarSignal.RemoveListener(SitAtBar);
			activateSignal.RemoveListener(Activate);
			animatePhilSignal.RemoveListener(AnimatePhil);
			enableTikiBarControllerSignal.RemoveListener(EnableTikiBarController);
			philGoToTikiBarSignal.RemoveListener(GotoTikiBar);
			philSignFixedSignal.RemoveListener(SignFixed);
			goToStartLocationSignal.RemoveListener(GoToStartLocation);
			postMinionPartyEndSignal.RemoveListener(StopSpinFireStick);
			triggerPartyStartSignal.RemoveListener(TriggerPartyStart);
			philPlayConfettiSignal.RemoveListener(PlayConFetti);
			endMinionPartySignal.RemoveListener(SkipParty);
			if (displayCameraControlsSignal != null)
			{
				displayCameraControlsSignal.RemoveListener(StartSpinFireStick);
			}
		}

		private void GoToStartLocation()
		{
			view.GoToStartLocation();
		}

		private void SignFixed()
		{
			view.SignFixed();
		}

		private void Activate(bool activate)
		{
			view.Activate(activate);
		}

		private void SitAtBar(bool sit)
		{
			view.SitAtBar(sit, teleportCharacterToTikiBarSignal);
		}

		private void Celebrate()
		{
			view.Celebrate();
		}

		private void PlayConFetti()
		{
			view.PlayConFetti();
		}

		private void TriggerPartyStart()
		{
			StopSpinFireStick();
			view.RemoveProp("Prop_FireStick_Prefab");
			view.StartParty();
		}

		private void PartyIntroFinished()
		{
			loadPartyAssetsSignal.Dispatch();
			if (!playerService.GetMinionPartyInstance().PartyPreSkip)
			{
				createDoobersSignal.Dispatch();
				customCameraPositionSignal.Dispatch(60001, new global::Kampai.Util.Boxed<global::System.Action>(StartTheParty));
			}
		}

		private void StartTheParty()
		{
			startMinionPartySignal.Dispatch();
			view.RemoveProp("Prop_PartyBox_Prefab");
		}

		private void SkipParty(bool isSkipping)
		{
			if (isSkipping)
			{
				view.PartySkip();
			}
		}

		private void StartSpinFireStick(bool display)
		{
			if (display)
			{
				view.SpinFireStick(display);
			}
		}

		private void StopSpinFireStick()
		{
			view.SpinFireStick(false);
		}

		private void GetAttention(bool enable)
		{
			view.GetAttention(enable);
		}

		private void BeginIntroLoop(bool showTikiBar)
		{
			view.RemoveProp("Prop_PartyBox_Prefab");
			view.RemoveProp("Prop_FireStick_Prefab");
			view.BeginIntroLoop();
			if (showTikiBar)
			{
				disableCamera();
				pickControllerModel.ForceDisabled = true;
				uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.SetBuildMenuEnabledSignal>().Dispatch(false);
			}
		}

		private void PlayIntro(bool showTikiBar)
		{
			view.PlayIntro();
			if (showTikiBar)
			{
				StartCoroutine(MoveToTikiBar());
			}
		}

		private global::System.Collections.IEnumerator MoveToTikiBar()
		{
			yield return new global::UnityEngine.WaitForSeconds(1f);
			global::Kampai.Game.TikiBarBuilding tikibarBuilding = playerService.GetByInstanceId<global::Kampai.Game.TikiBarBuilding>(313);
			cameraAutoMoveToBuildingSignal.Dispatch(tikibarBuilding, new global::Kampai.Game.PanInstructions(tikibarBuilding));
		}

		private void disableCamera()
		{
			cameraDisabled = true;
			disableCameraBehaviourSignal.Dispatch(7);
		}

		private void enableCamera()
		{
			cameraDisabled = false;
			enableCameraBehaviourSignal.Dispatch(7);
		}

		private void AnimatePhil(string animation)
		{
			if (animation.Equals("idle"))
			{
				view.StopWalking();
			}
			view.Animate(animation);
		}

		private void GotoTikiBar(bool pop)
		{
			view.GotoTikiBar(injectionBinder.GetInstance<global::UnityEngine.GameObject>(global::Kampai.Game.StaticItem.TIKI_BAR_BUILDING_ID_DEF), teleportCharacterToTikiBarSignal);
		}

		private void EnableTikiBarController()
		{
			view.EnableTikiBarController();
		}

		private void ResetTikiBarAnimParams()
		{
			bool animBool = view.GetAnimBool("PartySkip");
			tikiBarResetAnimParamSignal.Dispatch(animBool);
		}

		private void SendTikiBarSignal(string animation, global::System.Type type, object obj)
		{
			tikiBarSetAnimParamSignal.Dispatch(animation, type, obj);
			if (animation.Equals("NewMinionSequence") && cameraDisabled)
			{
				enableCamera();
				pickControllerModel.ForceDisabled = false;
				uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.SetBuildMenuEnabledSignal>().Dispatch(true);
			}
		}
	}
}
