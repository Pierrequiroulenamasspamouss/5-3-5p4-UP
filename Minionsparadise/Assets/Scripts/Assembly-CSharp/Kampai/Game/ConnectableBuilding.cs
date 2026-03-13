namespace Kampai.Game
{
	public class ConnectableBuilding : global::Kampai.Game.Building<global::Kampai.Game.ConnectableBuildingDefinition>
	{
		public global::Kampai.Game.ConnectableBuildingPieceType pieceType { get; set; }

		public int rotation { get; set; }

		public ConnectableBuilding(global::Kampai.Game.ConnectableBuildingDefinition def)
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
					rotation = global::System.Convert.ToInt32(reader.Value);
					break;
				}
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			case "PIECETYPE":
				reader.Read();
				pieceType = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Game.ConnectableBuildingPieceType>(reader);
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
			writer.WritePropertyName("pieceType");
			writer.WriteValue((int)pieceType);
			writer.WritePropertyName("rotation");
			writer.WriteValue(rotation);
		}

		public override global::Kampai.Game.View.BuildingObject AddBuildingObject(global::UnityEngine.GameObject gameObject)
		{
			return gameObject.AddComponent<global::Kampai.Game.View.ConnectableBuildingObject>();
		}
	}
}
