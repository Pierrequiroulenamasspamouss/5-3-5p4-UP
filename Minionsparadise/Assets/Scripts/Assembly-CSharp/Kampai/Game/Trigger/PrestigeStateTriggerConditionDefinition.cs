namespace Kampai.Game.Trigger
{
	public class PrestigeStateTriggerConditionDefinition : global::Kampai.Game.Trigger.PrestigeTriggerConditionDefinitionBase
	{
		public override int TypeCode
		{
			get
			{
				return 1165;
			}
		}

		public global::Kampai.Game.PrestigeState state { get; set; }

		public override global::Kampai.Game.Trigger.TriggerConditionType.Identifier type
		{
			get
			{
				return global::Kampai.Game.Trigger.TriggerConditionType.Identifier.PrestigeState;
			}
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteEnum(writer, state);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			state = global::Kampai.Util.BinarySerializationUtil.ReadEnum<global::Kampai.Game.PrestigeState>(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "STATE":
				reader.Read();
				state = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Game.PrestigeState>(reader);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}

		public override string ToString()
		{
			return string.Format("{0}, Operator: {1}, Type: {2}, state: {3}", GetType(), base.conditionOp, type, state);
		}

		protected override bool IsTriggered(global::Kampai.Game.IPrestigeService prestigeService, global::Kampai.Game.Prestige prestigeCharacter)
		{
			return prestigeCharacter != null && TestOperator((int)state, (int)prestigeCharacter.state);
		}
	}
}
