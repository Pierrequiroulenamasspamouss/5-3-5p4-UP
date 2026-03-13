namespace Kampai.Game
{
	public class ResourceBuildingDefinition : global::Kampai.Game.TaskableBuildingDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1060;
			}
		}

		public int ItemId { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(ItemId);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			ItemId = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "ITEMID":
				reader.Read();
				ItemId = global::System.Convert.ToInt32(reader.Value);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}

		public override global::Kampai.Game.Building BuildBuilding()
		{
			return new global::Kampai.Game.ResourceBuilding(this);
		}
	}
}
