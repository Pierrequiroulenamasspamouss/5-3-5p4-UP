namespace Kampai.Game
{
	public class QuestCompleteCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.UI.View.UpdateQuestBookBadgeSignal updateBadgeSignal { get; set; }

		[Inject]
		public global::Kampai.Game.GetNewQuestSignal getNewQuestSignal { get; set; }

		[Inject]
		public global::Kampai.Game.UpdateQuestWorldIconsSignal updateQuestWorldIconsSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService prestigeService { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public ILocalPersistanceService localPersist { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService characterService { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayPlayerTrainingSignal displaySignal { get; set; }

		[Inject]
		public global::Kampai.Game.ReconcileSalesSignal reconcileSalesSignal { get; set; }

		[Inject]
		public global::Kampai.Game.SetupSpecialEventCharacterSignal setupSpecialEventCharacterSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ValidateCurrentTriggerSignal validateCurrentTriggerSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateUIButtonsSignal updateStoreButtonsSignal { get; set; }

		[Inject]
		public global::Kampai.Game.Quest quest { get; set; }

		public override void Execute()
		{
			if (quest.Definition.SurfaceType == global::Kampai.Game.QuestSurfaceType.Character || quest.Definition.SurfaceType == global::Kampai.Game.QuestSurfaceType.Automatic)
			{
				CheckIfQuestLineIsCompleted();
			}
			updateBadgeSignal.Dispatch();
			updateQuestWorldIconsSignal.Dispatch(quest);
			getNewQuestSignal.Dispatch();
			prestigeService.UpdateEligiblePrestigeList();
			playerService.IncreaseCompletedQuests();
			string questGiver = string.Empty;
			global::Kampai.Game.QuestDefinition activeDefinition = quest.GetActiveDefinition();
			if (activeDefinition.SurfaceType == global::Kampai.Game.QuestSurfaceType.Character)
			{
				global::Kampai.Game.Prestige prestige = characterService.GetPrestige(quest.GetActiveDefinition().SurfaceID, false);
				if (prestige != null)
				{
					questGiver = prestige.Definition.LocalizedKey;
				}
			}
			else if (activeDefinition.SurfaceType == global::Kampai.Game.QuestSurfaceType.ProcedurallyGenerated)
			{
				displaySignal.Dispatch(19000013, false, new global::strange.extensions.signal.impl.Signal<bool>());
			}
			displaySignal.Dispatch(activeDefinition.QuestCompletePlayerTrainingCategoryItemId, false, new global::strange.extensions.signal.impl.Signal<bool>());
			global::Kampai.Game.PackDefinition packDefinition = definitionService.Get<global::Kampai.Game.PackDefinition>(50001);
			if (quest.Definition.ID == packDefinition.UnlockQuestId)
			{
				reconcileSalesSignal.Dispatch(0);
			}
			CheckIfSpecialEventMinionIsUnlocked();
			if (activeDefinition.QuestVersion != -1)
			{
				SendTelemetry(questGiver);
			}
			validateCurrentTriggerSignal.Dispatch();
			CleanPersistance(activeDefinition);
			updateStoreButtonsSignal.Dispatch(false);
		}

		private void CleanPersistance(global::Kampai.Game.QuestDefinition questDef)
		{
			if (localPersist.HasKey(questDef.ID.ToString()))
			{
				localPersist.DeleteKey(questDef.ID.ToString());
			}
		}

		private void SendTelemetry(string questGiver)
		{
			global::Kampai.Game.QuestDefinition activeDefinition = quest.GetActiveDefinition();
			if (activeDefinition == null)
			{
				return;
			}
			global::Kampai.Game.Transaction.TransactionDefinition reward = activeDefinition.GetReward(definitionService);
			int xPOutputForTransaction = global::Kampai.Game.Transaction.TransactionUtil.GetXPOutputForTransaction(reward);
			string localizedKey = activeDefinition.LocalizedKey;
			telemetryService.Send_Telemetry_EVT_GP_ACHIEVEMENTS_CHECKPOINTS_EAL(questService.GetEventName(localizedKey), global::Kampai.Common.Service.Telemetry.TelemetryAchievementType.Quest, xPOutputForTransaction, questGiver);
			for (int i = 0; i < global::Kampai.Game.Transaction.TransactionDataExtension.GetOutputCount(reward); i++)
			{
				global::Kampai.Util.QuantityItem outputItem = global::Kampai.Game.Transaction.TransactionDataExtension.GetOutputItem(reward, i);
				global::Kampai.Game.BuildingDefinition definition;
				if (outputItem != null && definitionService.TryGet<global::Kampai.Game.BuildingDefinition>(outputItem.ID, out definition))
				{
					telemetryService.Send_Telemetry_EVT_USER_ACQUIRES_BUILDING(definition.TaxonomyType, definition, activeDefinition.ID);
				}
			}
		}

		private void CheckIfSpecialEventMinionIsUnlocked()
		{
			foreach (global::Kampai.Game.SpecialEventItemDefinition item in definitionService.GetAll<global::Kampai.Game.SpecialEventItemDefinition>())
			{
				if (quest.Definition.ID == item.UnlockQuestID && item.IsActive)
				{
					setupSpecialEventCharacterSignal.Dispatch(item.ID);
					break;
				}
			}
		}

		private void CheckIfQuestLineIsCompleted()
		{
			global::Kampai.Game.QuestDefinition activeDefinition = quest.GetActiveDefinition();
			bool flag = false;
			global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.QuestLine> questLines = questService.GetQuestLines();
			global::Kampai.Game.QuestLine questLine = questLines[activeDefinition.QuestLineID];
			global::System.Collections.Generic.IList<global::Kampai.Game.QuestDefinition> quests = questLine.Quests;
			if (quests[0].ID == activeDefinition.ID)
			{
				flag = true;
				questService.SetQuestLineState(activeDefinition.QuestLineID, global::Kampai.Game.QuestLineState.Finished);
			}
			global::Kampai.Game.Prestige prestige = prestigeService.GetPrestige(activeDefinition.SurfaceID);
			if (prestige == null)
			{
				prestige = prestigeService.GetPrestige(questLine.GivenByCharacterID);
				if (prestige == null)
				{
					return;
				}
			}
			if (prestige.Definition.PrestigeLevelSettings != null && flag && (prestige.state == global::Kampai.Game.PrestigeState.Questing || prestige.state == global::Kampai.Game.PrestigeState.TaskableWhileQuesting))
			{
				prestigeService.ChangeToPrestigeState(prestige, global::Kampai.Game.PrestigeState.Taskable);
			}
		}
	}
}
