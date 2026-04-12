namespace Newtonsoft.Json
{
	public abstract class JsonConverter
	{
		public virtual bool CanRead
		{
			get
			{
				return true;
			}
		}

		public virtual bool CanWrite
		{
			get
			{
				return true;
			}
		}

		public abstract void WriteJson(global::Newtonsoft.Json.JsonWriter writer, object value, global::Newtonsoft.Json.JsonSerializer serializer);

		public abstract object ReadJson(global::Newtonsoft.Json.JsonReader reader, global::System.Type objectType, object existingValue, global::Newtonsoft.Json.JsonSerializer serializer);

		public abstract bool CanConvert(global::System.Type objectType);

		public virtual global::Newtonsoft.Json.Schema.JsonSchema GetSchema()
		{
			return null;
		}
	}
}
