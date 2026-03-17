namespace Kampai.Game
{
	public class DCNBuildingDefinition : global::Kampai.Game.BuildingDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1047;
			}
		}

		public int UnlockLevel { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(UnlockLevel);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			UnlockLevel = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "UNLOCKLEVEL":
				reader.Read();
				UnlockLevel = global::System.Convert.ToInt32(reader.Value);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}

		public override global::Kampai.Game.Building BuildBuilding()
		{
			return new global::Kampai.Game.DCNBuilding(this);
		}
	}
}
