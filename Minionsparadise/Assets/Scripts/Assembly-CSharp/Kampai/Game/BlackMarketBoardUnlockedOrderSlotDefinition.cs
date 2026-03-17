namespace Kampai.Game
{
	public class BlackMarketBoardUnlockedOrderSlotDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1036;
			}
		}

		public int UnlockLevel { get; set; }

		public int OrderSlots { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(UnlockLevel);
			writer.Write(OrderSlots);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			UnlockLevel = reader.ReadInt32();
			OrderSlots = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
				default:
					return base.DeserializeProperty(propertyName, reader, converters);
				case "ORDERSLOTS":
					reader.Read();
					OrderSlots = global::System.Convert.ToInt32(reader.Value);
					break;

				case "UNLOCKLEVEL":
					reader.Read();
					UnlockLevel = global::System.Convert.ToInt32(reader.Value);
					break;
			}
			return true;
		} 
	}
}
