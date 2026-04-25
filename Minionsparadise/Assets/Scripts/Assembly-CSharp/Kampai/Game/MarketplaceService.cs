namespace Kampai.Game
{
	public class MarketplaceService : global::Kampai.Game.IMarketplaceService
	{
		private enum CostType
		{
			minStrike = 0,
			maxStrike = 1
		}

		private float debugMultiplier = 1f;

		[Inject]
		public global::Kampai.Game.IConfigurationsService configurationsService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject(global::Kampai.Game.SocialServices.FACEBOOK)]
		public global::Kampai.Game.ISocialService facebookService { get; set; }

		public bool IsDebugMode { get; set; }

		public float DebugMultiplier
		{
			get
			{
				return debugMultiplier;
			}
			set
			{
				debugMultiplier = value;
			}
		}

		public int DebugSelectedItem { get; set; }

		public bool DebugFacebook { get; set; }

		public bool isServerKillSwitchEnabled
		{
			get
			{
				return configurationsService.isKillSwitchOn(global::Kampai.Game.KillSwitch.MARKETPLACESERVER);
			}
		}

		public bool GetItemDefinitionByItemID(int itemID, out global::Kampai.Game.MarketplaceItemDefinition itemDefinition)
		{
			global::Kampai.Game.MarketplaceDefinition marketplaceDefinition = definitionService.Get<global::Kampai.Game.MarketplaceDefinition>();
			bool result = false;
			itemDefinition = null;
			if (marketplaceDefinition != null && marketplaceDefinition.itemDefinitions != null)
			{
				foreach (global::Kampai.Game.MarketplaceItemDefinition itemDefinition2 in marketplaceDefinition.itemDefinitions)
				{
					if (itemDefinition2.ItemID == itemID)
					{
						itemDefinition = itemDefinition2;
						result = true;
						break;
					}
				}
			}
			return result;
		}

		private void SetItemPrice(int itemID, int price, global::Kampai.Game.MarketplaceService.CostType costType)
		{
			global::Kampai.Game.MarketplaceDefinition marketplaceDefinition = definitionService.Get<global::Kampai.Game.MarketplaceDefinition>();
			if (marketplaceDefinition == null || marketplaceDefinition.itemDefinitions == null)
			{
				return;
			}
			foreach (global::Kampai.Game.MarketplaceItemDefinition itemDefinition in marketplaceDefinition.itemDefinitions)
			{
				if (itemDefinition.ItemID != itemID)
				{
					continue;
				}
				switch (costType)
				{
				case global::Kampai.Game.MarketplaceService.CostType.minStrike:
					itemDefinition.MinStrikePrice = price;
					break;
				case global::Kampai.Game.MarketplaceService.CostType.maxStrike:
					itemDefinition.MaxStrikePrice = price;
					break;
				}
				break;
			}
		}

		public void SetMinStrikePrice(int itemID, int price)
		{
			SetItemPrice(itemID, price, global::Kampai.Game.MarketplaceService.CostType.minStrike);
		}

		public void SetMaxStrikePrice(int itemID, int price)
		{
			SetItemPrice(itemID, price, global::Kampai.Game.MarketplaceService.CostType.maxStrike);
		}

		public global::Kampai.Game.MarketplaceSaleSlot GetSlotByItem(global::Kampai.Game.MarketplaceSaleItem item)
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.MarketplaceSaleSlot> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.MarketplaceSaleSlot>();
			foreach (global::Kampai.Game.MarketplaceSaleSlot item2 in instancesByType)
			{
				global::Kampai.Game.MarketplaceSaleItem byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.MarketplaceSaleItem>(item2.itemId);
				if (byInstanceId == item)
				{
					return item2;
				}
			}
			return null;
		}

		public global::Kampai.Game.MarketplaceSaleSlot GetNextAvailableSlot()
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.MarketplaceSaleSlot> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.MarketplaceSaleSlot>();
			foreach (global::Kampai.Game.MarketplaceSaleSlot item in instancesByType)
			{
				global::Kampai.Game.MarketplaceSaleItem byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.MarketplaceSaleItem>(item.itemId);
				if (byInstanceId == null && item.state == global::Kampai.Game.MarketplaceSaleSlot.State.UNLOCKED && IsSlotVisible(item))
				{
					return item;
				}
			}
			return null;
		}

		public int GetSlotIndex(global::Kampai.Game.MarketplaceSaleSlot slot)
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.MarketplaceSaleSlot> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.MarketplaceSaleSlot>();
			if (instancesByType.Contains(slot))
			{
				return instancesByType.IndexOf(slot);
			}
			return -1;
		}

		public bool AreThereValidItemsInStorage()
		{
			foreach (global::Kampai.Game.Item sellableItem in playerService.GetSellableItems())
			{
				global::Kampai.Game.DynamicIngredientsDefinition dynamicIngredientsDefinition = sellableItem.Definition as global::Kampai.Game.DynamicIngredientsDefinition;
				if (dynamicIngredientsDefinition == null)
				{
					global::Kampai.Game.MarketplaceItemDefinition itemDefinition;
					GetItemDefinitionByItemID(sellableItem.Definition.ID, out itemDefinition);
					if (itemDefinition != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool IsUnlocked()
		{
			global::Kampai.Game.MarketplaceDefinition marketplaceDefinition = definitionService.Get<global::Kampai.Game.MarketplaceDefinition>();
			if (marketplaceDefinition == null)
			{
				global::UnityEngine.Debug.LogError("<color=red>[MARKETPLACE TRACE] MarketplaceService.IsUnlocked: MarketplaceDefinition is NULL! Returning false.</color>");
				return false;
			}
			float playerLevel = playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID);
			bool unlocked = playerLevel >= marketplaceDefinition.LevelGate;
			global::UnityEngine.Debug.Log(string.Format("<color=cyan>[MARKETPLACE TRACE] MarketplaceService.IsUnlocked: PlayerLevel={0}, LevelGate={1}, result={2}</color>", playerLevel, marketplaceDefinition.LevelGate, unlocked));
			return unlocked;
		}

		public bool AreThereSoldItems()
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.MarketplaceSaleSlot> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.MarketplaceSaleSlot>();
			foreach (global::Kampai.Game.MarketplaceSaleSlot item in instancesByType)
			{
				if (item.state != global::Kampai.Game.MarketplaceSaleSlot.State.LOCKED && IsSlotVisible(item))
				{
					global::Kampai.Game.MarketplaceSaleItem byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.MarketplaceSaleItem>(item.itemId);
					if (byInstanceId != null && byInstanceId.state == global::Kampai.Game.MarketplaceSaleItem.State.SOLD)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool IsSlotVisible(global::Kampai.Game.MarketplaceSaleSlot slot)
		{
			if (slot.Definition.type == global::Kampai.Game.MarketplaceSaleSlotDefinition.SlotType.PREMIUM_UNLOCKABLE && !facebookService.isLoggedIn && !DebugFacebook)
			{
				return false;
			}
			return true;
		}

		public bool AreTherePendingItems()
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.MarketplaceSaleSlot> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.MarketplaceSaleSlot>();
			foreach (global::Kampai.Game.MarketplaceSaleSlot item in instancesByType)
			{
				global::Kampai.Game.MarketplaceSaleItem byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.MarketplaceSaleItem>(item.itemId);
				if (byInstanceId != null && byInstanceId.state == global::Kampai.Game.MarketplaceSaleItem.State.PENDING)
				{
					return true;
				}
			}
			return false;
		}
	}
}
