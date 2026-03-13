namespace Kampai.UI.View
{
	public class LevelUpRewardMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.LevelUpRewardView>
	{
		private global::Kampai.Game.StartMinionPartySignal startMinionPartySignal;

		private global::System.Collections.Generic.List<global::Kampai.Game.View.RewardQuantity> quantityChange;

		private bool isInspiration;

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.UI.IFancyUIService fancyUIService { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSoundFX { get; set; }

		[Inject]
		public global::Kampai.UI.View.FTUELevelUpOpenSignal FTUEOpenSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.FTUELevelUpCloseSignal FTUECloseSignal { get; set; }

		[Inject]
		public global::Kampai.Game.UnlockMinionsSignal unlockMinionsSignal { get; set; }

		[Inject]
		public global::Kampai.Game.UpdatePlayerDLCTierSignal playerDLCTierSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideAllWayFindersSignal hideAllWayFindersSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.DisplayCameraControlsSignal displayCameraControlsSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.UIModel uiModel { get; set; }

		[Inject]
		public global::Kampai.UI.View.LevelUpBackButtonSignal levelUpBackButtonSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.RevealLevelUpUISignal revealLevelUpSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IGuestOfHonorService guestService { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowAllWayFindersSignal showAllWayFindersSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideItemPopupSignal hideItemPopupSignal { get; set; }

		[Inject]
		public global::Kampai.Game.UpdateAdHUDSignal updateAdHUDSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			startMinionPartySignal = gameContext.injectionBinder.GetInstance<global::Kampai.Game.StartMinionPartySignal>();
			startMinionPartySignal.AddListener(DisplayView);
			base.view.closeSignal.AddListener(Close);
			base.view.beginUnlockSignal.AddListener(BeginUnlock);
			base.view.closeBuffInfoSignal.AddListener(CloseBuffInfo);
			base.view.skrimButton.ClickedSignal.AddListener(SkrimClicked);
			base.view.skipButton.ClickedSignal.AddListener(SkrimClicked);
			levelUpBackButtonSignal.AddListener(SkrimClicked);
			revealLevelUpSignal.AddListener(RevealUI);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			base.view.closeSignal.RemoveListener(Close);
			base.view.beginUnlockSignal.RemoveListener(BeginUnlock);
			base.view.closeBuffInfoSignal.RemoveListener(CloseBuffInfo);
			base.view.skrimButton.ClickedSignal.RemoveListener(SkrimClicked);
			base.view.skipButton.ClickedSignal.RemoveListener(SkrimClicked);
			levelUpBackButtonSignal.RemoveListener(SkrimClicked);
			startMinionPartySignal.RemoveListener(DisplayView);
			revealLevelUpSignal.RemoveListener(RevealUI);
			uiModel.LevelUpUIOpen = false;
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			quantityChange = args.Get<global::System.Collections.Generic.List<global::Kampai.Game.View.RewardQuantity>>();
			isInspiration = args.Get<bool>();
			base.view.Init(playerService, definitionService, localService, fancyUIService, playSoundFX, quantityChange, guestService);
		}

		private void DisplayView()
		{
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.BuildingZoomSignal>().Dispatch(new global::Kampai.Game.BuildingZoomSettings(global::Kampai.Game.ZoomType.OUT, global::Kampai.Game.BuildingZoomType.TIKIBAR));
			hideAllWayFindersSignal.Dispatch();
			RevealUI();
		}

		private void RevealUI()
		{
			base.closeAllOtherMenuSignal.Dispatch(base.view.gameObject);
			FTUEOpenSignal.Dispatch();
			uiModel.LevelUpUIOpen = true;
			if (isInspiration)
			{
				base.view.StartAnimation();
			}
			else
			{
				StartCoroutine(WaitThenReleasePicker());
			}
		}

		private global::System.Collections.IEnumerator WaitThenReleasePicker()
		{
			yield return new global::UnityEngine.WaitForSeconds(base.view.waitForDooberTimer + base.view.openForTimer);
			Close();
		}

		protected override void OnCloseAllMenu(global::UnityEngine.GameObject exception)
		{
		}

		private void SkrimClicked()
		{
			if (base.view.coroutine != null)
			{
				StopCoroutine(base.view.coroutine);
				base.view.StartCoroutine(base.view.CloseDown());
			}
		}

		private void BeginUnlock()
		{
			unlockMinionsSignal.Dispatch();
		}

		private void CloseBuffInfo()
		{
			hideItemPopupSignal.Dispatch();
		}

		protected override void Close()
		{
			base.view.CleanupListeners();
			FTUECloseSignal.Dispatch();
			playerDLCTierSignal.Dispatch();
			if (!playerService.GetMinionPartyInstance().PartyPreSkip)
			{
				displayCameraControlsSignal.Dispatch(true);
				gameContext.injectionBinder.GetInstance<global::Kampai.Game.StartPartyFavorAnimationSignal>().Dispatch();
			}
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "screen_PhilsInspiration");
			global::Kampai.Game.MinionParty minionPartyInstance = playerService.GetMinionPartyInstance();
			if (!minionPartyInstance.IsPartyHappening)
			{
				showAllWayFindersSignal.Dispatch();
			}
			updateAdHUDSignal.Dispatch();
			global::Kampai.Game.TSMCharacter firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.TSMCharacter>(70008);
			if (firstInstanceByDefinitionId != null && !timeEventService.HasEventID(firstInstanceByDefinitionId.ID))
			{
				gameContext.injectionBinder.GetInstance<global::Kampai.Game.CheckTriggersSignal>().Dispatch(firstInstanceByDefinitionId.ID);
			}
		}
	}
}
