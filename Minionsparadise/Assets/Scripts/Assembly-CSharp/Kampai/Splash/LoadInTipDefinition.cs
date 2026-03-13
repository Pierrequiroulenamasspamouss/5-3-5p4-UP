namespace Kampai.Splash
{
	public class LoadInTipDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1192;
			}
		}

		public string Text { get; set; }

		public global::System.Collections.Generic.IList<global::Kampai.Splash.BucketAssignment> Buckets { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Text);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteBucketAssignment, Buckets);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Text = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			Buckets = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadBucketAssignment, Buckets);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "BUCKETS":
				reader.Read();
				Buckets = global::Kampai.Util.ReaderUtil.PopulateList<global::Kampai.Splash.BucketAssignment>(reader, converters, global::Kampai.Util.ReaderUtil.ReadBucketAssignment, Buckets);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			case "TEXT":
				reader.Read();
				Text = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			}
			return true;
		}
	}
}
