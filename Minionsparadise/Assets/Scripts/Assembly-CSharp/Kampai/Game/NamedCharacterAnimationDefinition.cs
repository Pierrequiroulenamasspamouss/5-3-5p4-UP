namespace Kampai.Game
{
	public class NamedCharacterAnimationDefinition : global::Kampai.Game.AnimationDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1074;
			}
		}

		public string StateMachine { get; set; }

		public float SpreadMin { get; set; }

		public float SpreadMax { get; set; }

		public int IdleCount { get; set; }

		public float AttentionDuration { get; set; }

		public global::Kampai.Game.CharacterUIAnimationDefinition characterUIAnimationDefinition { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, StateMachine);
			writer.Write(SpreadMin);
			writer.Write(SpreadMax);
			writer.Write(IdleCount);
			writer.Write(AttentionDuration);
			global::Kampai.Util.BinarySerializationUtil.WriteCharacterUIAnimationDefinition(writer, characterUIAnimationDefinition);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			StateMachine = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			SpreadMin = reader.ReadSingle();
			SpreadMax = reader.ReadSingle();
			IdleCount = reader.ReadInt32();
			AttentionDuration = reader.ReadSingle();
			characterUIAnimationDefinition = global::Kampai.Util.BinarySerializationUtil.ReadCharacterUIAnimationDefinition(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "STATEMACHINE":
				reader.Read();
				StateMachine = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "SPREADMIN":
				reader.Read();
				SpreadMin = global::System.Convert.ToSingle(reader.Value);
				break;
			case "SPREADMAX":
				reader.Read();
				SpreadMax = global::System.Convert.ToSingle(reader.Value);
				break;
			case "IDLECOUNT":
				reader.Read();
				IdleCount = global::System.Convert.ToInt32(reader.Value);
				break;
			case "ATTENTIONDURATION":
				reader.Read();
				AttentionDuration = global::System.Convert.ToSingle(reader.Value);
				break;
			case "CHARACTERUIANIMATIONDEFINITION":
				reader.Read();
				characterUIAnimationDefinition = global::Kampai.Util.ReaderUtil.ReadCharacterUIAnimationDefinition(reader, converters);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}
	}
}
