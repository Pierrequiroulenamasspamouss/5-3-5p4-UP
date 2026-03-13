namespace Kampai.Game
{
	public class LimitedQuestDefinition : global::Kampai.Game.QuestDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1131;
			}
		}

		public int ServerStartTimeUTC { get; set; }

		public int ServerStopTimeUTC { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(ServerStartTimeUTC);
			writer.Write(ServerStopTimeUTC);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			ServerStartTimeUTC = reader.ReadInt32();
			ServerStopTimeUTC = reader.ReadInt32();
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
					ServerStopTimeUTC = global::System.Convert.ToInt32(reader.Value);
					break;
				}
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			case "SERVERSTARTTIMEUTC":
				reader.Read();
				ServerStartTimeUTC = global::System.Convert.ToInt32(reader.Value);
				break;
			}
			return true;
		}
	}
}
