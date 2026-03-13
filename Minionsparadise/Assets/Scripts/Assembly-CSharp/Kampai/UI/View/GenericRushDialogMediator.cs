namespace Kampai.UI.View
{
	public class GenericRushDialogMediator : global::Kampai.UI.View.RushDialogMediator<global::Kampai.UI.View.GenericRushDialogView>
	{
		private readonly global::Kampai.Game.AdPlacementName adPlacementName = global::Kampai.Game.AdPlacementName.MISSING_RESOURCES;

		private global::Kampai.Game.AdPlacementInstance adPlacementInstance;

		[Inject]
		public global::Kampai.Game.IRewardedAdService rewardedAdService { get; set; }

		[Inject]
		public global::Kampai.Game.RewardedAdRewardSignal rewardedAdRewardSignal { get; set; }

		[Inject]
		public global::Kampai.Game.AdPlacementActivityStateChangedSignal adPlacementActivityStateChangedSignal { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			base.view.AdVideoButton.ClickedSignal.AddListener(AdVideoButtonClicked);
			rewardedAdRewardSignal.AddListener(OnRewardedAdReward);
			adPlacementActivityStateChangedSignal.AddListener(OnAdPlacementActivityStateChanged);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			base.view.AdVideoButton.ClickedSignal.RemoveListener(AdVideoButtonClicked);
			rewardedAdRewardSignal.RemoveListener(OnRewardedAdReward);
			adPlacementActivityStateChangedSignal.RemoveListener(OnAdPlacementActivityStateChanged);
		}

		protected override void SetHeadline(global::Kampai.Game.PendingCurrencyTransaction pct)
		{
			string transactionItemName = global::Kampai.Game.Transaction.TransactionUtil.GetTransactionItemName(pct.GetPendingTransaction(), base.definitionService);
			if (!string.IsNullOrEmpty(transactionItemName))
			{
				base.view.HeadlineTitle.text = base.localService.GetString(transactionItemName);
			}
			else if (dialogType == global::Kampai.UI.View.RushDialogView.RushDialogType.STORAGE_EXPAND)
			{
				base.view.HeadlineTitle.text = base.localService.GetString("ExpandStorage");
			}
			else if (pct.GetTransactionTarget() == global::Kampai.Game.TransactionTarget.CLEAR_DEBRIS)
			{
				base.view.HeadlineTitle.text = base.localService.GetString("ClearX", base.localService.GetString("Debris"));
			}
		}

		internal override global::Kampai.UI.View.RequiredItemView BuildItem(global::Kampai.Game.Definition definition, uint itemsInInventory, int itemsLack, bool isAvailable, global::Kampai.Main.ILocalizationService localService)
		{
			if (definition == null)
			{
				throw new global::System.ArgumentNullException("definition", "GenericRequiredItemBuilder: You are passing in null definitions!");
			}
			global::UnityEngine.GameObject original = global::Kampai.Util.KampaiResources.Load("cmp_CraftingResourceItem") as global::UnityEngine.GameObject;
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(original);
			global::Kampai.UI.View.GenericRequiredItemView component = gameObject.GetComponent<global::Kampai.UI.View.GenericRequiredItemView>();
			global::Kampai.Game.ItemDefinition itemDefinition = definition as global::Kampai.Game.ItemDefinition;
			global::UnityEngine.Sprite sprite = UIUtils.LoadSpriteFromPath(itemDefinition.Image);
			global::UnityEngine.Sprite maskSprite = UIUtils.LoadSpriteFromPath(itemDefinition.Mask);
			component.ItemIcon.sprite = sprite;
			component.ItemIcon.maskSprite = maskSprite;
			int num = ((itemsLack >= 0) ? itemsLack : 0);
			int num2 = (int)itemsInInventory + itemsLack;
			component.ItemQuantity.text = string.Format("{0}/{1}", itemsInInventory, num2);
			int num3 = global::UnityEngine.Mathf.FloorToInt(itemDefinition.BasePremiumCost * (float)num);
			num3 = ((num3 == 0 && num > 0) ? 1 : num3);
			component.Cost = num3;
			if (!isAvailable)
			{
				component.redBorder.gameObject.SetActive(true);
				component.ItemQuantity.color = global::UnityEngine.Color.red;
			}
			else
			{
				component.greenBorder.gameObject.SetActive(true);
				component.ItemQuantity.color = global::Kampai.Util.GameConstants.UI.UI_TEXT_GREY;
			}
			return component;
		}

		protected override void LoadItems(global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition, global::Kampai.UI.View.RushDialogView.RushDialogType type)
		{
			global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> inputs = transactionDefinition.Inputs;
			if (inputs != null)
			{
				int count = inputs.Count;
				bool showPurchaseButton = false;
				requiredItems = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
				requiredItemPremiumCosts = new global::System.Collections.Generic.List<int>();
				for (int i = 0; i < count; i++)
				{
					global::Kampai.Game.ItemDefinition itemdefinition = base.definitionService.Get<global::Kampai.Game.ItemDefinition>(inputs[i].ID);
					global::Kampai.Util.QuantityItem quantityItem = null;
					uint quantityByDefinitionId = base.playerService.GetQuantityByDefinitionId(inputs[i].ID);
					int num = (int)(inputs[i].Quantity - quantityByDefinitionId);
					bool flag = false;
					if (num <= 0)
					{
						flag = true;
					}
					else
					{
						flag = false;
						quantityItem = new global::Kampai.Util.QuantityItem(inputs[i].ID, (uint)num);
						requiredItems.Add(quantityItem);
					}
					global::Kampai.UI.View.RequiredItemView requiredItemView = BuildItem(itemdefinition, quantityByDefinitionId, num, flag, base.localService);
					global::Kampai.UI.View.GenericRequiredItemView genericRequiredItemView = requiredItemView as global::Kampai.UI.View.GenericRequiredItemView;
					if (genericRequiredItemView.Cost != 0)
					{
						requiredItemPremiumCosts.Add(genericRequiredItemView.Cost);
					}
					if (!flag)
					{
						requiredItemView.ClickedSignal.AddListener(delegate
						{
							gotoButtonHandler(itemdefinition.ID);
						});
					}
					base.view.AddRequiredItem(requiredItemView, i, base.view.ScrollViewParent);
				}
				if (requiredItems.Count != 0)
				{
					rushCost = base.playerService.CalculateRushCost(requiredItems);
					base.view.SetupItemCost(rushCost);
					showPurchaseButton = true;
				}
				base.view.SetupItemCount(count);
				base.view.SetupDialog(type, showPurchaseButton);
				UpdateAdButton();
				base.gameObject.SetActive(true);
			}
			else
			{
				logger.Debug("Showing rush dialog without require items");
			}
		}

		private void AdVideoButtonClicked()
		{
			if (adPlacementInstance != null)
			{
				rewardedAdService.ShowRewardedVideo(adPlacementInstance);
			}
		}

		private bool RewardedAdEnabledForDialogType()
		{
			if (pendingCurrencyTransaction == null)
			{
				return false;
			}
			global::Kampai.Game.TransactionTarget transactionTarget = pendingCurrencyTransaction.GetTransactionTarget();
			global::Kampai.Game.TransactionTarget transactionTarget2 = transactionTarget;
			if (transactionTarget2 == global::Kampai.Game.TransactionTarget.CLEAR_DEBRIS)
			{
				return true;
			}
			return false;
		}

		private void UpdateAdButton()
		{
			if (RewardedAdEnabledForDialogType())
			{
				bool flag = rewardedAdService.IsPlacementActive(adPlacementName);
				global::Kampai.Game.AdPlacementInstance placementInstance = rewardedAdService.GetPlacementInstance(adPlacementName);
				bool flag2 = SkipStorageCheckOnRushTransaction() || !base.playerService.isStorageFull();
				bool adButtonEnabled = flag && flag2 && IsRushCostAcceptableForAd(placementInstance) && placementInstance != null;
				base.view.EnableRewardedAdRushButton(adButtonEnabled);
				adPlacementInstance = placementInstance;
			}
		}

		private bool IsRushCostAcceptableForAd(global::Kampai.Game.AdPlacementInstance placement)
		{
			bool result = false;
			if (placement != null)
			{
				global::Kampai.Game.MissingResourcesRewardDefinition missingResourcesRewardDefinition = placement.Definition as global::Kampai.Game.MissingResourcesRewardDefinition;
				if (missingResourcesRewardDefinition != null && requiredItems.Count != 0)
				{
					int num = base.playerService.CalculateRushCost(requiredItems);
					result = num <= missingResourcesRewardDefinition.MaxCostPremiumCurrency;
				}
			}
			return result;
		}

		private void OnRewardedAdReward(global::Kampai.Game.AdPlacementInstance placement)
		{
			if (placement.Equals(adPlacementInstance))
			{
				ResetRushCosts();
				ExecuteRushTransaction();
				rewardedAdService.RewardPlayer(null, placement);
				base.telemetryService.Send_Telemetry_EVT_AD_INTERACTION(placement.Definition.Name, requiredItems, placement.RewardPerPeriodCount);
				adPlacementInstance = null;
			}
		}

		private void ResetRushCosts()
		{
			for (int i = 0; i < requiredItemPremiumCosts.Count; i++)
			{
				requiredItemPremiumCosts[i] = 0;
			}
		}

		private void OnAdPlacementActivityStateChanged(global::Kampai.Game.AdPlacementInstance placement, bool enabled)
		{
			UpdateAdButton();
		}
	}
}
