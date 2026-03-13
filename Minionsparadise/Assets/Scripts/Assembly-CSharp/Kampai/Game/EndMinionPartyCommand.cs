namespace Kampai.Game
{
	public class EndMinionPartyCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("EndMinionPartyCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.Game.PostMinionPartyEndSignal postMinionPartyEndSignal { get; set; }

		[Inject]
		public global::Kampai.Game.RestoreTaskingMinionsFromPartySignal restoreTaskingMinionFromPartySignal { get; set; }

		[Inject]
		public global::Kampai.Game.RestoreLeisureMinionsFromPartySignal restoreLeisureMinionsFromPartySignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowHUDSignal showHudSignal { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable uiContext { get; set; }

		[Inject]
		public global::Kampai.Game.CameraMoveToCustomPositionSignal customCameraPositionSignal { get; set; }

		[Inject]
		public global::Kampai.Util.IPartyFavorAnimationService partyFavorService { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowStoreSignal showStoreSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalMusicSignal musicSignal { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Game.IPartyService partyService { get; set; }

		[Inject]
		public global::Kampai.Game.StartPartyBuffTimerSignal startPartyBuff { get; set; }

		[Inject]
		public global::Kampai.Game.UnlockCharacterModel characterModel { get; set; }

		[Inject]
		public global::Kampai.Game.ShowDialogSignal showDialogSignal { get; set; }

		[Inject]
		public global::Kampai.Game.EndTownhallMinionPartyAnimationSignal endTownhallMinionPartyAnimationSignal { get; set; }

		[Inject]
		public global::Kampai.Game.RemoveCharacterFromPartyStageSignal removeStageCharacters { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService prestigeService { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowAllWayFindersSignal showAllWayfindersSignal { get; set; }

		[Inject]
		public global::Kampai.Game.UpdateAdHUDSignal updateAdHUDSignal { get; set; }

		[Inject]
		public bool isSkipping { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		public override void Execute()
		{
			logger.Debug("Minion Party Sequence Ending");
			startPartyBuff.Dispatch();
			if (!characterModel.stuartFirstTimeHonor && characterModel.characterUnlocks.Count == 0 && characterModel.minionUnlocks.Count == 0)
			{
				foreach (global::Kampai.Game.QuestDialogSetting item in characterModel.dialogQueue)
				{
					showDialogSignal.Dispatch("AlertPrePrestige", item, new global::Kampai.Util.Tuple<int, int>(0, 0));
				}
				characterModel.dialogQueue.Clear();
			}
			if (partyService.IsInspiredParty)
			{
				uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.ShouldRateAppSignal>().Dispatch(global::Kampai.Game.ConfigurationDefinition.RateAppAfterEvent.LevelUp);
			}
			if (isSkipping)
			{
				telemetryService.Send_Telemetry_EVT_PARTY_SKIPPED();
			}
			ShitThatNeedsToBeDoneOnPartyEnd();
		}

		private void ShitThatNeedsToBeDoneOnPartyEnd()
		{
			restoreTaskingMinionFromPartySignal.Dispatch();
			restoreLeisureMinionsFromPartySignal.Dispatch();
			partyFavorService.RemoveAllPartyFavorAnimations();
			showAllWayfindersSignal.Dispatch();
			endTownhallMinionPartyAnimationSignal.Dispatch();
			removeStageCharacters.Dispatch();
			showHudSignal.Dispatch(true);
			global::Kampai.UI.View.UIModel instance = uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.UIModel>();
			if (!instance.LevelUpUIOpen)
			{
				uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.ShowAllWayFindersSignal>().Dispatch();
			}
			showStoreSignal.Dispatch(true);
			questService.UpdateAllQuestsWithQuestStepType(global::Kampai.Game.QuestStepType.ThrowParty, global::Kampai.Game.QuestTaskTransition.Complete);
			customCameraPositionSignal.Dispatch(60006, new global::Kampai.Util.Boxed<global::System.Action>(OnFinalCameraPanComplete));
			postMinionPartyEndSignal.Dispatch();
			global::System.Collections.Generic.Dictionary<string, float> dictionary = new global::System.Collections.Generic.Dictionary<string, float>();
			dictionary.Add("endParty", 1f);
			global::System.Collections.Generic.Dictionary<string, float> type = dictionary;
			musicSignal.Dispatch("Play_partyMeterMusic_01", type);
			routineRunner.StartCoroutine(WaitAndResumeBGM());
		}

		private void OnFinalCameraPanComplete()
		{
			logger.Debug("Setting MinionParty.IsPartyHappening to false and releasing block.");
			global::Kampai.Game.MinionParty minionPartyInstance = playerService.GetMinionPartyInstance();
			minionPartyInstance.IsPartyHappening = false;
			prestigeService.UpdateEligiblePrestigeList();
			updateAdHUDSignal.Dispatch();
		}

		private global::System.Collections.IEnumerator WaitAndResumeBGM()
		{
			yield return new global::UnityEngine.WaitForSeconds(5.7f);
			global::System.Collections.Generic.Dictionary<string, float> parameters = new global::System.Collections.Generic.Dictionary<string, float>();
			musicSignal.Dispatch("Play_backGroundMusic_01", parameters);
		}
	}
}
