namespace Kampai.UI.View
{
	public class PlayerTrainingMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.PlayerTrainingView>
	{
		private bool openedFromSettingsMenu;

		private float startTime;

		private int triggeredID;

		private global::strange.extensions.signal.impl.Signal<bool> callback;

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.DisplaySettingsMenuSignal displaySettingsMenuSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.TempHideSettingsMenuSignal tempHideMenuSignal { get; set; }

		[Inject]
		public global::Kampai.UI.IFancyUIService fancyUIService { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		[Inject]
		public global::Kampai.Game.EnableCameraBehaviourSignal enableCameraSignal { get; set; }

		[Inject]
		public global::Kampai.Game.DisableCameraBehaviourSignal disableCameraSignal { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.UI.View.TrainingClosedSignal trainingClosedSignal { get; set; }

		[Inject]
		public global::Kampai.Game.UnlockMinionsSignal unlockMinionsSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.UIModel uiModel { get; set; }

		[Inject]
		public global::Kampai.Game.VillainLairModel lairModel { get; set; }

		[Inject]
		public global::Kampai.Main.MoveAudioListenerSignal toggleCharacterAudioSignal { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			toggleCharacterAudioSignal.Dispatch(false, base.view.minionSlots[1].transform);
			if (lairModel.currentActiveLair == null)
			{
				disableCameraSignal.Dispatch(2);
			}
			base.view.Init(definitionService, fancyUIService);
			base.view.confirmButton.ClickedSignal.AddListener(ButtonClose);
			base.view.completeSignal.AddListener(StartTimer);
			base.view.audioSignal.AddListener(PlayAudio);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			if (lairModel.currentActiveLair == null)
			{
				enableCameraSignal.Dispatch(2);
			}
			base.view.confirmButton.ClickedSignal.RemoveListener(ButtonClose);
			base.view.completeSignal.RemoveListener(StartTimer);
			base.view.audioSignal.RemoveListener(PlayAudio);
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			int defID = args.Get<int>();
			openedFromSettingsMenu = args.Get<bool>();
			callback = args.Get<global::strange.extensions.signal.impl.Signal<bool>>();
			uiModel.DisableBack = true;
			uiModel.PopupAnimationIsPlaying = true;
			if (openedFromSettingsMenu)
			{
				tempHideMenuSignal.Dispatch();
				displaySettingsMenuSignal.Dispatch(false);
			}
			base.closeAllOtherMenuSignal.Dispatch(base.view.gameObject);
			ExtractData(defID);
		}

		private void ExtractData(int defID)
		{
			triggeredID = defID;
			global::Kampai.Game.PlayerTrainingDefinition playerTrainingDefinition = definitionService.Get<global::Kampai.Game.PlayerTrainingDefinition>(defID);
			base.view.SetTitle(localizationService.GetString(playerTrainingDefinition.trainingTitleLocalizedKey));
			global::Kampai.Game.PlayerTrainingCardDefinition playerTrainingCardDefinition = definitionService.Get<global::Kampai.Game.PlayerTrainingCardDefinition>(playerTrainingDefinition.cardOneDefinitionID);
			global::Kampai.Game.PlayerTrainingCardDefinition playerTrainingCardDefinition2 = definitionService.Get<global::Kampai.Game.PlayerTrainingCardDefinition>(playerTrainingDefinition.cardTwoDefinitionID);
			global::Kampai.Game.PlayerTrainingCardDefinition playerTrainingCardDefinition3 = definitionService.Get<global::Kampai.Game.PlayerTrainingCardDefinition>(playerTrainingDefinition.cardThreeDefinitionID);
			base.view.SetCardTitles(localizationService.GetString(playerTrainingCardDefinition.cardTitleLocalizedKey), localizationService.GetString(playerTrainingCardDefinition2.cardTitleLocalizedKey), localizationService.GetString(playerTrainingCardDefinition3.cardTitleLocalizedKey));
			base.view.SetCardDescriptions(localizationService.GetString(playerTrainingCardDefinition.cardDescriptionLocalizedKey), localizationService.GetString(playerTrainingCardDefinition2.cardDescriptionLocalizedKey), localizationService.GetString(playerTrainingCardDefinition3.cardDescriptionLocalizedKey));
			base.view.SetTransitionOne(DetermineTransitionMask((int)playerTrainingDefinition.transitionOne));
			base.view.SetTransitionTwo(DetermineTransitionMask((int)playerTrainingDefinition.transitionTwo));
			SetupCardImages(playerTrainingCardDefinition, playerTrainingCardDefinition2, playerTrainingCardDefinition3);
			base.view.animator.Play("Open");
		}

		private void SetupCardImages(global::Kampai.Game.PlayerTrainingCardDefinition cardOne, global::Kampai.Game.PlayerTrainingCardDefinition cardTwo, global::Kampai.Game.PlayerTrainingCardDefinition cardThree)
		{
			if (cardOne.prestigeDefinitionID == 0 && cardOne.buildingDefinitionID == 0)
			{
				base.view.SetCardOneImages(cardOne.cardImages);
			}
			if (cardTwo.prestigeDefinitionID == 0 && cardTwo.buildingDefinitionID == 0)
			{
				base.view.SetCardTwoImages(cardTwo.cardImages);
			}
			if (cardThree.prestigeDefinitionID == 0 && cardThree.buildingDefinitionID == 0)
			{
				base.view.SetCardThreeImages(cardThree.cardImages);
			}
			base.view.prestigeDefinitionIDs.Add(cardOne.prestigeDefinitionID);
			base.view.prestigeDefinitionIDs.Add(cardTwo.prestigeDefinitionID);
			base.view.prestigeDefinitionIDs.Add(cardThree.prestigeDefinitionID);
			base.view.buildingDefinitionIDs.Add(cardOne.buildingDefinitionID);
			base.view.buildingDefinitionIDs.Add(cardTwo.buildingDefinitionID);
			base.view.buildingDefinitionIDs.Add(cardThree.buildingDefinitionID);
		}

		private string DetermineTransitionMask(int type)
		{
			switch (type)
			{
			default:
				return string.Empty;
			case 1:
				return "icn_nav_arrow_transistion_mask";
			case 2:
				return "icn_nav_plus_mask";
			case 3:
				return "icn_nav_equals_mask";
			case 4:
				return "icn_nav_ampersand_mask";
			}
		}

		private void PlayAudio()
		{
			soundFXSignal.Dispatch("Play_training_popUp_01");
		}

		private void StartTimer()
		{
			uiModel.PopupAnimationIsPlaying = false;
			uiModel.DisableBack = false;
			startTime = timeService.CurrentTime();
		}

		private void ButtonClose()
		{
			Close();
		}

		protected override void Close()
		{
			base.view.animator.Play("Close");
			unlockMinionsSignal.Dispatch();
			soundFXSignal.Dispatch("Play_menu_disappear_01");
			if (callback != null)
			{
				callback.Dispatch(true);
			}
		}

		private void FinishClose()
		{
			toggleCharacterAudioSignal.Dispatch(true, null);
			base.view.RemoveCoroutine();
			uiModel.DisableBack = false;
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "popup_PlayerTraining");
			hideSignal.Dispatch("PlayerTrainingSkrim");
			float num = (float)timeService.CurrentTime() - startTime;
			int fromSettings = (openedFromSettingsMenu ? 1 : 0);
			telemetryService.Send_Telemetry_EVT_PLAYER_TRAINING(triggeredID, fromSettings, (int)num);
			if (openedFromSettingsMenu)
			{
				openedFromSettingsMenu = false;
				displaySettingsMenuSignal.Dispatch(true);
			}
			trainingClosedSignal.Dispatch();
		}
	}
}
