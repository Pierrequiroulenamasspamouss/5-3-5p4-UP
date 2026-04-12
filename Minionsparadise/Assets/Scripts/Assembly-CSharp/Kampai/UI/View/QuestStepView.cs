namespace Kampai.UI.View
{
	public class QuestStepView : global::Kampai.Util.KampaiView
	{
		[global::UnityEngine.Header("Task Name / Icon")]
		public global::UnityEngine.UI.Text taskNameText;

		public global::UnityEngine.UI.Text taskActionText;

		public global::Kampai.UI.View.KampaiClickableImage taskIconImage;

		public global::Kampai.UI.View.KampaiClickableImage taskSecondaryImage;

		public global::UnityEngine.Vector2 taskIconFullSizeMin;

		public global::UnityEngine.Vector2 taskIconFullSizeMax;

		private global::UnityEngine.Vector2 originalTaskIconMinAnchor;

		private global::UnityEngine.Vector2 originalTaskIconMaxAnchor;

		[global::UnityEngine.Header("Task State Button")]
		public ScrollableButtonView taskStateButton;

		[global::UnityEngine.Header("Deliver/Harvest Panel")]
		public global::UnityEngine.GameObject deliverPanel;

		public global::Kampai.UI.View.KampaiImage purchaseDeliverIconImage;

		public global::UnityEngine.UI.Text purchaseDeliverLeftText;

		[global::UnityEngine.Header("Rush Progress Panel")]
		public global::UnityEngine.RectTransform rushProgressPanel;

		public global::UnityEngine.UI.Text rushProgressText;

		[global::UnityEngine.Header("Progress Bar")]
		public global::UnityEngine.RectTransform progressBar;

		public global::UnityEngine.UI.Image progressFill;

		public global::UnityEngine.UI.Text progressText;

		[global::UnityEngine.Header("Task")]
		public ScrollableButtonView taskPurchaseButton;

		public ScrollableButtonView taskActionButton;

		public global::UnityEngine.UI.Image taskButtonCurrencyImage;

		public global::UnityEngine.UI.Text taskButtonCostText;

		public global::UnityEngine.UI.Text taskButtonCompleteText;

		[global::UnityEngine.Header("Complete")]
		public global::UnityEngine.UI.Text completeText;

		[global::UnityEngine.Header("Build Master Plan")]
		public ScrollableButtonView buildButton;

		public ScrollableButtonView goToLairButton;

		internal int stepNumber;

		internal int questInstanceID;

		internal global::Kampai.Game.Quest quest;

		internal global::Kampai.Game.QuestStep step;

		internal global::Kampai.Game.QuestStepDefinition stepDefinition;

		internal int constructionRushCost;

		internal int collectionRushCost;

		internal int collectionAmountNeeded;

		private global::Kampai.Game.IPlayerService playerService;

		private global::Kampai.Game.ITimeEventService timeEventService;

		private global::Kampai.Main.ILocalizationService localService;

		private global::UnityEngine.Vector3 originalGoToScale;

		private global::UnityEngine.Vector3 originalDeliverScale;

		private bool hideTaskButton;

		internal bool taskIconFullSize;

		internal void Init(global::Kampai.Game.IPlayerService playerService, global::Kampai.Game.ITimeEventService timeEventService, global::Kampai.Main.ILocalizationService localService)
		{
			global::UnityEngine.RectTransform rectTransform = taskIconImage.rectTransform;
			originalTaskIconMinAnchor = rectTransform.anchorMin;
			originalTaskIconMaxAnchor = rectTransform.anchorMax;
			this.playerService = playerService;
			this.timeEventService = timeEventService;
			this.localService = localService;
			rushProgressPanel.gameObject.SetActive(false);
			progressBar.gameObject.SetActive(true);
			taskActionButton.gameObject.SetActive(false);
			taskPurchaseButton.gameObject.SetActive(false);
			completeText.gameObject.SetActive(false);
			buildButton.gameObject.SetActive(false);
			goToLairButton.gameObject.SetActive(false);
			originalGoToScale = taskStateButton.transform.localScale;
			originalDeliverScale = taskActionButton.transform.localScale;
			taskPurchaseButton.EnableDoubleConfirm();
		}

		internal void FillPanelWIthIcon()
		{
			global::UnityEngine.RectTransform rectTransform = taskIconImage.rectTransform;
			rectTransform.anchorMin = taskIconFullSizeMin;
			rectTransform.anchorMax = taskIconFullSizeMax;
			taskIconFullSize = true;
		}

		internal void ReducePanelWithIconSize()
		{
			if (taskIconFullSize)
			{
				global::UnityEngine.RectTransform rectTransform = taskIconImage.rectTransform;
				rectTransform.anchorMin = originalTaskIconMinAnchor;
				rectTransform.anchorMax = originalTaskIconMaxAnchor;
			}
		}

		internal void SetTaskButtonState(bool hideTaskButton)
		{
			this.hideTaskButton = hideTaskButton;
		}

		public void SetupTaskDescIcon(global::UnityEngine.Sprite mainSprite, global::UnityEngine.Sprite maskSprite, bool isLeisure = false)
		{
			taskIconImage.sprite = mainSprite;
			taskIconImage.maskSprite = maskSprite;
			taskSecondaryImage.gameObject.SetActive(isLeisure);
			if (isLeisure)
			{
				taskSecondaryImage.sprite = UIUtils.LoadSpriteFromPath("img_throwparty_fill");
				taskSecondaryImage.maskSprite = UIUtils.LoadSpriteFromPath("img_throwparty_mask");
			}
		}

		public void Update()
		{
			if (step.TrackedID != 0 && step.state == global::Kampai.Game.QuestStepState.Inprogress && stepDefinition.Type == global::Kampai.Game.QuestStepType.Construction && stepDefinition.ItemAmount == 1)
			{
				int timeRemaining = timeEventService.GetTimeRemaining(step.TrackedID);
				if (UpdateRushProgressBar(timeRemaining))
				{
					int eventDuration = timeEventService.GetEventDuration(step.TrackedID);
					int num = eventDuration - timeRemaining;
					UpdateProgressBar(num, eventDuration, true);
				}
			}
		}

		internal void SetupStepAction(string actionText)
		{
			taskActionText.text = actionText;
		}

		internal void SetupStepDesc(string desc)
		{
			taskNameText.text = desc;
		}

		private void EnableStateButton(bool enable)
		{
			bool flag = EnableProgressiveGotoButton(enable);
			taskStateButton.gameObject.SetActive(flag);
			taskIconImage.EnableClick(flag);
			taskSecondaryImage.EnableClick(flag);
		}

		private int GetCompleteStepCount()
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.QuestStep> steps = quest.Steps;
			for (int num = steps.Count - 1; num >= 0; num--)
			{
				global::Kampai.Game.QuestStep questStep = steps[num];
				if (questStep.state == global::Kampai.Game.QuestStepState.Complete)
				{
					return num + 1;
				}
			}
			return 0;
		}

		private bool EnableProgressiveGotoButton(bool enable)
		{
			if (!quest.Definition.ProgressiveGoto)
			{
				return enable;
			}
			if (stepNumber == GetCompleteStepCount())
			{
				return enable;
			}
			return false;
		}

		public void UpdateTaskStateButton(global::Kampai.Game.IQuestStepController questStepController, global::Kampai.Game.VillainLairModel villainLairModel)
		{
			EnableStateButton(true);
			switch (questStepController.StepState)
			{
			case global::Kampai.Game.QuestStepState.Notstarted:
				EnableStateButton(true);
				break;
			case global::Kampai.Game.QuestStepState.Inprogress:
				break;
			case global::Kampai.Game.QuestStepState.Ready:
				UpdateTaskReadyStateButton(questStepController, villainLairModel);
				break;
			case global::Kampai.Game.QuestStepState.WaitComplete:
				ToggleGoToButton(false);
				break;
			case global::Kampai.Game.QuestStepState.Complete:
				EnableStateButton(false);
				completeText.text = localService.GetString("Complete");
				completeText.gameObject.SetActive(true);
				break;
			case global::Kampai.Game.QuestStepState.RunningStartScript:
			case global::Kampai.Game.QuestStepState.RunningCompleteScript:
				break;
			}
		}

		private void UpdateTaskReadyStateButton(global::Kampai.Game.IQuestStepController questStepController, global::Kampai.Game.VillainLairModel villainLairModel)
		{
			switch (questStepController.StepType)
			{
			case global::Kampai.Game.QuestStepType.Delivery:
				EnableStateButton(false);
				break;
			case global::Kampai.Game.QuestStepType.Construction:
				completeText.text = localService.GetString("Ready");
				completeText.gameObject.SetActive(true);
				break;
			case global::Kampai.Game.QuestStepType.MasterPlanComponentBuild:
			case global::Kampai.Game.QuestStepType.MasterPlanBuild:
				if (villainLairModel.currentActiveLair != null)
				{
					buildButton.gameObject.SetActive(true);
					goToLairButton.gameObject.SetActive(false);
					taskStateButton.gameObject.SetActive(false);
				}
				else
				{
					taskStateButton.gameObject.SetActive(true);
					buildButton.gameObject.SetActive(false);
					goToLairButton.gameObject.SetActive(true);
				}
				break;
			case global::Kampai.Game.QuestStepType.MasterPlanTask:
				ToggleGoToButton(questStepController.NeedGoToButton);
				break;
			}
		}

		public void ToggleGoToButton(bool isEnabled)
		{
			bool active = EnableProgressiveGotoButton(isEnabled);
			taskStateButton.gameObject.SetActive(active);
		}

		public void UpdateTaskButton(bool isPremium, bool show = true, int cost = 0, string locKey = null)
		{
			if (isPremium)
			{
				taskActionButton.gameObject.SetActive(false);
			}
			else
			{
				taskPurchaseButton.gameObject.SetActive(false);
			}
			if (!show || hideTaskButton)
			{
				taskPurchaseButton.gameObject.SetActive(false);
				taskActionButton.gameObject.SetActive(false);
			}
			else if (!isPremium)
			{
				taskActionButton.gameObject.SetActive(true);
				taskButtonCompleteText.text = localService.GetString(locKey);
				taskButtonCompleteText.gameObject.SetActive(true);
				taskButtonCostText.gameObject.SetActive(false);
				taskButtonCurrencyImage.gameObject.SetActive(false);
			}
			else
			{
				taskPurchaseButton.gameObject.SetActive(true);
				taskButtonCostText.text = cost.ToString();
				taskButtonCurrencyImage.gameObject.SetActive(true);
				taskButtonCompleteText.gameObject.SetActive(false);
				taskButtonCostText.gameObject.SetActive(true);
			}
		}

		public void UpdateDeliverButton(bool isActive)
		{
			deliverPanel.gameObject.SetActive(isActive);
			if (!isActive)
			{
				UpdateTaskButton(false, false);
			}
		}

		public void CheckIfItemIsOneOffCraftable(global::Kampai.Game.ItemDefinition itemDef)
		{
			global::Kampai.Game.DynamicIngredientsDefinition dynamicIngredientsDefinition = itemDef as global::Kampai.Game.DynamicIngredientsDefinition;
			if (dynamicIngredientsDefinition == null)
			{
				return;
			}
			global::System.Collections.Generic.ICollection<global::Kampai.Game.CraftingBuilding> byDefinitionId = playerService.GetByDefinitionId<global::Kampai.Game.CraftingBuilding>(dynamicIngredientsDefinition.CraftingBuildingId);
			foreach (global::Kampai.Game.CraftingBuilding item in byDefinitionId)
			{
				if (item.RecipeInQueue.Contains(itemDef.ID) || item.CompletedCrafts.Contains(itemDef.ID))
				{
					taskPurchaseButton.gameObject.SetActive(false);
					taskActionButton.gameObject.SetActive(false);
					break;
				}
			}
		}

		private bool UpdateRushProgressBar(int timeRemaining)
		{
			if (rushProgressPanel == null || rushProgressText == null)
			{
				return false;
			}
			rushProgressPanel.gameObject.SetActive(true);
			if (timeRemaining < 0)
			{
				UpdateTaskButton(false, false);
				rushProgressText.text = UIUtils.FormatTime(0, localService);
				return false;
			}
			rushProgressText.text = UIUtils.FormatTime(timeRemaining, localService);
			constructionRushCost = timeEventService.CalculateRushCostForTimer(timeRemaining, global::Kampai.Game.RushActionType.CONSTRUCTION);
			int cost = constructionRushCost;
			UpdateTaskButton(true, true, cost);
			return true;
		}

		public void UpdateProgressBar(float progress, float complete, bool isTimer = false)
		{
			float num = global::UnityEngine.Mathf.Min(progress, complete);
			float num2 = num / complete;
			if (isTimer)
			{
				progressText.text = string.Format("{0}%", (int)(num2 * 100f));
			}
			else
			{
				progressText.text = localService.GetString("OfComplete", num, complete);
				progressText.text = progressText.text.Replace(" ", string.Empty);
			}
			progressFill.rectTransform.anchorMax = new global::UnityEngine.Vector2(num2, 1f);
		}

		internal void HighlightGoTo(bool isHighlighted)
		{
			global::UnityEngine.Animator[] componentsInChildren = taskStateButton.GetComponentsInChildren<global::UnityEngine.Animator>();
			if (isHighlighted)
			{
				global::UnityEngine.Animator[] array = componentsInChildren;
				foreach (global::UnityEngine.Animator animator in array)
				{
					animator.enabled = false;
				}
				global::Kampai.Util.TweenUtil.Throb(taskStateButton.transform, 0.85f, 0.5f, out originalGoToScale);
				return;
			}
			Go.killAllTweensWithTarget(taskStateButton.transform);
			taskStateButton.transform.localScale = originalGoToScale;
			global::UnityEngine.Animator[] array2 = componentsInChildren;
			foreach (global::UnityEngine.Animator animator2 in array2)
			{
				animator2.enabled = true;
			}
		}

		internal void HighlightDeliver(bool isHighlighted)
		{
			global::UnityEngine.Animator[] componentsInChildren = taskActionButton.GetComponentsInChildren<global::UnityEngine.Animator>();
			if (isHighlighted)
			{
				global::UnityEngine.Animator[] array = componentsInChildren;
				foreach (global::UnityEngine.Animator animator in array)
				{
					animator.enabled = false;
				}
				global::Kampai.Util.TweenUtil.Throb(taskActionButton.transform, 0.85f, 0.5f, out originalDeliverScale);
				return;
			}
			Go.killAllTweensWithTarget(taskActionButton.transform);
			taskActionButton.transform.localScale = originalDeliverScale;
			global::UnityEngine.Animator[] array2 = componentsInChildren;
			foreach (global::UnityEngine.Animator animator2 in array2)
			{
				animator2.enabled = true;
			}
		}
	}
}
