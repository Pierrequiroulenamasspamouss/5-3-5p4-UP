namespace Kampai.Game
{
	public class VillainLairEntranceBuildingDefinition : global::Kampai.Game.RepairableBuildingDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1067;
			}
		}

		public string AspirationalMessage_NeedLevel { get; set; }

		public string AspirationalMessage_NeedKevinsQuest { get; set; }

		public int UnlockAtLevel { get; set; }

		public int UpgradedMinionsNeeded { get; set; }

		public int TransactionID { get; set; }

		public global::UnityEngine.Vector3 HarvestableIconOffset { get; set; }

		public int VillainLairDefinitionID { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, AspirationalMessage_NeedLevel);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, AspirationalMessage_NeedKevinsQuest);
			writer.Write(UnlockAtLevel);
			writer.Write(UpgradedMinionsNeeded);
			writer.Write(TransactionID);
			global::Kampai.Util.BinarySerializationUtil.WriteVector3(writer, HarvestableIconOffset);
			writer.Write(VillainLairDefinitionID);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			AspirationalMessage_NeedLevel = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			AspirationalMessage_NeedKevinsQuest = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			UnlockAtLevel = reader.ReadInt32();
			UpgradedMinionsNeeded = reader.ReadInt32();
			TransactionID = reader.ReadInt32();
			HarvestableIconOffset = global::Kampai.Util.BinarySerializationUtil.ReadVector3(reader);
			VillainLairDefinitionID = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "ASPIRATIONALMESSAGE_NEEDLEVEL":
				reader.Read();
				AspirationalMessage_NeedLevel = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "ASPIRATIONALMESSAGE_NEEDKEVINSQUEST":
				reader.Read();
				AspirationalMessage_NeedKevinsQuest = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "UNLOCKATLEVEL":
				reader.Read();
				UnlockAtLevel = global::System.Convert.ToInt32(reader.Value);
				break;
			case "UPGRADEDMINIONSNEEDED":
				reader.Read();
				UpgradedMinionsNeeded = global::System.Convert.ToInt32(reader.Value);
				break;
			case "TRANSACTIONID":
				reader.Read();
				TransactionID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "HARVESTABLEICONOFFSET":
				reader.Read();
				HarvestableIconOffset = global::Kampai.Util.ReaderUtil.ReadVector3(reader, converters);
				break;
			case "VILLAINLAIRDEFINITIONID":
				reader.Read();
				VillainLairDefinitionID = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public override global::Kampai.Game.Building BuildBuilding()
		{
			return new global::Kampai.Game.VillainLairEntranceBuilding(this);
		}
	}
}
