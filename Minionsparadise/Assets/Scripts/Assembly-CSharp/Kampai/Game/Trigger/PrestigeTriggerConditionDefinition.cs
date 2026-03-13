namespace Kampai.Game.Trigger
{
	public class PrestigeTriggerConditionDefinition : global::Kampai.Game.Trigger.PrestigeTriggerConditionDefinitionBase
	{
		public override int TypeCode
		{
			get
			{
				return 1166;
			}
		}

		public int duration { get; set; }

		public override global::Kampai.Game.Trigger.TriggerConditionType.Identifier type
		{
			get
			{
				return global::Kampai.Game.Trigger.TriggerConditionType.Identifier.Prestige;
			}
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(duration);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			duration = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "DURATION":
				reader.Read();
				duration = global::System.Convert.ToInt32(reader.Value);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}

		public override string ToString()
		{
			return string.Format("{0}, Operator: {1}, Type: {2}, duration: {3}", GetType(), base.conditionOp, type, duration);
		}

		protected override bool IsTriggered(global::Kampai.Game.IPrestigeService prestigeService, global::Kampai.Game.Prestige prestigeCharacter)
		{
			int idlePrestigeDuration = prestigeService.GetIdlePrestigeDuration(base.prestigeDefinitionID);
			return TestOperator(duration, idlePrestigeDuration);
		}
	}
}
