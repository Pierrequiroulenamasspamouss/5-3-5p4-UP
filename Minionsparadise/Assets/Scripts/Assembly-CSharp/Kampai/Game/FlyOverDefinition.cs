namespace Kampai.Game
{
	public class FlyOverDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1087;
			}
		}

		public float time { get; set; }

		public global::System.Collections.Generic.IList<global::Kampai.Game.FlyOverNode> path { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(time);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteFlyOverNode, path);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			time = reader.ReadSingle();
			path = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadFlyOverNode, path);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "PATH":
				reader.Read();
				path = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadFlyOverNode, path);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			case "TIME":
				reader.Read();
				time = global::System.Convert.ToSingle(reader.Value);
				break;
			}
			return true;
		}
	}
}
