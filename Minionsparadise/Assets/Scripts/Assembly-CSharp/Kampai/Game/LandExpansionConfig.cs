namespace Kampai.Game
{
	public class LandExpansionConfig : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1099;
			}
		}

		public int expansionId { get; set; }

		public global::System.Collections.Generic.IList<int> adjacentExpansionIds { get; set; }

		public global::System.Collections.Generic.IList<int> containedDebris { get; set; }

		public global::System.Collections.Generic.IList<int> containedAspirationalBuildings { get; set; }

		public int transactionId { get; set; }

		public global::Kampai.Game.Location routingSlot { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(expansionId);
			global::Kampai.Util.BinarySerializationUtil.WriteListInt32(writer, adjacentExpansionIds);
			global::Kampai.Util.BinarySerializationUtil.WriteListInt32(writer, containedDebris);
			global::Kampai.Util.BinarySerializationUtil.WriteListInt32(writer, containedAspirationalBuildings);
			writer.Write(transactionId);
			global::Kampai.Util.BinarySerializationUtil.WriteLocation(writer, routingSlot);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			expansionId = reader.ReadInt32();
			adjacentExpansionIds = global::Kampai.Util.BinarySerializationUtil.ReadListInt32(reader, adjacentExpansionIds);
			containedDebris = global::Kampai.Util.BinarySerializationUtil.ReadListInt32(reader, containedDebris);
			containedAspirationalBuildings = global::Kampai.Util.BinarySerializationUtil.ReadListInt32(reader, containedAspirationalBuildings);
			transactionId = reader.ReadInt32();
			routingSlot = global::Kampai.Util.BinarySerializationUtil.ReadLocation(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "EXPANSIONID":
				reader.Read();
				expansionId = global::System.Convert.ToInt32(reader.Value);
				break;
			case "ADJACENTEXPANSIONIDS":
				reader.Read();
				adjacentExpansionIds = global::Kampai.Util.ReaderUtil.PopulateListInt32(reader, adjacentExpansionIds);
				break;
			case "CONTAINEDDEBRIS":
				reader.Read();
				containedDebris = global::Kampai.Util.ReaderUtil.PopulateListInt32(reader, containedDebris);
				break;
			case "CONTAINEDASPIRATIONALBUILDINGS":
				reader.Read();
				containedAspirationalBuildings = global::Kampai.Util.ReaderUtil.PopulateListInt32(reader, containedAspirationalBuildings);
				break;
			case "TRANSACTIONID":
				reader.Read();
				transactionId = global::System.Convert.ToInt32(reader.Value);
				break;
			case "ROUTINGSLOT":
				reader.Read();
				routingSlot = global::Kampai.Util.ReaderUtil.ReadLocation(reader, converters);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}
	}
}
