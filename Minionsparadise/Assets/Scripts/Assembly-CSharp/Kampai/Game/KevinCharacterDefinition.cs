namespace Kampai.Game
{
	public class KevinCharacterDefinition : global::Kampai.Game.FrolicCharacterDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1079;
			}
		}

		public string WelcomeHutStateMachine { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, WelcomeHutStateMachine);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			WelcomeHutStateMachine = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "WELCOMEHUTSTATEMACHINE":
				reader.Read();
				WelcomeHutStateMachine = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}

		public override global::Kampai.Game.Instance Build()
		{
			return new global::Kampai.Game.KevinCharacter(this);
		}
	}
}
