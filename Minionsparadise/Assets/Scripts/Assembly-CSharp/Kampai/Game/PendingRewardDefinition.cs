namespace Kampai.Game
{
	public class PendingRewardDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1017;
			}
		}

		public string aspirationalLocKey { get; set; }

		public int awardAtLevel { get; set; }

		public string hudReminderImage { get; set; }

		public string hudReminderMask { get; set; }

		public global::System.Collections.Generic.List<int> transactions { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, aspirationalLocKey);
			writer.Write(awardAtLevel);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, hudReminderImage);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, hudReminderMask);
			global::Kampai.Util.BinarySerializationUtil.WriteListInt32(writer, transactions);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			aspirationalLocKey = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			awardAtLevel = reader.ReadInt32();
			hudReminderImage = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			hudReminderMask = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			transactions = global::Kampai.Util.BinarySerializationUtil.ReadListInt32(reader, transactions);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "ASPIRATIONALLOCKEY":
				reader.Read();
				aspirationalLocKey = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "AWARDATLEVEL":
				reader.Read();
				awardAtLevel = global::System.Convert.ToInt32(reader.Value);
				break;
			case "HUDREMINDERIMAGE":
				reader.Read();
				hudReminderImage = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "HUDREMINDERMASK":
				reader.Read();
				hudReminderMask = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "TRANSACTIONS":
				reader.Read();
				transactions = global::Kampai.Util.ReaderUtil.PopulateListInt32(reader, transactions);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}
	}
}
