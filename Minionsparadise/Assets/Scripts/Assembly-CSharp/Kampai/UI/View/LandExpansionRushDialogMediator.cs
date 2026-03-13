namespace Kampai.UI.View
{
	public class LandExpansionRushDialogMediator : global::Kampai.UI.View.RushDialogMediator<global::Kampai.UI.View.LandExpansionRushDialogView>
	{
		private global::Kampai.Game.LandExpansionConfig expansionDefinition;

		private global::Kampai.Game.BridgeBuilding bridgeBuilding;

		private global::Kampai.Game.Transaction.TransactionDefinition transactionDef;

		private global::System.Collections.IEnumerator PointerDownWait;

		[Inject]
		public global::Kampai.Game.ILandExpansionConfigService landExpansionConfigService { get; set; }

		[Inject]
		public global::Kampai.Game.PurchaseLandExpansionSignal purchaseSignal { get; set; }

		[Inject]
		public global::Kampai.Game.RepairBridgeSignal repairBridgeSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.UI.View.DisplayItemPopupSignal displayItemPopupSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideItemPopupSignal hideItemPopupSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetStorageCapacitySignal setStorageSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.OpenStoreHighlightItemSignal openStoreSignal { get; set; }

		[Inject]
		public global::Kampai.Game.PanAndOpenModalSignal moveToBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CameraAutoZoomSignal autoZoomSignal { get; set; }

		[Inject(global::Kampai.Main.MainElement.CAMERA)]
		public global::UnityEngine.Camera mainCamera { get; set; }

		[Inject]
		public global::Kampai.UI.View.CraftingModalParams craftingModalParams { get; set; }

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			global::Kampai.UI.BuildingPopupPositionData buildingPopupPositionData = args.Get<global::Kampai.UI.BuildingPopupPositionData>();
			base.view.InitProgrammatic(buildingPopupPositionData, base.localService);
			dialogType = args.Get<global::Kampai.UI.View.RushDialogView.RushDialogType>();
			switch (dialogType)
			{
			case global::Kampai.UI.View.RushDialogView.RushDialogType.LAND_EXPANSION:
			{
				int expansion = args.Get<int>();
				expansionDefinition = landExpansionConfigService.GetExpansionConfig(expansion);
				transactionDef = base.definitionService.Get<global::Kampai.Game.Transaction.TransactionDefinition>(expansionDefinition.transactionId);
				break;
			}
			case global::Kampai.UI.View.RushDialogView.RushDialogType.BRIDGE_QUEST:
			{
				int id = args.Get<int>();
				bridgeBuilding = base.playerService.GetByInstanceId<global::Kampai.Game.BridgeBuilding>(id);
				if (bridgeBuilding.BridgeId == 0)
				{
					return;
				}
				global::Kampai.Game.BridgeDefinition bridgeDefinition = base.definitionService.Get(bridgeBuilding.BridgeId) as global::Kampai.Game.BridgeDefinition;
				transactionDef = base.definitionService.Get(bridgeDefinition.TransactionId) as global::Kampai.Game.Transaction.TransactionDefinition;
				break;
			}
			default:
				logger.Error("Unsupported dialog type: {0}", dialogType);
				break;
			}
			LoadItems(transactionDef, dialogType);
		}

		protected override void OnMenuClose()
		{
			SendRushTelemetry();
			global::System.Collections.Generic.IList<global::Kampai.UI.View.RequiredItemView> itemList = base.view.GetItemList();
			if (itemList != null)
			{
				foreach (global::Kampai.UI.View.RequiredItemView item in itemList)
				{
					if (item != null)
					{
						if (item.FullyAvailable || !item.IsIngredient)
						{
							item.pointerUpSignal.RemoveListener(PointerUpAvailable);
							item.pointerDownSignal.RemoveListener(PointerDownAvailable);
						}
						else
						{
							item.pointerUpSignal.RemoveListener(PointerUpUnavailable);
							item.pointerDownSignal.RemoveListener(PointerDownUnavailable);
						}
					}
				}
			}
			switch (dialogType)
			{
			case global::Kampai.UI.View.RushDialogView.RushDialogType.LAND_EXPANSION:
				base.hideSkrim.Dispatch("LandExpansionSkrim");
				break;
			case global::Kampai.UI.View.RushDialogView.RushDialogType.BRIDGE_QUEST:
				base.hideSkrim.Dispatch("BridgeSkrim");
				break;
			}
			base.guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "popup_Confirmation_Expansion");
		}

		private void PointerDown(int itemDefinitionID, global::UnityEngine.RectTransform rectTransform, bool isAvailable)
		{
			if (!isClosing && itemDefinitionID != 0)
			{
				if (PointerDownWait != null)
				{
					StopCoroutine(PointerDownWait);
					PointerDownWait = null;
				}
				displayItemPopupSignal.Dispatch(itemDefinitionID, rectTransform, (!isAvailable) ? global::Kampai.UI.View.UIPopupType.GENERICGOTO : global::Kampai.UI.View.UIPopupType.GENERIC);
			}
		}

		private void PointerUp(bool isAvailable)
		{
			if (PointerDownWait == null)
			{
				PointerDownWait = HideItemPopupAfter((!isAvailable) ? 1f : 0.5f);
				StartCoroutine(PointerDownWait);
			}
		}

		private void PointerDownAvailable(int itemDefinitionID, global::UnityEngine.RectTransform rectTransform)
		{
			PointerDown(itemDefinitionID, rectTransform, true);
		}

		private void PointerUpAvailable()
		{
			PointerUp(true);
		}

		private void PointerDownUnavailable(int itemDefinitionID, global::UnityEngine.RectTransform rectTransform)
		{
			PointerDown(itemDefinitionID, rectTransform, false);
		}

		private void PointerUpUnavailable()
		{
			PointerUp(false);
		}

		private global::System.Collections.IEnumerator HideItemPopupAfter(float seconds)
		{
			yield return new global::UnityEngine.WaitForSeconds(seconds);
			hideItemPopupSignal.Dispatch();
		}

		protected override void PurchaseSuccess()
		{
			purchaseSuccess = true;
			playSFXSignal.Dispatch("Play_button_click_01");
			TryRunTheActualTransaction();
		}

		private void PerformTransactionSuccessAction(global::Kampai.Game.LandExpansionConfig config)
		{
			switch (dialogType)
			{
			case global::Kampai.UI.View.RushDialogView.RushDialogType.LAND_EXPANSION:
				purchaseSignal.Dispatch(config.expansionId, true);
				break;
			case global::Kampai.UI.View.RushDialogView.RushDialogType.BRIDGE_QUEST:
				questService.UpdateAllQuestsWithQuestStepType(global::Kampai.Game.QuestStepType.BridgeRepair, global::Kampai.Game.QuestTaskTransition.Start, bridgeBuilding);
				questService.UpdateAllQuestsWithQuestStepType(global::Kampai.Game.QuestStepType.BridgeRepair, global::Kampai.Game.QuestTaskTransition.Complete, bridgeBuilding);
				repairBridgeSignal.Dispatch(bridgeBuilding);
				break;
			}
			base.setGrindCurrencySignal.Dispatch();
		}

		protected override void Close()
		{
			base.Close();
			if (dialogType == global::Kampai.UI.View.RushDialogView.RushDialogType.LAND_EXPANSION)
			{
				global::Kampai.Game.HighlightLandExpansionSignal instance = gameContext.injectionBinder.GetInstance<global::Kampai.Game.HighlightLandExpansionSignal>();
				instance.Dispatch(expansionDefinition.expansionId, false);
			}
			playSFXSignal.Dispatch("Play_menu_disappear_01");
			hideItemPopupSignal.Dispatch();
			base.view.Close();
		}

		internal override global::Kampai.UI.View.RequiredItemView BuildItem(global::Kampai.Game.Definition definition, uint itemsInInventory, int itemsLack, bool isAvailable, global::Kampai.Main.ILocalizationService localService)
		{
			if (definition == null)
			{
				throw new global::System.ArgumentNullException("definition", "RequiredItemBuilder: You are passing in null definitions!");
			}
			global::UnityEngine.GameObject original = ((definition.ID != 0) ? (global::Kampai.Util.KampaiResources.Load("cmp_RequiredItem_ForShow") as global::UnityEngine.GameObject) : (global::Kampai.Util.KampaiResources.Load("cmp_CurrencyRequiredItem_ForShow") as global::UnityEngine.GameObject));
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(original);
			global::Kampai.UI.View.RequiredItemView component = gameObject.GetComponent<global::Kampai.UI.View.RequiredItemView>();
			global::Kampai.Game.ItemDefinition itemDefinition = definition as global::Kampai.Game.ItemDefinition;
			global::UnityEngine.Sprite sprite = UIUtils.LoadSpriteFromPath(itemDefinition.Image);
			global::UnityEngine.Sprite maskSprite = UIUtils.LoadSpriteFromPath(itemDefinition.Mask);
			component.ItemIcon.sprite = sprite;
			component.ItemIcon.maskSprite = maskSprite;
			int num = (int)itemsInInventory + itemsLack;
			int num2 = ((itemsLack >= 0) ? itemsLack : 0);
			component.ItemNeeded = num;
			component.ItemDefinitionID = itemDefinition.ID;
			if (definition.ID == 0)
			{
				component.ItemQuantity.text = UIUtils.FormatLargeNumber(num);
			}
			else
			{
				component.ItemQuantity.text = string.Format("{0}/{1}", itemsInInventory, num);
			}
			component.PurchasePanel.SetActive(!isAvailable);
			int num3 = global::UnityEngine.Mathf.FloorToInt(itemDefinition.BasePremiumCost * (float)num2);
			num3 = ((num3 == 0 && num2 > 0) ? 1 : num3);
			component.ItemCost.text = UIUtils.FormatLargeNumber(num3);
			if (component.DashedCircle != null)
			{
				component.DashedCircle.SetActive(!isAvailable);
			}
			if (component.SolidCircle != null)
			{
				component.SolidCircle.SetActive(isAvailable);
			}
			component.FullyAvailable = isAvailable;
			component.IsIngredient = itemDefinition is global::Kampai.Game.IngredientsItemDefinition;
			if (!isAvailable)
			{
				component.ItemQuantity.color = global::UnityEngine.Color.red;
				global::Kampai.Util.QuantityItem item = new global::Kampai.Util.QuantityItem(itemDefinition.ID, (uint)num2);
				component.RushBtn.RushCost = num3;
				component.RushBtn.Item = item;
			}
			if (isAvailable || !component.IsIngredient)
			{
				component.pointerUpSignal.AddListener(PointerUpAvailable);
				component.pointerDownSignal.AddListener(PointerDownAvailable);
			}
			else
			{
				component.pointerUpSignal.AddListener(PointerUpUnavailable);
				component.pointerDownSignal.AddListener(PointerDownUnavailable);
			}
			return component;
		}

		protected override void RushTransactionCallback(global::Kampai.Game.PendingCurrencyTransaction pct)
		{
			purchaseInProgress = false;
			if (pct.Success)
			{
				isClosing = true;
				PurchaseSuccess();
				base.soundFXSignal.Dispatch("Play_button_premium_01");
				base.setPremiumCurrencySignal.Dispatch();
				base.setGrindCurrencySignal.Dispatch();
			}
			else if (pct.ParentSuccess)
			{
				PurchaseButtonClicked();
			}
		}

		private void ExpansionTransactionCallback(global::Kampai.Game.PendingCurrencyTransaction pct)
		{
			if (!pct.Success)
			{
				return;
			}
			foreach (global::Kampai.Util.QuantityItem output in pct.GetPendingTransaction().Outputs)
			{
				global::Kampai.Game.Definition definition = base.definitionService.Get<global::Kampai.Game.Definition>(output.ID);
				global::Kampai.Game.LandExpansionConfig landExpansionConfig = definition as global::Kampai.Game.LandExpansionConfig;
				if (landExpansionConfig != null)
				{
					PerformTransactionSuccessAction(landExpansionConfig);
				}
			}
			setStorageSignal.Dispatch();
			Close();
		}

		private void TryRunTheActualTransaction()
		{
			switch (dialogType)
			{
			case global::Kampai.UI.View.RushDialogView.RushDialogType.BRIDGE_QUEST:
				base.playerService.RunEntireTransaction(transactionDef, global::Kampai.Game.TransactionTarget.REPAIR_BRIDGE, ExpansionTransactionCallback);
				break;
			case global::Kampai.UI.View.RushDialogView.RushDialogType.LAND_EXPANSION:
				base.playerService.RunEntireTransaction(transactionDef, global::Kampai.Game.TransactionTarget.LAND_EXPANSION, ExpansionTransactionCallback);
				break;
			}
		}

		protected override void LoadItems(global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition, global::Kampai.UI.View.RushDialogView.RushDialogType type)
		{
			if (type == global::Kampai.UI.View.RushDialogView.RushDialogType.LAND_EXPANSION)
			{
				LoadExpansionItems(transactionDefinition, type);
			}
			else
			{
				LoadNormalItems(transactionDef, type);
			}
		}

		private void LoadExpansionItems(global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition, global::Kampai.UI.View.RushDialogView.RushDialogType type)
		{
			global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> inputs = transactionDefinition.Inputs;
			if (inputs == null)
			{
				return;
			}
			bool showPurchaseButton = false;
			requiredItems = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
			requiredItemPremiumCosts = new global::System.Collections.Generic.List<int>();
			int num = 0;
			for (int i = 0; i < inputs.Count; i++)
			{
				int iD = inputs[i].ID;
				int num2 = -1;
				global::Kampai.Game.ItemDefinition definition = base.definitionService.Get<global::Kampai.Game.ItemDefinition>(iD);
				if (iD > 1)
				{
					num2 = num++;
				}
				global::Kampai.Util.QuantityItem quantityItem = null;
				uint quantityByDefinitionId = base.playerService.GetQuantityByDefinitionId(iD);
				int num3 = (int)(inputs[i].Quantity - quantityByDefinitionId);
				bool flag = false;
				if (num3 <= 0)
				{
					flag = true;
				}
				else
				{
					flag = false;
					quantityItem = new global::Kampai.Util.QuantityItem(iD, (uint)num3);
					requiredItems.Add(quantityItem);
				}
				global::Kampai.UI.View.RequiredItemView requiredItemView = BuildItem(definition, quantityByDefinitionId, num3, flag, base.localService);
				if (!flag)
				{
					requiredItemView.RushBtn.RushButtonClickedSignal.AddListener(base.IndividualRushButtonClicked);
				}
				int intFromFormattedLargeNumber = UIUtils.GetIntFromFormattedLargeNumber(requiredItemView.ItemCost.text);
				if (intFromFormattedLargeNumber != 0)
				{
					requiredItemPremiumCosts.Add(intFromFormattedLargeNumber);
				}
				if (num2 != -1)
				{
					base.view.AddRequiredItem(requiredItemView, num2, base.view.ScrollViewParent);
				}
				else
				{
					base.view.AddRequiredItem(requiredItemView, -1, base.view.CurrencyScrollViewParent);
				}
			}
			if (num <= 2)
			{
				base.view.MoveRequiredItem();
			}
			if (requiredItems.Count != 0)
			{
				rushCost = base.playerService.CalculateRushCost(requiredItems);
				base.view.SetupItemCost(rushCost);
				showPurchaseButton = true;
			}
			base.view.SetupItemCount(num);
			base.view.SetupDialog(type, showPurchaseButton);
			base.gameObject.SetActive(true);
		}

		private void LoadNormalItems(global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition, global::Kampai.UI.View.RushDialogView.RushDialogType type)
		{
			global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> inputs = transactionDefinition.Inputs;
			if (inputs != null)
			{
				int count = inputs.Count;
				int num = -1;
				int index = 0;
				bool showPurchaseButton = false;
				requiredItems = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
				requiredItemPremiumCosts = new global::System.Collections.Generic.List<int>();
				for (int i = 0; i < count; i++)
				{
					global::Kampai.Game.ItemDefinition itemDefinition = base.definitionService.Get<global::Kampai.Game.ItemDefinition>(inputs[i].ID);
					if (itemDefinition.ID == 0)
					{
						num = i;
					}
					global::Kampai.Util.QuantityItem quantityItem = null;
					uint quantityByDefinitionId = base.playerService.GetQuantityByDefinitionId(inputs[i].ID);
					int num2 = (int)(inputs[i].Quantity - quantityByDefinitionId);
					bool flag = false;
					if (num2 <= 0)
					{
						flag = true;
					}
					else
					{
						flag = false;
						quantityItem = new global::Kampai.Util.QuantityItem(inputs[i].ID, (uint)num2);
						requiredItems.Add(quantityItem);
					}
					global::Kampai.UI.View.RequiredItemView requiredItemView = BuildItem(itemDefinition, quantityByDefinitionId, num2, flag, base.localService);
					if (!flag)
					{
						requiredItemView.RushBtn.RushButtonClickedSignal.AddListener(base.IndividualRushButtonClicked);
					}
					int intFromFormattedLargeNumber = UIUtils.GetIntFromFormattedLargeNumber(requiredItemView.ItemCost.text);
					if (intFromFormattedLargeNumber != 0)
					{
						requiredItemPremiumCosts.Add(intFromFormattedLargeNumber);
					}
					if (num < 0)
					{
						index = i;
					}
					else if (num == i)
					{
						index = -1;
					}
					else if (i > num)
					{
						index = i - 1;
					}
					base.view.AddRequiredItem(requiredItemView, index, (num != i) ? base.view.ScrollViewParent : base.view.CurrencyScrollViewParent);
				}
				if (requiredItems.Count != 0)
				{
					rushCost = base.playerService.CalculateRushCost(requiredItems);
					base.view.SetupItemCost(rushCost);
					showPurchaseButton = true;
				}
				if (num < 0)
				{
					base.view.SetupItemCount(count);
				}
				else
				{
					base.view.SetupItemCount(count - 1);
				}
				base.view.SetupDialog(type, showPurchaseButton);
				base.gameObject.SetActive(true);
			}
			else
			{
				logger.Debug("Showing rush dialog without require items");
			}
		}

		private void SendRushTelemetry()
		{
			if (requiredItems != null && requiredItems.Count != 0)
			{
				string sourceName = "unknown";
				switch (dialogType)
				{
				case global::Kampai.UI.View.RushDialogView.RushDialogType.LAND_EXPANSION:
					sourceName = "LandExpansion";
					break;
				case global::Kampai.UI.View.RushDialogView.RushDialogType.BRIDGE_QUEST:
					sourceName = "BrokenBridge";
					break;
				}
				global::Kampai.Game.PendingCurrencyTransaction pct = new global::Kampai.Game.PendingCurrencyTransaction(transactionDef, true, rushCost, null, null);
				bool flag = purchaseSuccess | purchaseInProgress;
				base.telemetryService.Send_Telemetry_EVT_PINCH_PROMPT(sourceName, pct, requiredItems, flag.ToString());
			}
		}

		private void OpenBuildingsStore(int buildingDefId)
		{
			openStoreSignal.Dispatch(buildingDefId, true);
			float currentPercentage = mainCamera.GetComponent<global::Kampai.Game.View.ZoomView>().GetCurrentPercentage();
			if (currentPercentage > 0.4f)
			{
				autoZoomSignal.Dispatch(0.4f);
			}
		}

		private void GotoResourceBuilding(int itemDefinitionId)
		{
			Close();
			int buildingDefintionIDFromItemDefintionID = base.definitionService.GetBuildingDefintionIDFromItemDefintionID(itemDefinitionId);
			global::System.Collections.Generic.ICollection<global::Kampai.Game.Building> byDefinitionId = base.playerService.GetByDefinitionId<global::Kampai.Game.Building>(buildingDefintionIDFromItemDefintionID);
			if (byDefinitionId.Count == 0)
			{
				OpenBuildingsStore(buildingDefintionIDFromItemDefintionID);
				return;
			}
			global::Kampai.Game.Building suitableBuilding = global::Kampai.Util.GotoBuildingHelpers.GetSuitableBuilding(byDefinitionId);
			if (suitableBuilding.State == global::Kampai.Game.BuildingState.Inventory)
			{
				OpenBuildingsStore(buildingDefintionIDFromItemDefintionID);
				return;
			}
			global::Kampai.Game.CraftingBuilding craftingBuilding = suitableBuilding as global::Kampai.Game.CraftingBuilding;
			if (craftingBuilding != null)
			{
				craftingModalParams.itemId = itemDefinitionId;
				craftingModalParams.highlight = true;
			}
			moveToBuildingSignal.Dispatch(suitableBuilding.ID, false);
		}

		public override void OnRegister()
		{
			base.OnRegister();
			base.gotoSignal.AddListener(GotoResourceBuilding);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			base.gotoSignal.RemoveListener(GotoResourceBuilding);
		}
	}
}
