namespace Kampai.Game.Transaction
{
	public class WeightedDefinition : global::Kampai.Game.Definition, global::Kampai.Util.IBuilder<global::Kampai.Game.Instance>
	{
		public override int TypeCode
		{
			get
			{
				return 1022;
			}
		}

		public global::System.Collections.Generic.IList<global::Kampai.Game.Transaction.WeightedQuantityItem> Entities { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, Entities);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Entities = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, Entities);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "ENTITIES":
				reader.Read();
				Entities = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, Entities);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}

		public global::Kampai.Game.Instance Build()
		{
			return new global::Kampai.Game.Transaction.WeightedInstance(this);
		}
	}
}
