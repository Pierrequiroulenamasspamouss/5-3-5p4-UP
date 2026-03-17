namespace Kampai.Game
{
	public class PhilCharacterDefinition : global::Kampai.Game.NamedCharacterDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1080;
			}
		}

		public string TikiBarStateMachine { get; set; }

		public global::System.Collections.Generic.IList<global::UnityEngine.Vector3> IntroPath { get; set; }

		public float IntroTime { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, TikiBarStateMachine);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteVector3, IntroPath);
			writer.Write(IntroTime);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			TikiBarStateMachine = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			IntroPath = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadVector3, IntroPath);
			IntroTime = reader.ReadSingle();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "TIKIBARSTATEMACHINE":
				reader.Read();
				TikiBarStateMachine = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "INTROPATH":
				reader.Read();
				IntroPath = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadVector3, IntroPath);
				break;
			case "INTROTIME":
				reader.Read();
				IntroTime = global::System.Convert.ToSingle(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public override global::Kampai.Game.Instance Build()
		{
			return new global::Kampai.Game.PhilCharacter(this);
		}
	}
}
