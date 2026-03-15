namespace Kampai.Game
{
	public class TSMCharacterSelectedCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("TSMCharacterSelectedCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IPlayerService PlayerService { get; set; }

		[Inject]
		public global::Kampai.UI.View.PromptReceivedSignal promptReceivedSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ShowDialogSignal showDialog { get; set; }

		[Inject]
		public global::Kampai.Game.ITriggerService triggerService { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayTreasureTeaserSignal displayTreasureTeaserSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowProceduralQuestPanelSignal showProceduralQuestSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.UIModel uiModel { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.MinionParty minionPartyInstance = PlayerService.GetMinionPartyInstance();
			if (!minionPartyInstance.IsPartyHappening && !minionPartyInstance.IsPartyReady && !SetupTriggerDialog())
			{
				SetupDynamicQuest();
			}
		}

		private bool SetupTriggerDialog()
		{
			global::Kampai.Game.Trigger.TriggerInstance activeTrigger = triggerService.ActiveTrigger;
			if (activeTrigger == null)
			{
				return false;
			}
			ProcessTrigger(activeTrigger);
			return true;
		}

		private void SetupDynamicQuest()
		{
			global::Kampai.Game.TSMCharacter firstInstanceByDefinitionId = PlayerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.TSMCharacter>(70008);
			if (firstInstanceByDefinitionId != null && !firstInstanceByDefinitionId.HasShownIntroNarrative)
			{
				global::Kampai.Game.QuestDialogSetting type = new global::Kampai.Game.QuestDialogSetting();
				global::Kampai.Util.Tuple<int, int> type2 = new global::Kampai.Util.Tuple<int, int>(0, 0);
				promptReceivedSignal.AddOnce(DialogDismissed);
				showDialog.Dispatch("TSMNarrative", type, type2);
				firstInstanceByDefinitionId.HasShownIntroNarrative = true;
			}
			else
			{
				DialogDismissed(0, 0);
			}
		}

		private void DialogDismissed(int param1, int param2)
		{
			if (param1 == 0 && param2 == 0)
			{
				global::Kampai.Game.Quest firstInstanceByDefinitionId = PlayerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Quest>(77777);
				if (firstInstanceByDefinitionId == null)
				{
					logger.Warning("Ignoring tsm selection since there is no quest available, he's probably walking away.");
				}
				else if (firstInstanceByDefinitionId.GetActiveDefinition().SurfaceType == global::Kampai.Game.QuestSurfaceType.ProcedurallyGenerated)
				{
					showProceduralQuestSignal.Dispatch(firstInstanceByDefinitionId.ID);
				}
			}
		}

		private void ProcessTrigger(global::Kampai.Game.Trigger.TriggerInstance instance)
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.Trigger.TriggerRewardDefinition> rewards = instance.Definition.rewards;
			if (rewards != null && rewards.Count > 0 && rewards[0].type == global::Kampai.Game.Trigger.TriggerRewardType.Identifier.CaptainTease)
			{
				if (!uiModel.CaptainTeaserModalOpen)
				{
					uiModel.CaptainTeaserModalOpen = true;
					displayTreasureTeaserSignal.Dispatch(instance);
				}
			}
			else
			{
				OpenCaptainTriggerModal("popup_TSM_Gift_Upsell", instance);
			}
		}

		private void OpenCaptainTriggerModal(string prefab, global::Kampai.Game.Trigger.TriggerInstance instance)
		{
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Queue, prefab);
			iGUICommand.skrimScreen = "ProceduralTaskSkrim";
			iGUICommand.darkSkrim = true;
			iGUICommand.Args.Add(typeof(global::Kampai.Game.Trigger.TriggerInstance), instance);
			guiService.Execute(iGUICommand);
		}
	}
}
