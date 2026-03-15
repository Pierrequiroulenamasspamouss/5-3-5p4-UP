namespace Kampai.UI.View
{
	public class QuestStepMediator : global::strange.extensions.mediation.impl.Mediator
	{
		public const float PLACEMENT_ZOOM_LEVEL = 0.4f;

		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("QuestStepMediator") as global::Kampai.Util.IKampaiLogger;

		private global::Kampai.Game.IQuestController questController;

		private global::Kampai.Game.IQuestStepController questStepController;

		[Inject]
		public global::Kampai.UI.View.QuestStepView view { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseQuestBookSignal closeSignal { get; set; }

		[Inject]
		public global::Kampai.Game.QuestStepRushSignal stepRushSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal globalSFXSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.UI.View.FTUEQuestFinished ftueQuestFinished { get; set; }

		[Inject]
		public global::Kampai.Game.DeliverTaskItemSignal deliverTaskItemSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.RushRevealBuildingSignal rushRevealBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ConstructionCompleteSignal constructionCompleteSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IGoToService goToService { get; set; }

		[Inject]
		public global::Kampai.Game.CreateMasterPlanComponentSignal createComponentSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CreateMasterPlanSignal createMasterPlanSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IMasterPlanService masterPlanService { get; set; }

		[Inject]
		public global::Kampai.Game.VillainLairModel villainLairModel { get; set; }

		[Inject]
		public global::Kampai.UI.View.UIModel uiModel { get; set; }

		public override void OnRegister()
		{
			view.Init(playerService, timeEventService, localService);
			view.taskPurchaseButton.ClickedSignal.AddListener(TaskButtonClicked);
			view.buildButton.ClickedSignal.AddListener(TaskButtonClicked);
			view.taskActionButton.ClickedSignal.AddListener(TaskButtonClicked);
			view.taskStateButton.ClickedSignal.AddListener(GoToClicked);
			view.taskIconImage.ClickedSignal.AddListener(GoToClicked);
			view.taskSecondaryImage.ClickedSignal.AddListener(GoToClicked);
			view.goToLairButton.ClickedSignal.AddListener(GoToClicked);
			constructionCompleteSignal.AddListener(ConstructionComplete);
			closeSignal.AddListener(OnCloseQuestBook);
			Init();
		}

		public override void OnRemove()
		{
			view.taskActionButton.ClickedSignal.RemoveListener(TaskButtonClicked);
			view.buildButton.ClickedSignal.RemoveListener(TaskButtonClicked);
			view.taskPurchaseButton.ClickedSignal.RemoveListener(TaskButtonClicked);
			view.taskStateButton.ClickedSignal.RemoveListener(GoToClicked);
			view.taskIconImage.ClickedSignal.RemoveListener(GoToClicked);
			view.taskSecondaryImage.ClickedSignal.RemoveListener(GoToClicked);
			view.goToLairButton.ClickedSignal.RemoveListener(GoToClicked);
			constructionCompleteSignal.RemoveListener(ConstructionComplete);
			closeSignal.RemoveListener(OnCloseQuestBook);
		}

		private void Init()
		{
			if (QuestControllerValidating())
			{
				view.SetupStepAction(questStepController.GetStepAction(localService));
				view.SetupStepDesc(questStepController.GetStepDescription(localService, definitionService));
				global::UnityEngine.Sprite mainSprite = null;
				global::UnityEngine.Sprite maskSprite = null;
				questStepController.GetStepDescIcon(definitionService, out mainSprite, out maskSprite);
				view.SetupTaskDescIcon(mainSprite, maskSprite, questStepController.StepType == global::Kampai.Game.QuestStepType.Leisure);
				UpdateQuestState();
				CheckSizing();
			}
		}

		private void CheckSizing()
		{
			global::Kampai.Game.BuildingRepairQuestStepController buildingRepairQuestStepController = questStepController as global::Kampai.Game.BuildingRepairQuestStepController;
			if (buildingRepairQuestStepController == null)
			{
				return;
			}
			if (questStepController.StepState == global::Kampai.Game.QuestStepState.Complete || questStepController.NeedActiveProgressBar)
			{
				if (view.taskIconFullSize)
				{
					view.ReducePanelWithIconSize();
				}
			}
			else
			{
				view.FillPanelWIthIcon();
			}
		}

		private bool QuestControllerValidating()
		{
			global::Kampai.Game.IQuestController obj = this.questController ?? questService.GetQuestControllerByInstanceID(view.questInstanceID);
			global::Kampai.Game.IQuestController questController = obj;
			this.questController = obj;
			if (questController == null)
			{
				logger.Error("Quest Controller Doesn't exist for instance {0}", view.questInstanceID);
				return false;
			}
			questStepController = questStepController ?? this.questController.GetStepController(view.stepNumber);
			if (questStepController == null)
			{
				logger.Error("Step Controller Doesn't Exist For Quest {0} Index {1}", this.questController.ID, view.stepNumber);
				return false;
			}
			return true;
		}

		private void ConstructionComplete(int instanceId)
		{
			if (QuestControllerValidating() && questStepController.StepInstanceTrackedID == instanceId && questStepController.StepType == global::Kampai.Game.QuestStepType.Construction)
			{
				view.rushProgressPanel.gameObject.SetActive(false);
				UpdateQuestState();
			}
		}

		private void UpdateQuestState()
		{
			if (QuestControllerValidating())
			{
				global::Kampai.Game.QuestStepState stepState = questStepController.StepState;
				view.UpdateTaskStateButton(questStepController, villainLairModel);
				bool needActiveProgressBar = questStepController.NeedActiveProgressBar;
				bool needActiveDeliverButton = questStepController.NeedActiveDeliverButton;
				view.UpdateDeliverButton(needActiveDeliverButton);
				view.progressBar.gameObject.SetActive(needActiveProgressBar);
				if (needActiveProgressBar)
				{
					view.UpdateProgressBar(questStepController.ProgressBarAmount, questStepController.ProgressBarTotal);
				}
				if (needActiveDeliverButton)
				{
					SetupDeliverButton();
				}
				else if (stepState == global::Kampai.Game.QuestStepState.WaitComplete)
				{
					view.UpdateTaskButton(false, true, 0, "Done");
				}
				if (questController != null && !questController.AreAllStepsComplete && stepState == global::Kampai.Game.QuestStepState.Complete)
				{
					globalSFXSignal.Dispatch("Play_completePartQuest_01");
				}
			}
		}

		private void SetupDeliverButton()
		{
			global::Kampai.Game.ItemDefinition definition;
			if (!definitionService.TryGet<global::Kampai.Game.ItemDefinition>(questStepController.ItemDefinitionID, out definition) && (questStepController.StepType == global::Kampai.Game.QuestStepType.MasterPlanComponentBuild || questStepController.StepType == global::Kampai.Game.QuestStepType.MasterPlanBuild))
			{
				view.UpdateTaskButton(false, true, 0, questStepController.DeliverButtonLocKey);
				if (!questStepController.NeedGoToButton)
				{
					view.ToggleGoToButton(false);
				}
				return;
			}
			int amountNeeded = questStepController.AmountNeeded;
			bool flag = amountNeeded > 0;
			int cost = (flag ? global::UnityEngine.Mathf.FloorToInt((float)amountNeeded * definition.BasePremiumCost) : 0);
			view.UpdateTaskButton(flag, true, cost, questStepController.DeliverButtonLocKey);
			if (flag)
			{
				view.purchaseDeliverLeftText.text = string.Format("+{0}", amountNeeded);
			}
			else
			{
				view.purchaseDeliverLeftText.gameObject.SetActive(false);
			}
			UIUtils.SetItemIcon(view.purchaseDeliverIconImage, definition);
			view.CheckIfItemIsOneOffCraftable(definition);
		}

		private void GoToClicked()
		{
			if (goToService != null && !uiModel.GoToClicked)
			{
				uiModel.GoToClicked = true;
				goToService.GoToClicked(view.step, view.stepDefinition, questController, view.stepNumber);
			}
		}

		private void ConstructionTransactionCallback(global::Kampai.Game.PendingCurrencyTransaction pct)
		{
			if (pct.Success)
			{
				globalSFXSignal.Dispatch("Play_button_premium_01");
				timeEventService.RushEvent(view.step.TrackedID);
				if (questController.GetStepController(view.stepNumber).StepState == global::Kampai.Game.QuestStepState.Ready)
				{
					questController.AutoGrantReward = true;
				}
				stepRushSignal.Dispatch(new global::Kampai.Util.Tuple<int, int>(questController.Definition.ID, view.stepNumber));
				UpdateQuestState();
				rushRevealBuildingSignal.Dispatch(view.step.TrackedID);
			}
		}

		private void TaskButtonClicked()
		{
			if ((!view.taskPurchaseButton.gameObject.activeSelf || view.taskPurchaseButton.isDoubleConfirmed()) && !OnMasterPlanQuestClick())
			{
				if (view.stepDefinition.Type == global::Kampai.Game.QuestStepType.Construction && view.step.state == global::Kampai.Game.QuestStepState.Inprogress)
				{
					playerService.ProcessRush(view.constructionRushCost, true, ConstructionTransactionCallback, view.stepDefinition.ItemDefinitionID);
				}
				else
				{
					deliverTaskItemSignal.Dispatch(new global::Kampai.Util.Tuple<int, int>(questController.ID, view.stepNumber));
					UpdateQuestState();
					ftueQuestFinished.Dispatch();
				}
				soundFXSignal.Dispatch("Play_button_click_01");
			}
		}

		private bool OnMasterPlanQuestClick()
		{
			global::strange.extensions.injector.api.ICrossContextInjectionBinder injectionBinder = gameContext.injectionBinder;
			if (view.stepDefinition.Type == global::Kampai.Game.QuestStepType.MasterPlanComponentBuild || view.stepDefinition.Type == global::Kampai.Game.QuestStepType.MasterPlanBuild)
			{
				global::Kampai.Game.MasterPlanQuestType.Component component = view.quest as global::Kampai.Game.MasterPlanQuestType.Component;
				if (component != null)
				{
					global::Kampai.Game.MasterPlanDefinition definition = component.masterPlan.Definition;
					closeSignal.Dispatch();
					if (component.component == null)
					{
						createMasterPlanSignal.Dispatch(definition);
						UpdateQuestState();
						injectionBinder.GetInstance<global::Kampai.Game.QuestHarvestableSignal>().Dispatch(view.quest);
						UpdateWayFinder();
						return true;
					}
					createComponentSignal.Dispatch(definition, component.index);
					UpdateQuestState();
					injectionBinder.GetInstance<global::Kampai.Game.QuestHarvestableSignal>().Dispatch(view.quest);
					UpdateWayFinder();
					return true;
				}
			}
			global::Kampai.Game.MasterPlanQuestStepController masterPlanQuestStepController = questStepController as global::Kampai.Game.MasterPlanQuestStepController;
			if (masterPlanQuestStepController != null)
			{
				global::Kampai.Game.MasterPlanComponent component2 = masterPlanQuestStepController.Component;
				if (view.taskPurchaseButton.gameObject.activeSelf)
				{
					injectionBinder.GetInstance<global::Kampai.Game.MasterPlanRushTaskSignal>().Dispatch(component2, view.stepNumber);
				}
				else
				{
					injectionBinder.GetInstance<global::Kampai.Game.MasterPlanTaskCompleteSignal>().Dispatch(component2, view.stepNumber);
				}
				gameContext.injectionBinder.GetInstance<global::Kampai.Game.MasterPlanComponentTaskUpdatedSignal>().Dispatch(component2);
				if (view.quest.state == global::Kampai.Game.QuestState.Harvestable)
				{
					injectionBinder.GetInstance<global::Kampai.Game.QuestHarvestableSignal>().Dispatch(view.quest);
				}
				UpdateQuestState();
				return true;
			}
			return false;
		}

		private void UpdateWayFinder()
		{
			masterPlanService.SetWayfinderState();
		}

		private void OnCloseQuestBook()
		{
			view.taskActionButton.GetComponent<global::UnityEngine.UI.Button>().interactable = false;
			view.buildButton.GetComponent<global::UnityEngine.UI.Button>().interactable = false;
			view.taskPurchaseButton.GetComponent<global::UnityEngine.UI.Button>().interactable = false;
			view.taskStateButton.GetComponent<global::UnityEngine.UI.Button>().interactable = false;
			view.taskIconImage.EnableClick(false);
			view.taskSecondaryImage.EnableClick(false);
			view.goToLairButton.GetComponent<global::UnityEngine.UI.Button>().interactable = false;
		}
	}
}
