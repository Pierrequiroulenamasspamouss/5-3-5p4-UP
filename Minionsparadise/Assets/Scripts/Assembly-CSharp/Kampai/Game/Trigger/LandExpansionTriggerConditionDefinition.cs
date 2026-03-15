namespace Kampai.Game.Trigger
{
	public class LandExpansionTriggerConditionDefinition : global::Kampai.Game.Trigger.TriggerConditionDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1159;
			}
		}

		public int landExpansionId { get; set; }

		public bool isPurchased { get; set; }

		public override global::Kampai.Game.Trigger.TriggerConditionType.Identifier type
		{
			get
			{
				return global::Kampai.Game.Trigger.TriggerConditionType.Identifier.LandExpansion;
			}
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(landExpansionId);
			writer.Write(isPurchased);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			landExpansionId = reader.ReadInt32();
			isPurchased = reader.ReadBoolean();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "ISPURCHASED":
				reader.Read();
				isPurchased = global::System.Convert.ToBoolean(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			case "LANDEXPANSIONID":
				reader.Read();
				landExpansionId = global::System.Convert.ToInt32(reader.Value);
				break;
			}
			return true;
		}

		public override string ToString()
		{
			return string.Format("{0}, Operator: {1}, Type: {2}, landExpansionId: {3}, isPurchased: {4}", GetType(), base.conditionOp, type, landExpansionId, isPurchased);
		}

		public override bool IsTriggered(global::strange.extensions.context.api.ICrossContextCapable gameContext)
		{
			global::Kampai.Game.IPlayerService instance = gameContext.injectionBinder.GetInstance<global::Kampai.Game.IPlayerService>();
			global::Kampai.Game.PurchasedLandExpansion byInstanceId = instance.GetByInstanceId<global::Kampai.Game.PurchasedLandExpansion>(354);
			return isPurchased == byInstanceId.HasPurchased(landExpansionId);
		}
	}
}
