namespace Kampai.Game
{
	public class QuestController : global::Kampai.Game.IQuestController
	{
		private global::Kampai.Game.Quest quest;

		private global::System.Collections.Generic.List<global::Kampai.Game.IQuestStepController> questStepControllers = new global::System.Collections.Generic.List<global::Kampai.Game.IQuestStepController>();

		private global::Kampai.Util.IKampaiLogger logger;

		private global::Kampai.Game.IPlayerService playerService;

		private global::Kampai.Game.IDefinitionService definitionService;

		private global::Kampai.Game.IPrestigeService prestigeService;

		private global::Kampai.Game.IQuestScriptService questScriptService;

		private global::strange.extensions.context.api.ICrossContextCapable gameContext;

		public int ID
		{
			get
			{
				return quest.ID;
			}
		}

		public global::Kampai.Game.Quest Quest
		{
			get
			{
				return quest;
			}
		}

		public int StepCount
		{
			get
			{
				return quest.Steps.Count;
			}
		}

		public int QuestIconTrackedInstanceID
		{
			get
			{
				return quest.QuestIconTrackedInstanceId;
			}
		}

		public bool AreAllStepsComplete
		{
			get
			{
				foreach (global::Kampai.Game.QuestStep step in quest.Steps)
				{
					if (step.state != global::Kampai.Game.QuestStepState.Complete)
					{
						return false;
					}
				}
				return true;
			}
		}

		public global::Kampai.Game.QuestDefinition Definition
		{
			get
			{
				return quest.GetActiveDefinition();
			}
		}

		public global::Kampai.Game.QuestState State
		{
			get
			{
				return quest.state;
			}
		}

		public bool AutoGrantReward
		{
			get
			{
				return quest.AutoGrantReward;
			}
			set
			{
				quest.AutoGrantReward = value;
			}
		}

		protected global::Kampai.Game.QuestSurfaceType surfaceType
		{
			get
			{
				return Definition.SurfaceType;
			}
		}

		public QuestController(global::Kampai.Game.Quest quest, global::Kampai.Util.IKampaiLogger logger, global::Kampai.Game.IPlayerService playerService, global::Kampai.Game.IPrestigeService prestigeService, global::Kampai.Game.IDefinitionService definitionService, global::Kampai.Game.IQuestScriptService questScriptService, global::strange.extensions.context.api.ICrossContextCapable gameContext, global::Kampai.Game.Environment environment)
		{
			this.quest = quest;
			this.logger = logger;
			this.playerService = playerService;
			this.prestigeService = prestigeService;
			this.definitionService = definitionService;
			this.questScriptService = questScriptService;
			this.gameContext = gameContext;
			if (Definition.QuestSteps == null)
			{
				return;
			}
			for (int i = 0; i < Definition.QuestSteps.Count; i++)
			{
				global::Kampai.Game.IQuestStepController questStepController = CreateController(Definition.QuestSteps[i].Type, quest, i, environment);
				if (questStepController != null)
				{
					questStepControllers.Add(questStepController);
				}
			}
		}

		private global::Kampai.Game.IQuestStepController CreateController(global::Kampai.Game.QuestStepType stepType, global::Kampai.Game.Quest quest, int questStepIndex, global::Kampai.Game.Environment environment)
		{
			global::Kampai.Game.IQuestStepController result = null;
			switch (stepType)
			{
			case global::Kampai.Game.QuestStepType.StageRepair:
			case global::Kampai.Game.QuestStepType.CabanaRepair:
			case global::Kampai.Game.QuestStepType.WelcomeHutRepair:
			case global::Kampai.Game.QuestStepType.FountainRepair:
			case global::Kampai.Game.QuestStepType.StorageRepair:
			case global::Kampai.Game.QuestStepType.LairPortalRepair:
			case global::Kampai.Game.QuestStepType.MinionUpgradeBuildingRepair:
				result = new global::Kampai.Game.BuildingRepairQuestStepController(quest, questStepIndex, questScriptService, playerService, gameContext, logger);
				break;
			case global::Kampai.Game.QuestStepType.Construction:
				result = new global::Kampai.Game.ConstuctionQuestStepController(quest, questStepIndex, questScriptService, playerService, gameContext, logger);
				break;
			case global::Kampai.Game.QuestStepType.Delivery:
				result = new global::Kampai.Game.DeliveryQuestStepController(quest, questStepIndex, questScriptService, playerService, definitionService, gameContext, logger);
				break;
			case global::Kampai.Game.QuestStepType.Harvest:
				result = new global::Kampai.Game.HarvestQuestStepController(quest, questStepIndex, questScriptService, playerService, definitionService, gameContext, logger);
				break;
			case global::Kampai.Game.QuestStepType.Leisure:
				result = new global::Kampai.Game.LeisureQuestStepController(quest, questStepIndex, questScriptService, playerService, gameContext, logger);
				break;
			case global::Kampai.Game.QuestStepType.Mignette:
				result = new global::Kampai.Game.MignetteQuestStepController(quest, questStepIndex, questScriptService, playerService, gameContext, logger);
				break;
			case global::Kampai.Game.QuestStepType.OrderBoard:
				result = new global::Kampai.Game.OrderBoardQuestStepController(quest, questStepIndex, questScriptService, playerService, gameContext, logger);
				break;
			case global::Kampai.Game.QuestStepType.ThrowParty:
				result = new global::Kampai.Game.ThrowPartyQuestStepController(quest, questStepIndex, questScriptService, playerService, gameContext, logger);
				break;
			case global::Kampai.Game.QuestStepType.BridgeRepair:
				result = new global::Kampai.Game.BridgeRepairQuestStepController(quest, questStepIndex, questScriptService, playerService, definitionService, gameContext, logger, environment);
				break;
			case global::Kampai.Game.QuestStepType.MinionTask:
				result = new global::Kampai.Game.MinionTaskQuestStepController(quest, questStepIndex, questScriptService, playerService, gameContext, logger);
				break;
			case global::Kampai.Game.QuestStepType.MinionUpgrade:
				result = new global::Kampai.Game.UpgradeMinionQuestStepController(quest, questStepIndex, questScriptService, playerService, gameContext, logger);
				break;
			case global::Kampai.Game.QuestStepType.HaveUpgradedMinions:
				result = new global::Kampai.Game.MinionUpgradeToLevelQuestStepController(quest, questStepIndex, questScriptService, playerService, gameContext, logger);
				break;
			case global::Kampai.Game.QuestStepType.PlayAnyLeisure:
			case global::Kampai.Game.QuestStepType.HarvestAnyLeisure:
				result = new global::Kampai.Game.AnyLeisureQuestStepController(quest, questStepIndex, questScriptService, playerService, gameContext, logger);
				break;
			case global::Kampai.Game.QuestStepType.MysteryBoxOnboarding:
				result = new global::Kampai.Game.MysteryBoxOnboardingQuestStepController(quest, questStepIndex, questScriptService, playerService, gameContext, logger);
				break;
			default:
				logger.Error("Using unused QuestStepType: {0}, your quest won't have this step", stepType);
				break;
			}
			return result;
		}

		public int GetIdleTime()
		{
			return gameContext.injectionBinder.GetInstance<global::Kampai.Game.IPlayerDurationService>().GetGameTimeDuration(quest);
		}

		public global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> GetRequiredQuantityItems()
		{
			global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> list = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
			for (int i = 0; i < StepCount; i++)
			{
				global::Kampai.Game.IQuestStepController stepController = GetStepController(i);
				uint amountNeeded = (uint)stepController.AmountNeeded;
				if (amountNeeded != 0)
				{
					global::Kampai.Util.QuantityItem item = new global::Kampai.Util.QuantityItem(stepController.ItemDefinitionID, amountNeeded);
					list.Add(item);
				}
			}
			return list;
		}

		public void SetUpTracking()
		{
			if (!NeedQuestTracking())
			{
				return;
			}
			AssignBuildingTrackIdToAllQuestStep();
			switch (surfaceType)
			{
			case global::Kampai.Game.QuestSurfaceType.Building:
				if (!AssignQuestIconTrackedBuildingInstanceID(quest))
				{
					logger.Log(global::Kampai.Util.KampaiLogLevel.Error, "Quest tracking instance is not set! This quest: {0} won't have any icons in the game world", quest.GetActiveDefinition().ID.ToString());
				}
				break;
			case global::Kampai.Game.QuestSurfaceType.Character:
				if (!AssignQuestIconTrackedCharacterInstanceID(quest))
				{
					logger.Log(global::Kampai.Util.KampaiLogLevel.Error, "Quest tracking instance is not set! This quest: {0} won't have any icons in the game world", quest.GetActiveDefinition().ID.ToString());
				}
				break;
			case global::Kampai.Game.QuestSurfaceType.Automatic:
			case global::Kampai.Game.QuestSurfaceType.LimitedEvent:
			case global::Kampai.Game.QuestSurfaceType.TimedEvent:
			case global::Kampai.Game.QuestSurfaceType.Bridge:
				AssignQuestIconTrackedInstanceID(quest);
				break;
			}
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.UpdateQuestWorldIconsSignal>().Dispatch(quest);
		}

		private bool NeedQuestTracking()
		{
			if (quest.Steps == null || quest.Steps.Count == 0)
			{
				if (Definition.SurfaceID > 0 && Definition.SurfaceType == global::Kampai.Game.QuestSurfaceType.Automatic)
				{
					return true;
				}
				return false;
			}
			return true;
		}

		private void AssignBuildingTrackIdToAllQuestStep()
		{
			foreach (global::Kampai.Game.IQuestStepController questStepController in questStepControllers)
			{
				questStepController.SetupTracking();
			}
		}

		private void AssignQuestIconTrackedInstanceID(global::Kampai.Game.Quest quest)
		{
			if (!AssignQuestIconTrackedBuildingInstanceID(quest) && !AssignQuestIconTrackedCharacterInstanceID(quest))
			{
				logger.Log(global::Kampai.Util.KampaiLogLevel.Error, "Quest tracking instance is not set! This quest: {0} won't have any icons in the game world", quest.GetActiveDefinition().ID.ToString());
			}
		}

		private bool AssignQuestIconTrackedCharacterInstanceID(global::Kampai.Game.Quest quest)
		{
			global::Kampai.Game.Prestige prestige = prestigeService.GetPrestige(quest.GetActiveDefinition().SurfaceID);
			if (prestige != null)
			{
				quest.QuestIconTrackedInstanceId = prestige.trackedInstanceId;
				return true;
			}
			logger.Log(global::Kampai.Util.KampaiLogLevel.Error, "Character doesn't exist for the quest surface id: {0}. This quest: {1} won't have any icons in the game world", quest.GetActiveDefinition().SurfaceID, quest.GetActiveDefinition().ID.ToString());
			return false;
		}

		private bool AssignQuestIconTrackedBuildingInstanceID(global::Kampai.Game.Quest quest)
		{
			global::System.Collections.Generic.ICollection<global::Kampai.Game.Building> byDefinitionId = playerService.GetByDefinitionId<global::Kampai.Game.Building>(quest.GetActiveDefinition().SurfaceID);
			foreach (global::Kampai.Game.Building item in byDefinitionId)
			{
				global::Kampai.Game.BuildingState state = item.State;
				if (state != global::Kampai.Game.BuildingState.Complete && state != global::Kampai.Game.BuildingState.Construction && state != global::Kampai.Game.BuildingState.Inventory)
				{
					quest.QuestIconTrackedInstanceId = item.ID;
					return true;
				}
			}
			return false;
		}

		public int IsTrackingOneOffCraftable(int itemDefinitionID)
		{
			int num = 0;
			foreach (global::Kampai.Game.IQuestStepController questStepController in questStepControllers)
			{
				num += questStepController.IsTrackingOneOffCraftable(itemDefinitionID);
			}
			return num;
		}

		public bool IsTrackingThisBuilding(int buildingID, global::Kampai.Game.QuestStepType StepType)
		{
			if (State == global::Kampai.Game.QuestState.RunningTasks)
			{
				foreach (global::Kampai.Game.IQuestStepController questStepController in questStepControllers)
				{
					if (questStepController.StepState == global::Kampai.Game.QuestStepState.Notstarted && questStepController.StepInstanceTrackedID == buildingID && questStepController.StepType == StepType)
					{
						return true;
					}
				}
			}
			return false;
		}

		public global::Kampai.Game.IQuestStepController GetStepController(int stepIndex)
		{
			if (stepIndex >= questStepControllers.Count)
			{
				logger.Error("Step Controller doesn't exist for step index {0}", stepIndex);
				return null;
			}
			return questStepControllers[stepIndex];
		}

		public void DeleteQuest()
		{
			if (quest != null)
			{
				playerService.Remove(quest);
			}
		}

		public void OnQuestScriptComplete(global::Kampai.Game.QuestScriptInstance questScriptInstance)
		{
			if (quest.state == global::Kampai.Game.QuestState.RunningTasks)
			{
				int questStepID = questScriptInstance.QuestStepID;
				if (questStepID < 0 || questStepID > quest.Steps.Count)
				{
					logger.Error("QuestService:OnQuestScriptComplete: QuestStepId {0} is out of range! Can't mark as complete!", questStepID);
				}
				else if (quest.Steps[questScriptInstance.QuestStepID].state == global::Kampai.Game.QuestStepState.RunningStartScript)
				{
					quest.Steps[questScriptInstance.QuestStepID].state = global::Kampai.Game.QuestStepState.Inprogress;
				}
				else
				{
					quest.Steps[questScriptInstance.QuestStepID].state = global::Kampai.Game.QuestStepState.Complete;
					CheckAndUpdateQuestCompleteState();
				}
			}
			else if (quest.state != global::Kampai.Game.QuestState.Harvestable)
			{
				gameContext.injectionBinder.GetInstance<global::Kampai.Game.GoToNextQuestStateSignal>().Dispatch(Definition.ID);
			}
		}

		public void RushQuestStep(int stepIndex)
		{
			global::Kampai.Game.IQuestStepController questStepController = questStepControllers[stepIndex];
			questStepController.GoToNextState(true);
		}

		public void UpdateTask(global::Kampai.Game.QuestStepType stepType, global::Kampai.Game.QuestTaskTransition questTaskTransition = global::Kampai.Game.QuestTaskTransition.Start, global::Kampai.Game.Building building = null, int buildingDefId = 0, int itemDefId = 0)
		{
			foreach (global::Kampai.Game.IQuestStepController questStepController in questStepControllers)
			{
				if (questStepController.StepType == stepType && questStepController.StepState != global::Kampai.Game.QuestStepState.Complete)
				{
					questStepController.UpdateTask(questTaskTransition, building, buildingDefId, itemDefId);
				}
			}
		}

		public void CheckAndUpdateQuestCompleteState()
		{
			foreach (global::Kampai.Game.QuestStep step in quest.Steps)
			{
				if (step.state != global::Kampai.Game.QuestStepState.Complete)
				{
					quest.AutoGrantReward = false;
					gameContext.injectionBinder.GetInstance<global::Kampai.Game.UpdateQuestWorldIconsSignal>().Dispatch(quest);
					return;
				}
			}
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.GoToNextQuestStateSignal>().Dispatch(Definition.ID);
		}

		public void GoToQuestState(global::Kampai.Game.QuestState targetState)
		{
			global::strange.extensions.injector.api.ICrossContextInjectionBinder injectionBinder = gameContext.injectionBinder;
			quest.state = targetState;
			switch (targetState)
			{
			case global::Kampai.Game.QuestState.Notstarted:
				break;
			case global::Kampai.Game.QuestState.RunningStartScript:
				questScriptService.StartQuestScript(quest, true);
				injectionBinder.GetInstance<global::Kampai.Game.UpdateQuestWorldIconsSignal>().Dispatch(quest);
				break;
			case global::Kampai.Game.QuestState.RunningTasks:
				UpdateTask(global::Kampai.Game.QuestStepType.Construction);
				UpdateTask(global::Kampai.Game.QuestStepType.Harvest);
				UpdateTask(global::Kampai.Game.QuestStepType.MinionUpgrade);
				UpdateTask(global::Kampai.Game.QuestStepType.HaveUpgradedMinions);
				UpdateTask(global::Kampai.Game.QuestStepType.MysteryBoxOnboarding);
				injectionBinder.GetInstance<global::Kampai.Game.UpdateQuestWorldIconsSignal>().Dispatch(quest);
				gameContext.injectionBinder.GetInstance<global::Kampai.Game.StartGameTimeTrackingSignal>().Dispatch(quest);
				break;
			case global::Kampai.Game.QuestState.RunningCompleteScript:
				questScriptService.StartQuestScript(quest, false);
				break;
			case global::Kampai.Game.QuestState.Harvestable:
				injectionBinder.GetInstance<global::Kampai.Game.QuestHarvestableSignal>().Dispatch(quest);
				break;
			case global::Kampai.Game.QuestState.Complete:
				if (quest.GetActiveDefinition().GetReward(definitionService) != null)
				{
					injectionBinder.GetInstance<global::Kampai.Game.RemoveQuestWorldIconSignal>().Dispatch(quest);
				}
				questScriptService.StartQuestScript(quest, false, true);
				injectionBinder.GetInstance<global::Kampai.Game.QuestCompleteSignal>().Dispatch(quest);
				break;
			}
		}

		public void ProcessAutomaticQuest()
		{
			if (!questScriptService.HasScript(quest, true))
			{
				if (Definition.QuestSteps == null || Definition.QuestSteps.Count == 0)
				{
					if (!questScriptService.HasScript(quest, false))
					{
						GoToQuestState(global::Kampai.Game.QuestState.Complete);
					}
					else
					{
						GoToQuestState(global::Kampai.Game.QuestState.RunningCompleteScript);
					}
				}
				else
				{
					GoToQuestState(global::Kampai.Game.QuestState.RunningTasks);
				}
			}
			else
			{
				GoToQuestState(global::Kampai.Game.QuestState.RunningStartScript);
			}
		}

		public void Debug_SetQuestToInProgressIfNotAlready()
		{
			if (quest.state == global::Kampai.Game.QuestState.Notstarted)
			{
				quest.state = global::Kampai.Game.QuestState.RunningTasks;
			}
			foreach (global::Kampai.Game.QuestStep step in quest.Steps)
			{
				step.state = ((step.state != global::Kampai.Game.QuestStepState.Notstarted) ? step.state : global::Kampai.Game.QuestStepState.Inprogress);
			}
		}
	}
}
