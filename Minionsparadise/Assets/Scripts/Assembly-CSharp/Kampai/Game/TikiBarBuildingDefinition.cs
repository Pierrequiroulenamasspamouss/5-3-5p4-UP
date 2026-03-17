namespace Kampai.Game
{
	public class TikiBarBuildingDefinition : global::Kampai.Game.TaskableMinionPartyBuildingDefinition, global::Kampai.Game.ZoomableBuildingDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1065;
			}
		}

		public global::UnityEngine.Vector3 zoomOffset { get; set; }

		public global::UnityEngine.Vector3 zoomEulers { get; set; }

		public float zoomFOV { get; set; }

		public string noSignPrefab { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteVector3(writer, zoomOffset);
			global::Kampai.Util.BinarySerializationUtil.WriteVector3(writer, zoomEulers);
			writer.Write(zoomFOV);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, noSignPrefab);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			zoomOffset = global::Kampai.Util.BinarySerializationUtil.ReadVector3(reader);
			zoomEulers = global::Kampai.Util.BinarySerializationUtil.ReadVector3(reader);
			zoomFOV = reader.ReadSingle();
			noSignPrefab = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "ZOOMOFFSET":
				reader.Read();
				zoomOffset = global::Kampai.Util.ReaderUtil.ReadVector3(reader, converters);
				break;
			case "ZOOMEULERS":
				reader.Read();
				zoomEulers = global::Kampai.Util.ReaderUtil.ReadVector3(reader, converters);
				break;
			case "ZOOMFOV":
				reader.Read();
				zoomFOV = global::System.Convert.ToSingle(reader.Value);
				break;
			case "NOSIGNPREFAB":
				reader.Read();
				noSignPrefab = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public override global::Kampai.Game.Building BuildBuilding()
		{
			return new global::Kampai.Game.TikiBarBuilding(this);
		}
	}
}
