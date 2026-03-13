namespace Kampai.Game.Trigger
{
	public class PrestigeLevelTriggerConditionDefinition : global::Kampai.Game.Trigger.PrestigeTriggerConditionDefinitionBase
	{
		public override int TypeCode
		{
			get
			{
				return 1163;
			}
		}

		public int level { get; set; }

		public override global::Kampai.Game.Trigger.TriggerConditionType.Identifier type
		{
			get
			{
				return global::Kampai.Game.Trigger.TriggerConditionType.Identifier.PrestigeLevel;
			}
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(level);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			level = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "LEVEL":
				reader.Read();
				level = global::System.Convert.ToInt32(reader.Value);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}

		public override string ToString()
		{
			return string.Format("{0}, Operator: {1}, Type: {2}, level: {3}", GetType(), base.conditionOp, type, level);
		}

		protected override bool IsTriggered(global::Kampai.Game.IPrestigeService prestigeService, global::Kampai.Game.Prestige prestigeCharacter)
		{
			return prestigeCharacter != null && TestOperator(level, prestigeCharacter.CurrentPrestigeLevel);
		}
	}
}
