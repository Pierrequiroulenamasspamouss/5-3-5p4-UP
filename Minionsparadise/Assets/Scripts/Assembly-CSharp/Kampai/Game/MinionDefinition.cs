namespace Kampai.Game
{
	public class MinionDefinition : global::Kampai.Game.Definition, global::Kampai.Util.IBuilder<global::Kampai.Game.Instance>
	{
		public override int TypeCode
		{
			get
			{
				return 1112;
			}
		}

		public uint Eyes { get; set; }

		public global::Kampai.Game.MinionBody Body { get; set; }

		public global::Kampai.Game.MinionHair Hair { get; set; }

		public int LeisureCooldownTime { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(Eyes);
			global::Kampai.Util.BinarySerializationUtil.WriteEnum(writer, Body);
			global::Kampai.Util.BinarySerializationUtil.WriteEnum(writer, Hair);
			writer.Write(LeisureCooldownTime);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Eyes = reader.ReadUInt32();
			Body = global::Kampai.Util.BinarySerializationUtil.ReadEnum<global::Kampai.Game.MinionBody>(reader);
			Hair = global::Kampai.Util.BinarySerializationUtil.ReadEnum<global::Kampai.Game.MinionHair>(reader);
			LeisureCooldownTime = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "EYES":
				reader.Read();
				Eyes = global::System.Convert.ToUInt32(reader.Value);
				break;
			case "BODY":
				reader.Read();
				Body = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Game.MinionBody>(reader);
				break;
			case "HAIR":
				reader.Read();
				Hair = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Game.MinionHair>(reader);
				break;
			case "LEISURECOOLDOWNTIME":
				reader.Read();
				LeisureCooldownTime = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public global::Kampai.Game.Instance Build()
		{
			return new global::Kampai.Game.Minion(this);
		}
	}
}
