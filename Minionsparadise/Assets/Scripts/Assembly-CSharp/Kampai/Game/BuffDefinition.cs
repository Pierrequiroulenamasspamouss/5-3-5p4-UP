namespace Kampai.Game
{
	public class BuffDefinition : global::Kampai.Game.DisplayableDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1114;
			}
		}

		public global::Kampai.Game.BuffType buffType { get; set; }

		public global::System.Collections.Generic.List<float> starMultiplierValue { get; set; }

		public string buffSimpleMask { get; set; }

		public string buffDetailLocalizedKey { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteEnum(writer, buffType);
			global::Kampai.Util.BinarySerializationUtil.WriteListSingle(writer, starMultiplierValue);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, buffSimpleMask);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, buffDetailLocalizedKey);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			buffType = global::Kampai.Util.BinarySerializationUtil.ReadEnum<global::Kampai.Game.BuffType>(reader);
			starMultiplierValue = global::Kampai.Util.BinarySerializationUtil.ReadListSingle(reader, starMultiplierValue);
			buffSimpleMask = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			buffDetailLocalizedKey = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "BUFFTYPE":
				reader.Read();
				buffType = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Game.BuffType>(reader);
				break;
			case "STARMULTIPLIERVALUE":
				reader.Read();
				starMultiplierValue = global::Kampai.Util.ReaderUtil.PopulateListSingle(reader, starMultiplierValue);
				break;
			case "BUFFSIMPLEMASK":
				reader.Read();
				buffSimpleMask = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "BUFFDETAILLOCALIZEDKEY":
				reader.Read();
				buffDetailLocalizedKey = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}
	}
}
