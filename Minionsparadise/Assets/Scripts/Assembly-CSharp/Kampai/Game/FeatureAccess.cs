namespace Kampai.Game
{
	public class FeatureAccess : global::Kampai.Util.IFastJSONDeserializable
	{
		[global::Newtonsoft.Json.JsonProperty(NullValueHandling = global::Newtonsoft.Json.NullValueHandling.Ignore)]
		public int accessPercentage;

		[global::Newtonsoft.Json.JsonProperty(NullValueHandling = global::Newtonsoft.Json.NullValueHandling.Ignore)]
		public global::System.Collections.Generic.IList<string> userIdWhitelist;

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
			case "USERIDWHITELIST":
				reader.Read();
				userIdWhitelist = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadString, userIdWhitelist);
				break;
			case "ACCESSPERCENTAGE":
				reader.Read();
				accessPercentage = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				// The instruction's default case was syntactically incorrect and logically flawed.
				// Since FeatureAccess implements an interface and not a base class with DeserializeProperty,
				// calling base.DeserializeProperty is not possible.
				// The original code returned false for unhandled properties in its default.
				// Assuming the intent is to return false for unhandled properties,
				// or if this class is expected to be a base class for others,
				// then the default should indicate that the property was not handled here.
				return false;
			}
			return true;
		}
	}
}
