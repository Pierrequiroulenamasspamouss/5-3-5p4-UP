namespace Kampai.Game
{
	public class UpsellService : global::Kampai.Game.IUpsellService
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("UpsellService") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Splash.DLCModel dlcModel { get; set; }

		[Inject]
		public global::Kampai.Game.StartSaleSignal startSaleSignal { get; set; }

		[Inject]
		public global::Kampai.Game.EndSaleSignal endSaleSignal { get; set; }

		public global::Kampai.Util.QuantityItem GetItemDef(int index, global::Kampai.Game.SalePackDefinition saleDefinition)
		{
			if (saleDefinition == null || saleDefinition.TransactionDefinition == null || saleDefinition.TransactionDefinition.Outputs == null)
			{
				return null;
			}
			return GetItemDef(index, saleDefinition.TransactionDefinition.Outputs);
		}

		public global::Kampai.Util.QuantityItem GetItemDef(int index, global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> outputs)
		{
			if (outputs == null || outputs.Count == 0)
			{
				return null;
			}
			int num = 0;
			for (int i = 0; i < outputs.Count; i++)
			{
				global::Kampai.Util.QuantityItem quantityItem = outputs[i];
				if (quantityItem == null)
				{
					continue;
				}
				global::Kampai.Game.Definition definition = definitionService.Get(quantityItem.ID);
				if (definition != null && definition.ID != 2 && !(definition is global::Kampai.Game.UnlockDefinition))
				{
					if (num == index)
					{
						return quantityItem;
					}
					num++;
				}
			}
			return null;
		}

		public uint SumOutput(global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> inputs, int id)
		{
			if (inputs == null)
			{
				return 0u;
			}
			uint num = 0u;
			for (int i = 0; i < inputs.Count; i++)
			{
				global::Kampai.Util.QuantityItem quantityItem = inputs[i];
				if (quantityItem != null && quantityItem.ID == id)
				{
					num += quantityItem.Quantity;
				}
			}
			return num;
		}

		public int GetInputsCount(global::Kampai.Game.SalePackDefinition saleDefinition)
		{
			if (saleDefinition == null || saleDefinition.TransactionDefinition == null || saleDefinition.TransactionDefinition.Inputs == null)
			{
				return 0;
			}
			return saleDefinition.TransactionDefinition.Inputs.Count;
		}

		public void SetupFreeItemButton(global::Kampai.UI.View.LocalizeView localizeView, global::Kampai.UI.View.ButtonView buttonView, string buttonLocKey, global::UnityEngine.RuntimeAnimatorController freeCollectButtonAnimator)
		{
			if (freeCollectButtonAnimator == null)
			{
				freeCollectButtonAnimator = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>("asm_buttonClick_Tertiary");
			}
			if (buttonView == null || localizeView == null)
			{
				if (buttonView == null)
				{
					logger.Error("Purchase Currency Button GameObject is null");
				}
				if (localizeView == null)
				{
					logger.Error("Purchase Button Cost GameObject is null");
				}
			}
			else
			{
				localizeView.LocKey = buttonLocKey;
				global::UnityEngine.Animator component = buttonView.GetComponent<global::UnityEngine.Animator>();
				if (component == null || freeCollectButtonAnimator == null)
				{
					logger.Error(string.Format("Button animator is null: {0}", buttonView));
				}
				else
				{
					component.runtimeAnimatorController = freeCollectButtonAnimator;
				}
			}
		}

		public global::Kampai.Game.Sale GetSaleInstanceFromID(global::System.Collections.Generic.List<global::Kampai.Game.Sale> playerSales, int id)
		{
			if (playerSales == null)
			{
				return null;
			}
			global::Kampai.Game.Sale sale = null;
			for (int i = 0; i < playerSales.Count; i++)
			{
				sale = playerSales[i];
				if (sale != null && sale.Definition.ID == id)
				{
					return sale;
				}
			}
			return null;
		}

		public void SetupBuyButton(global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> inputs, global::Kampai.UI.View.KampaiImage inputItemIcon, global::Kampai.Game.Definition definition)
		{
			if (inputs == null || inputs.Count == 0)
			{
				return;
			}
			global::Kampai.Util.QuantityItem quantityItem = inputs[0];
			if (quantityItem == null)
			{
				logger.Error(string.Format("Item input is null for sale pack definition: {0}", definition));
				return;
			}
			global::Kampai.Game.ItemDefinition itemDefinition = definitionService.Get<global::Kampai.Game.ItemDefinition>(quantityItem.ID);
			if (itemDefinition == null)
			{
				logger.Error(string.Format("Item input is not an item definition for definition: {0}", definition));
				return;
			}
			inputItemIcon.gameObject.SetActive(true);
			UIUtils.SetItemIcon(inputItemIcon, itemDefinition);
		}

		public global::Kampai.Game.SalePackDefinition GetSalePackDefinition(int upsellDefId)
		{
			if (definitionService == null)
			{
				return null;
			}
			global::Kampai.Game.SalePackDefinition definition = null;
			definitionService.TryGet<global::Kampai.Game.SalePackDefinition>(upsellDefId, out definition);
			return definition;
		}

		public void ScheduleSales(global::System.Collections.Generic.IList<global::Kampai.Game.Sale> saleInstances)
		{
			if (saleInstances != null && saleInstances.Count != 0)
			{
				for (int i = 0; i < saleInstances.Count; i++)
				{
					ScheduleSale(saleInstances[i]);
				}
			}
		}

		public global::Kampai.Game.Sale AddNewSaleInstance(global::Kampai.Game.SalePackDefinition salePackDefinition)
		{
			if (salePackDefinition == null)
			{
				return null;
			}
			logger.Debug("Adding a new Sale to the player Data. ID = " + salePackDefinition.ID);
			global::Kampai.Game.Sale sale = new global::Kampai.Game.Sale(salePackDefinition);
			sale.Purchased = false;
			playerService.Add(sale);
			return sale;
		}

		public void ScheduleSale(global::Kampai.Game.Sale saleInstance)
		{
			if (saleInstance == null)
			{
				logger.Error("Unable to find sale instance");
			}
			else
			{
				if (saleInstance.Finished || !IsSaleAssetsFullyDownloaded(saleInstance.Definition))
				{
					return;
				}
				if (!saleInstance.Started)
				{
					if (CanSaleProceed(saleInstance))
					{
						int eventTime = saleInstance.Definition.UTCStartDate - timeService.CurrentTime();
						timeEventService.AddEvent(saleInstance.ID, timeService.CurrentTime(), eventTime, startSaleSignal);
					}
				}
				else if (!timeEventService.HasEventID(saleInstance.ID) && saleInstance.Definition.Duration > 0)
				{
					timeEventService.AddEvent(saleInstance.ID, saleInstance.UTCUserStartTime, saleInstance.Definition.Duration, endSaleSignal);
				}
			}
		}

		public bool IsSaleUpsellInstance(global::Kampai.Game.Sale saleInstance)
		{
			return saleInstance != null && !saleInstance.Finished && !PackUtil.HasPurchasedEnough(saleInstance.Definition, playerService) && IsSaleAssetsFullyDownloaded(saleInstance.Definition);
		}

		private bool IsSaleAssetsFullyDownloaded(global::Kampai.Game.SalePackDefinition saleDefinition)
		{
			if (saleDefinition.TransactionDefinition == null || saleDefinition.TransactionDefinition.ID == 0)
			{
				return true;
			}
			global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition = saleDefinition.TransactionDefinition.ToDefinition();
			if (transactionDefinition != null)
			{
				foreach (global::Kampai.Util.QuantityItem output in transactionDefinition.Outputs)
				{
					if (output.ID == 1 || output.ID == 0 || output.ID == 9 || output.ID == 6 || output.ID == 700 || output.ID == 701 || output.ID == 702)
					{
						continue;
					}
					global::Kampai.Game.Definition definition = definitionService.Get<global::Kampai.Game.Definition>(output.ID);
					if (!(definition is global::Kampai.Game.MinionDefinition))
					{
						global::Kampai.Game.ItemDefinition itemDefinition = definition as global::Kampai.Game.ItemDefinition;
						if (itemDefinition != null && !string.IsNullOrEmpty(itemDefinition.Image) && !string.IsNullOrEmpty(itemDefinition.Mask) && (!global::Kampai.Util.KampaiResources.FileDownloaded(itemDefinition.Image, dlcModel) || !global::Kampai.Util.KampaiResources.FileDownloaded(itemDefinition.Mask, dlcModel)))
						{
							return false;
						}
						global::Kampai.Game.BuildingDefinition buildingDefinition = definition as global::Kampai.Game.BuildingDefinition;
						if (buildingDefinition != null && !global::Kampai.Util.KampaiResources.FileDownloaded(buildingDefinition.Prefab, dlcModel))
						{
							return false;
						}
					}
				}
				return true;
			}
			return false;
		}

		private bool CanSaleProceed(global::Kampai.Game.Sale saleInstance)
		{
			global::Kampai.Game.SalePackDefinition definition = saleInstance.Definition;
			int unlockLevel = definition.UnlockLevel;
			global::Kampai.Game.SalePackType type = definition.Type;
			if (unlockLevel > 0)
			{
				if (!questService.IsQuestCompleted(definition.UnlockQuestId) || playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID) < unlockLevel)
				{
					return false;
				}
			}
			else if (type != global::Kampai.Game.SalePackType.Upsell && playerService.GetHighestFtueCompleted() < 999999)
			{
				return false;
			}
			return true;
		}
	}
}
