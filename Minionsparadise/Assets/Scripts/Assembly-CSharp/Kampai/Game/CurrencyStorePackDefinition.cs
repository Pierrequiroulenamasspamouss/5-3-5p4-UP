namespace Kampai.Game
{
	[global::Kampai.Util.RequiresJsonConverter]
	public class CurrencyStorePackDefinition : global::Kampai.Game.PackDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1144;
			}
		}

		public int StoreUnlockFTUELevel { get; set; }

		public string SaleBanner { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(StoreUnlockFTUELevel);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, SaleBanner);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			StoreUnlockFTUELevel = reader.ReadInt32();
			SaleBanner = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "SALEBANNER":
				reader.Read();
				SaleBanner = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			case "STOREUNLOCKFTUELEVEL":
				reader.Read();
				StoreUnlockFTUELevel = global::System.Convert.ToInt32(reader.Value);
				break;
			}
			return true;
		}

		public override void Serialize(global::Newtonsoft.Json.JsonWriter writer)
		{
			writer.WriteStartObject();
			SerializeProperties(writer);
			writer.WriteEndObject();
		}

		protected override void SerializeProperties(global::Newtonsoft.Json.JsonWriter writer)
		{
			base.SerializeProperties(writer);
			if (base.LocalizedKey != null)
			{
				writer.WritePropertyName("LocalizedKey");
				writer.WriteValue(base.LocalizedKey);
			}
			writer.WritePropertyName("ID");
			writer.WriteValue(ID);
			writer.WritePropertyName("Disabled");
			writer.WriteValue(base.Disabled);
			if (Image != null)
			{
				writer.WritePropertyName("Image");
				writer.WriteValue(Image);
			}
			if (Mask != null)
			{
				writer.WritePropertyName("Mask");
				writer.WriteValue(Mask);
			}
			if (Description != null)
			{
				writer.WritePropertyName("Description");
				writer.WriteValue(Description);
			}
			if (base.TaxonomyHighLevel != null)
			{
				writer.WritePropertyName("TaxonomyHighLevel");
				writer.WriteValue(base.TaxonomyHighLevel);
			}
			if (base.TaxonomySpecific != null)
			{
				writer.WritePropertyName("TaxonomySpecific");
				writer.WriteValue(base.TaxonomySpecific);
			}
			if (base.TaxonomyType != null)
			{
				writer.WritePropertyName("TaxonomyType");
				writer.WriteValue(base.TaxonomyType);
			}
			if (base.TaxonomyOther != null)
			{
				writer.WritePropertyName("TaxonomyOther");
				writer.WriteValue(base.TaxonomyOther);
			}
			if (base.VFX != null)
			{
				writer.WritePropertyName("VFX");
				writer.WriteValue(base.VFX);
			}
			if (base.VFXOffset != null)
			{
				writer.WritePropertyName("VFXOffset");
				writer.WriteStartObject();
				writer.WritePropertyName("x");
				writer.WriteValue(base.VFXOffset.x);
				writer.WritePropertyName("y");
				writer.WriteValue(base.VFXOffset.y);
				writer.WritePropertyName("z");
				writer.WriteValue(base.VFXOffset.z);
				writer.WriteEndObject();
			}
			if (base.Audio != null)
			{
				writer.WritePropertyName("Audio");
				writer.WriteValue(base.Audio);
			}
			writer.WritePropertyName("COPPAGated");
			writer.WriteValue(base.COPPAGated);
			if (base.PlatformStoreSku != null)
			{
				writer.WritePropertyName("PlatformStoreSku");
				writer.WriteStartArray();
				global::System.Collections.Generic.IEnumerator<global::Kampai.Game.PlatformStoreSkuDefinition> enumerator = base.PlatformStoreSku.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						global::Kampai.Game.PlatformStoreSkuDefinition current = enumerator.Current;
						writer.WriteStartObject();
						if (current.appleAppstore != null)
						{
							writer.WritePropertyName("appleAppstore");
							writer.WriteValue(current.appleAppstore);
						}
						if (current.googlePlay != null)
						{
							writer.WritePropertyName("googlePlay");
							writer.WriteValue(current.googlePlay);
						}
						if (current.defaultStore != null)
						{
							writer.WritePropertyName("defaultStore");
							writer.WriteValue(current.defaultStore);
						}
						writer.WriteEndObject();
					}
				}
				finally
				{
					enumerator.Dispose();
				}
				writer.WriteEndArray();
			}
			writer.WritePropertyName("ActiveSKUIndex");
			writer.WriteValue(base.ActiveSKUIndex);
			writer.WritePropertyName("StoreUnlockFTUELevel");
			writer.WriteValue(StoreUnlockFTUELevel);
			if (SaleBanner != null)
			{
				writer.WritePropertyName("SaleBanner");
				writer.WriteValue(SaleBanner);
			}
		}
	}
}
