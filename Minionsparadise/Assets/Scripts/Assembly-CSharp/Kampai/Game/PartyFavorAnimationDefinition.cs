namespace Kampai.Game
{
	public class PartyFavorAnimationDefinition : global::Kampai.Game.AnimationDefinition
	{
		public const int INFINITE_MINIONS = 4;

		public override int TypeCode
		{
			get
			{
				return 1113;
			}
		}

		public int AnimationID { get; set; }

		public int FootprintID { get; set; }

		public string Prefab { get; set; }

		public int ItemID { get; set; }

		public int UnlockId { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(AnimationID);
			writer.Write(FootprintID);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Prefab);
			writer.Write(ItemID);
			writer.Write(UnlockId);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			AnimationID = reader.ReadInt32();
			FootprintID = reader.ReadInt32();
			Prefab = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			ItemID = reader.ReadInt32();
			UnlockId = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "ANIMATIONID":
				reader.Read();
				AnimationID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "FOOTPRINTID":
				reader.Read();
				FootprintID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PREFAB":
				reader.Read();
				Prefab = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "ITEMID":
				reader.Read();
				ItemID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "UNLOCKID":
				reader.Read();
				UnlockId = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}
	}
}
