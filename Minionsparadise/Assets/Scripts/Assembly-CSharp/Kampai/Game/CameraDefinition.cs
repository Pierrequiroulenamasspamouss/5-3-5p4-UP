namespace Kampai.Game
{
	public class CameraDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1070;
			}
		}

		public float MaxZoomInLevel { get; set; }

		public float MaxZoomOutLevel { get; set; }

		public float ZoomInBounceSpeed { get; set; }

		public float ZoomOutBounceSpeed { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(MaxZoomInLevel);
			writer.Write(MaxZoomOutLevel);
			writer.Write(ZoomInBounceSpeed);
			writer.Write(ZoomOutBounceSpeed);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			MaxZoomInLevel = reader.ReadSingle();
			MaxZoomOutLevel = reader.ReadSingle();
			ZoomInBounceSpeed = reader.ReadSingle();
			ZoomOutBounceSpeed = reader.ReadSingle();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "MAXZOOMINLEVEL":
				reader.Read();
				MaxZoomInLevel = global::System.Convert.ToSingle(reader.Value);
				break;
			case "MAXZOOMOUTLEVEL":
				reader.Read();
				MaxZoomOutLevel = global::System.Convert.ToSingle(reader.Value);
				break;
			case "ZOOMINBOUNCESPEED":
				reader.Read();
				ZoomInBounceSpeed = global::System.Convert.ToSingle(reader.Value);
				break;
			case "ZOOMOUTBOUNCESPEED":
				reader.Read();
				ZoomOutBounceSpeed = global::System.Convert.ToSingle(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}
	}
}
