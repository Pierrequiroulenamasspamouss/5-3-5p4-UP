namespace Kampai.Game
{
	[global::Kampai.Util.RequiresJsonConverter]
	public class SalePackDefinition : global::Kampai.Game.PackDefinition, global::Kampai.Util.IBuilder<global::Kampai.Game.Instance>, global::Kampai.Util.IUTCRangeable
	{
		public override int TypeCode
		{
			get
			{
				return 1136;
			}
		}

		public global::Kampai.Game.SalePackType Type { get; set; }

		public int UTCStartDate { get; set; }

		public int UTCEndDate { get; set; }

		public int Impressions { get; set; }

		public int ImpressionInterval { get; set; }

		public int Duration { get; set; }

		public global::Kampai.Game.SalePackMessageType MessageType { get; set; }

		public global::Kampai.Game.SalePackMessageLinkType MessageLinkType { get; set; }

		public string MessageImage { get; set; }

		public string MessageMask { get; set; }

		public string GlassIconImage { get; set; }

		public string GlassIconMask { get; set; }

		public string MessageUrl { get; set; }

		public string ServerSaleId { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteEnum(writer, Type);
			writer.Write(UTCStartDate);
			writer.Write(UTCEndDate);
			writer.Write(Impressions);
			writer.Write(ImpressionInterval);
			writer.Write(Duration);
			global::Kampai.Util.BinarySerializationUtil.WriteEnum(writer, MessageType);
			global::Kampai.Util.BinarySerializationUtil.WriteEnum(writer, MessageLinkType);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, MessageImage);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, MessageMask);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, GlassIconImage);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, GlassIconMask);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, MessageUrl);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, ServerSaleId);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Type = global::Kampai.Util.BinarySerializationUtil.ReadEnum<global::Kampai.Game.SalePackType>(reader);
			UTCStartDate = reader.ReadInt32();
			UTCEndDate = reader.ReadInt32();
			Impressions = reader.ReadInt32();
			ImpressionInterval = reader.ReadInt32();
			Duration = reader.ReadInt32();
			MessageType = global::Kampai.Util.BinarySerializationUtil.ReadEnum<global::Kampai.Game.SalePackMessageType>(reader);
			MessageLinkType = global::Kampai.Util.BinarySerializationUtil.ReadEnum<global::Kampai.Game.SalePackMessageLinkType>(reader);
			MessageImage = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			MessageMask = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			GlassIconImage = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			GlassIconMask = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			MessageUrl = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			ServerSaleId = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "TYPE":
				reader.Read();
				Type = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Game.SalePackType>(reader);
				break;
			case "UTCSTARTDATE":
				reader.Read();
				UTCStartDate = global::System.Convert.ToInt32(reader.Value);
				break;
			case "UTCENDDATE":
				reader.Read();
				UTCEndDate = global::System.Convert.ToInt32(reader.Value);
				break;
			case "IMPRESSIONS":
				reader.Read();
				Impressions = global::System.Convert.ToInt32(reader.Value);
				break;
			case "IMPRESSIONINTERVAL":
				reader.Read();
				ImpressionInterval = global::System.Convert.ToInt32(reader.Value);
				break;
			case "DURATION":
				reader.Read();
				Duration = global::System.Convert.ToInt32(reader.Value);
				break;
			case "MESSAGETYPE":
				reader.Read();
				MessageType = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Game.SalePackMessageType>(reader);
				break;
			case "MESSAGELINKTYPE":
				reader.Read();
				MessageLinkType = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Game.SalePackMessageLinkType>(reader);
				break;
			case "MESSAGEIMAGE":
				reader.Read();
				MessageImage = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "MESSAGEMASK":
				reader.Read();
				MessageMask = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "GLASSICONIMAGE":
				reader.Read();
				GlassIconImage = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "GLASSICONMASK":
				reader.Read();
				GlassIconMask = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "MESSAGEURL":
				reader.Read();
				MessageUrl = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "SERVERSALEID":
				reader.Read();
				ServerSaleId = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
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
			writer.WritePropertyName("Type");
			writer.WriteValue((int)Type);
			writer.WritePropertyName("UTCStartDate");
			writer.WriteValue(UTCStartDate);
			writer.WritePropertyName("UTCEndDate");
			writer.WriteValue(UTCEndDate);
			writer.WritePropertyName("Impressions");
			writer.WriteValue(Impressions);
			writer.WritePropertyName("ImpressionInterval");
			writer.WriteValue(ImpressionInterval);
			writer.WritePropertyName("Duration");
			writer.WriteValue(Duration);
			writer.WritePropertyName("MessageType");
			writer.WriteValue((int)MessageType);
			writer.WritePropertyName("MessageLinkType");
			writer.WriteValue((int)MessageLinkType);
			if (MessageImage != null)
			{
				writer.WritePropertyName("MessageImage");
				writer.WriteValue(MessageImage);
			}
			if (MessageMask != null)
			{
				writer.WritePropertyName("MessageMask");
				writer.WriteValue(MessageMask);
			}
			if (GlassIconImage != null)
			{
				writer.WritePropertyName("GlassIconImage");
				writer.WriteValue(GlassIconImage);
			}
			if (GlassIconMask != null)
			{
				writer.WritePropertyName("GlassIconMask");
				writer.WriteValue(GlassIconMask);
			}
			if (MessageUrl != null)
			{
				writer.WritePropertyName("MessageUrl");
				writer.WriteValue(MessageUrl);
			}
			if (ServerSaleId != null)
			{
				writer.WritePropertyName("ServerSaleId");
				writer.WriteValue(ServerSaleId);
			}
		}

		public global::Kampai.Game.Instance Build()
		{
			return new global::Kampai.Game.Sale(this);
		}
	}
}
