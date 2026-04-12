namespace Kampai.Game
{
	public class SpecialEventItemDefinition : global::Kampai.Game.ItemDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1095;
			}
		}

		public bool IsActive { get; set; }

		public string Paintover { get; set; }

		public int EventMinionCostumeId { get; set; }

		public string EventMinionController { get; set; }

		public int AwardCostumeId { get; set; }

		public int PrestigeDefinitionID { get; set; }

		public int UnlockQuestID { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(IsActive);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Paintover);
			writer.Write(EventMinionCostumeId);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, EventMinionController);
			writer.Write(AwardCostumeId);
			writer.Write(PrestigeDefinitionID);
			writer.Write(UnlockQuestID);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			IsActive = reader.ReadBoolean();
			Paintover = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			EventMinionCostumeId = reader.ReadInt32();
			EventMinionController = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			AwardCostumeId = reader.ReadInt32();
			PrestigeDefinitionID = reader.ReadInt32();
			UnlockQuestID = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "ISACTIVE":
				reader.Read();
				IsActive = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "PAINTOVER":
				reader.Read();
				Paintover = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "EVENTMINIONCOSTUMEID":
				reader.Read();
				EventMinionCostumeId = global::System.Convert.ToInt32(reader.Value);
				break;
			case "EVENTMINIONCONTROLLER":
				reader.Read();
				EventMinionController = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "AWARDCOSTUMEID":
				reader.Read();
				AwardCostumeId = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PRESTIGEDEFINITIONID":
				reader.Read();
				PrestigeDefinitionID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "UNLOCKQUESTID":
				reader.Read();
				UnlockQuestID = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public override global::Kampai.Game.Instance Build()
		{
			return new global::Kampai.Game.SpecialEventItem(this);
		}
	}
}
