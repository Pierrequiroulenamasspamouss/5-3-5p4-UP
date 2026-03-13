namespace Kampai.Game
{
	public class LocationIncidentalAnimationDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1078;
			}
		}

		public int AnimationId { get; set; }

		public global::Kampai.Game.FloatLocation Location { get; set; }

		public global::Kampai.Game.Angle Rotation { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(AnimationId);
			global::Kampai.Util.BinarySerializationUtil.WriteFloatLocation(writer, Location);
			global::Kampai.Util.BinarySerializationUtil.WriteAngle(writer, Rotation);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			AnimationId = reader.ReadInt32();
			Location = global::Kampai.Util.BinarySerializationUtil.ReadFloatLocation(reader);
			Rotation = global::Kampai.Util.BinarySerializationUtil.ReadAngle(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "ANIMATIONID":
				reader.Read();
				AnimationId = global::System.Convert.ToInt32(reader.Value);
				break;
			case "LOCATION":
				reader.Read();
				Location = global::Kampai.Util.ReaderUtil.ReadFloatLocation(reader, converters);
				break;
			case "ROTATION":
				reader.Read();
				Rotation = global::Kampai.Util.ReaderUtil.ReadAngle(reader, converters);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}
	}
}
