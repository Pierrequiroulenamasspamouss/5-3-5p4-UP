namespace Kampai.Game
{
	public class GetNewQuestCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("GetNewQuestCommand") as global::Kampai.Util.IKampaiLogger;

		private global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.QuestLine> questLines;

		private global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.IQuestController> questMap;

		private global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<int>> questUnlockTree;

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService characterService { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		public override void Execute()
		{
			logger.Debug("Get New Quest Command");
			questLines = questService.GetQuestLines();
			questMap = questService.GetQuestMap();
			questUnlockTree = questService.GetQuestUnlockTree();
			foreach (global::Kampai.Game.QuestLine value in questLines.Values)
			{
				if (value.Quests != null && value.Quests.Count != 0 && value.state != global::Kampai.Game.QuestLineState.Finished)
				{
					global::Kampai.Game.MasterPlanQuestType.PlanDefinition planDefinition = value as global::Kampai.Game.MasterPlanQuestType.PlanDefinition;
					if (planDefinition != null)
					{
						ExamineQuestLine(planDefinition);
					}
					else
					{
						ExamineQuestLine(value);
					}
				}
			}
		}

		private void ExamineQuestLine(global::Kampai.Game.MasterPlanQuestType.PlanDefinition planQuestLine)
		{
			if (planQuestLine == null || planQuestLine.state == global::Kampai.Game.QuestLineState.NotStarted)
			{
				return;
			}
			int questLineID = planQuestLine.QuestLineID;
			if (planQuestLine.components != null)
			{
				if (!questService.ContainsQuest(questLineID))
				{
					questService.AddMasterPlanQuest(planQuestLine.plan);
				}
				return;
			}
			if (!questService.ContainsQuest(questLineID))
			{
				bool isBuildQuest = planQuestLine.component.State > global::Kampai.Game.MasterPlanComponentState.TasksCollected;
				questService.AddQuest(planQuestLine.component, isBuildQuest);
			}
			global::Kampai.Game.IQuestController questControllerByInstanceID = questService.GetQuestControllerByInstanceID(questLineID);
			if (questControllerByInstanceID != null)
			{
				if (questControllerByInstanceID.State != global::Kampai.Game.QuestState.Complete)
				{
					questControllerByInstanceID.SetUpTracking();
				}
				else
				{
					questService.AddQuest(planQuestLine.component, true);
				}
			}
		}

		private void ExamineQuestLine(global::Kampai.Game.QuestLine questLine)
		{
			for (int i = 0; i < questLine.Quests.Count; i++)
			{
				global::Kampai.Game.QuestDefinition questDefinition = questLine.Quests[i];
				int iD = questDefinition.ID;
				if (UnlockValidation(questLine, questDefinition, i))
				{
					logger.Info("Unlocked Quest {0}", iD);
					global::Kampai.Game.Quest quest = new global::Kampai.Game.Quest(questDefinition);
					quest.Initialize();
					if (!QuestValidation(quest))
					{
						continue;
					}
					global::Kampai.Game.IQuestController questController = questService.AddQuest(quest);
					logger.Debug("Unlocking New Quests... Quest Def ID: {0} Quest Surface Type: {1}", questController.Definition.ID, questController.Definition.SurfaceType.ToString());
					if (questDefinition.QuestVersion != -1)
					{
						if (questDefinition.SurfaceType == global::Kampai.Game.QuestSurfaceType.Automatic || questDefinition.SurfaceType == global::Kampai.Game.QuestSurfaceType.Bridge)
						{
							questController.ProcessAutomaticQuest();
						}
						else
						{
							questController.GoToQuestState(global::Kampai.Game.QuestState.Notstarted);
						}
					}
					else
					{
						questController.GoToQuestState(global::Kampai.Game.QuestState.Complete);
					}
					TryDeleteOldQuest(questLine, questDefinition, i);
					if (quest.state != global::Kampai.Game.QuestState.Complete)
					{
						routineRunner.StartCoroutine(WaitAFrame(questController));
					}
					break;
				}
				global::Kampai.Game.IQuestController value;
				questMap.TryGetValue(iD, out value);
				if (value != null)
				{
					value.SetUpTracking();
				}
				if (questMap.ContainsKey(iD))
				{
					break;
				}
			}
		}

		private void TryDeleteOldQuest(global::Kampai.Game.QuestLine questLine, global::Kampai.Game.QuestDefinition qd, int index)
		{
			if (index < questLine.Quests.Count - 1)
			{
				int iD = questLine.Quests[index + 1].ID;
				if (questMap.ContainsKey(iD) && QuestDeleteValidation(iD))
				{
					logger.Debug("Removing Quests... Quest Def ID: {0}", iD);
					questService.RemoveQuest(questMap[iD]);
				}
			}
			int unlockQuestId = qd.UnlockQuestId;
			if (unlockQuestId != 0 && questMap.ContainsKey(unlockQuestId) && QuestDeleteValidation(unlockQuestId))
			{
				global::Kampai.Game.QuestDefinition definition = questMap[unlockQuestId].Definition;
				if (questLine.unlockByQuestLine == definition.QuestLineID)
				{
					logger.Debug("Removing Dependency Quests... Quest Def ID: {0}", unlockQuestId);
					questService.RemoveQuest(questMap[unlockQuestId]);
				}
			}
		}

		private bool UnlockValidation(global::Kampai.Game.QuestLine questLine, global::Kampai.Game.QuestDefinition questDefinition, int indexInQuestLine)
		{
			int iD = questDefinition.ID;
			if (questMap.ContainsKey(iD))
			{
				return false;
			}
			if (playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID) < questDefinition.UnlockLevel)
			{
				return false;
			}
			if (questDefinition.UnlockQuestId != 0 && !questService.IsQuestCompleted(questDefinition.UnlockQuestId))
			{
				return false;
			}
			if (indexInQuestLine < questLine.Quests.Count - 1)
			{
				int iD2 = questLine.Quests[indexInQuestLine + 1].ID;
				if (questMap.ContainsKey(iD2) && questMap[iD2].State != global::Kampai.Game.QuestState.Complete)
				{
					return false;
				}
				if (!questMap.ContainsKey(iD2))
				{
					return false;
				}
			}
			if (questUnlockTree.ContainsKey(iD))
			{
				foreach (int item in questUnlockTree[iD])
				{
					if (questMap.ContainsKey(item) || questService.IsQuestCompleted(item))
					{
						if (indexInQuestLine == questLine.Quests.Count - 1)
						{
							questLine.state = global::Kampai.Game.QuestLineState.Finished;
						}
						return false;
					}
				}
			}
			if (questDefinition.SurfaceType == global::Kampai.Game.QuestSurfaceType.Character || (questDefinition.SurfaceType == global::Kampai.Game.QuestSurfaceType.Automatic && questDefinition.SurfaceID > 0))
			{
				global::Kampai.Game.Prestige prestige = characterService.GetPrestige(questDefinition.SurfaceID);
				if (prestige == null || (prestige.state != global::Kampai.Game.PrestigeState.Questing && prestige.state != global::Kampai.Game.PrestigeState.TaskableWhileQuesting))
				{
					return false;
				}
				if (questLine.GivenByCharacterID != 0)
				{
					global::Kampai.Game.Prestige prestige2 = characterService.GetPrestige(questLine.GivenByCharacterID);
					if (prestige2 == null || (prestige2.state != global::Kampai.Game.PrestigeState.Questing && prestige2.state != global::Kampai.Game.PrestigeState.TaskableWhileQuesting) || prestige2.CurrentPrestigeLevel < questLine.GivenByCharacterPrestigeLevel)
					{
						return false;
					}
					if (prestige2.state == global::Kampai.Game.PrestigeState.Questing)
					{
						bool flag = true;
						int iD3 = prestige2.Definition.ID;
						if (prestige2.Definition.Type == global::Kampai.Game.PrestigeType.Villain || (prestige2.CurrentPrestigeLevel > 0 && (iD3 == 40003 || iD3 == 40004)))
						{
							flag = false;
						}
						global::Kampai.Game.View.BuildingManagerView component = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
						global::Kampai.Game.View.TikiBarBuildingObjectView tikiBarBuildingObjectView = component.GetBuildingObject(313) as global::Kampai.Game.View.TikiBarBuildingObjectView;
						if (tikiBarBuildingObjectView != null && !tikiBarBuildingObjectView.ContainsCharacter(prestige2.trackedInstanceId) && flag)
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		private bool QuestDeleteValidation(int targetQuestDeleteDefID)
		{
			if (questUnlockTree.ContainsKey(targetQuestDeleteDefID))
			{
				foreach (int item in questUnlockTree[targetQuestDeleteDefID])
				{
					if (!questMap.ContainsKey(item) && !IsQuestDeleted(targetQuestDeleteDefID))
					{
						return false;
					}
				}
				return true;
			}
			return true;
		}

		private bool IsQuestDeleted(int questDefID)
		{
			if (questUnlockTree.ContainsKey(questDefID))
			{
				foreach (int item in questUnlockTree[questDefID])
				{
					if (!questMap.ContainsKey(item) && !IsQuestDeleted(item))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		private bool QuestValidation(global::Kampai.Game.Quest quest)
		{
			if (quest.GetActiveDefinition().SurfaceType == global::Kampai.Game.QuestSurfaceType.LimitedEvent)
			{
				global::Kampai.Game.LimitedQuestDefinition limitedQuestDefinition = quest.GetActiveDefinition() as global::Kampai.Game.LimitedQuestDefinition;
				if (limitedQuestDefinition == null)
				{
					return false;
				}
				if (limitedQuestDefinition.ServerStartTimeUTC > timeService.CurrentTime() || limitedQuestDefinition.ServerStopTimeUTC < timeService.CurrentTime())
				{
					return false;
				}
			}
			return true;
		}

		private global::System.Collections.IEnumerator WaitAFrame(global::Kampai.Game.IQuestController questController)
		{
			yield return null;
			questController.SetUpTracking();
		}
	}
}
