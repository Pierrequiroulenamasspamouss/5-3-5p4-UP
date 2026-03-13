namespace Kampai.Game
{
	public class AchievementDefinition : global::Kampai.Game.Definition, global::Kampai.Util.IBuilder<global::Kampai.Game.Instance>
	{
		public override int TypeCode
		{
			get
			{
				return 1016;
			}
		}

		public global::Kampai.Game.AchievementType.AchievementTypeIdentifier Type { get; set; }

		public int DefinitionID { get; set; }

		public global::Kampai.Game.AchievementID AchievementID { get; set; }

		public int Steps { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteEnum(writer, Type);
			writer.Write(DefinitionID);
			global::Kampai.Util.BinarySerializationUtil.WriteAchievementID(writer, AchievementID);
			writer.Write(Steps);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Type = global::Kampai.Util.BinarySerializationUtil.ReadEnum<global::Kampai.Game.AchievementType.AchievementTypeIdentifier>(reader);
			DefinitionID = reader.ReadInt32();
			AchievementID = global::Kampai.Util.BinarySerializationUtil.ReadAchievementID(reader);
			Steps = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "TYPE":
				reader.Read();
				Type = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Game.AchievementType.AchievementTypeIdentifier>(reader);
				break;
			case "DEFINITIONID":
				reader.Read();
				DefinitionID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "ACHIEVEMENTID":
				reader.Read();
				AchievementID = global::Kampai.Util.ReaderUtil.ReadAchievementID(reader, converters);
				break;
			case "STEPS":
				reader.Read();
				Steps = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public global::Kampai.Game.Instance Build()
		{
			return new global::Kampai.Game.Achievement(this);
		}
	}
}
