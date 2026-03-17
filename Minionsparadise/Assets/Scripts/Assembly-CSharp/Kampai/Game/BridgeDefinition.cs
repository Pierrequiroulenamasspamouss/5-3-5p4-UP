namespace Kampai.Game
{
	public class BridgeDefinition : global::Kampai.Game.ItemDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1088;
			}
		}

		public global::Kampai.Game.Location location { get; set; }

		public int TransactionId { get; set; }

		public int BuildingId { get; set; }

		public int RepairedBuildingID { get; set; }

		public int LandExpansionID { get; set; }

		public global::Kampai.Game.BridgeScreenPosition cameraPan { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteLocation(writer, location);
			writer.Write(TransactionId);
			writer.Write(BuildingId);
			writer.Write(RepairedBuildingID);
			writer.Write(LandExpansionID);
			global::Kampai.Util.BinarySerializationUtil.WriteBridgeScreenPosition(writer, cameraPan);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			location = global::Kampai.Util.BinarySerializationUtil.ReadLocation(reader);
			TransactionId = reader.ReadInt32();
			BuildingId = reader.ReadInt32();
			RepairedBuildingID = reader.ReadInt32();
			LandExpansionID = reader.ReadInt32();
			cameraPan = global::Kampai.Util.BinarySerializationUtil.ReadBridgeScreenPosition(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "LOCATION":
				reader.Read();
				location = global::Kampai.Util.ReaderUtil.ReadLocation(reader, converters);
				break;
			case "TRANSACTIONID":
				reader.Read();
				TransactionId = global::System.Convert.ToInt32(reader.Value);
				break;
			case "BUILDINGID":
				reader.Read();
				BuildingId = global::System.Convert.ToInt32(reader.Value);
				break;
			case "REPAIREDBUILDINGID":
				reader.Read();
				RepairedBuildingID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "LANDEXPANSIONID":
				reader.Read();
				LandExpansionID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "CAMERAPAN":
				reader.Read();
				cameraPan = global::Kampai.Util.ReaderUtil.ReadBridgeScreenPosition(reader, converters);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}
	}
}
