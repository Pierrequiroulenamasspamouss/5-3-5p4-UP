namespace Kampai.Game.Trigger
{
	public class SegmentTriggerConditionDefinition : global::Kampai.Game.Trigger.TriggerConditionDefinition
	{
		public string Segment;

		public override int TypeCode
		{
			get
			{
				return 1172;
			}
		}

		public override global::Kampai.Game.Trigger.TriggerConditionType.Identifier type
		{
			get
			{
				return global::Kampai.Game.Trigger.TriggerConditionType.Identifier.Segment;
			}
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Segment);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Segment = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "SEGMENT":
				reader.Read();
				Segment = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}

		public override string ToString()
		{
			return string.Format("{0}, Operator: {1}, Type: {2}, Segment: {3}", GetType(), base.conditionOp, type, Segment);
		}

		public override bool IsTriggered(global::strange.extensions.context.api.ICrossContextCapable gameContext)
		{
			global::Kampai.Game.IPlayerService instance = gameContext.injectionBinder.GetInstance<global::Kampai.Game.IPlayerService>();
			return instance.IsInSegment(Segment);
		}
	}
}
