namespace Kampai.Splash
{
	public class LoadinTipBucketDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1191;
			}
		}

		public int Min { get; set; }

		public int Max { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(Min);
			writer.Write(Max);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Min = reader.ReadInt32();
			Max = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			default:
			{
                        int num = 1; //FIX USE OF UNASSIGNED VARIABLE
                        if (num == 1)
				{
					reader.Read();
					Max = global::System.Convert.ToInt32(reader.Value);
					break;
				}
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			case "MIN":
				reader.Read();
				Min = global::System.Convert.ToInt32(reader.Value);
				break;
			}
			return true;
		}
	}
}
