namespace Kampai.Game
{
	public class PlayerTrainingCategoryDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1124;
			}
		}

		public string categoryTitleLocalizedKey { get; set; }

		public global::System.Collections.Generic.List<int> trainingDefinitionIDs { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, categoryTitleLocalizedKey);
			global::Kampai.Util.BinarySerializationUtil.WriteListInt32(writer, trainingDefinitionIDs);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			categoryTitleLocalizedKey = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			trainingDefinitionIDs = global::Kampai.Util.BinarySerializationUtil.ReadListInt32(reader, trainingDefinitionIDs);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "TRAININGDEFINITIONIDS":
				reader.Read();
				trainingDefinitionIDs = global::Kampai.Util.ReaderUtil.PopulateListInt32(reader, trainingDefinitionIDs);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			case "CATEGORYTITLELOCALIZEDKEY":
				reader.Read();
				categoryTitleLocalizedKey = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			}
			return true;
		}
	}
}
