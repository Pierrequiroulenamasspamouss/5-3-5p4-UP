namespace Kampai.Game
{
	public class CompositeBuildingDefinition : global::Kampai.Game.BuildingDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1041;
			}
		}

		public int MaxPieces { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(MaxPieces);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			MaxPieces = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "MAXPIECES":
				reader.Read();
				MaxPieces = global::System.Convert.ToInt32(reader.Value);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}

		public override global::Kampai.Game.Building BuildBuilding()
		{
			return new global::Kampai.Game.CompositeBuilding(this);
		}
	}
}
