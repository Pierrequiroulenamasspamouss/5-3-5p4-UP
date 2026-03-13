namespace Kampai.Game
{
	public class QuestService : global::Kampai.Game.IQuestService
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("QuestService") as global::Kampai.Util.IKampaiLogger;

		private global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.QuestLine> questLines = new global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.QuestLine>();

		private global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<int>> questUnlockTree = new global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<int>>();

		private global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.IQuestController> quests = new global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.IQuestController>();

		private bool isInitialized;

		private bool isMinionPartyUnlocked;

		private bool pulseMoveBuildingAccept;

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService prestigeService { get; set; }

		[Inject]
		public global::Kampai.Game.QuestTimeoutSignal timeoutSignal { get; set; }

		[Inject]
		public global::Kampai.Game.UpdateQuestWorldIconsSignal updateWorldIconSignal { get; set; }

		[Inject]
		public global::Kampai.Game.TimedQuestNotificationSignal questNoteSignal { get; set; }

		[Inject]
		public global::Kampai.Game.StartMinionPartyUnlockSequenceSignal startPartyUnlockSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestScriptService questScriptService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject(global::Kampai.Main.LocalizationServices.EVENT)]
		public global::Kampai.Main.ILocalizationService eventsLocalService { get; set; }

		[Inject]
		public global::Kampai.Game.Environment environment { get; set; }

		[Inject]
		public global::Kampai.Game.IMasterPlanQuestService masterPlanQuestService { get; set; }

		public void Initialize()
		{
			LoadQuestLines();
			CreateQuestMap();
			CreateQuestUnlockTree();
			UpdateQuestLineStateBasedOnDependency();
			global::System.Collections.Generic.ICollection<global::Kampai.Game.Quest> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.Quest>();
			foreach (global::Kampai.Game.Quest item in instancesByType)
			{
				CheckQuestVersion(item);
				global::Kampai.Game.QuestState state = item.state;
				if (state != global::Kampai.Game.QuestState.Complete)
				{
					updateWorldIconSignal.Dispatch(item);
				}
				CheckAndStartQuestTimers(item);
				switch (state)
				{
				case global::Kampai.Game.QuestState.RunningStartScript:
					questScriptService.StartQuestScript(item, true);
					break;
				case global::Kampai.Game.QuestState.RunningCompleteScript:
					questScriptService.StartQuestScript(item, false);
					break;
				case global::Kampai.Game.QuestState.RunningTasks:
				{
					for (int i = 0; i < item.Steps.Count; i++)
					{
						global::Kampai.Game.QuestStepState state2 = item.Steps[i].state;
						if (state2 == global::Kampai.Game.QuestStepState.RunningStartScript || state2 == global::Kampai.Game.QuestStepState.RunningCompleteScript)
						{
							questScriptService.StartQuestScript(item, state2 == global::Kampai.Game.QuestStepState.RunningStartScript, false, i, true);
						}
					}
					break;
				}
				}
			}
			UpdateAllQuestsWithQuestStepType(global::Kampai.Game.QuestStepType.Construction);
			UpdateAllQuestsWithQuestStepType(global::Kampai.Game.QuestStepType.Harvest);
			UpdateMasterPlanQuestLine();
			isMinionPartyUnlocked = playerService.IsMinionPartyUnlocked();
			isInitialized = true;
		}

		public global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.QuestLine> GetQuestLines()
		{
			return questLines;
		}

		public global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.IQuestController> GetQuestMap()
		{
			return quests;
		}

		public global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<int>> GetQuestUnlockTree()
		{
			return questUnlockTree;
		}

		public global::Kampai.Game.IQuestController GetQuestControllerByDefinitionID(int questDefinitionId)
		{
			if (!quests.ContainsKey(questDefinitionId))
			{
				logger.Info("QuestService: Quest {0} doesn't exist in the quest map.", questDefinitionId);
				return null;
			}
			return quests[questDefinitionId];
		}

		public global::Kampai.Game.Quest GetQuestByInstanceId(int id)
		{
			global::Kampai.Game.IQuestController questControllerByInstanceID = GetQuestControllerByInstanceID(id);
			return (questControllerByInstanceID != null) ? questControllerByInstanceID.Quest : null;
		}

		public global::Kampai.Game.IQuestController GetQuestControllerByInstanceID(int questInstanceId)
		{
			global::Kampai.Game.Quest byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Quest>(questInstanceId);
			if (byInstanceId == null)
			{
				if (quests.ContainsKey(questInstanceId))
				{
					return quests[questInstanceId];
				}
				logger.Error("quest doesn't exist for quest instance {0}", questInstanceId);
				return null;
			}
			int iD = byInstanceId.GetActiveDefinition().ID;
			return GetQuestControllerByDefinitionID(iD);
		}

		public bool ContainsQuest(int questInstanceId)
		{
			global::Kampai.Game.Quest byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Quest>(questInstanceId);
			if (byInstanceId == null)
			{
				return quests.ContainsKey(questInstanceId);
			}
			int iD = byInstanceId.GetActiveDefinition().ID;
			return quests.ContainsKey(iD);
		}

		public global::Kampai.Game.IQuestStepController GetQuestStepController(int questDefinitionID, int questStepIndex)
		{
			if (!quests.ContainsKey(questDefinitionID))
			{
				logger.Info("QuestService: Quest {0} doesn't exist in the quest map.", questDefinitionID);
				return null;
			}
			return quests[questDefinitionID].GetStepController(questStepIndex);
		}

		public bool HasActiveQuest(int surfaceId)
		{
			foreach (global::Kampai.Game.IQuestController value in quests.Values)
			{
				if (value.Definition.SurfaceID == surfaceId && value.State != global::Kampai.Game.QuestState.Complete)
				{
					return true;
				}
			}
			return false;
		}

		public global::Kampai.Game.IQuestController AddQuest(global::Kampai.Game.MasterPlanComponent componentQuest, bool isBuildQuest)
		{
			if (isBuildQuest)
			{
				int key = 711;
				if (quests.ContainsKey(key))
				{
					quests.Remove(key);
				}
				global::Kampai.Game.IQuestController questController = new global::Kampai.Game.MasterPlanQuestController(componentQuest, logger, playerService, definitionService, gameContext, masterPlanQuestService, true);
				quests.Add(key, questController);
				return questController;
			}
			int iD = componentQuest.ID;
			if (quests.ContainsKey(iD))
			{
				quests.Remove(iD);
			}
			global::Kampai.Game.IQuestController questController2 = new global::Kampai.Game.MasterPlanQuestController(componentQuest, logger, playerService, definitionService, gameContext, masterPlanQuestService, false);
			quests.Add(iD, questController2);
			return questController2;
		}

		public global::Kampai.Game.IQuestController AddMasterPlanQuest(global::Kampai.Game.MasterPlan masterPlanQuest)
		{
			if (masterPlanQuest == null)
			{
				return null;
			}
			int iD = masterPlanQuest.ID;
			if (quests.ContainsKey(iD))
			{
				quests.Remove(iD);
			}
			global::Kampai.Game.IQuestController questController = new global::Kampai.Game.MasterPlanQuestController(masterPlanQuest, logger, playerService, definitionService, gameContext, masterPlanQuestService);
			quests.Add(iD, questController);
			return questController;
		}

		public global::Kampai.Game.IQuestController AddQuest(global::Kampai.Game.Quest quest)
		{
			int iD = quest.GetActiveDefinition().ID;
			if (quests.ContainsKey(iD))
			{
				logger.Error("QuestService: Quest {0} already added.", iD);
				return quests[iD];
			}
			global::Kampai.Game.IQuestController questController = new global::Kampai.Game.QuestController(quest, logger, playerService, prestigeService, definitionService, questScriptService, gameContext, environment);
			quests.Add(iD, questController);
			playerService.Add(quest);
			SetQuestLineState(quest.GetActiveDefinition().QuestLineID, global::Kampai.Game.QuestLineState.Started);
			CheckAndStartQuestTimers(quest);
			return questController;
		}

		public int GetLongestIdleQuestDuration()
		{
			global::Kampai.Game.IQuestController longestIdleQuestController = GetLongestIdleQuestController();
			if (longestIdleQuestController == null)
			{
				return 0;
			}
			return longestIdleQuestController.GetIdleTime();
		}

		public global::Kampai.Game.IQuestController GetLongestIdleQuestController()
		{
			int num = 0;
			global::Kampai.Game.IQuestController result = null;
			foreach (global::System.Collections.Generic.KeyValuePair<int, global::Kampai.Game.IQuestController> quest in quests)
			{
				global::Kampai.Game.IQuestController value = quest.Value;
				if (value.State == global::Kampai.Game.QuestState.RunningTasks)
				{
					int idleTime = value.GetIdleTime();
					if (idleTime > num)
					{
						result = value;
						num = idleTime;
					}
				}
			}
			return result;
		}

		public int GetIdleQuestDuration(int questDefinitionID)
		{
			if (quests.ContainsKey(questDefinitionID))
			{
				return quests[questDefinitionID].GetIdleTime();
			}
			return 0;
		}

		public void RemoveQuest(global::Kampai.Game.IQuestController questController)
		{
			global::Kampai.Game.QuestDefinition definition = questController.Definition;
			int iD = definition.ID;
			if (!quests.ContainsKey(iD))
			{
				logger.Info("QuestService: Quest {0} has already been removed.", iD);
				return;
			}
			if (definition.SurfaceType == global::Kampai.Game.QuestSurfaceType.LimitedEvent || definition.SurfaceType == global::Kampai.Game.QuestSurfaceType.TimedEvent)
			{
				timeEventService.RemoveEvent(questController.ID);
			}
			questController.DeleteQuest();
			quests.Remove(iD);
			questController = null;
		}

		public void RemoveQuest(int questDefinitionID)
		{
			if (!quests.ContainsKey(questDefinitionID))
			{
				logger.Info("QuestService: Quest {0} has already been removed.", questDefinitionID);
			}
			else
			{
				RemoveQuest(quests[questDefinitionID]);
			}
		}

		public void SetQuestLineState(int questLineId, global::Kampai.Game.QuestLineState targetState)
		{
			if (questLines.ContainsKey(questLineId))
			{
				global::Kampai.Game.QuestLine questLine = questLines[questLineId];
				if (questLine.state == global::Kampai.Game.QuestLineState.NotStarted)
				{
					questLine.state = targetState;
				}
				else if (questLine.state == global::Kampai.Game.QuestLineState.Started && targetState == global::Kampai.Game.QuestLineState.Finished)
				{
					questLine.state = targetState;
				}
			}
		}

		public int IsOneOffCraftableDisplayable(int questDefinitionId, int trackedItemDefinitionID)
		{
			if (questDefinitionId == 0)
			{
				return 0;
			}
			if (!quests.ContainsKey(questDefinitionId))
			{
				return 0;
			}
			global::Kampai.Game.IQuestController questController = quests[questDefinitionId];
			if (questController.Definition.QuestVersion == -1)
			{
				return 0;
			}
			return questController.IsTrackingOneOffCraftable(trackedItemDefinitionID);
		}

		public bool IsQuestCompleted(int questDefinitionID)
		{
			if (!isInitialized)
			{
				return false;
			}
			if (questDefinitionID == 0)
			{
				return true;
			}
			if (quests.ContainsKey(questDefinitionID))
			{
				return quests[questDefinitionID].State == global::Kampai.Game.QuestState.Complete;
			}
			global::Kampai.Game.QuestDefinition questDefinition = definitionService.Get<global::Kampai.Game.QuestDefinition>(questDefinitionID);
			int unlockQuestId = questDefinition.UnlockQuestId;
			uint quantity = playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID);
			if (quantity < questDefinition.UnlockLevel || (unlockQuestId != 0 && quests.ContainsKey(questDefinition.UnlockQuestId) && quests[questDefinition.UnlockQuestId].State != global::Kampai.Game.QuestState.Complete))
			{
				return false;
			}
			return IsQuestAlreadyFinished(questDefinitionID);
		}

		private void CheckQuestVersion(global::Kampai.Game.Quest quest)
		{
			if (quest.GetActiveDefinition().QuestVersion == -1 && !quest.state.Equals(global::Kampai.Game.QuestState.Complete) && !quest.state.Equals(global::Kampai.Game.QuestState.Notstarted))
			{
				global::Kampai.Game.IQuestController questController = quests[quest.GetActiveDefinition().ID];
				questController.GoToQuestState(global::Kampai.Game.QuestState.Complete);
			}
			else if (quest.QuestVersion < quest.GetActiveDefinition().QuestVersion)
			{
				ReconstructQuest(quest);
			}
		}

		private void ReconstructQuest(global::Kampai.Game.Quest quest)
		{
			global::Kampai.Game.QuestState state = quest.state;
			quest.Clear();
			quest.state = state;
			int iD = quest.GetActiveDefinition().ID;
			quests.Remove(iD);
			global::Kampai.Game.IQuestController questController = new global::Kampai.Game.QuestController(quest, logger, playerService, prestigeService, definitionService, questScriptService, gameContext, environment);
			quests.Add(iD, questController);
			questController.SetUpTracking();
		}

		private bool IsQuestAlreadyFinished(int questDefinitionId)
		{
			global::Kampai.Game.QuestDefinition questDefinition = definitionService.Get<global::Kampai.Game.QuestDefinition>(questDefinitionId);
			if (questLines.ContainsKey(questDefinition.QuestLineID))
			{
				global::Kampai.Game.QuestLine questLine = questLines[questDefinition.QuestLineID];
				if (questLine.state == global::Kampai.Game.QuestLineState.NotStarted)
				{
					return false;
				}
				if (questLine.state == global::Kampai.Game.QuestLineState.Finished)
				{
					return true;
				}
				if (questLine.state == global::Kampai.Game.QuestLineState.Started)
				{
					foreach (global::Kampai.Game.QuestDefinition quest in questLine.Quests)
					{
						if (quests.ContainsKey(quest.ID))
						{
							if (quest.NarrativeOrder >= questDefinition.NarrativeOrder)
							{
								return true;
							}
							return false;
						}
					}
					return false;
				}
			}
			if (!questUnlockTree.ContainsKey(questDefinitionId))
			{
				return true;
			}
			foreach (int item in questUnlockTree[questDefinitionId])
			{
				if (quests.ContainsKey(item))
				{
					return true;
				}
			}
			foreach (int item2 in questUnlockTree[questDefinitionId])
			{
				if (IsQuestAlreadyFinished(item2))
				{
					return true;
				}
			}
			return false;
		}

		public void UnlockMinionParty(int QuestDefinitionID)
		{
			if (!isMinionPartyUnlocked)
			{
				global::Kampai.Game.MinionPartyDefinition minionPartyDefinition = definitionService.Get<global::Kampai.Game.MinionPartyDefinition>(80000);
				if (minionPartyDefinition != null && QuestDefinitionID == minionPartyDefinition.UnlockQuestID)
				{
					startPartyUnlockSignal.Dispatch();
					isMinionPartyUnlocked = true;
				}
			}
		}

		public void RushQuestStep(int questId, int step)
		{
			global::Kampai.Game.IQuestController questController = quests[questId];
			questController.RushQuestStep(step);
		}

		public bool IsBridgeQuestComplete(int bridgeDefId)
		{
			foreach (global::System.Collections.Generic.KeyValuePair<int, global::Kampai.Game.IQuestController> quest in quests)
			{
				global::Kampai.Game.IQuestController value = quest.Value;
				if (value.State != global::Kampai.Game.QuestState.Complete)
				{
					continue;
				}
				global::Kampai.Game.QuestDefinition definition = value.Definition;
				if (definition.SurfaceType != global::Kampai.Game.QuestSurfaceType.Bridge)
				{
					continue;
				}
				global::System.Collections.Generic.IList<global::Kampai.Game.QuestStepDefinition> questSteps = definition.QuestSteps;
				for (int i = 0; i < questSteps.Count; i++)
				{
					global::Kampai.Game.QuestStepDefinition questStepDefinition = questSteps[i];
					if (questStepDefinition.Type == global::Kampai.Game.QuestStepType.BridgeRepair && questStepDefinition.ItemDefinitionID == bridgeDefId)
					{
						return IsQuestCompleted(definition.ID);
					}
				}
			}
			return false;
		}

		public void UpdateAllQuestsWithQuestStepType(global::Kampai.Game.QuestStepType type, global::Kampai.Game.QuestTaskTransition questTaskTransition = global::Kampai.Game.QuestTaskTransition.Start, global::Kampai.Game.Building building = null, int buildingDefId = 0, int item = 0)
		{
			foreach (global::System.Collections.Generic.KeyValuePair<int, global::Kampai.Game.IQuestController> quest in quests)
			{
				global::Kampai.Game.IQuestController value = quest.Value;
				if (value.State == global::Kampai.Game.QuestState.RunningTasks)
				{
					value.UpdateTask(type, questTaskTransition, building, buildingDefId, item);
				}
			}
		}

		private void CheckAndStartQuestTimers(global::Kampai.Game.Quest quest)
		{
			if (quest.GetActiveDefinition().SurfaceType == global::Kampai.Game.QuestSurfaceType.LimitedEvent)
			{
				global::Kampai.Game.LimitedQuestDefinition limitedQuestDefinition = quest.GetActiveDefinition() as global::Kampai.Game.LimitedQuestDefinition;
				if (limitedQuestDefinition != null)
				{
					if (limitedQuestDefinition.ServerStopTimeUTC <= timeService.CurrentTime())
					{
						RemoveQuest(quest.GetActiveDefinition().ID);
						return;
					}
					int eventTime = limitedQuestDefinition.ServerStopTimeUTC - limitedQuestDefinition.ServerStartTimeUTC;
					timeEventService.AddEvent(quest.ID, limitedQuestDefinition.ServerStartTimeUTC, eventTime, timeoutSignal);
				}
			}
			else
			{
				if (quest.GetActiveDefinition().SurfaceType != global::Kampai.Game.QuestSurfaceType.TimedEvent || quest.state == global::Kampai.Game.QuestState.Notstarted || quest.state == global::Kampai.Game.QuestState.Complete)
				{
					return;
				}
				global::Kampai.Game.TimedQuestDefinition timedQuestDefinition = quest.GetActiveDefinition() as global::Kampai.Game.TimedQuestDefinition;
				if (timedQuestDefinition != null)
				{
					if (quest.UTCQuestStartTime == 0)
					{
						logger.Log(global::Kampai.Util.KampaiLogLevel.Error, "The UTCQuestStartTime is not set for the timed quest!");
						return;
					}
					if (timeService.CurrentTime() > timedQuestDefinition.Duration + quest.UTCQuestStartTime)
					{
						RemoveQuest(quest.GetActiveDefinition().ID);
						return;
					}
					questNoteSignal.Dispatch(quest.ID);
					timeEventService.AddEvent(quest.ID, quest.UTCQuestStartTime, timedQuestDefinition.Duration, timeoutSignal);
				}
			}
		}

		private void CreateQuestMap()
		{
			global::System.Collections.Generic.ICollection<global::Kampai.Game.Quest> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.Quest>();
			foreach (global::Kampai.Game.Quest item in instancesByType)
			{
				global::Kampai.Game.QuestDefinition activeDefinition = item.GetActiveDefinition();
				int iD = activeDefinition.ID;
				if (quests.ContainsKey(iD))
				{
					continue;
				}
				logger.Info("Restoring quest def id:{0} from player data", iD);
				global::Kampai.Game.IQuestController value = new global::Kampai.Game.QuestController(item, logger, playerService, prestigeService, definitionService, questScriptService, gameContext, environment);
				quests.Add(iD, value);
				if (!item.IsDynamic())
				{
					global::Kampai.Game.QuestLine questLine = questLines[activeDefinition.QuestLineID];
					if (questLine.state == global::Kampai.Game.QuestLineState.NotStarted)
					{
						questLine.state = global::Kampai.Game.QuestLineState.Started;
					}
					if (questLine.Quests.Count == activeDefinition.NarrativeOrder + 1 && item.state == global::Kampai.Game.QuestState.Complete)
					{
						questLine.state = global::Kampai.Game.QuestLineState.Finished;
					}
				}
			}
		}

		private void LoadQuestLines()
		{
			global::System.Collections.Generic.Dictionary<int, int> dictionary = new global::System.Collections.Generic.Dictionary<int, int>();
			global::System.Collections.Generic.IList<global::Kampai.Game.QuestDefinition> all = definitionService.GetAll<global::Kampai.Game.QuestDefinition>();
			foreach (global::Kampai.Game.QuestDefinition item in all)
			{
				if (item.SurfaceID < 0 || item.SurfaceType < global::Kampai.Game.QuestSurfaceType.Building)
				{
					continue;
				}
				dictionary.Add(item.ID, item.QuestLineID);
				if (questLines.ContainsKey(item.QuestLineID))
				{
					int num = 0;
					bool flag = false;
					global::System.Collections.Generic.IList<global::Kampai.Game.QuestDefinition> list = questLines[item.QuestLineID].Quests;
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].NarrativeOrder < item.NarrativeOrder)
						{
							list.Insert(i, item);
							flag = true;
							break;
						}
						if (list[i].NarrativeOrder == item.NarrativeOrder)
						{
							if (item.ID >= list[i].ID)
							{
								list.Insert(i, item);
								flag = true;
								break;
							}
							num = i;
						}
					}
					if (!flag)
					{
						if (num == 0)
						{
							list.Add(item);
						}
						else
						{
							list.Insert(num, item);
						}
					}
				}
				else
				{
					global::System.Collections.Generic.List<global::Kampai.Game.QuestDefinition> list2 = new global::System.Collections.Generic.List<global::Kampai.Game.QuestDefinition>();
					list2.Add(item);
					global::Kampai.Game.QuestLine questLine = new global::Kampai.Game.QuestLine();
					questLine.state = global::Kampai.Game.QuestLineState.NotStarted;
					questLine.Quests = list2;
					questLines.Add(item.QuestLineID, questLine);
				}
			}
			SetupQuestLineCharacterInfo(dictionary);
		}

		private void CreateQuestLine(global::Kampai.Game.MasterPlanComponent component)
		{
			if (questLines.ContainsKey(component.ID))
			{
				questLines.Remove(component.ID);
			}
			global::Kampai.Game.QuestLine value = masterPlanQuestService.ConvertComponentToQuestLine(component);
			questLines.Add(component.ID, value);
		}

		private void CreateQuestLine(global::Kampai.Game.MasterPlan masterPlan)
		{
			if (questLines.ContainsKey(masterPlan.ID))
			{
				questLines.Remove(masterPlan.ID);
			}
			global::Kampai.Game.QuestLine value = masterPlanQuestService.ConvertMasterPlanToQuestLine(masterPlan);
			questLines.Add(masterPlan.ID, value);
		}

		public void UpdateMasterPlanQuestLine()
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.MasterPlan> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.MasterPlan>();
			global::System.Collections.Generic.IList<global::Kampai.Game.MasterPlanComponent> instancesByType2 = playerService.GetInstancesByType<global::Kampai.Game.MasterPlanComponent>();
			for (int i = 0; i < instancesByType.Count; i++)
			{
				global::Kampai.Game.MasterPlan masterPlan = instancesByType[i];
				CreateQuestLine(masterPlan);
				for (int j = 0; j < instancesByType2.Count; j++)
				{
					global::Kampai.Game.MasterPlanComponent masterPlanComponent = instancesByType2[j];
					if (masterPlan.ID == masterPlanComponent.planTrackingInstance && masterPlanComponent.State >= global::Kampai.Game.MasterPlanComponentState.InProgress)
					{
						CreateQuestLine(masterPlanComponent);
					}
				}
			}
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.GetNewQuestSignal>().Dispatch();
		}

		private void SetupQuestLineCharacterInfo(global::System.Collections.Generic.Dictionary<int, int> questToQuestLine)
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.PrestigeDefinition> all = definitionService.GetAll<global::Kampai.Game.PrestigeDefinition>();
			foreach (global::Kampai.Game.PrestigeDefinition item in all)
			{
				int iD = item.ID;
				if (item.Type != global::Kampai.Game.PrestigeType.Minion || item.PrestigeLevelSettings == null)
				{
					continue;
				}
				for (int i = 0; i < item.PrestigeLevelSettings.Count; i++)
				{
					global::Kampai.Game.CharacterPrestigeLevelDefinition characterPrestigeLevelDefinition = item.PrestigeLevelSettings[i];
					global::Kampai.Game.Prestige prestige = prestigeService.GetPrestige(iD, false);
					if (characterPrestigeLevelDefinition.UnlockQuestID != 0 && questToQuestLine.ContainsKey(characterPrestigeLevelDefinition.UnlockQuestID))
					{
						int num = questToQuestLine[characterPrestigeLevelDefinition.UnlockQuestID];
						global::Kampai.Game.QuestLine questLine = questLines[num];
						questLine.UnlockCharacterPrestigeLevel = i;
						if (prestige != null && prestige.CurrentPrestigeLevel >= i && questLine.state == global::Kampai.Game.QuestLineState.NotStarted)
						{
							SetQuestLineState(num, global::Kampai.Game.QuestLineState.Started);
						}
					}
					if (characterPrestigeLevelDefinition.AttachedQuestID == 0 || !questToQuestLine.ContainsKey(characterPrestigeLevelDefinition.AttachedQuestID))
					{
						continue;
					}
					int num2 = questToQuestLine[characterPrestigeLevelDefinition.AttachedQuestID];
					global::Kampai.Game.QuestLine questLine2 = questLines[num2];
					questLine2.GivenByCharacterID = iD;
					questLine2.GivenByCharacterPrestigeLevel = i;
					if (prestige != null)
					{
						if (prestige.CurrentPrestigeLevel > i || (prestige.CurrentPrestigeLevel == i && prestige.state == global::Kampai.Game.PrestigeState.Taskable))
						{
							questLine2.state = global::Kampai.Game.QuestLineState.Finished;
						}
						else if (prestige.CurrentPrestigeLevel == i && (prestige.state == global::Kampai.Game.PrestigeState.Questing || prestige.state == global::Kampai.Game.PrestigeState.TaskableWhileQuesting) && questLine2.state == global::Kampai.Game.QuestLineState.NotStarted)
						{
							SetQuestLineState(num2, global::Kampai.Game.QuestLineState.Started);
						}
					}
				}
			}
		}

		private void CreateQuestUnlockTree()
		{
			foreach (global::System.Collections.Generic.KeyValuePair<int, global::Kampai.Game.QuestLine> questLine in questLines)
			{
				global::System.Collections.Generic.IList<global::Kampai.Game.QuestDefinition> list = questLine.Value.Quests;
				int count = list.Count;
				for (int i = 0; i < count; i++)
				{
					global::Kampai.Game.QuestDefinition questDefinition = list[i];
					int iD = questDefinition.ID;
					if (questDefinition.SurfaceID < 0)
					{
						continue;
					}
					if (i < count - 1)
					{
						global::Kampai.Game.QuestDefinition questDefinition2 = list[i + 1];
						int iD2 = questDefinition2.ID;
						if (!questUnlockTree.ContainsKey(questDefinition2.ID))
						{
							global::System.Collections.Generic.List<int> list2 = new global::System.Collections.Generic.List<int>();
							list2.Add(iD);
							questUnlockTree.Add(iD2, list2);
						}
						else if (!questUnlockTree[iD2].Contains(iD))
						{
							questUnlockTree[iD2].Add(iD);
						}
					}
					if (questDefinition.UnlockQuestId != 0)
					{
						if (!questUnlockTree.ContainsKey(questDefinition.UnlockQuestId))
						{
							global::System.Collections.Generic.List<int> list3 = new global::System.Collections.Generic.List<int>();
							list3.Add(iD);
							questUnlockTree.Add(questDefinition.UnlockQuestId, list3);
						}
						else if (!questUnlockTree[questDefinition.UnlockQuestId].Contains(iD))
						{
							questUnlockTree[questDefinition.UnlockQuestId].Add(iD);
						}
						SetQuestDependency(questDefinition);
					}
				}
			}
		}

		private void SetQuestDependency(global::Kampai.Game.QuestDefinition questDefinition)
		{
			global::Kampai.Game.QuestDefinition questDefinition2 = definitionService.Get<global::Kampai.Game.QuestDefinition>(questDefinition.UnlockQuestId);
			if (questLines.ContainsKey(questDefinition.QuestLineID) && questLines.ContainsKey(questDefinition2.QuestLineID) && questLines[questDefinition2.QuestLineID].Quests[0].ID == questDefinition2.ID && questDefinition.NarrativeOrder == 0)
			{
				questLines[questDefinition.QuestLineID].unlockByQuestLine = questLines[questDefinition2.QuestLineID].QuestLineID;
			}
		}

		private void UpdateQuestLineStateBasedOnDependency()
		{
			global::System.Collections.Generic.List<global::Kampai.Game.QuestLine> list = new global::System.Collections.Generic.List<global::Kampai.Game.QuestLine>();
			foreach (global::System.Collections.Generic.KeyValuePair<int, global::Kampai.Game.QuestLine> questLine2 in questLines)
			{
				global::Kampai.Game.QuestLine value = questLine2.Value;
				if (value.state == global::Kampai.Game.QuestLineState.NotStarted || list.Contains(value))
				{
					continue;
				}
				int unlockByQuestLine = value.unlockByQuestLine;
				while (unlockByQuestLine != 0 && unlockByQuestLine != -1 && questLines.ContainsKey(unlockByQuestLine))
				{
					global::Kampai.Game.QuestLine questLine = questLines[unlockByQuestLine];
					if (list.Contains(questLine))
					{
						break;
					}
					questLine.state = global::Kampai.Game.QuestLineState.Finished;
					if (questLine.GivenByCharacterID != 0)
					{
						global::Kampai.Game.Prestige prestige = prestigeService.GetPrestige(questLine.GivenByCharacterID);
						if (prestige.CurrentPrestigeLevel <= questLine.GivenByCharacterPrestigeLevel)
						{
							prestigeService.ChangeToPrestigeState(prestige, global::Kampai.Game.PrestigeState.Taskable, prestige.CurrentPrestigeLevel + 1);
						}
						checkForInjectedPrestige(prestige, questLine);
					}
					list.Add(questLine);
					unlockByQuestLine = questLine.unlockByQuestLine;
				}
			}
		}

		public void checkForInjectedPrestige(global::Kampai.Game.Prestige p, global::Kampai.Game.QuestLine prevQuestLine)
		{
			if (p.CurrentPrestigeLevel >= p.Definition.PrestigeLevelSettings.Count - 1)
			{
				return;
			}
			for (int i = p.CurrentPrestigeLevel; i < p.Definition.PrestigeLevelSettings.Count - 1; i++)
			{
				global::Kampai.Game.QuestDefinition questDefinition = definitionService.Get<global::Kampai.Game.QuestDefinition>(p.Definition.PrestigeLevelSettings[i].AttachedQuestID);
				if (questDefinition != null && questLines.ContainsKey(questDefinition.QuestLineID))
				{
					global::Kampai.Game.QuestLine questLine = questLines[questDefinition.QuestLineID];
					if (questLine.QuestLineID == prevQuestLine.QuestLineID && playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID) >= p.Definition.PrestigeLevelSettings[i + 1].UnlockLevel)
					{
						logger.Log(global::Kampai.Util.KampaiLogLevel.Info, "Character " + p.Definition.LocalizedKey + " has a lower prestige than they should. User has already completed questLine " + questLine.QuestLineID + " increasing current prestige to " + (i + 1));
						p.CurrentPrestigeLevel = i + 1;
						break;
					}
				}
			}
		}

		public string GetQuestName(string key, params object[] args)
		{
			if (localService.Contains(key))
			{
				return localService.GetString(key, args);
			}
			string ext;
			return localService.GetString(ParseLocalizationKey(key, out ext), args);
		}

		public string GetEventName(string key, params object[] args)
		{
			if (key == null)
			{
				return key;
			}
			string text = key;
			string ext;
			key = ParseLocalizationKey(key, out ext);
			if (eventsLocalService.Contains(key))
			{
				text = eventsLocalService.GetString(key, args);
				if (!string.IsNullOrEmpty(ext))
				{
					text = text + " " + ext;
				}
			}
			else
			{
				logger.Warning("QuestService: Failed to translate Event Name: {0} - Translation Service: {1}, {2}", text, eventsLocalService.IsInitialized(), eventsLocalService.GetLanguageKey());
			}
			return text;
		}

		public void SetPulseMoveBuildingAccept(bool enablePulse)
		{
			pulseMoveBuildingAccept = enablePulse;
		}

		public bool ShouldPulseMoveButtonAccept()
		{
			return pulseMoveBuildingAccept;
		}

		private string ParseLocalizationKey(string key, out string ext)
		{
			string[] array = key.Split('_');
			if (array.Length > 1)
			{
				ext = array[1];
				return string.Format("{0}_{1}", array[0], ext);
			}
			if (key.Length > 2)
			{
				ext = key.Substring(key.Length - 2, 2);
				char[] array2 = ext.ToCharArray();
				if (char.IsNumber(array2[0]) || char.IsNumber(array2[1]))
				{
					return key.Remove(key.Length - 2);
				}
			}
			ext = null;
			return key;
		}
	}
}
