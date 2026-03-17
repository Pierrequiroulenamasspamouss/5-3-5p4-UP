namespace Kampai.Game.Transaction
{
	public static class TransactionUtil
	{
		public static int GetPremiumCostForTransaction(global::Kampai.Game.Transaction.TransactionDefinition transaction)
		{
			return SumOutputsForStaticItem(transaction, global::Kampai.Game.StaticItem.PREMIUM_CURRENCY_ID, true);
		}

		public static int GetPremiumOutputForTransaction(global::Kampai.Game.Transaction.TransactionDefinition transaction)
		{
			return SumOutputsForStaticItem(transaction, global::Kampai.Game.StaticItem.PREMIUM_CURRENCY_ID);
		}

		public static int GetGrindOutputForTransaction(global::Kampai.Game.Transaction.TransactionDefinition transaction)
		{
			return SumOutputsForStaticItem(transaction, global::Kampai.Game.StaticItem.GRIND_CURRENCY_ID);
		}

		public static int GetXPOutputForTransaction(global::Kampai.Game.Transaction.TransactionDefinition transaction)
		{
			return SumOutputsForStaticItem(transaction, global::Kampai.Game.StaticItem.XP_ID);
		}

		public static string GetTransactionItemName(global::Kampai.Game.Transaction.TransactionDefinition transaction, global::Kampai.Game.IDefinitionService definitionService)
		{
			if (transaction.Outputs != null)
			{
				foreach (global::Kampai.Util.QuantityItem output in transaction.Outputs)
				{
					global::Kampai.Game.ItemDefinition definition = null;
					definitionService.TryGet<global::Kampai.Game.ItemDefinition>(output.ID, out definition);
					if (definition != null)
					{
						int iD = definition.ID;
						if (iD == 2 || iD == 0 || iD == 1)
						{
							return string.Empty;
						}
						return definition.LocalizedKey;
					}
				}
			}
			else if (transaction.Inputs != null)
			{
				global::Kampai.Game.ItemDefinition definition2 = null;
				definitionService.TryGet<global::Kampai.Game.ItemDefinition>(transaction.Inputs[0].ID, out definition2);
				if (definition2 != null)
				{
					int iD2 = definition2.ID;
					if (iD2 == 2 || iD2 == 0 || iD2 == 1)
					{
						return string.Empty;
					}
					return definition2.LocalizedKey;
				}
			}
			return string.Empty;
		}

		public static int SumOutputsForStaticItem(global::Kampai.Game.Transaction.TransactionDefinition transaction, global::Kampai.Game.StaticItem staticItem, bool inputs = false)
		{
			int num = 0;
			if (transaction != null)
			{
				global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> list;
				if (inputs)
				{
					global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> inputs2 = transaction.Inputs;
					list = inputs2;
				}
				else
				{
					list = transaction.Outputs;
				}
				global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> list2 = list;
				if (list2 != null)
				{
					foreach (global::Kampai.Util.QuantityItem item in list2)
					{
						if (item.ID == (int)staticItem)
						{
							num += (int)item.Quantity;
						}
					}
				}
			}
			return num;
		}

		public static int SumInputsForStaticItem(global::Kampai.Game.Transaction.TransactionDefinition transaction, global::Kampai.Game.StaticItem staticItem)
		{
			return SumOutputsForStaticItem(transaction, staticItem, true);
		}

		public static bool IsOnlyPremiumInputs(global::Kampai.Game.Transaction.TransactionDefinition def)
		{
			return IsOnlyIDInputs(def, 1);
		}

		public static bool IsOnlyGrindInputs(global::Kampai.Game.Transaction.TransactionDefinition def)
		{
			return IsOnlyIDInputs(def, 0);
		}

		public static bool IsOnlyIDInputs(global::Kampai.Game.Transaction.TransactionDefinition def, int id)
		{
			bool result = false;
			global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> inputs = def.Inputs;
			if (inputs != null && inputs.Count > 0)
			{
				result = inputs[0].ID == id;
				foreach (global::Kampai.Util.QuantityItem item in inputs)
				{
					if (item.ID != id)
					{
						result = false;
					}
				}
			}
			return result;
		}

		public static int ExtractQuantityFromTransaction(global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition, int definitionID)
		{
			return ExtractQuantityFromQuantityItemList(transactionDefinition.Outputs, definitionID);
		}

		public static int ExtractQuantityFromTransaction(global::Kampai.Game.Transaction.TransactionInstance transactionInstance, int definitionID)
		{
			return ExtractQuantityFromQuantityItemList(transactionInstance.Outputs, definitionID);
		}

		private static int ExtractQuantityFromQuantityItemList(global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> itemList, int definitionID)
		{
			int result = 0;
			if (itemList != null)
			{
				foreach (global::Kampai.Util.QuantityItem item in itemList)
				{
					if (item.ID == definitionID)
					{
						result = (int)item.Quantity;
						break;
					}
				}
			}
			return result;
		}

		public static int GetTransactionCurrencyCost(global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition, global::Kampai.Game.IDefinitionService definitionService, global::Kampai.Game.IPlayerService playerService, global::Kampai.Game.StaticItem currencyType)
		{
			int num = 0;
			foreach (global::Kampai.Util.QuantityItem output in transactionDefinition.Outputs)
			{
				switch (currencyType)
				{
				case global::Kampai.Game.StaticItem.GRIND_CURRENCY_ID:
				{
					global::Kampai.Game.Definition definition2 = definitionService.Get(output.ID);
					global::Kampai.Game.ItemDefinition itemDefinition2 = definition2 as global::Kampai.Game.ItemDefinition;
					if (itemDefinition2 != null)
					{
						num += (int)(itemDefinition2.BaseGrindCost * output.Quantity);
					}
					global::Kampai.Game.BuildingDefinition buildingDefinition = definition2 as global::Kampai.Game.BuildingDefinition;
					if (buildingDefinition != null)
					{
						global::System.Collections.Generic.ICollection<global::Kampai.Game.Building> byDefinitionId = playerService.GetByDefinitionId<global::Kampai.Game.Building>(output.ID);
						int num2 = byDefinitionId.Count * buildingDefinition.IncrementalCost;
						global::Kampai.Game.Transaction.TransactionDefinition transaction = definitionService.Get<global::Kampai.Game.Transaction.TransactionDefinition>(definitionService.getItemTransactionID(output.ID));
						int num3 = SumInputsForStaticItem(transaction, global::Kampai.Game.StaticItem.GRIND_CURRENCY_ID) + num2;
						num += (int)(num3 * output.Quantity);
					}
					break;
				}
				case global::Kampai.Game.StaticItem.PREMIUM_CURRENCY_ID:
				{
					global::Kampai.Game.Definition definition = definitionService.Get(output.ID);
					global::Kampai.Game.ItemDefinition itemDefinition = definition as global::Kampai.Game.ItemDefinition;
					if (itemDefinition != null)
					{
						num += (int)(itemDefinition.BasePremiumCost * (float)output.Quantity);
					}
					break;
				}
				}
			}
			return num;
		}
	}
}
