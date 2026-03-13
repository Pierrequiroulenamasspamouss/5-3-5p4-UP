namespace Kampai.Main
{
	public class LocalStringConverter : global::Newtonsoft.Json.JsonConverter
	{
		private const string SINGLE_KEY = "Single";

		private const string MULTIPLE_KEY = "Multiple";

		private const string UNDEFINED = "UNDEFINED";

		public override bool CanConvert(global::System.Type objectType)
		{
			return objectType == typeof(global::Kampai.Main.ILocalString);
		}

		public override object ReadJson(global::Newtonsoft.Json.JsonReader reader, global::System.Type objectType, object existingValue, global::Newtonsoft.Json.JsonSerializer serializer)
		{
			global::Newtonsoft.Json.Linq.JToken jToken = global::Newtonsoft.Json.Linq.JToken.Load(reader);
			if (jToken.Type == global::Newtonsoft.Json.Linq.JTokenType.String)
			{
				return new global::Kampai.Main.LocalString(global::Newtonsoft.Json.Linq.LinqExtensions.Value<string>(jToken));
			}
			if (jToken.Type == global::Newtonsoft.Json.Linq.JTokenType.Object)
			{
				string single = jToken.Value<string>("Single");
				string multiple = jToken.Value<string>("Multiple");
				return new global::Kampai.Main.LocalQuantityString(single, multiple);
			}
			return "UNDEFINED";
		}

		public override void WriteJson(global::Newtonsoft.Json.JsonWriter writer, object value, global::Newtonsoft.Json.JsonSerializer serializer)
		{
		}
	}
}
