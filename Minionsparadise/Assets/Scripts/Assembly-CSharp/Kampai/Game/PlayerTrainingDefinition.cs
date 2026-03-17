namespace Kampai.Game
{
	public class PlayerTrainingDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1125;
			}
		}

		public string trainingTitleLocalizedKey { get; set; }

		public int cardOneDefinitionID { get; set; }

		public int cardTwoDefinitionID { get; set; }

		public int cardThreeDefinitionID { get; set; }

		public global::Kampai.Game.TransitionType transitionOne { get; set; }

		public global::Kampai.Game.TransitionType transitionTwo { get; set; }

		public bool disableAutomaticDisplay { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, trainingTitleLocalizedKey);
			writer.Write(cardOneDefinitionID);
			writer.Write(cardTwoDefinitionID);
			writer.Write(cardThreeDefinitionID);
			global::Kampai.Util.BinarySerializationUtil.WriteEnum(writer, transitionOne);
			global::Kampai.Util.BinarySerializationUtil.WriteEnum(writer, transitionTwo);
			writer.Write(disableAutomaticDisplay);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			trainingTitleLocalizedKey = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			cardOneDefinitionID = reader.ReadInt32();
			cardTwoDefinitionID = reader.ReadInt32();
			cardThreeDefinitionID = reader.ReadInt32();
			transitionOne = global::Kampai.Util.BinarySerializationUtil.ReadEnum<global::Kampai.Game.TransitionType>(reader);
			transitionTwo = global::Kampai.Util.BinarySerializationUtil.ReadEnum<global::Kampai.Game.TransitionType>(reader);
			disableAutomaticDisplay = reader.ReadBoolean();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "TRAININGTITLELOCALIZEDKEY":
				reader.Read();
				trainingTitleLocalizedKey = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "CARDONEDEFINITIONID":
				reader.Read();
				cardOneDefinitionID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "CARDTWODEFINITIONID":
				reader.Read();
				cardTwoDefinitionID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "CARDTHREEDEFINITIONID":
				reader.Read();
				cardThreeDefinitionID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "TRANSITIONONE":
				reader.Read();
				transitionOne = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Game.TransitionType>(reader);
				break;
			case "TRANSITIONTWO":
				reader.Read();
				transitionTwo = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Game.TransitionType>(reader);
				break;
			case "DISABLEAUTOMATICDISPLAY":
				reader.Read();
				disableAutomaticDisplay = global::System.Convert.ToBoolean(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}
	}
}
