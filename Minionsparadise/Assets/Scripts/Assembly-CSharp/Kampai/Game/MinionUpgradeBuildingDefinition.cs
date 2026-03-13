namespace Kampai.Game
{
	public class MinionUpgradeBuildingDefinition : global::Kampai.Game.RepairableBuildingDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1058;
			}
		}

		public string AspirationalMessage_NeedLevel { get; set; }

		public string AspirationalMessage_NeedQuest { get; set; }

		public int UnlockAtLevel { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, AspirationalMessage_NeedLevel);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, AspirationalMessage_NeedQuest);
			writer.Write(UnlockAtLevel);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			AspirationalMessage_NeedLevel = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			AspirationalMessage_NeedQuest = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			UnlockAtLevel = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "ASPIRATIONALMESSAGE_NEEDLEVEL":
				reader.Read();
				AspirationalMessage_NeedLevel = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "ASPIRATIONALMESSAGE_NEEDQUEST":
				reader.Read();
				AspirationalMessage_NeedQuest = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "UNLOCKATLEVEL":
				reader.Read();
				UnlockAtLevel = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public override global::Kampai.Game.Building BuildBuilding()
		{
			return new global::Kampai.Game.MinionUpgradeBuilding(this);
		}
	}
}
