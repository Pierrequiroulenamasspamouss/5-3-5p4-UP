namespace Kampai.Game
{
	public class UserUpdateRequest : global::Kampai.Util.IFastJSONSerializable
	{
		[global::Newtonsoft.Json.JsonProperty("synergyId")]
		public string SynergyID { get; set; }

		public virtual void Serialize(global::Newtonsoft.Json.JsonWriter writer)
		{
			writer.WriteStartObject();
			SerializeProperties(writer);
			writer.WriteEndObject();
		}

		protected virtual void SerializeProperties(global::Newtonsoft.Json.JsonWriter writer)
		{
			if (SynergyID != null)
			{
				writer.WritePropertyName("synergyId");
				writer.WriteValue(SynergyID);
			}
		}
	}
}
