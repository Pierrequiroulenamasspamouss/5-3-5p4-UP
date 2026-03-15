namespace Kampai.Game.Trigger
{
	public class PurchaseTriggerConditionDefinition : global::Kampai.Game.Trigger.TriggerConditionDefinition
	{
		public enum PurchaseTriggerMode
		{
			GAME_SECONDS = 0,
			CAL_SECONDS = 1,
			TRANSACTIONS = 2,
			SKU = 3,
			USD = 4
		}

		public override int TypeCode
		{
			get
			{
				return 1167;
			}
		}

		public global::Kampai.Game.Trigger.PurchaseTriggerConditionDefinition.PurchaseTriggerMode mode { get; set; }

		public uint quantity { get; set; }

		public string sku { get; set; }

		public override global::Kampai.Game.Trigger.TriggerConditionType.Identifier type
		{
			get
			{
				return global::Kampai.Game.Trigger.TriggerConditionType.Identifier.Purchase;
			}
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteEnum(writer, mode);
			writer.Write(quantity);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, sku);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			mode = global::Kampai.Util.BinarySerializationUtil.ReadEnum<global::Kampai.Game.Trigger.PurchaseTriggerConditionDefinition.PurchaseTriggerMode>(reader);
			quantity = reader.ReadUInt32();
			sku = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "MODE":
				reader.Read();
				mode = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Game.Trigger.PurchaseTriggerConditionDefinition.PurchaseTriggerMode>(reader);
				break;
			case "QUANTITY":
				reader.Read();
				quantity = global::System.Convert.ToUInt32(reader.Value);
				break;
			case "SKU":
				reader.Read();
				sku = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public override string ToString()
		{
			return string.Format("{0}, Operator: {1}, Type: {2}, mode: {3}, quantity: {4}, sku: {5}", GetType(), base.conditionOp, type, mode, quantity, sku);
		}

		public override bool IsTriggered(global::strange.extensions.context.api.ICrossContextCapable gameContext)
		{
			global::strange.extensions.injector.api.IInjectionBinder injectionBinder = gameContext.injectionBinder;
			global::Kampai.Game.IPlayerService instance = injectionBinder.GetInstance<global::Kampai.Game.IPlayerService>();
			if (instance == null)
			{
				return false;
			}
			switch (mode)
			{
			case global::Kampai.Game.Trigger.PurchaseTriggerConditionDefinition.PurchaseTriggerMode.CAL_SECONDS:
			{
				global::Kampai.Game.ITimeService instance3 = injectionBinder.GetInstance<global::Kampai.Game.ITimeService>();
				return TestOperator(quantity, instance3.CurrentTime() - instance.GetQuantity(global::Kampai.Game.StaticItem.LAST_CAL_TIME_PURCHASE));
			}
			case global::Kampai.Game.Trigger.PurchaseTriggerConditionDefinition.PurchaseTriggerMode.GAME_SECONDS:
			{
				global::Kampai.Game.IPlayerDurationService instance2 = injectionBinder.GetInstance<global::Kampai.Game.IPlayerDurationService>();
				return TestOperator(quantity, instance2.TotalGamePlaySeconds - instance.GetQuantity(global::Kampai.Game.StaticItem.LAST_GAME_TIME_PURCHASE));
			}
			case global::Kampai.Game.Trigger.PurchaseTriggerConditionDefinition.PurchaseTriggerMode.TRANSACTIONS:
				return TestOperator(quantity, instance.GetQuantity(global::Kampai.Game.StaticItem.TRANSACTIONS_LIFETIME_COUNT_ID));
			case global::Kampai.Game.Trigger.PurchaseTriggerConditionDefinition.PurchaseTriggerMode.SKU:
				return TestOperator(quantity, global::System.Convert.ToUInt32(instance.MTXPurchaseCount(sku)));
			case global::Kampai.Game.Trigger.PurchaseTriggerConditionDefinition.PurchaseTriggerMode.USD:
				return false;
			default:
			{
				global::Kampai.Util.IKampaiLogger kampaiLogger = global::Elevation.Logging.LogManager.GetClassLogger("PurchaseTriggerConditionDefinition") as global::Kampai.Util.IKampaiLogger;
				kampaiLogger.Fatal(global::Kampai.Util.FatalCode.TR_INVALID_PURCHASE_MODE, (int)mode);
				return false;
			}
			}
		}
	}
}
