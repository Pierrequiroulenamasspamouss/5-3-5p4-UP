namespace Kampai.Game
{
	public class HelpTipDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1003;
			}
		}

		public string Title { get; set; }

		public string Message { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Title);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Message);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Title = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			Message = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
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
					Message = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
					break;
				}
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			case "TITLE":
				reader.Read();
				Title = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			}
			return true;
		}
	}
}
