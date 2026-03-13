namespace Kampai.Util
{
	public static class BuildingPacksHelper
	{
		public static bool UpdateTransactionUnlocksList(global::Kampai.Game.Transaction.TransactionInstance transaction, global::strange.extensions.injector.api.IInjectionBinder binder)
		{
			bool result = false;
			global::Kampai.Game.IDefinitionService instance = binder.GetInstance<global::Kampai.Game.IDefinitionService>();
			global::Kampai.Game.IPlayerService instance2 = binder.GetInstance<global::Kampai.Game.IPlayerService>();
			global::System.Collections.Generic.List<global::Kampai.Game.UnlockDefinition> all = instance.GetAll<global::Kampai.Game.UnlockDefinition>();
			global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem> list = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
			global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> outputs = transaction.Outputs;
			foreach (global::Kampai.Util.QuantityItem item in outputs)
			{
				global::Kampai.Game.BuildingDefinition definition;
				if (!instance.TryGet<global::Kampai.Game.BuildingDefinition>(item.ID, out definition))
				{
					continue;
				}
				result = true;
				foreach (global::Kampai.Game.UnlockDefinition item2 in all)
				{
					if (item2.ReferencedDefinitionID != definition.ID)
					{
						continue;
					}
					bool flag = false;
					foreach (global::Kampai.Util.QuantityItem item3 in list)
					{
						if (item3.ID == item2.ID)
						{
							item3.Quantity += item.Quantity;
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						uint unlockedQuantityOfID = (uint)instance2.GetUnlockedQuantityOfID(item.ID);
						uint quantity = ((!item2.Delta) ? (unlockedQuantityOfID + item.Quantity) : item.Quantity);
						list.Add(new global::Kampai.Util.QuantityItem(item2.ID, quantity));
					}
				}
			}
			foreach (global::Kampai.Util.QuantityItem item4 in list)
			{
				bool flag2 = false;
				foreach (global::Kampai.Util.QuantityItem item5 in outputs)
				{
					if (item5.ID == item4.ID)
					{
						item5.Quantity = item4.Quantity;
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					outputs.Add(item4);
				}
			}
			return result;
		}
	}
}
