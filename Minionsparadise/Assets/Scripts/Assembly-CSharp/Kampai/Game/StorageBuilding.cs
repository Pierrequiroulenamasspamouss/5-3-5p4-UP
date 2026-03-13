namespace Kampai.Game
{
	public class StorageBuilding : global::Kampai.Game.RepairableBuilding<global::Kampai.Game.StorageBuildingDefinition>
	{
		public int CurrentStorageBuildingLevel { get; set; }

		[global::Newtonsoft.Json.JsonIgnore]
		public bool MenuOpened { get; set; }

		[global::Newtonsoft.Json.JsonIgnore]
		public bool MenuOpening { get; set; }

		public StorageBuilding(global::Kampai.Game.StorageBuildingDefinition def)
			: base(def)
		{
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "CURRENTSTORAGEBUILDINGLEVEL":
				reader.Read();
				CurrentStorageBuildingLevel = global::System.Convert.ToInt32(reader.Value);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}

		public override void Serialize(global::Newtonsoft.Json.JsonWriter writer)
		{
			writer.WriteStartObject();
			SerializeProperties(writer);
			writer.WriteEndObject();
		}

		protected override void SerializeProperties(global::Newtonsoft.Json.JsonWriter writer)
		{
			base.SerializeProperties(writer);
			writer.WritePropertyName("CurrentStorageBuildingLevel");
			writer.WriteValue(CurrentStorageBuildingLevel);
		}

		public override global::Kampai.Game.View.BuildingObject AddBuildingObject(global::UnityEngine.GameObject gameObject)
		{
			return gameObject.AddComponent<global::Kampai.Game.View.StorageBuildingObject>();
		}
	}
}
