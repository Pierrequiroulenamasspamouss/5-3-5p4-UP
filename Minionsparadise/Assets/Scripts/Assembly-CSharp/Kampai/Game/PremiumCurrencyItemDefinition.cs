namespace Kampai.Game
{
	public class PremiumCurrencyItemDefinition : global::Kampai.Game.CurrencyItemDefinition, global::Kampai.Game.MTXItem
	{
		public override int TypeCode
		{
			get
			{
				return 1138;
			}
		}

		public global::System.Collections.Generic.IList<global::Kampai.Game.PlatformStoreSkuDefinition> PlatformStoreSku { get; set; }

		public int ActiveSKUIndex { get; set; }

		public string SKU
		{
			get
			{
				if (PlatformStoreSku == null || PlatformStoreSku.Count <= ActiveSKUIndex)
				{
					return string.Empty;
				}
				string sku = PlatformStoreSku[ActiveSKUIndex].defaultStore;
				if (string.IsNullOrEmpty(sku))
				{
					sku = PlatformStoreSku[ActiveSKUIndex].googlePlay;
				}
				return sku ?? string.Empty;
			}
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WritePlatformStoreSkuDefinition, PlatformStoreSku);
			writer.Write(ActiveSKUIndex);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			PlatformStoreSku = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadPlatformStoreSkuDefinition, PlatformStoreSku);
			ActiveSKUIndex = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "ACTIVESKUINDEX":
				reader.Read();
				ActiveSKUIndex = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			case "PLATFORMSTORESKU":
				reader.Read();
				PlatformStoreSku = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadPlatformStoreSkuDefinition, PlatformStoreSku);
				break;
			}
			return true;
		}
	}
}
