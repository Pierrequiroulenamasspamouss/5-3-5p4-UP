namespace Kampai.Game
{
	public class UserLoginRequest : global::Kampai.Util.IFastJSONSerializable
	{
		[global::Newtonsoft.Json.JsonProperty("identityId")]
		public string IdentityID { get; set; }

		[global::Newtonsoft.Json.JsonProperty("userId")]
		public string UserID { get; set; }

		[global::Newtonsoft.Json.JsonProperty("anonymousSecret")]
		public string AnonymousSecret { get; set; }

		public virtual void Serialize(global::Newtonsoft.Json.JsonWriter writer)
		{
			writer.WriteStartObject();
			SerializeProperties(writer);
			writer.WriteEndObject();
		}

		protected virtual void SerializeProperties(global::Newtonsoft.Json.JsonWriter writer)
		{
			if (IdentityID != null)
			{
				writer.WritePropertyName("identityId");
				writer.WriteValue(IdentityID);
			}
			if (UserID != null)
			{
				writer.WritePropertyName("userId");
				writer.WriteValue(UserID);
			}
			if (AnonymousSecret != null)
			{
				writer.WritePropertyName("anonymousSecret");
				writer.WriteValue(AnonymousSecret);
			}
		}
	}
}
