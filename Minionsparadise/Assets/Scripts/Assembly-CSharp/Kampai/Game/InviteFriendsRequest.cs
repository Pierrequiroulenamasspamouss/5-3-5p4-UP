namespace Kampai.Game
{
	public class InviteFriendsRequest : global::Kampai.Util.IFastJSONSerializable
	{
		[global::Newtonsoft.Json.JsonProperty("identityType")]
		public global::Kampai.Game.IdentityType IdentityType { get; set; }

		[global::Newtonsoft.Json.JsonProperty("externalIds")]
		public global::System.Collections.Generic.IList<string> ExternalIds { get; set; }

		public virtual void Serialize(global::Newtonsoft.Json.JsonWriter writer)
		{
			writer.WriteStartObject();
			SerializeProperties(writer);
			writer.WriteEndObject();
		}

		protected virtual void SerializeProperties(global::Newtonsoft.Json.JsonWriter writer)
		{
			writer.WritePropertyName("identityType");
			writer.WriteValue((int)IdentityType);
			if (ExternalIds == null)
			{
				return;
			}
			writer.WritePropertyName("externalIds");
			writer.WriteStartArray();
			global::System.Collections.Generic.IEnumerator<string> enumerator = ExternalIds.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.Current;
					writer.WriteValue(current);
				}
			}
			finally
			{
				enumerator.Dispose();
			}
			writer.WriteEndArray();
		}
	}
}
