namespace Kampai.Common.Controller
{
	public class ClientTimerMetricsDTO : global::Kampai.Util.IFastJSONSerializable
	{
		public global::System.Collections.Generic.Dictionary<string, float> timerEvents { get; set; }

		public virtual void Serialize(global::Newtonsoft.Json.JsonWriter writer)
		{
			writer.WriteStartObject();
			SerializeProperties(writer);
			writer.WriteEndObject();
		}

		protected virtual void SerializeProperties(global::Newtonsoft.Json.JsonWriter writer)
		{
			if (timerEvents == null)
			{
				return;
			}
			writer.WritePropertyName("timerEvents");
			writer.WriteStartObject();
			global::System.Collections.Generic.Dictionary<string, float>.Enumerator enumerator = timerEvents.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					global::System.Collections.Generic.KeyValuePair<string, float> current = enumerator.Current;
					writer.WritePropertyName(global::System.Convert.ToString(current.Key));
					writer.WriteValue(current.Value);
				}
			}
			finally
			{
				enumerator.Dispose();
			}
			writer.WriteEndObject();
		}
	}
}
