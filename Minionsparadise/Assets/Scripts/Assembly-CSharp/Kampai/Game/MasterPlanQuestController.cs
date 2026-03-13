namespace Kampai.Game
{
	public class MasterPlanQuestController : global::Kampai.Game.IQuestController
	{
		private readonly global::Kampai.Game.IDefinitionService definitionService;

		private readonly global::strange.extensions.context.api.ICrossContextCapable gameContext;

		private readonly global::Kampai.Util.IKampaiLogger logger;

		private readonly global::Kampai.Game.IPlayerService playerService;

		private readonly global::Kampai.Game.MasterPlanQuestType.Component quest;

		private readonly global::System.Collections.Generic.List<global::Kampai.Game.IQuestStepController> questStepControllers = new global::System.Collections.Generic.List<global::Kampai.Game.IQuestStepController>();

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

		public global::Kampai.Game.QuestDefinition Definition
		{
			get
			{
				return quest.GetActiveDefinition();
			}
		}

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

		public int QuestIconTrackedInstanceID
		{
			get
			{
				return quest.QuestIconTrackedInstanceId;
			}
		}

		public global::Kampai.Game.QuestState State
		{
			get
			{
				return quest.state;
			}
		}

		public int StepCount
		{
			get
			{
				return quest.Steps.Count;
			}
		}

		protected global::Kampai.Game.QuestSurfaceType surfaceType
		{
			get
			{
				return Definition.SurfaceType;
			}
		}

		public MasterPlanQuestController(global::Kampai.Game.MasterPlanComponent component, global::Kampai.Util.IKampaiLogger logger, global::Kampai.Game.IPlayerService playerService, global::Kampai.Game.IDefinitionService definitionService, global::strange.extensions.context.api.ICrossContextCapable gameContext, global::Kampai.Game.IMasterPlanQuestService masterPlanQuestService, bool isBuild)
		{
			quest = masterPlanQuestService.ConvertMasterPlanComponentToQuest(component, isBuild) as global::Kampai.Game.MasterPlanQuestType.Component;
			quest.isBuildQuest = isBuild;
			this.logger = logger;
			this.playerService = playerService;
			this.definitionService = definitionService;
			this.gameContext = gameContext;
			if (Definition.QuestSteps == null)
			{
				return;
			}
			for (int i = 0; i < Definition.QuestSteps.Count; i++)
			{
				object obj;
				if (Definition.QuestSteps[i].Type != global::Kampai.Game.QuestStepType.MasterPlanComponentBuild)
				{
					global::Kampai.Game.IQuestStepController questStepController = CreateController(quest.component.tasks[i].Definition.Type, quest, i);
					obj = questStepController;
				}
				else
				{
					obj = new global::Kampai.Game.MasterPlanQuest.BuildComponentQuestStepController(quest, i, playerService, gameContext, logger);
				}
				global::Kampai.Game.IQuestStepController questStepController2 = (global::Kampai.Game.IQuestStepController)obj;
				if (questStepController2 != null)
				{
					questStepControllers.Add(questStepController2);
				}
			}
		}

		public MasterPlanQuestController(global::Kampai.Game.MasterPlan plan, global::Kampai.Util.IKampaiLogger logger, global::Kampai.Game.IPlayerService playerService, global::Kampai.Game.IDefinitionService definitionService, global::strange.extensions.context.api.ICrossContextCapable gameContext, global::Kampai.Game.IMasterPlanQuestService masterPlanQuestService)
		{
			quest = masterPlanQuestService.ConvertMasterPlanToQuest(plan) as global::Kampai.Game.MasterPlanQuestType.Component;
			this.logger = logger;
			this.playerService = playerService;
			this.definitionService = definitionService;
			this.gameContext = gameContext;
			if (Definition.QuestSteps != null)
			{
				for (int i = 0; i < Definition.QuestSteps.Count; i++)
				{
					global::Kampai.Game.IQuestStepController item = new global::Kampai.Game.MasterPlanQuest.BuildComponentQuestStepController(quest, i, playerService, gameContext, logger);
					questStepControllers.Add(item);
				}
			}
		}

		public void CheckAndUpdateQuestCompleteState()
		{
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

		public void DeleteQuest()
		{
		}

		public int GetIdleTime()
		{
			return 0;
		}

		public global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> GetRequiredQuantityItems()
		{
			return new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
		}

		public global::Kampai.Game.IQuestStepController GetStepController(int stepIndex)
		{
			if (stepIndex < questStepControllers.Count)
			{
				return questStepControllers[stepIndex];
			}
			logger.Error("Step Controller doesn't exist for step index {0}", stepIndex);
			return null;
		}

		public void GoToQuestState(global::Kampai.Game.QuestState targetState)
		{
		}

		public int IsTrackingOneOffCraftable(int itemDefinitionID)
		{
			return 0;
		}

		public bool IsTrackingThisBuilding(int buildingID, global::Kampai.Game.QuestStepType StepType)
		{
			return false;
		}

		public void OnQuestScriptComplete(global::Kampai.Game.QuestScriptInstance questScriptInstance)
		{
		}

		public void ProcessAutomaticQuest()
		{
		}

		public void RushQuestStep(int stepIndex)
		{
		}

		public void SetUpTracking()
		{
			AssignBuildingTrackIdToAllQuestStep();
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.UpdateQuestWorldIconsSignal>().Dispatch(quest);
		}

		public void UpdateTask(global::Kampai.Game.QuestStepType stepType, global::Kampai.Game.QuestTaskTransition questTaskTransition = global::Kampai.Game.QuestTaskTransition.Start, global::Kampai.Game.Building building = null, int buildingDefId = 0, int itemDefId = 0)
		{
			foreach (global::Kampai.Game.IQuestStepController questStepController in questStepControllers)
			{
				questStepController.UpdateTask(questTaskTransition, building, buildingDefId, itemDefId);
			}
		}

		private global::Kampai.Game.IQuestStepController CreateController(global::Kampai.Game.MasterPlanComponentTaskType stepType, global::Kampai.Game.Quest quest, int questStepIndex)
		{
			global::Kampai.Game.IQuestStepController result = null;
			switch (stepType)
			{
			case global::Kampai.Game.MasterPlanComponentTaskType.Deliver:
				result = new global::Kampai.Game.MasterPlanQuest.DeliverTaskQuestStepController(quest, questStepIndex, definitionService, playerService, gameContext, logger);
				break;
			case global::Kampai.Game.MasterPlanComponentTaskType.Collect:
				result = new global::Kampai.Game.MasterPlanQuest.CollectTaskQuestStepController(quest, questStepIndex, definitionService, playerService, gameContext, logger);
				break;
			case global::Kampai.Game.MasterPlanComponentTaskType.CompleteOrders:
				result = new global::Kampai.Game.MasterPlanQuest.CompleteOrdersQuestStepController(quest, questStepIndex, definitionService, playerService, gameContext, logger);
				break;
			case global::Kampai.Game.MasterPlanComponentTaskType.PlayMiniGame:
				result = new global::Kampai.Game.MasterPlanQuest.PlayMiniGameQuestStepController(quest, questStepIndex, definitionService, playerService, gameContext, logger);
				break;
			case global::Kampai.Game.MasterPlanComponentTaskType.MiniGameScore:
				result = new global::Kampai.Game.MasterPlanQuest.MiniGameScoreQuestStepController(quest, questStepIndex, definitionService, playerService, gameContext, logger);
				break;
			case global::Kampai.Game.MasterPlanComponentTaskType.EarnPartyPoints:
				result = new global::Kampai.Game.MasterPlanQuest.EarnPartyPointsQuestStepController(quest, questStepIndex, definitionService, playerService, gameContext, logger);
				break;
			case global::Kampai.Game.MasterPlanComponentTaskType.EarnLeisurePartyPoints:
				result = new global::Kampai.Game.MasterPlanQuest.EarnLeisurePartyPointsQuestStepController(quest, questStepIndex, definitionService, playerService, gameContext, logger);
				break;
			case global::Kampai.Game.MasterPlanComponentTaskType.EarnMignettePartyPoints:
				result = new global::Kampai.Game.MasterPlanQuest.EarnMignettePartyPointsQuestStepController(quest, questStepIndex, definitionService, playerService, gameContext, logger);
				break;
			case global::Kampai.Game.MasterPlanComponentTaskType.EarnSandDollars:
				result = new global::Kampai.Game.MasterPlanQuest.EarnSandDollarsQuestStepController(quest, questStepIndex, definitionService, playerService, gameContext, logger);
				break;
			default:
				logger.Error("Using unused QuestStepType: {0}, your quest won't have this step", stepType);
				break;
			}
			return result;
		}

		private void AssignBuildingTrackIdToAllQuestStep()
		{
			foreach (global::Kampai.Game.IQuestStepController questStepController in questStepControllers)
			{
				questStepController.SetupTracking();
			}
		}
	}
}
