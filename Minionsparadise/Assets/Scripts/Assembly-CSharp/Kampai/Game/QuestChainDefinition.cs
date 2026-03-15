namespace Kampai.Game
{
	public class QuestChainDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1132;
			}
		}

		public string Name { get; set; }

		public string Summary { get; set; }

		public int Giver { get; set; }

		public int Level { get; set; }

		public global::System.Collections.Generic.IList<global::Kampai.Game.QuestChainStepDefinition> Steps { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Name);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Summary);
			writer.Write(Giver);
			writer.Write(Level);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteQuestChainStepDefinition, Steps);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Name = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			Summary = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			Giver = reader.ReadInt32();
			Level = reader.ReadInt32();
			Steps = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadQuestChainStepDefinition, Steps);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "NAME":
				reader.Read();
				Name = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "SUMMARY":
				reader.Read();
				Summary = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "GIVER":
				reader.Read();
				Giver = global::System.Convert.ToInt32(reader.Value);
				break;
			case "LEVEL":
				reader.Read();
				Level = global::System.Convert.ToInt32(reader.Value);
				break;
			case "STEPS":
				reader.Read();
				Steps = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadQuestChainStepDefinition, Steps);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}
	}
}
