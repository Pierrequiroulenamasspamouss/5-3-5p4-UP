namespace Kampai.Game
{
	public class DebrisBuilding : global::Kampai.Game.TaskableBuilding<global::Kampai.Game.DebrisBuildingDefinition>, global::Kampai.Game.DestructibleBuilding
	{
		public bool PaidInputCostToClear { get; set; }

		public DebrisBuilding(global::Kampai.Game.DebrisBuildingDefinition def)
			: base(def)
		{
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "PAIDINPUTCOSTTOCLEAR":
				reader.Read();
				PaidInputCostToClear = global::System.Convert.ToBoolean(reader.Value);
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
			writer.WritePropertyName("PaidInputCostToClear");
			writer.WriteValue(PaidInputCostToClear);
		}

		public override global::Kampai.Game.View.BuildingObject AddBuildingObject(global::UnityEngine.GameObject gameObject)
		{
			return gameObject.AddComponent<global::Kampai.Game.View.DebrisBuildingObject>();
		}

		public override int GetTransactionID(global::Kampai.Game.IDefinitionService definitionService)
		{
			return base.Definition.TransactionID;
		}
	}
}
