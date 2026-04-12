namespace Kampai.Game
{
	public class PlayerTrainingCardDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1123;
			}
		}

		public string cardTitleLocalizedKey { get; set; }

		public string cardDescriptionLocalizedKey { get; set; }

		public global::System.Collections.Generic.List<global::Kampai.Game.ImageMaskCombo> cardImages { get; set; }

		public int prestigeDefinitionID { get; set; }

		public int buildingDefinitionID { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, cardTitleLocalizedKey);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, cardDescriptionLocalizedKey);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteImageMaskCombo, cardImages);
			writer.Write(prestigeDefinitionID);
			writer.Write(buildingDefinitionID);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			cardTitleLocalizedKey = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			cardDescriptionLocalizedKey = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			cardImages = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadImageMaskCombo, cardImages);
			prestigeDefinitionID = reader.ReadInt32();
			buildingDefinitionID = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "CARDTITLELOCALIZEDKEY":
				reader.Read();
				cardTitleLocalizedKey = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "CARDDESCRIPTIONLOCALIZEDKEY":
				reader.Read();
				cardDescriptionLocalizedKey = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "CARDIMAGES":
				reader.Read();
				cardImages = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadImageMaskCombo, cardImages);
				break;
			case "PRESTIGEDEFINITIONID":
				reader.Read();
				prestigeDefinitionID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "BUILDINGDEFINITIONID":
				reader.Read();
				buildingDefinitionID = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}
	}
}
