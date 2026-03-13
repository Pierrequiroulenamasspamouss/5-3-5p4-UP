namespace Kampai.Game
{
	public class LeisureBuildingDefintiion : global::Kampai.Game.AnimatingBuildingDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1052;
			}
		}

		public int LeisureTimeDuration { get; set; }

		public int PartyPointsReward { get; set; }

		public string VFXPrefab { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(LeisureTimeDuration);
			writer.Write(PartyPointsReward);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, VFXPrefab);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			LeisureTimeDuration = reader.ReadInt32();
			PartyPointsReward = reader.ReadInt32();
			VFXPrefab = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "LEISURETIMEDURATION":
				reader.Read();
				LeisureTimeDuration = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PARTYPOINTSREWARD":
				reader.Read();
				PartyPointsReward = global::System.Convert.ToInt32(reader.Value);
				break;
			case "VFXPREFAB":
				reader.Read();
				VFXPrefab = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public override global::Kampai.Game.Building BuildBuilding()
		{
			return new global::Kampai.Game.LeisureBuilding(this);
		}
	}
}
