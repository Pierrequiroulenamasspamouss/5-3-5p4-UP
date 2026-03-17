namespace Kampai.Game
{
	public class DecorationBuildingDefinition : global::Kampai.Game.BuildingDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1044;
			}
		}

		public int Cost { get; set; }

		public int XPReward { get; set; }

		public bool AutoPlace { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(Cost);
			writer.Write(XPReward);
			writer.Write(AutoPlace);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Cost = reader.ReadInt32();
			XPReward = reader.ReadInt32();
			AutoPlace = reader.ReadBoolean();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "COST":
				reader.Read();
				Cost = global::System.Convert.ToInt32(reader.Value);
				break;
			case "XPREWARD":
				reader.Read();
				XPReward = global::System.Convert.ToInt32(reader.Value);
				break;
			case "AUTOPLACE":
				reader.Read();
				AutoPlace = global::System.Convert.ToBoolean(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public override global::Kampai.Game.Building BuildBuilding()
		{
			return new global::Kampai.Game.DecorationBuilding(this);
		}
	}
}
