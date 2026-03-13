namespace Kampai.Game
{
	public class BridgeBuilding : global::Kampai.Game.Building<global::Kampai.Game.BridgeBuildingDefinition>
	{
		public int BridgeId { get; set; }

		public int UnlockLevel { get; set; }

		public BridgeBuilding(global::Kampai.Game.BridgeBuildingDefinition def)
			: base(def)
		{
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
					UnlockLevel = global::System.Convert.ToInt32(reader.Value);
					break;
				}
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			case "BRIDGEID":
				reader.Read();
				BridgeId = global::System.Convert.ToInt32(reader.Value);
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
			writer.WritePropertyName("BridgeId");
			writer.WriteValue(BridgeId);
			writer.WritePropertyName("UnlockLevel");
			writer.WriteValue(UnlockLevel);
		}

		public override global::Kampai.Game.View.BuildingObject AddBuildingObject(global::UnityEngine.GameObject gameObject)
		{
			return gameObject.AddComponent<global::Kampai.Game.View.BridgeBuildingObject>();
		}
	}
}
