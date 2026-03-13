namespace Kampai.Game
{
	public class LandExpansionBuilding : global::Kampai.Game.Building<global::Kampai.Game.LandExpansionBuildingDefinition>, global::Kampai.Game.DestructibleBuilding
	{
		public int ExpansionID { get; set; }

		public int MinimumLevel { get; set; }

		public LandExpansionBuilding(global::Kampai.Game.LandExpansionBuildingDefinition def)
			: base(def)
		{
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "MINIMUMLEVEL":
				reader.Read();
				MinimumLevel = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			case "EXPANSIONID":
				reader.Read();
				ExpansionID = global::System.Convert.ToInt32(reader.Value);
				break;
			}
			return true;
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
			writer.WritePropertyName("ExpansionID");
			writer.WriteValue(ExpansionID);
			writer.WritePropertyName("MinimumLevel");
			writer.WriteValue(MinimumLevel);
		}

		public override global::Kampai.Game.View.BuildingObject AddBuildingObject(global::UnityEngine.GameObject gameObject)
		{
			return gameObject.AddComponent<global::Kampai.Game.View.LandExpansionBuildingObject>();
		}
	}
}
