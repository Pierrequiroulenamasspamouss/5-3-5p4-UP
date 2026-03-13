namespace Kampai.Game
{
	public class FillOrderRequest : global::Kampai.Util.IFastJSONSerializable
	{
		[global::Newtonsoft.Json.JsonProperty("orderId")]
		public int OrderID { get; set; }

		public virtual void Serialize(global::Newtonsoft.Json.JsonWriter writer)
		{
			writer.WriteStartObject();
			SerializeProperties(writer);
			writer.WriteEndObject();
		}

		protected virtual void SerializeProperties(global::Newtonsoft.Json.JsonWriter writer)
		{
			writer.WritePropertyName("orderId");
			writer.WriteValue(OrderID);
		}
	}
}
