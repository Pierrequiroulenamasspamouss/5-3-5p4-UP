namespace Kampai.Game.Trigger
{
	public class HelpButtonTriggerConditionDefinition : global::Kampai.Game.Trigger.TriggerConditionDefinition
	{
		public enum HelpButtonTriggerMode
		{
			CLICKS = 0,
			TIME_SINCE_LAST_CLICK = 1
		}

		public override int TypeCode
		{
			get
			{
				return 1157;
			}
		}

		public global::Kampai.Game.Trigger.HelpButtonTriggerConditionDefinition.HelpButtonTriggerMode mode { get; set; }

		public int tipDefinitionId { get; set; }

		public int quantity { get; set; }

		public override global::Kampai.Game.Trigger.TriggerConditionType.Identifier type
		{
			get
			{
				return global::Kampai.Game.Trigger.TriggerConditionType.Identifier.HelpButton;
			}
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteEnum(writer, mode);
			writer.Write(tipDefinitionId);
			writer.Write(quantity);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			mode = global::Kampai.Util.BinarySerializationUtil.ReadEnum<global::Kampai.Game.Trigger.HelpButtonTriggerConditionDefinition.HelpButtonTriggerMode>(reader);
			tipDefinitionId = reader.ReadInt32();
			quantity = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "MODE":
				reader.Read();
				mode = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Game.Trigger.HelpButtonTriggerConditionDefinition.HelpButtonTriggerMode>(reader);
				break;
			case "TIPDEFINITIONID":
				reader.Read();
				tipDefinitionId = global::System.Convert.ToInt32(reader.Value);
				break;
			case "QUANTITY":
				reader.Read();
				quantity = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public override bool IsTriggered(global::strange.extensions.context.api.ICrossContextCapable gameContext)
		{
			global::Kampai.Game.IHelpTipTrackingService instance = gameContext.injectionBinder.GetInstance<global::Kampai.Game.IHelpTipTrackingService>();
			switch (mode)
			{
			case global::Kampai.Game.Trigger.HelpButtonTriggerConditionDefinition.HelpButtonTriggerMode.CLICKS:
				return TestOperator(quantity, instance.GetHelpTipShowCount(tipDefinitionId));
			case global::Kampai.Game.Trigger.HelpButtonTriggerConditionDefinition.HelpButtonTriggerMode.TIME_SINCE_LAST_CLICK:
				return TestOperator(quantity, instance.GetSecondsSinceHelpTipShown(tipDefinitionId));
			default:
				return false;
			}
		}
	}
}
