namespace Kampai.Game
{
	public class SocialSettingsDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1140;
			}
		}

		public bool ShowFacebookConnectPopup { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(ShowFacebookConnectPopup);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			ShowFacebookConnectPopup = reader.ReadBoolean();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "SHOWFACEBOOKCONNECTPOPUP":
				reader.Read();
				ShowFacebookConnectPopup = global::System.Convert.ToBoolean(reader.Value);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}
	}
}
