namespace Kampai.Game.Trigger
{
	public class AvailableLandTriggerConditionDefinition : global::Kampai.Game.Trigger.TriggerConditionDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1152;
			}
		}

		public int avaliableLand { get; set; }

		public override global::Kampai.Game.Trigger.TriggerConditionType.Identifier type
		{
			get
			{
				return global::Kampai.Game.Trigger.TriggerConditionType.Identifier.AvailableLand;
			}
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(avaliableLand);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			avaliableLand = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "AVALIABLELAND":
				reader.Read();
				avaliableLand = global::System.Convert.ToInt32(reader.Value);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}

		public override string ToString()
		{
			return string.Format("{0}, Operator: {1}, Type: {2}, avaliableLand: {3}", GetType(), base.conditionOp, type, avaliableLand);
		}

		public override bool IsTriggered(global::strange.extensions.context.api.ICrossContextCapable gameContext)
		{
			global::strange.extensions.injector.api.ICrossContextInjectionBinder injectionBinder = gameContext.injectionBinder;
			if (injectionBinder == null)
			{
				return false;
			}
			global::Kampai.Game.IBuildingUtilities instance = injectionBinder.GetInstance<global::Kampai.Game.IBuildingUtilities>();
			return instance != null && TestOperator(instance.AvailableLandSpaceCount(), avaliableLand);
		}
	}
}
