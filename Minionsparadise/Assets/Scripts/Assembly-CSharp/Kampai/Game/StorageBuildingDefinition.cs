namespace Kampai.Game
{
	public class StorageBuildingDefinition : global::Kampai.Game.AnimatingBuildingDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1063;
			}
		}

		public int Capacity { get; set; }

		public global::System.Collections.Generic.IList<global::Kampai.Game.StorageUpgradeDefinition> StorageUpgrades { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(Capacity);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteStorageUpgradeDefinition, StorageUpgrades);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Capacity = reader.ReadInt32();
			StorageUpgrades = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadStorageUpgradeDefinition, StorageUpgrades);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			default:
			{
                        int num = 1; //FIX USE OF UNASSIGNED VARIABLE
                        if (num == 1)
				{
					reader.Read();
					StorageUpgrades = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadStorageUpgradeDefinition, StorageUpgrades);
					break;
				}
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			case "CAPACITY":
				reader.Read();
				Capacity = global::System.Convert.ToInt32(reader.Value);
				break;
			}
			return true;
		}

		public override global::Kampai.Game.Building BuildBuilding()
		{
			return new global::Kampai.Game.StorageBuilding(this);
		}
	}
}
