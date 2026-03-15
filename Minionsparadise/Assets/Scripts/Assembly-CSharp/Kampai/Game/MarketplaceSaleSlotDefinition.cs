namespace Kampai.Game
{
	public class MarketplaceSaleSlotDefinition : global::Kampai.Game.Definition, global::Kampai.Util.IBuilder<global::Kampai.Game.Instance>
	{
		public enum SlotType
		{
			DEFAULT = 0,
			FACEBOOK_UNLOCKABLE = 1,
			PREMIUM_UNLOCKABLE = 2
		}

		public override int TypeCode
		{
			get
			{
				return 1104;
			}
		}

		public global::Kampai.Game.MarketplaceSaleSlotDefinition.SlotType type { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteEnum(writer, type);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			type = global::Kampai.Util.BinarySerializationUtil.ReadEnum<global::Kampai.Game.MarketplaceSaleSlotDefinition.SlotType>(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "TYPE":
				reader.Read();
				type = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Game.MarketplaceSaleSlotDefinition.SlotType>(reader);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}

		public global::Kampai.Game.Instance Build()
		{
			return new global::Kampai.Game.MarketplaceSaleSlot(this);
		}
	}
}
