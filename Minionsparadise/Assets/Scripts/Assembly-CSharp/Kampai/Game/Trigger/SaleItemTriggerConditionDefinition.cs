namespace Kampai.Game.Trigger
{
	public class SaleItemTriggerConditionDefinition : global::Kampai.Game.Trigger.TriggerConditionDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1170;
			}
		}

		public int remainingTime { get; set; }

		public override global::Kampai.Game.Trigger.TriggerConditionType.Identifier type
		{
			get
			{
				return global::Kampai.Game.Trigger.TriggerConditionType.Identifier.MarketplaceSaleItem;
			}
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(remainingTime);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			remainingTime = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "REMAININGTIME":
				reader.Read();
				remainingTime = global::System.Convert.ToInt32(reader.Value);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}

		public override bool IsTriggered(global::strange.extensions.context.api.ICrossContextCapable gameContext)
		{
			global::Kampai.Game.IPlayerService instance = gameContext.injectionBinder.GetInstance<global::Kampai.Game.IPlayerService>();
			if (instance == null)
			{
				return false;
			}
			global::Kampai.Game.PlayerService playerService = instance as global::Kampai.Game.PlayerService;
			if (playerService == null || playerService.timeService == null)
			{
				return false;
			}
			global::System.Collections.Generic.ICollection<global::Kampai.Game.MarketplaceSaleItem> byDefinitionId = playerService.GetByDefinitionId<global::Kampai.Game.MarketplaceSaleItem>(1000008094);
			return CheckUserMarketplaceItems(playerService.timeService, byDefinitionId);
		}

		public override string ToString()
		{
			return string.Format("{0}, Operator: {1}, Type: {2}, remainingTime: {3}", GetType(), base.conditionOp, type, remainingTime);
		}

		public bool CheckUserMarketplaceItems(global::Kampai.Game.ITimeService timeService, global::System.Collections.Generic.ICollection<global::Kampai.Game.MarketplaceSaleItem> items)
		{
			if (items == null || items.Count == 0)
			{
				return false;
			}
			int saleRemaingTime = int.MaxValue;
			global::Kampai.Game.MarketplaceSaleItem nextForSaleItem = null;
			saleRemaingTime = GetClosestSaleItem(timeService, items, saleRemaingTime, ref nextForSaleItem);
			if (saleRemaingTime == int.MaxValue)
			{
				return false;
			}
			return TestOperator(remainingTime, saleRemaingTime);
		}

		public static int GetClosestSaleItem(global::Kampai.Game.ITimeService timeService, global::System.Collections.Generic.ICollection<global::Kampai.Game.MarketplaceSaleItem> items, int saleRemaingTime, ref global::Kampai.Game.MarketplaceSaleItem nextForSaleItem)
		{
			int num = timeService.CurrentTime();
			foreach (global::Kampai.Game.MarketplaceSaleItem item in items)
			{
				if (item != null)
				{
					int num2 = item.LengthOfSale + item.SaleStartTime - num;
					saleRemaingTime = global::UnityEngine.Mathf.Min(saleRemaingTime, num2);
					if (saleRemaingTime == num2)
					{
						nextForSaleItem = item;
					}
				}
			}
			return saleRemaingTime;
		}
	}
}
