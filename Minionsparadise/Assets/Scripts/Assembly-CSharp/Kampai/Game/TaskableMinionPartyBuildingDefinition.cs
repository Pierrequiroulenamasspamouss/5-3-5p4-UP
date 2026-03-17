namespace Kampai.Game
{
	public abstract class TaskableMinionPartyBuildingDefinition : global::Kampai.Game.TaskableBuildingDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1064;
			}
		}

		public global::System.Collections.Generic.IList<global::Kampai.Game.MinionPartyPrefabDefinition> MinionPartyPrefabs { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteMinionPartyPrefabDefinition, MinionPartyPrefabs);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			MinionPartyPrefabs = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadMinionPartyPrefabDefinition, MinionPartyPrefabs);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "MINIONPARTYPREFABS":
				reader.Read();
				MinionPartyPrefabs = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadMinionPartyPrefabDefinition, MinionPartyPrefabs);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}
	}
}
