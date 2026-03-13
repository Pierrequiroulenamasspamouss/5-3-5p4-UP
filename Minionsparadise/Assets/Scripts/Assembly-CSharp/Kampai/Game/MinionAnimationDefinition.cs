namespace Kampai.Game
{
	public class MinionAnimationDefinition : global::Kampai.Game.AnimationDefinition
	{
		public global::System.Collections.Generic.Dictionary<string, object> arguments;

		public override int TypeCode
		{
			get
			{
				return 1076;
			}
		}

		public string StateMachine { get; set; }

		public bool FaceCamera { get; set; }

		public float AnimationSeconds { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, StateMachine);
			writer.Write(FaceCamera);
			writer.Write(AnimationSeconds);
			global::Kampai.Util.BinarySerializationUtil.WriteDictionary(writer, arguments);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			StateMachine = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			FaceCamera = reader.ReadBoolean();
			AnimationSeconds = reader.ReadSingle();
			arguments = global::Kampai.Util.BinarySerializationUtil.ReadDictionary(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "STATEMACHINE":
				reader.Read();
				StateMachine = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "FACECAMERA":
				reader.Read();
				FaceCamera = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "ANIMATIONSECONDS":
				reader.Read();
				AnimationSeconds = global::System.Convert.ToSingle(reader.Value);
				break;
			case "ARGUMENTS":
				reader.Read();
				arguments = global::Kampai.Util.ReaderUtil.ReadDictionary(reader);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}
	}
}
