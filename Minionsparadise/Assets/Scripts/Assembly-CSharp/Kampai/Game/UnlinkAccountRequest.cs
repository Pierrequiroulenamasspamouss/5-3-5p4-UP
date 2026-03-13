namespace Kampai.Game
{
	public class UnlinkAccountRequest : global::Kampai.Util.IFastJSONSerializable
	{
		public string identityType { get; set; }

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
		}
	}
}
