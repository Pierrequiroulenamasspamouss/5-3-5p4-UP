namespace Kampai.Game
{
	public class CustomCameraPositionDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1071;
			}
		}

		public float xPos { get; set; }

		public float yPos { get; set; }

		public float zPos { get; set; }

		public float xRotation { get; set; }

		public float yRotation { get; set; }

		public float zRotation { get; set; }

		public float FOV { get; set; }

		public float nearClip { get; set; }

		public float farClip { get; set; }

		public bool enableCameraControl { get; set; }

		public string panSound { get; set; }

		public float duration { get; set; }

		public CustomCameraPositionDefinition()
		{
			duration = 1f;
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(xPos);
			writer.Write(yPos);
			writer.Write(zPos);
			writer.Write(xRotation);
			writer.Write(yRotation);
			writer.Write(zRotation);
			writer.Write(FOV);
			writer.Write(nearClip);
			writer.Write(farClip);
			writer.Write(enableCameraControl);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, panSound);
			writer.Write(duration);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			xPos = reader.ReadSingle();
			yPos = reader.ReadSingle();
			zPos = reader.ReadSingle();
			xRotation = reader.ReadSingle();
			yRotation = reader.ReadSingle();
			zRotation = reader.ReadSingle();
			FOV = reader.ReadSingle();
			nearClip = reader.ReadSingle();
			farClip = reader.ReadSingle();
			enableCameraControl = reader.ReadBoolean();
			panSound = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			duration = reader.ReadSingle();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "XPOS":
				reader.Read();
				xPos = global::System.Convert.ToSingle(reader.Value);
				break;
			case "YPOS":
				reader.Read();
				yPos = global::System.Convert.ToSingle(reader.Value);
				break;
			case "ZPOS":
				reader.Read();
				zPos = global::System.Convert.ToSingle(reader.Value);
				break;
			case "XROTATION":
				reader.Read();
				xRotation = global::System.Convert.ToSingle(reader.Value);
				break;
			case "YROTATION":
				reader.Read();
				yRotation = global::System.Convert.ToSingle(reader.Value);
				break;
			case "ZROTATION":
				reader.Read();
				zRotation = global::System.Convert.ToSingle(reader.Value);
				break;
			case "FOV":
				reader.Read();
				FOV = global::System.Convert.ToSingle(reader.Value);
				break;
			case "NEARCLIP":
				reader.Read();
				nearClip = global::System.Convert.ToSingle(reader.Value);
				break;
			case "FARCLIP":
				reader.Read();
				farClip = global::System.Convert.ToSingle(reader.Value);
				break;
			case "ENABLECAMERACONTROL":
				reader.Read();
				enableCameraControl = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "PANSOUND":
				reader.Read();
				panSound = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "DURATION":
				reader.Read();
				duration = global::System.Convert.ToSingle(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}
	}
}
