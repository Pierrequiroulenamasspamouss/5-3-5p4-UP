namespace Kampai.UI.View
{
	public class MinionSliderView : global::Kampai.Util.KampaiView
	{
		public global::UnityEngine.GameObject ClockPanel;

		public global::UnityEngine.GameObject AvailableMinionPanel;

		public global::UnityEngine.GameObject lockedPanel;

		public global::UnityEngine.GameObject HarvestPanel;

		public global::UnityEngine.GameObject PartyIcon;

		public global::UnityEngine.GameObject ClockIcon;

		public global::UnityEngine.Transform ClockGroupForPulsing;

		public global::UnityEngine.UI.Text durationText;

		public global::UnityEngine.UI.Text costText;

		public global::UnityEngine.UI.Text minionCount;

		public global::UnityEngine.UI.Text lockedText;

		public global::UnityEngine.UI.Text lockedCostText;

		public global::UnityEngine.UI.Text availableText;

		public global::UnityEngine.UI.Text rushText;

		public global::UnityEngine.UI.Text confirmText;

		public global::UnityEngine.UI.Text minionLevel;

		public global::Kampai.UI.View.KampaiImage buttonImage;

		public global::Kampai.UI.View.KampaiImage buttonFillImage;

		public global::Kampai.UI.View.KampaiImage costImage;

		public global::Kampai.UI.View.KampaiImage rushCostImage;

		public global::Kampai.UI.View.KampaiImage rushFillImage;

		public global::Kampai.UI.View.KampaiImage levelArrow;

		public ScrollableButtonView rushButton;

		public ScrollableButtonView callButton;

		public UnlockableScrollableButtonView lockedButton;

		public ScrollableButtonView harvestButton;

		public global::Kampai.Game.ResourceBuilding building;

		public global::Kampai.Game.VillainLairResourcePlot resourcePlot;

		public bool isResourcePlotSlider;

		public float PaddingInPixels;

		internal global::strange.extensions.signal.impl.Signal completeSignal = new global::strange.extensions.signal.impl.Signal();

		internal global::Kampai.Game.IPlayerService playerService;

		internal int identifier;

		internal int minionID;

		internal bool isLockedHighlighted;

		internal global::Kampai.UI.View.MinionSliderState state;

		public double startTime;

		private uint rushCost;

		private int harvestTime;

		private int count;

		public global::UnityEngine.Color lockedTextColor;

		public global::UnityEngine.Color lockedOverlayColorWithAlpha;

		private bool completed;

		private bool isPartying;

		internal bool isCorrectBuffType;

		private global::Kampai.Main.ILocalizationService localService;

		private global::Kampai.Game.IDefinitionService definitionService;

		private global::UnityEngine.Vector3 originalScale = new global::UnityEngine.Vector3(1f, 1f, 1f);

		private global::Kampai.UI.View.ModalSettings modalSettings;

		private static global::UnityEngine.RuntimeAnimatorController harvestController;

		private static global::UnityEngine.RuntimeAnimatorController purchaseController;

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		internal void Init(global::Kampai.Main.ILocalizationService localService, global::Kampai.Game.IDefinitionService definitionService)
		{
			this.localService = localService;
			this.definitionService = definitionService;
			SetPartyState(false);
			UpdateHarvestTime();
			setMinionText();
			lockedButton.EnableDoubleConfirm();
			if (harvestController == null)
			{
				harvestController = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>("asm_buttonClick_Harvest");
			}
			if (purchaseController == null)
			{
				purchaseController = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>("asm_buttonClick_Purchase");
			}
		}

		internal void UpdateHarvestTime()
		{
			if (definitionService != null)
			{
				if (isResourcePlotSlider)
				{
					harvestTime = resourcePlot.parentLair.Definition.SecondsToHarvest;
				}
				else
				{
					harvestTime = BuildingUtil.GetHarvestTimeForTaskableBuilding(building, definitionService);
				}
			}
		}

		internal void UpdateLockedButton()
		{
			if ((!isResourcePlotSlider) ? ((identifier == 2 && building.MinionSlotsOwned < 2) || !modalSettings.enableLockedButtons) : (resourcePlot.State == global::Kampai.Game.BuildingState.Inaccessible))
			{
				lockedButton.gameObject.GetComponent<global::UnityEngine.UI.Button>().interactable = false;
				lockedText.color = lockedTextColor;
				lockedCostText.color = lockedTextColor;
				buttonFillImage.Overlay = lockedOverlayColorWithAlpha;
				costImage.Overlay = lockedOverlayColorWithAlpha;
			}
			else
			{
				lockedText.color = global::UnityEngine.Color.white;
				lockedCostText.color = global::UnityEngine.Color.white;
				buttonFillImage.Overlay = global::UnityEngine.Color.clear;
				costImage.Overlay = global::UnityEngine.Color.clear;
				lockedButton.gameObject.GetComponent<global::UnityEngine.UI.Button>().interactable = true;
			}
		}

		internal void SetMinionLevel()
		{
			int highestUntaskedMinionLevel = playerService.GetHighestUntaskedMinionLevel();
			levelArrow.gameObject.SetActive(highestUntaskedMinionLevel != 0);
			minionLevel.text = (highestUntaskedMinionLevel + 1).ToString();
		}

		internal void SetIdleMinionCount()
		{
			count = playerService.GetIdleMinions().Count;
			minionCount.text = count.ToString();
			if (state == global::Kampai.UI.View.MinionSliderState.Harvestable)
			{
				harvestButton.GetComponent<global::UnityEngine.UI.Button>().interactable = modalSettings.enableHarvestButtons;
			}
			else
			{
				harvestButton.GetComponent<global::UnityEngine.UI.Button>().interactable = false;
			}
			if (state == global::Kampai.UI.View.MinionSliderState.Available)
			{
				SetCallButtonState();
			}
		}

		private void SetCallButtonState()
		{
			if (count == 0)
			{
				SetCallHighlight(false);
				callButton.Disable();
				callButton.GetComponent<global::UnityEngine.UI.Button>().interactable = false;
			}
			else
			{
				callButton.ResetAnim();
				callButton.GetComponent<global::UnityEngine.UI.Button>().interactable = modalSettings.enableCallButtons;
			}
		}

		public void Update()
		{
			if (minionID == -1)
			{
				return;
			}
			int num = 0;
			num = ((!isResourcePlotSlider) ? timeEventService.GetTimeRemaining(minionID) : timeEventService.GetTimeRemaining(resourcePlot.ID));
			bool flag = playerService.GetMinionPartyInstance().IsBuffHappening && isCorrectBuffType;
			if (isPartying != flag)
			{
				SetPartyState(flag);
			}
			if (num > harvestTime)
			{
				durationText.text = UIUtils.FormatTime(harvestTime, localService);
				rushCost = (uint)timeEventService.CalculateRushCostForTimer(harvestTime, global::Kampai.Game.RushActionType.HARVESTING);
				costText.text = string.Format("{0}", rushCost);
				return;
			}
			if (!isResourcePlotSlider)
			{
				if (num == -1 && building.State != global::Kampai.Game.BuildingState.Working)
				{
					completed = true;
				}
			}
			else if (num <= 0 && resourcePlot.State == global::Kampai.Game.BuildingState.Working)
			{
				completed = true;
			}
			string text = UIUtils.FormatTime(num, localService);
			if (num > -1 && !completed)
			{
				durationText.text = text;
				rushCost = (uint)timeEventService.CalculateRushCostForTimer(num, global::Kampai.Game.RushActionType.HARVESTING);
				if (rushCost == 0 && state != global::Kampai.UI.View.MinionSliderState.Rushable)
				{
					SetMinionSliderState(global::Kampai.UI.View.MinionSliderState.Rushable);
				}
				else if (rushCost != 0)
				{
					costText.text = string.Format("{0}", rushCost);
				}
				if (num <= 0)
				{
					completed = true;
				}
			}
			if (completed)
			{
				ClearSlot();
				completeSignal.Dispatch();
				completed = false;
			}
		}

		internal void SetPartyState(bool isPartying)
		{
			this.isPartying = isPartying;
			ClockIcon.SetActive(!isPartying);
			PartyIcon.SetActive(isPartying);
			if (isPartying)
			{
				global::UnityEngine.Vector3 vector;
				global::Kampai.Util.TweenUtil.Throb(ClockGroupForPulsing, 1.1f, 0.2f, out vector);
				UIUtils.FlashingColor(durationText, 0);
				return;
			}
			Go.killAllTweensWithTarget(ClockGroupForPulsing);
			Go.killAllTweensWithTarget(durationText);
			durationText.color = global::UnityEngine.Color.white;
			ClockGroupForPulsing.localScale = global::UnityEngine.Vector3.one;
		}

		internal void ClearSlot()
		{
			costText.text = string.Empty;
			minionID = -1;
			SetMinionSliderState(global::Kampai.UI.View.MinionSliderState.Harvestable);
		}

		internal void CallMinion()
		{
			if (isResourcePlotSlider)
			{
				harvestTime = resourcePlot.parentLair.Definition.SecondsToHarvest;
			}
			else
			{
				harvestTime = BuildingUtil.GetHarvestTimeForTaskableBuilding(building, definitionService);
			}
			durationText.text = UIUtils.FormatTime(harvestTime, localService);
			rushCost = (uint)timeEventService.CalculateRushCostForTimer(harvestTime, global::Kampai.Game.RushActionType.HARVESTING);
			if (rushCost == 0)
			{
				SetMinionSliderState(global::Kampai.UI.View.MinionSliderState.Rushable);
				return;
			}
			costText.text = string.Format("{0}", rushCost);
			SetMinionSliderState(global::Kampai.UI.View.MinionSliderState.Working);
		}

		internal void ChangeMinionCount(bool increase)
		{
			if (increase)
			{
				count++;
			}
			else
			{
				count--;
			}
			minionCount.text = count.ToString();
			SetCallButtonState();
		}

		internal void setMinionText()
		{
			availableText.text = localService.GetString("ResourceAvailable");
		}

		internal void PurchaseSlot()
		{
			SetMinionSliderState(global::Kampai.UI.View.MinionSliderState.Available);
			minionID = -1;
		}

		internal void SetMinionSliderState(global::Kampai.UI.View.MinionSliderState i_state)
		{
			state = i_state;
			switch (state)
			{
			case global::Kampai.UI.View.MinionSliderState.Working:
				SetSliderWorking();
				break;
			case global::Kampai.UI.View.MinionSliderState.Available:
				SetMinionLevel();
				SetSliderAvailable();
				break;
			case global::Kampai.UI.View.MinionSliderState.Locked:
				SetSliderLocked();
				break;
			case global::Kampai.UI.View.MinionSliderState.Harvestable:
				SetSliderHarvestable();
				break;
			case global::Kampai.UI.View.MinionSliderState.Rushable:
				SetSliderRushable();
				break;
			}
			SetIdleMinionCount();
		}

		private void SetSliderAvailable()
		{
			callButton.gameObject.SetActive(true);
			harvestButton.gameObject.SetActive(false);
			ResetAndHideRushButton();
			ClockPanel.SetActive(false);
			AvailableMinionPanel.SetActive(true);
			ResetAndHideLockedButton();
			HarvestPanel.SetActive(false);
			if (modalSettings.enableCallThrob)
			{
				SetCallHighlight(true);
			}
		}

		private void SetSliderLocked()
		{
			callButton.gameObject.SetActive(false);
			ResetAndHideRushButton();
			harvestButton.gameObject.SetActive(false);
			ClockPanel.SetActive(false);
			AvailableMinionPanel.SetActive(false);
			lockedPanel.SetActive(true);
			HarvestPanel.SetActive(false);
			if (modalSettings.enableLockedThrob && lockedButton.GetComponent<global::UnityEngine.UI.Button>().interactable)
			{
				SetLockedHighlight(true);
			}
		}

		private void SetSliderHarvestable()
		{
			harvestButton.gameObject.SetActive(true);
			callButton.gameObject.SetActive(false);
			ResetAndHideRushButton();
			ClockPanel.SetActive(false);
			AvailableMinionPanel.SetActive(false);
			ResetAndHideLockedButton();
			HarvestPanel.SetActive(true);
			harvestButton.GetComponent<global::UnityEngine.UI.Button>().interactable = modalSettings.enableHarvestButtons;
		}

		private void SetSliderWorking()
		{
			harvestButton.gameObject.SetActive(false);
			callButton.gameObject.SetActive(false);
			rushButton.gameObject.SetActive(true);
			rushButton.EnableDoubleConfirm();
			ClockPanel.SetActive(true);
			AvailableMinionPanel.SetActive(false);
			ResetAndHideLockedButton();
			HarvestPanel.SetActive(false);
			rushButton.GetComponent<global::UnityEngine.UI.Button>().interactable = modalSettings.enableRushButtons;
			if (modalSettings.enableRushThrob)
			{
				SetRushHighlight(true);
			}
			rushFillImage.color = global::Kampai.Util.GameConstants.UI.UI_PURCHASE_BUTTON_COLOR;
			rushButton.EnableDoubleConfirm();
			global::UnityEngine.Animator component = rushButton.gameObject.GetComponent<global::UnityEngine.Animator>();
			component.runtimeAnimatorController = purchaseController;
			component.Play("Normal");
			rushCostImage.gameObject.SetActive(true);
			rushText.gameObject.SetActive(false);
			costText.gameObject.SetActive(true);
		}

		private void SetSliderRushable()
		{
			harvestButton.gameObject.SetActive(false);
			callButton.gameObject.SetActive(false);
			rushButton.gameObject.SetActive(true);
			rushButton.EnableDoubleConfirm();
			ClockPanel.SetActive(true);
			AvailableMinionPanel.SetActive(false);
			ResetAndHideLockedButton();
			HarvestPanel.SetActive(false);
			rushButton.GetComponent<global::UnityEngine.UI.Button>().interactable = modalSettings.enableRushButtons;
			rushFillImage.color = global::Kampai.Util.GameConstants.UI.UI_ACTION_BUTTON_COLOR;
			rushButton.DisableDoubleConfirm();
			global::UnityEngine.Animator component = rushButton.gameObject.GetComponent<global::UnityEngine.Animator>();
			component.runtimeAnimatorController = harvestController;
			component.Play("Normal");
			rushCostImage.gameObject.SetActive(false);
			rushText.gameObject.SetActive(true);
			costText.gameObject.SetActive(false);
			confirmText.gameObject.SetActive(false);
		}

		private void ResetAndHideRushButton()
		{
			rushButton.ResetAnim();
			StartCoroutine(WaitAFrame(rushButton.gameObject));
		}

		private void ResetAndHideLockedButton()
		{
			lockedButton.ResetAnim();
			StartCoroutine(WaitAFrame(lockedPanel));
		}

		private global::System.Collections.IEnumerator WaitAFrame(global::UnityEngine.GameObject go)
		{
			yield return new global::UnityEngine.WaitForEndOfFrame();
			if (go != null)
			{
				go.SetActive(false);
			}
		}

		internal void SetRushHighlight(bool isHighlighted)
		{
			if (!rushButton.enabled)
			{
				return;
			}
			isLockedHighlighted = true;
			global::UnityEngine.Animator[] componentsInChildren = rushButton.GetComponentsInChildren<global::UnityEngine.Animator>();
			if (isHighlighted)
			{
				global::UnityEngine.Animator[] array = componentsInChildren;
				foreach (global::UnityEngine.Animator animator in array)
				{
					animator.enabled = false;
				}
				global::Kampai.Util.TweenUtil.Throb(rushButton.transform, 0.85f, 0.5f, out originalScale);
				return;
			}
			isLockedHighlighted = false;
			Go.killAllTweensWithTarget(rushButton.transform);
			rushButton.transform.localScale = originalScale;
			global::UnityEngine.Animator[] array2 = componentsInChildren;
			foreach (global::UnityEngine.Animator animator2 in array2)
			{
				animator2.enabled = true;
			}
		}

		internal void SetCallHighlight(bool isHighlighted)
		{
			if (!callButton.enabled)
			{
				return;
			}
			isLockedHighlighted = true;
			global::UnityEngine.Animator[] componentsInChildren = callButton.GetComponentsInChildren<global::UnityEngine.Animator>();
			if (isHighlighted)
			{
				global::UnityEngine.Animator[] array = componentsInChildren;
				foreach (global::UnityEngine.Animator animator in array)
				{
					animator.enabled = false;
				}
				global::Kampai.Util.TweenUtil.Throb(callButton.transform, 0.85f, 0.5f, out originalScale);
				return;
			}
			isLockedHighlighted = false;
			Go.killAllTweensWithTarget(callButton.transform);
			callButton.transform.localScale = originalScale;
			global::UnityEngine.Animator[] array2 = componentsInChildren;
			foreach (global::UnityEngine.Animator animator2 in array2)
			{
				animator2.enabled = true;
			}
		}

		internal void SetLockedHighlight(bool isHighlighted)
		{
			if (!lockedButton.enabled)
			{
				return;
			}
			isLockedHighlighted = true;
			global::UnityEngine.Animator[] componentsInChildren = lockedButton.GetComponentsInChildren<global::UnityEngine.Animator>();
			if (isHighlighted)
			{
				global::UnityEngine.Animator[] array = componentsInChildren;
				foreach (global::UnityEngine.Animator animator in array)
				{
					animator.enabled = false;
				}
				global::Kampai.Util.TweenUtil.Throb(lockedButton.transform, 0.85f, 0.5f, out originalScale);
				return;
			}
			isLockedHighlighted = false;
			Go.killAllTweensWithTarget(lockedButton.transform);
			lockedButton.transform.localScale = originalScale;
			global::UnityEngine.Animator[] array2 = componentsInChildren;
			foreach (global::UnityEngine.Animator animator2 in array2)
			{
				animator2.enabled = true;
			}
		}

		internal uint GetRushCost()
		{
			return rushCost;
		}

		internal void SetRushCost()
		{
			if (!isResourcePlotSlider)
			{
				rushCost = (uint)building.Definition.RushCost;
			}
		}

		internal int GetPurchaseCost()
		{
			if (isResourcePlotSlider)
			{
				return -1;
			}
			return building.GetSlotCostByIndex(identifier);
		}

		internal void SetModalSettings(global::Kampai.UI.View.ModalSettings modalSettings)
		{
			this.modalSettings = modalSettings;
		}
	}
}
