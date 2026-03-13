namespace Kampai.Game
{
	public abstract class MasterPlanQuestStepController : global::Kampai.Game.QuestStepController
	{
		protected readonly global::Kampai.Game.IDefinitionService definitionService;

		protected readonly global::Kampai.Game.MasterPlanComponentTask task;

		protected readonly global::Kampai.Game.MasterPlanComponentTaskDefinition taskDefinition;

		protected readonly global::Kampai.Game.MasterPlanQuestType.ComponentTaskDefinition taskQuestDef;

		protected int m_amountNeeded;

		protected bool m_needActiveDeliverButton = true;

		protected bool m_needActiveProgressBar = true;

		protected global::Kampai.Game.MasterPlanQuestType.Component questComponent;

		protected global::Kampai.Game.MasterPlanQuestType.ComponentTask taskQuest;

		public global::Kampai.Game.MasterPlanComponent Component
		{
			get
			{
				return (questComponent != null) ? questComponent.component : null;
			}
		}

		public global::Kampai.Game.MasterPlanQuestType.Component ComponentDef
		{
			get
			{
				return questComponent;
			}
		}

		public override string DeliverButtonLocKey
		{
			get
			{
				return "Complete";
			}
		}

		public override bool NeedActiveProgressBar
		{
			get
			{
				return m_needActiveProgressBar;
			}
		}

		public override global::Kampai.Game.QuestStepState StepState
		{
			get
			{
				return GetStepState();
			}
		}

		protected abstract string DescriptionLocKey { get; }

		protected MasterPlanQuestStepController(global::Kampai.Game.Quest quest, int stepIndex, global::Kampai.Game.IDefinitionService definitionService, global::Kampai.Game.IPlayerService playerService, global::strange.extensions.context.api.ICrossContextCapable gameContext, global::Kampai.Util.IKampaiLogger logger)
			: base(quest, stepIndex, null, playerService, gameContext, logger)
		{
			questComponent = quest as global::Kampai.Game.MasterPlanQuestType.Component;
			this.definitionService = definitionService;
			taskQuestDef = base.questStepDefinition as global::Kampai.Game.MasterPlanQuestType.ComponentTaskDefinition;
			taskDefinition = ((taskQuestDef != null) ? taskQuestDef.taskDefinition : null);
			taskQuest = base.questStep as global::Kampai.Game.MasterPlanQuestType.ComponentTask;
			task = ((taskQuest != null) ? taskQuest.task : null);
		}

		public override string GetStepAction(global::Kampai.Main.ILocalizationService localService)
		{
			string key = string.Empty;
			switch (taskDefinition.Type)
			{
			case global::Kampai.Game.MasterPlanComponentTaskType.Deliver:
				key = "MasterPlanTaskDeliverQuestTitle";
				break;
			case global::Kampai.Game.MasterPlanComponentTaskType.Collect:
				key = "MasterPlanTaskCollectQuestTitle";
				break;
			case global::Kampai.Game.MasterPlanComponentTaskType.CompleteOrders:
				key = "MasterPlanTaskCompleteOrdersQuestTitle";
				break;
			case global::Kampai.Game.MasterPlanComponentTaskType.PlayMiniGame:
				key = "MasterPlanTaskPlayMiniGameQuestTitle";
				break;
			case global::Kampai.Game.MasterPlanComponentTaskType.MiniGameScore:
				key = "MasterPlanTaskMiniGameScoreQuestTitle";
				break;
			case global::Kampai.Game.MasterPlanComponentTaskType.EarnPartyPoints:
				key = "MasterPlanTaskEarnPartyPointsQuestTitle";
				break;
			case global::Kampai.Game.MasterPlanComponentTaskType.EarnLeisurePartyPoints:
				key = "MasterPlanTaskEarnLeisurePartyPointsQuestTitle";
				break;
			case global::Kampai.Game.MasterPlanComponentTaskType.EarnMignettePartyPoints:
				key = "MasterPlanTaskEarnMignettePartyPointsQuestTitle";
				break;
			case global::Kampai.Game.MasterPlanComponentTaskType.EarnSandDollars:
				key = "MasterPlanTaskEarnSandDollarsQuestTitle";
				break;
			}
			return localService.GetStringUpper(key);
		}

		public override string GetStepDescription(global::Kampai.Main.ILocalizationService localService, global::Kampai.Game.IDefinitionService defService)
		{
			return localService.GetString(DescriptionLocKey, DescriptionArgs(localService));
		}

		public global::Kampai.Game.QuestStepState GetStepState()
		{
			if (task.isComplete)
			{
				return global::Kampai.Game.QuestStepState.Complete;
			}
			return (!task.isHarvestable) ? global::Kampai.Game.QuestStepState.Inprogress : global::Kampai.Game.QuestStepState.Ready;
		}

		public override void SetupTracking()
		{
		}

		public override void UpdateTask(global::Kampai.Game.QuestTaskTransition questTaskTransition, global::Kampai.Game.Building building, int buildingDefId, int itemDefId)
		{
		}

		public global::Kampai.Game.IQuestStepController UpdateTaskInfo()
		{
			base.questStep.state = GetStepState();
			return this;
		}

		protected abstract object[] DescriptionArgs(global::Kampai.Main.ILocalizationService localizationService);
	}
}
