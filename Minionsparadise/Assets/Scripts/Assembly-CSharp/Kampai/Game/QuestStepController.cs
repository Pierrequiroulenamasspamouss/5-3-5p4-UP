namespace Kampai.Game
{
	public abstract class QuestStepController : global::Kampai.Game.IQuestStepController
	{
		protected global::Kampai.Game.IPlayerService playerService;

		protected global::Kampai.Game.IQuestScriptService questScriptService;

		protected global::strange.extensions.context.api.ICrossContextCapable gameContext;

		protected global::Kampai.Util.IKampaiLogger logger;

		private global::Kampai.Game.Quest quest;

		protected readonly int stepIndex;

		public global::Kampai.Game.QuestStepType StepType
		{
			get
			{
				return questStepDefinition.Type;
			}
		}

		public virtual global::Kampai.Game.QuestStepState StepState
		{
			get
			{
				return questStep.state;
			}
		}

		public int StepInstanceTrackedID
		{
			get
			{
				return questStep.TrackedID;
			}
		}

		public int ItemDefinitionID
		{
			get
			{
				return questStepDefinition.ItemDefinitionID;
			}
		}

		public virtual string DeliverButtonLocKey
		{
			get
			{
				if (StepState == global::Kampai.Game.QuestStepState.WaitComplete)
				{
					return "Done";
				}
				return string.Empty;
			}
		}

		public virtual bool NeedActiveDeliverButton
		{
			get
			{
				return false;
			}
		}

		public virtual bool NeedActiveProgressBar
		{
			get
			{
				return true;
			}
		}

		public virtual bool NeedGoToButton
		{
			get
			{
				return true;
			}
		}

		public int ProgressBarAmount
		{
			get
			{
				if (StepState == global::Kampai.Game.QuestStepState.Complete || StepState == global::Kampai.Game.QuestStepState.WaitComplete)
				{
					return questStepDefinition.ItemAmount;
				}
				return questStep.AmountCompleted;
			}
		}

		public int ProgressBarTotal
		{
			get
			{
				return questStepDefinition.ItemAmount;
			}
		}

		public virtual int AmountNeeded
		{
			get
			{
				return 0;
			}
		}

		protected global::Kampai.Game.QuestStepDefinition questStepDefinition
		{
			get
			{
				return quest.GetActiveDefinition().QuestSteps[stepIndex];
			}
		}

		protected global::Kampai.Game.QuestStep questStep
		{
			get
			{
				return quest.Steps[stepIndex];
			}
			set
			{
				quest.Steps[stepIndex] = value;
			}
		}

		protected bool isProceduralQuest
		{
			get
			{
				return quest.GetActiveDefinition().SurfaceType == global::Kampai.Game.QuestSurfaceType.ProcedurallyGenerated;
			}
		}

		protected int QuestInstanceID
		{
			get
			{
				return quest.ID;
			}
		}

		public QuestStepController(global::Kampai.Game.Quest quest, int stepIndex, global::Kampai.Game.IQuestScriptService questScriptService, global::Kampai.Game.IPlayerService playerService, global::strange.extensions.context.api.ICrossContextCapable gameContext, global::Kampai.Util.IKampaiLogger logger)
		{
			this.quest = quest;
			this.stepIndex = stepIndex;
			this.playerService = playerService;
			this.questScriptService = questScriptService;
			this.gameContext = gameContext;
			this.logger = logger;
			if (stepIndex >= quest.Steps.Count)
			{
				logger.Warning("Step Index is out of range for your current quest! {0} Setting it to 0", stepIndex);
				stepIndex = 0;
			}
		}

		public int IsTrackingOneOffCraftable(int itemDefinitionID)
		{
			int result = 0;
			if (questStepDefinition.ItemDefinitionID == itemDefinitionID && questStep.state == global::Kampai.Game.QuestStepState.Inprogress)
			{
				result = questStepDefinition.ItemAmount;
			}
			return result;
		}

		public void GoToNextState(bool isTaskComplete = false)
		{
			if (isTaskComplete)
			{
				quest.Steps[stepIndex].state = global::Kampai.Game.QuestStepState.Ready;
			}
			switch (quest.Steps[stepIndex].state)
			{
			case global::Kampai.Game.QuestStepState.Notstarted:
				if (questScriptService == null || !questScriptService.HasScript(quest, true, stepIndex, true))
				{
					InprogressStateCheck();
				}
				else
				{
					GoToTaskState(global::Kampai.Game.QuestStepState.RunningStartScript);
				}
				break;
			case global::Kampai.Game.QuestStepState.RunningStartScript:
				InprogressStateCheck();
				break;
			case global::Kampai.Game.QuestStepState.Inprogress:
				GoToTaskState(global::Kampai.Game.QuestStepState.Ready);
				break;
			case global::Kampai.Game.QuestStepState.Ready:
			case global::Kampai.Game.QuestStepState.WaitComplete:
				if (questScriptService == null || !questScriptService.HasScript(quest, false, stepIndex, true))
				{
					GoToTaskState(global::Kampai.Game.QuestStepState.Complete);
				}
				else
				{
					GoToTaskState(global::Kampai.Game.QuestStepState.RunningCompleteScript);
				}
				break;
			case global::Kampai.Game.QuestStepState.RunningCompleteScript:
				GoToTaskState(global::Kampai.Game.QuestStepState.Complete);
				break;
			case global::Kampai.Game.QuestStepState.Complete:
				break;
			}
		}

		public void GoToTaskState(global::Kampai.Game.QuestStepState targetState)
		{
			if (questScriptService != null)
			{
				global::Kampai.Game.QuestStepState state = questStep.state;
				questStep.state = targetState;
				switch (targetState)
				{
				case global::Kampai.Game.QuestStepState.RunningStartScript:
					questScriptService.StartQuestScript(quest, true, false, stepIndex, true);
					break;
				case global::Kampai.Game.QuestStepState.RunningCompleteScript:
					questScriptService.StartQuestScript(quest, false, false, stepIndex, true);
					break;
				}
				gameContext.injectionBinder.GetInstance<global::Kampai.Game.QuestTaskStateChangeSignal>().Dispatch(quest, stepIndex, state);
			}
		}

		private void InprogressStateCheck()
		{
			GoToTaskState(global::Kampai.Game.QuestStepState.Inprogress);
			if (StepType == global::Kampai.Game.QuestStepType.Construction && questStep.AmountCompleted >= questStepDefinition.ItemAmount)
			{
				GoToNextState(true);
			}
		}

		public virtual void SetupTracking()
		{
			questStep.TrackedID = questStepDefinition.ItemDefinitionID;
			if (questStep.TrackedID == 0)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.QS_NO_SUCH_TRACKED_ID, "Item definition id not found for {0} Type quests", questStepDefinition.Type);
			}
		}

		public abstract void UpdateTask(global::Kampai.Game.QuestTaskTransition questTaskTransition, global::Kampai.Game.Building building, int buildingDefId, int itemDefId);

		public abstract string GetStepAction(global::Kampai.Main.ILocalizationService localService);

		public abstract string GetStepDescription(global::Kampai.Main.ILocalizationService localService, global::Kampai.Game.IDefinitionService defService);

		public abstract void GetStepDescIcon(global::Kampai.Game.IDefinitionService defService, out global::UnityEngine.Sprite mainSprite, out global::UnityEngine.Sprite maskSprite);
	}
}
