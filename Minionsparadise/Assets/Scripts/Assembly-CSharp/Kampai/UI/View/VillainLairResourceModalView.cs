namespace Kampai.UI.View
{
	public class VillainLairResourceModalView : global::Kampai.UI.View.PopupMenuView
	{
		[global::UnityEngine.Header("Plot ID")]
		public global::UnityEngine.UI.Text title;

		public global::Kampai.UI.View.ButtonView prevButton;

		public global::Kampai.UI.View.ButtonView nextButton;

		[global::UnityEngine.Header("Resource Info")]
		public global::Kampai.UI.View.KampaiImage resourceItem;

		public global::UnityEngine.UI.Text productionDescription;

		public global::UnityEngine.UI.Text resourceItemAmt;

		[global::UnityEngine.Header("Call Minions")]
		public ScrollableButtonView callMinionButton;

		public global::UnityEngine.GameObject availableMinionsPanel;

		public global::UnityEngine.UI.Text idleMinionCount;

		public global::Kampai.UI.View.KampaiImage levelArrow;

		public global::UnityEngine.UI.Text minionLevel;

		[global::UnityEngine.Header("Rush and Timer")]
		public ScrollableButtonView rushButton;

		public global::UnityEngine.UI.Text rushCost;

		public global::UnityEngine.UI.Text clockTime;

		public global::UnityEngine.UI.Text rushText;

		public global::UnityEngine.GameObject clockPanel;

		public global::UnityEngine.GameObject clockIcon;

		public global::Kampai.UI.View.KampaiImage rushFillImage;

		public global::Kampai.UI.View.KampaiImage premiumIcon;

		[global::UnityEngine.Header("Collect")]
		public global::Kampai.UI.View.ButtonView collectButton;

		public global::Kampai.UI.View.KampaiImage collectButtonImage;

		public global::UnityEngine.GameObject collectPanel;

		[global::UnityEngine.Header("PartyBuff")]
		public global::UnityEngine.GameObject partyBuffPanel;

		public global::UnityEngine.UI.Text partyBuffAmt;

		public global::UnityEngine.GameObject clockInBuffIcon;

		public global::UnityEngine.RectTransform clockInBuffIconTransform;

		private int minionsNeeded = 1;

		internal int rushPrice;

		private global::UnityEngine.RuntimeAnimatorController harvestController;

		private global::UnityEngine.RuntimeAnimatorController purchaseController;

		internal void Setup()
		{
			if (harvestController == null)
			{
				harvestController = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>("asm_buttonClick_Harvest");
			}
			if (purchaseController == null)
			{
				purchaseController = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>("asm_buttonClick_Purchase");
			}
		}

		internal void SetResourcePlotTitle(string titleString)
		{
			title.text = titleString;
		}

		internal void SetResourceDescription(global::Kampai.Game.ItemDefinition itemDef, string desc)
		{
			productionDescription.text = desc;
			resourceItem.sprite = UIUtils.LoadSpriteFromPath(itemDef.Image);
			resourceItem.maskSprite = UIUtils.LoadSpriteFromPath(itemDef.Mask);
			collectButtonImage.sprite = resourceItem.sprite;
			collectButtonImage.maskSprite = resourceItem.maskSprite;
		}

		internal void SetResourceItemAmount(int itemAmount)
		{
			resourceItemAmt.text = itemAmount.ToString();
		}

		internal void EnableArrows(bool enable)
		{
			prevButton.gameObject.SetActive(enable);
			nextButton.gameObject.SetActive(enable);
		}

		internal void SetClockTimeAndRushCost(string timeRemaining, string rushCostAmt)
		{
			clockTime.text = timeRemaining;
			rushCost.text = rushCostAmt;
		}

		internal void SetPartyInfo(float boost, string boostString, bool isOn = true)
		{
			if (partyBuffPanel != null)
			{
				partyBuffAmt.text = boostString;
				bool flag = isOn && (int)(boost * 100f) != 100;
				partyBuffPanel.SetActive(flag);
				clockIcon.SetActive(!flag);
				clockInBuffIcon.SetActive(flag);
				if (flag)
				{
					global::UnityEngine.Vector3 originalScale;
					global::Kampai.Util.TweenUtil.Throb(clockInBuffIconTransform, 1.1f, 0.2f, out originalScale);
					UIUtils.FlashingColor(clockTime, 0);
					return;
				}
				Go.killAllTweensWithTarget(clockInBuffIconTransform);
				Go.killAllTweensWithTarget(clockTime);
				clockTime.color = global::UnityEngine.Color.white;
				clockInBuffIconTransform.localScale = global::UnityEngine.Vector3.one;
			}
		}

		internal void SetAvailableMinionInformation(int count)
		{
			idleMinionCount.text = count.ToString();
			if (count < minionsNeeded)
			{
				callMinionButton.Disable();
				callMinionButton.GetComponent<global::UnityEngine.UI.Button>().interactable = false;
				callMinionButton.enabled = false;
			}
			else
			{
				callMinionButton.ResetAnim();
				callMinionButton.GetComponent<global::UnityEngine.UI.Button>().interactable = true;
				callMinionButton.enabled = true;
			}
		}

		internal void SetMinionLevel(global::Kampai.Game.IPlayerService playerService)
		{
			int highestUntaskedMinionLevel = playerService.GetHighestUntaskedMinionLevel();
			levelArrow.gameObject.SetActive(highestUntaskedMinionLevel != 0);
			minionLevel.text = (highestUntaskedMinionLevel + 1).ToString();
		}

		internal void SetStateCallMinion()
		{
			EnableCallMinion(true);
			EnableRushAndClock(false);
			EnableCollect(false);
		}

		internal void SetStateRush()
		{
			EnableCallMinion(false);
			EnableRushAndClock(true);
			EnableCollect(false);
		}

		internal void SetStateFreeRush()
		{
			rushFillImage.color = global::Kampai.Util.GameConstants.UI.UI_ACTION_BUTTON_COLOR;
			rushButton.DisableDoubleConfirm();
			global::UnityEngine.Animator component = rushButton.gameObject.GetComponent<global::UnityEngine.Animator>();
			component.runtimeAnimatorController = harvestController;
			component.Play("Normal");
			rushCost.gameObject.SetActive(false);
			premiumIcon.gameObject.SetActive(false);
			rushText.gameObject.SetActive(true);
		}

		internal void SetStateCollect()
		{
			EnableCallMinion(false);
			EnableRushAndClock(false);
			EnableCollect(true);
		}

		private void EnableCallMinion(bool enable)
		{
			callMinionButton.gameObject.SetActive(enable);
			availableMinionsPanel.SetActive(enable);
		}

		private void EnableRushAndClock(bool enable)
		{
			if (enable)
			{
				rushFillImage.color = global::Kampai.Util.GameConstants.UI.UI_PURCHASE_BUTTON_COLOR;
				global::UnityEngine.Animator component = rushButton.gameObject.GetComponent<global::UnityEngine.Animator>();
				component.runtimeAnimatorController = purchaseController;
				component.Play("Normal");
				rushCost.gameObject.SetActive(true);
				premiumIcon.gameObject.SetActive(true);
				rushText.gameObject.SetActive(false);
				rushButton.EnableDoubleConfirm();
				rushButton.ResetAnim();
			}
			rushButton.gameObject.SetActive(enable);
			clockPanel.gameObject.SetActive(enable);
		}

		private void EnableCollect(bool enable)
		{
			collectButton.gameObject.SetActive(enable);
			collectPanel.gameObject.SetActive(enable);
		}
	}
}
