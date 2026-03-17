namespace Kampai.UI.View
{
	public class VillainLairUnlockRushDialogMediator : global::Kampai.UI.View.RushDialogMediator<global::Kampai.UI.View.VillainLairUnlockRushDialogView>
	{
		private global::Kampai.Game.Transaction.TransactionDefinition transactionDef;

		private global::System.Collections.IEnumerator PointerDownWait;

		private global::Kampai.Game.VillainLairEntranceBuilding lair;

		[Inject]
		public global::Kampai.UI.View.DisplayItemPopupSignal displayItemPopupSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideItemPopupSignal hideItemPopupSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetStorageCapacitySignal setStorageSignal { get; set; }

		[Inject]
		public global::Kampai.Common.RepairBuildingSignal repairBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IGoToService gotoService { get; set; }

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

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			global::Kampai.UI.BuildingPopupPositionData buildingPopupPositionData = args.Get<global::Kampai.UI.BuildingPopupPositionData>();
			base.view.InitProgrammatic(buildingPopupPositionData, base.localService);
			dialogType = args.Get<global::Kampai.UI.View.RushDialogView.RushDialogType>();
			int id = args.Get<int>();
			lair = base.playerService.GetByInstanceId<global::Kampai.Game.VillainLairEntranceBuilding>(id);
			transactionDef = base.definitionService.Get<global::Kampai.Game.Transaction.TransactionDefinition>(lair.Definition.TransactionID);
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
						if (!item.FullyAvailable && item.IsIngredient)
						{
							item.pointerUpSignal.RemoveListener(PointerUpWithGoto);
							item.pointerDownSignal.RemoveListener(PointerDownWithGoto);
						}
						else
						{
							item.pointerUpSignal.RemoveListener(PointerUpWithoutGoto);
							item.pointerDownSignal.RemoveListener(PointerDownWithoutGoto);
						}
					}
				}
			}
			base.guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "screen_UnlockLair");
			base.hideSkrim.Dispatch("VillainLairPortalSkrim");
		}

		private void PointerDown(int itemDefinitionID, global::UnityEngine.RectTransform rectTransform, bool showGoto)
		{
			if (!isClosing)
			{
				if (PointerDownWait != null)
				{
					StopCoroutine(PointerDownWait);
					PointerDownWait = null;
				}
				displayItemPopupSignal.Dispatch(itemDefinitionID, rectTransform, showGoto ? global::Kampai.UI.View.UIPopupType.GENERICGOTO : global::Kampai.UI.View.UIPopupType.GENERIC);
			}
		}

		private void PointerUp(bool showGoto)
		{
			if (PointerDownWait == null)
			{
				PointerDownWait = HideItemPopupAfter((!showGoto) ? 0.5f : 1f);
				StartCoroutine(PointerDownWait);
			}
		}

		private global::System.Collections.IEnumerator HideItemPopupAfter(float seconds)
		{
			yield return new global::UnityEngine.WaitForSeconds(seconds);
			hideItemPopupSignal.Dispatch();
		}

		private void PointerDownWithGoto(int itemDefinitionID, global::UnityEngine.RectTransform rectTransform)
		{
			PointerDown(itemDefinitionID, rectTransform, true);
		}

		private void PointerUpWithGoto()
		{
			PointerUp(true);
		}

		private void PointerDownWithoutGoto(int itemDefinitionID, global::UnityEngine.RectTransform rectTransform)
		{
			PointerDown(itemDefinitionID, rectTransform, false);
		}

		private void PointerUpWithoutGoto()
		{
			PointerUp(false);
		}

		protected override void PurchaseSuccess()
		{
			purchaseSuccess = true;
			playSFXSignal.Dispatch("Play_button_click_01");
			base.playerService.RunEntireTransaction(transactionDef, global::Kampai.Game.TransactionTarget.NO_VISUAL, RepairPortalTransactionCallback);
		}

		protected override void Close()
		{
			base.Close();
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
			global::UnityEngine.GameObject original = global::Kampai.Util.KampaiResources.Load("cmp_UnlockLair") as global::UnityEngine.GameObject;
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
			component.ItemQuantity.text = string.Format("{0}/{1}", itemsInInventory, num);
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
			if (!isAvailable)
			{
				component.ItemQuantity.color = global::UnityEngine.Color.red;
				global::Kampai.Util.QuantityItem item = new global::Kampai.Util.QuantityItem(itemDefinition.ID, (uint)num2);
				component.RushBtn.RushCost = num3;
				component.RushBtn.Item = item;
			}
			component.FullyAvailable = isAvailable;
			component.IsIngredient = itemDefinition is global::Kampai.Game.IngredientsItemDefinition;
			if (!isAvailable && component.IsIngredient)
			{
				component.pointerUpSignal.AddListener(PointerUpWithGoto);
				component.pointerDownSignal.AddListener(PointerDownWithGoto);
			}
			else
			{
				component.pointerUpSignal.AddListener(PointerUpWithoutGoto);
				component.pointerDownSignal.AddListener(PointerDownWithoutGoto);
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

		private void RepairPortalTransactionCallback(global::Kampai.Game.PendingCurrencyTransaction pct)
		{
			if (pct.Success)
			{
				setStorageSignal.Dispatch();
				lair.SetState(global::Kampai.Game.BuildingState.Broken);
				repairBuildingSignal.Dispatch(lair);
				Close();
			}
		}

		protected override void LoadItems(global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition, global::Kampai.UI.View.RushDialogView.RushDialogType type)
		{
			global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> inputs = transactionDefinition.Inputs;
			if (inputs != null)
			{
				int count = inputs.Count;
				int num = -1;
				bool showPurchaseButton = false;
				requiredItems = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
				requiredItemPremiumCosts = new global::System.Collections.Generic.List<int>();
				for (int i = 0; i < count; i++)
				{
					global::Kampai.Game.ItemDefinition definition = base.definitionService.Get<global::Kampai.Game.ItemDefinition>(inputs[i].ID);
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
					global::Kampai.UI.View.RequiredItemView requiredItemView = BuildItem(definition, quantityByDefinitionId, num2, flag, base.localService);
					if (!flag)
					{
						requiredItemView.RushBtn.RushButtonClickedSignal.AddListener(base.IndividualRushButtonClicked);
					}
					int intFromFormattedLargeNumber = UIUtils.GetIntFromFormattedLargeNumber(requiredItemView.ItemCost.text);
					if (intFromFormattedLargeNumber != 0)
					{
						requiredItemPremiumCosts.Add(intFromFormattedLargeNumber);
					}
					base.view.AddRequiredItem(requiredItemView, i, base.view.ScrollViewParent);
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
				string sourceName = "VillainLairUnlock";
				global::Kampai.Game.PendingCurrencyTransaction pct = new global::Kampai.Game.PendingCurrencyTransaction(transactionDef, true, rushCost, null, null);
				bool flag = purchaseSuccess | purchaseInProgress;
				base.telemetryService.Send_Telemetry_EVT_PINCH_PROMPT(sourceName, pct, requiredItems, flag.ToString());
			}
		}

		private void GotoResourceBuilding(int itemDefinitionId)
		{
			Close();
			gotoService.GoToBuildingFromItem(itemDefinitionId);
		}
	}
}
