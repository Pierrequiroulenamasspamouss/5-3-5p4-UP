namespace Kampai.UI
{
	public class CurrencyStoreService : global::Kampai.UI.ICurrencyStoreService
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("CurrencyStoreService") as global::Kampai.Util.IKampaiLogger;

		private global::Kampai.UI.CurrencyStoreLocalState localState;

		private global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<int>> badgeIgnoreList = new global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<int>>();

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public ILocalPersistanceService localPersistanceService { get; set; }

		[Inject]
		public global::Kampai.Common.ICoppaService coppaService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localeService { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[PostConstruct]
		public void PostConstruct()
		{
			LoadPersist();
		}

		public void Initialize()
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.CurrencyStoreCategoryDefinition> currencyStoreCategoryDefinitions = definitionService.GetCurrencyStoreCategoryDefinitions();
			for (int i = 0; i < currencyStoreCategoryDefinitions.Count; i++)
			{
				global::Kampai.Game.CurrencyStoreCategoryDefinition currencyStoreCategoryDefinition = currencyStoreCategoryDefinitions[i];
				int iD = currencyStoreCategoryDefinition.ID;
				for (int j = 0; j < currencyStoreCategoryDefinition.StoreItemDefinitionIDs.Count; j++)
				{
					int num = currencyStoreCategoryDefinition.StoreItemDefinitionIDs[j];
					global::Kampai.Game.StoreItemDefinition definition;
					if (!definitionService.TryGet<global::Kampai.Game.StoreItemDefinition>(num, out definition))
					{
						logger.Warning("Unable to find store item def with id: {0}", num);
					}
					else if (!definition.EnableBadging)
					{
						if (badgeIgnoreList.ContainsKey(iD))
						{
							badgeIgnoreList[iD].Add(num);
							continue;
						}
						global::System.Collections.Generic.List<int> list = new global::System.Collections.Generic.List<int>();
						list.Add(num);
						badgeIgnoreList.Add(iD, list);
					}
				}
			}
		}

		public bool IsValidCurrencyItem(int storeItemDefinitionID, global::Kampai.Game.StoreCategoryType type, bool countInLocked = true)
		{
			global::Kampai.Game.StoreItemDefinition definition;
			if (!definitionService.TryGet<global::Kampai.Game.StoreItemDefinition>(storeItemDefinitionID, out definition))
			{
				logger.Warning("Unable to find store item def with id: {0}", storeItemDefinitionID);
				return false;
			}
			if (!definition.IsOnSale(global::UnityEngine.Application.platform, timeService, localeService, logger))
			{
				logger.Warning("Item not valid for current store: {0}", storeItemDefinitionID);
				return false;
			}
			global::Kampai.Game.CurrencyItemDefinition definition2;
			if (!definitionService.TryGet<global::Kampai.Game.CurrencyItemDefinition>(definition.ReferencedDefID, out definition2))
			{
				logger.Warning("Unable to find currency item def with id: {0}", definition.ReferencedDefID);
				return false;
			}
			/* if (definition2.COPPAGated && coppaService.Restricted())
			{
				return false;
			} */
			if (definition.Type == global::Kampai.Game.StoreItemType.SalePack)
			{
				global::Kampai.Game.PackDefinition definition3 = GetCurrencyStorePackDefinition(definition.ReferencedDefID);
				if (definition3 == null)
				{
					logger.Error("Unable to find PackDefinition with id: {0}", definition.ReferencedDefID);
					return false;
				}
				if (PackUtil.HasPurchasedEnough(definition3, playerService))
				{
					return false;
				}
			}
			return true;
		}

		public bool ShouldPackBeVisuallyLocked(global::Kampai.Game.PackDefinition currencyStorePackDefinition)
		{
			return false; // Force unlocked
		}

		private bool ShouldItemBeMarkedAsViewed(global::Kampai.Game.StoreItemDefinition storeItemDef)
		{
			if (storeItemDef.Type != global::Kampai.Game.StoreItemType.SalePack)
			{
				return true;
			}
			global::Kampai.Game.PackDefinition definition = GetCurrencyStorePackDefinition(storeItemDef.ReferencedDefID);
			return definition != null && !ShouldPackBeVisuallyLocked(definition);
		}

		public bool HasPurchasedEnough(global::Kampai.Game.PackDefinition currencyStorePackDefinition)
		{
			return PackUtil.HasPurchasedEnough(currencyStorePackDefinition, playerService);
		}

		public global::Kampai.Game.PackDefinition GetCurrencyStorePackDefinition(int packDefinitionId)
		{
			global::Kampai.Game.CurrencyStorePackDefinition currencyDef;
			if (definitionService.TryGet<global::Kampai.Game.CurrencyStorePackDefinition>(packDefinitionId, out currencyDef))
			{
				return currencyDef;
			}
			global::Kampai.Game.SalePackDefinition saleDef;
			if (definitionService.TryGet<global::Kampai.Game.SalePackDefinition>(packDefinitionId, out saleDef))
			{
				return saleDef;
			}
			logger.Error("The Store Pack you are trying to find doesn't exist, id: {0}", packDefinitionId);
			return null;
		}

		public int GetBadgeCount(global::Kampai.Game.CurrencyStoreCategoryDefinition currencyStoreCategoryDef)
		{
			int num = 0;
			int iD = currencyStoreCategoryDef.ID;
			for (int i = 0; i < currencyStoreCategoryDef.StoreItemDefinitionIDs.Count; i++)
			{
				int num2 = currencyStoreCategoryDef.StoreItemDefinitionIDs[i];
				if ((!badgeIgnoreList.ContainsKey(iD) || !badgeIgnoreList[iD].Contains(num2)) && (!localState.ItemsViewedMap.ContainsKey(iD) || !localState.ItemsViewedMap[iD].Contains(num2)) && IsValidCurrencyItem(num2, currencyStoreCategoryDef.StoreCategoryType, false) && ShouldItemBeMarkedAsViewed(definitionService.Get<global::Kampai.Game.StoreItemDefinition>(num2)))
				{
					num++;
				}
			}
			return num;
		}

		public void MarkCategoryAsViewed(int categoryDefinitionID)
		{
			global::Kampai.Game.CurrencyStoreCategoryDefinition currencyStoreCategoryDef = definitionService.Get<global::Kampai.Game.CurrencyStoreCategoryDefinition>(categoryDefinitionID);
			MarkCategoryAsViewed(currencyStoreCategoryDef);
		}

		public void MarkCategoryAsViewed(global::Kampai.Game.CurrencyStoreCategoryDefinition currencyStoreCategoryDef)
		{
			int iD = currencyStoreCategoryDef.ID;
			for (int i = 0; i < currencyStoreCategoryDef.StoreItemDefinitionIDs.Count; i++)
			{
				int num = currencyStoreCategoryDef.StoreItemDefinitionIDs[i];
				if ((!badgeIgnoreList.ContainsKey(iD) || !badgeIgnoreList[iD].Contains(num)) && (!localState.ItemsViewedMap.ContainsKey(iD) || !localState.ItemsViewedMap[iD].Contains(num)) && IsValidCurrencyItem(num, currencyStoreCategoryDef.StoreCategoryType, false) && ShouldItemBeMarkedAsViewed(definitionService.Get<global::Kampai.Game.StoreItemDefinition>(num)))
				{
					if (!localState.ItemsViewedMap.ContainsKey(iD))
					{
						global::System.Collections.Generic.List<int> list = new global::System.Collections.Generic.List<int>();
						list.Add(num);
						localState.ItemsViewedMap.Add(iD, list);
					}
					else
					{
						localState.ItemsViewedMap[iD].Add(num);
					}
				}
			}
			PersistLocalState();
		}

		private void PersistLocalState()
		{
			if (localState != null)
			{
				try
				{
					string data = global::Newtonsoft.Json.JsonConvert.SerializeObject(localState);
					localPersistanceService.PutDataPlayer("CurrencyStoreLocalSave", data);
					return;
				}
				catch (global::Newtonsoft.Json.JsonSerializationException ex)
				{
					logger.Error("PersistLocalState(): Json Parse Err: {0}", ex);
					return;
				}
			}
			localPersistanceService.DeleteKeyPlayer("CurrencyStoreLocalSave");
		}

		private void LoadPersist()
		{
			if (localPersistanceService.HasKeyPlayer("CurrencyStoreLocalSave"))
			{
				string dataPlayer = localPersistanceService.GetDataPlayer("CurrencyStoreLocalSave");
				if (dataPlayer != null)
				{
					try
					{
						localState = global::Newtonsoft.Json.JsonConvert.DeserializeObject<global::Kampai.UI.CurrencyStoreLocalState>(dataPlayer);
					}
					catch (global::Newtonsoft.Json.JsonSerializationException e)
					{
						HandleJsonException(e);
					}
					catch (global::Newtonsoft.Json.JsonReaderException e2)
					{
						HandleJsonException(e2);
					}
				}
			}
			if (localState == null)
			{
				localState = new global::Kampai.UI.CurrencyStoreLocalState();
			}
		}

		private void HandleJsonException(global::System.Exception e)
		{
			logger.Error("CurrencyStoreLocalState.LoadFromPersistence(): Json Parse Err: {0}", e);
			localState = new global::Kampai.UI.CurrencyStoreLocalState();
		}
	}
}
