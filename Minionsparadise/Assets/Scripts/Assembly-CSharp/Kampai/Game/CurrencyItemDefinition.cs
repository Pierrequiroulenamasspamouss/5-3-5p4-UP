namespace Kampai.Game
{
	[global::Kampai.Util.RequiresJsonConverter]
	public class CurrencyItemDefinition : global::Kampai.Game.TaxonomyDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1139;
			}
		}

		public string VFX { get; set; }

		public global::Kampai.Util.Vector3Serialize VFXOffset { get; set; }

		public string Audio { get; set; }

		public bool COPPAGated { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, VFX);
			global::Kampai.Util.BinarySerializationUtil.WriteVector3Serialize(writer, VFXOffset);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Audio);
			writer.Write(COPPAGated);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			VFX = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			VFXOffset = global::Kampai.Util.BinarySerializationUtil.ReadVector3Serialize(reader);
			Audio = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			COPPAGated = reader.ReadBoolean();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "VFX":
				reader.Read();
				VFX = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "VFXOFFSET":
				reader.Read();
				VFXOffset = global::Kampai.Util.ReaderUtil.ReadVector3Serialize(reader, converters);
				break;
			case "AUDIO":
				reader.Read();
				Audio = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "COPPAGATED":
				reader.Read();
				COPPAGated = global::System.Convert.ToBoolean(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}
	}
}
