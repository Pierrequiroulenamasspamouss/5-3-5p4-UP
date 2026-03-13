namespace Kampai.Game.Transaction
{
	public class WeightedQuantityItem : global::Kampai.Util.QuantityItem
	{
		public override int TypeCode
		{
			get
			{
				return 1023;
			}
		}

		public uint Weight { get; set; }

		public WeightedQuantityItem()
		{
			base.Quantity = 1u;
		}

		public WeightedQuantityItem(int id, uint quantity, uint weight)
			: base(id, quantity)
		{
			Weight = weight;
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(Weight);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Weight = reader.ReadUInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "WEIGHT":
				reader.Read();
				Weight = global::System.Convert.ToUInt32(reader.Value);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}
	}
}
