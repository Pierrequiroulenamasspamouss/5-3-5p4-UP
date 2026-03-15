namespace Kampai.Game
{
	public class NotificationSystemDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1012;
			}
		}

		public global::System.Collections.Generic.List<global::Kampai.Game.NotificationReminder> notificationReminders { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteNotificationReminder, notificationReminders);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			notificationReminders = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadNotificationReminder, notificationReminders);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "NOTIFICATIONREMINDERS":
				reader.Read();
				notificationReminders = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadNotificationReminder, notificationReminders);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}
	}
}
