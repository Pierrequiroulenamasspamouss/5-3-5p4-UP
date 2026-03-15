namespace Kampai.Game
{
	public class StickerDefinition : global::Kampai.Game.DisplayableDefinition, global::Kampai.Util.IBuilder<global::Kampai.Game.Instance>
	{
		public override int TypeCode
		{
			get
			{
				return 1141;
			}
		}

		public int CharacterID { get; set; }

		public bool IsLimitedTime { get; set; }

		public int EventDefinitionID { get; set; }

		public int UnlockLevel { get; set; }

		public bool deprecated { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(CharacterID);
			writer.Write(IsLimitedTime);
			writer.Write(EventDefinitionID);
			writer.Write(UnlockLevel);
			writer.Write(deprecated);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			CharacterID = reader.ReadInt32();
			IsLimitedTime = reader.ReadBoolean();
			EventDefinitionID = reader.ReadInt32();
			UnlockLevel = reader.ReadInt32();
			deprecated = reader.ReadBoolean();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "CHARACTERID":
				reader.Read();
				CharacterID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "ISLIMITEDTIME":
				reader.Read();
				IsLimitedTime = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "EVENTDEFINITIONID":
				reader.Read();
				EventDefinitionID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "UNLOCKLEVEL":
				reader.Read();
				UnlockLevel = global::System.Convert.ToInt32(reader.Value);
				break;
			case "DEPRECATED":
				reader.Read();
				deprecated = global::System.Convert.ToBoolean(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public global::Kampai.Game.Instance Build()
		{
			return new global::Kampai.Game.Sticker(this);
		}
	}
}
