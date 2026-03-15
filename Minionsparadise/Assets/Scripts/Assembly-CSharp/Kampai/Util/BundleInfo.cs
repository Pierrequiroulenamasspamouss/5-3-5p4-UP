namespace Kampai.Util
{
	public class BundleInfo : global::Kampai.Util.IFastJSONDeserializable
	{
		public string name { get; set; }

		public string originalName { get; set; }

		public int tier { get; set; }

		public string sum { get; set; }

		public ulong size { get; set; }

		public bool shared { get; set; }

		public bool shaders { get; set; }

		public bool audio { get; set; }

		public bool isZipped { get; set; }

		public ulong zipsize { get; set; }

		public string zipsum { get; set; }

		public bool isPackaged { get; set; }

		public bool isStreamable
		{
			get
			{
				return isPackaged && !isZipped;
			}
		}

		public virtual object Deserialize(global::Newtonsoft.Json.JsonReader reader, JsonConverters converters = null)
		{
			if (reader.TokenType == global::Newtonsoft.Json.JsonToken.None)
			{
				reader.Read();
			}
			global::Kampai.Util.ReaderUtil.EnsureToken(global::Newtonsoft.Json.JsonToken.StartObject, reader);
			while (reader.Read())
			{
				switch (reader.TokenType)
				{
				case global::Newtonsoft.Json.JsonToken.PropertyName:
				{
					string propertyName = ((string)reader.Value).ToUpper();
					if (!DeserializeProperty(propertyName, reader, converters))
					{
						reader.Skip();
					}
					break;
				}
				case global::Newtonsoft.Json.JsonToken.EndObject:
					return this;
				default:
					throw new global::Newtonsoft.Json.JsonSerializationException(string.Format("Unexpected token when deserializing object: {0}. {1}", reader.TokenType, global::Kampai.Util.ReaderUtil.GetPositionInSource(reader)));
				case global::Newtonsoft.Json.JsonToken.Comment:
					break;
				}
			}
			throw new global::Newtonsoft.Json.JsonSerializationException("Unexpected end when deserializing object.");
		}

		protected virtual bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "NAME":
				reader.Read();
				name = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "ORIGINALNAME":
				reader.Read();
				originalName = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "TIER":
				reader.Read();
				tier = global::System.Convert.ToInt32(reader.Value);
				break;
			case "SUM":
				reader.Read();
				sum = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "SIZE":
				reader.Read();
				size = global::System.Convert.ToUInt64(reader.Value);
				break;
			case "SHARED":
				reader.Read();
				shared = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "SHADERS":
				reader.Read();
				shaders = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "AUDIO":
				reader.Read();
				audio = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "ISZIPPED":
				reader.Read();
				isZipped = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "ZIPSIZE":
				reader.Read();
				zipsize = global::System.Convert.ToUInt64(reader.Value);
				break;
			case "ZIPSUM":
				reader.Read();
				zipsum = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "ISPACKAGED":
				reader.Read();
				isPackaged = global::System.Convert.ToBoolean(reader.Value);
				break;
			default:
				return false;
			}
			return true;
		}
	}
}
