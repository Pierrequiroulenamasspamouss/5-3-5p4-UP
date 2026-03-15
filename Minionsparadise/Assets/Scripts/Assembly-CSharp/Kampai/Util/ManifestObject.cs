namespace Kampai.Util
{
	public class ManifestObject : global::Kampai.Util.IFastJSONDeserializable
	{
		public string id { get; set; }

		public string baseURL { get; set; }

		public global::System.Collections.Generic.List<global::Kampai.Util.BundleInfo> bundles { get; set; }

		[global::Kampai.Util.Deserializer("ReaderUtil.ReadStringDictionary")]
		public global::System.Collections.Generic.Dictionary<string, string> assets { get; set; }

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
			case "ID":
				reader.Read();
				id = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "BASEURL":
				reader.Read();
				baseURL = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "BUNDLES":
				reader.Read();
				bundles = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, bundles);
				break;
			case "ASSETS":
				reader.Read();
				assets = global::Kampai.Util.ReaderUtil.ReadStringDictionary(reader, converters);
				break;
			default:
				return false;
			}
			return true;
		}
	}
}
