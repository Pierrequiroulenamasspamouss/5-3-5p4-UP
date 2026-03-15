namespace Kampai.Game
{
	public class AccountLinkRequest : global::Kampai.Util.IFastJSONSerializable
	{
		public string identityType { get; set; }

		public string externalId { get; set; }

		public string credentials { get; set; }

		public virtual void Serialize(global::Newtonsoft.Json.JsonWriter writer)
		{
			writer.WriteStartObject();
			SerializeProperties(writer);
			writer.WriteEndObject();
		}

		protected virtual void SerializeProperties(global::Newtonsoft.Json.JsonWriter writer)
		{
			if (identityType != null)
			{
				writer.WritePropertyName("identityType");
				writer.WriteValue(identityType);
			}
			if (externalId != null)
			{
				writer.WritePropertyName("externalId");
				writer.WriteValue(externalId);
			}
			if (credentials != null)
			{
				writer.WritePropertyName("credentials");
				writer.WriteValue(credentials);
			}
		}
	}
}
