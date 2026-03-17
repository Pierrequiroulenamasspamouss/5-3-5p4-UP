namespace Kampai.Game
{
	public class MasterPlanComponentBuildingDefinition : global::Kampai.Game.BuildingDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1054;
			}
		}

		public global::Kampai.Game.Location placementLocation { get; set; }

		public string animationController { get; set; }

		public string environmentalAudio { get; set; }

		public string dropAnimationController { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteLocation(writer, placementLocation);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, animationController);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, environmentalAudio);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, dropAnimationController);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			placementLocation = global::Kampai.Util.BinarySerializationUtil.ReadLocation(reader);
			animationController = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			environmentalAudio = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			dropAnimationController = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "PLACEMENTLOCATION":
				reader.Read();
				placementLocation = global::Kampai.Util.ReaderUtil.ReadLocation(reader, converters);
				break;
			case "ANIMATIONCONTROLLER":
				reader.Read();
				animationController = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "ENVIRONMENTALAUDIO":
				reader.Read();
				environmentalAudio = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "DROPANIMATIONCONTROLLER":
				reader.Read();
				dropAnimationController = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public override global::Kampai.Game.Building BuildBuilding()
		{
			return new global::Kampai.Game.MasterPlanComponentBuilding(this);
		}
	}
}
