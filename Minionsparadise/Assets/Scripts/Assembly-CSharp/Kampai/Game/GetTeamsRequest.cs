namespace Kampai.Game
{
	public class GetTeamsRequest : global::Kampai.Util.IFastJSONSerializable
	{
		[global::Newtonsoft.Json.JsonProperty("teamIds")]
		public global::System.Collections.Generic.IList<long> TeamIDs { get; set; }

		public virtual void Serialize(global::Newtonsoft.Json.JsonWriter writer)
		{
			writer.WriteStartObject();
			SerializeProperties(writer);
			writer.WriteEndObject();
		}

		protected virtual void SerializeProperties(global::Newtonsoft.Json.JsonWriter writer)
		{
			if (TeamIDs == null)
			{
				return;
			}
			writer.WritePropertyName("teamIds");
			writer.WriteStartArray();
			global::System.Collections.Generic.IEnumerator<long> enumerator = TeamIDs.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					long current = enumerator.Current;
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
