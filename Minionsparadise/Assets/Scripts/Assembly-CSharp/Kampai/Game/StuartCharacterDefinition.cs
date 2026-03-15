namespace Kampai.Game
{
	public class StuartCharacterDefinition : global::Kampai.Game.FrolicCharacterDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1082;
			}
		}

		public string StageStateMachine { get; set; }

		public global::Kampai.Game.MinionAnimationDefinition OnStageAnimation { get; set; }

		public int OnStageIdleAnimationCount { get; set; }

		public int OnStageTicketFilledAnimationCount { get; set; }

		public int OnStagePerformAnimationCount { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, StageStateMachine);
			global::Kampai.Util.BinarySerializationUtil.WriteObject(writer, OnStageAnimation);
			writer.Write(OnStageIdleAnimationCount);
			writer.Write(OnStageTicketFilledAnimationCount);
			writer.Write(OnStagePerformAnimationCount);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			StageStateMachine = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			OnStageAnimation = global::Kampai.Util.BinarySerializationUtil.ReadObject<global::Kampai.Game.MinionAnimationDefinition>(reader);
			OnStageIdleAnimationCount = reader.ReadInt32();
			OnStageTicketFilledAnimationCount = reader.ReadInt32();
			OnStagePerformAnimationCount = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "STAGESTATEMACHINE":
				reader.Read();
				StageStateMachine = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "ONSTAGEANIMATION":
				reader.Read();
				OnStageAnimation = global::Kampai.Util.FastJSONDeserializer.Deserialize<global::Kampai.Game.MinionAnimationDefinition>(reader, converters);
				break;
			case "ONSTAGEIDLEANIMATIONCOUNT":
				reader.Read();
				OnStageIdleAnimationCount = global::System.Convert.ToInt32(reader.Value);
				break;
			case "ONSTAGETICKETFILLEDANIMATIONCOUNT":
				reader.Read();
				OnStageTicketFilledAnimationCount = global::System.Convert.ToInt32(reader.Value);
				break;
			case "ONSTAGEPERFORMANIMATIONCOUNT":
				reader.Read();
				OnStagePerformAnimationCount = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public override global::Kampai.Game.Instance Build()
		{
			return new global::Kampai.Game.StuartCharacter(this);
		}
	}
}
