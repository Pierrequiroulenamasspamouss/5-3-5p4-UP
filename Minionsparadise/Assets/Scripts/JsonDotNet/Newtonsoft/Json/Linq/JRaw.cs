namespace Newtonsoft.Json.Linq
{
	public class JRaw : global::Newtonsoft.Json.Linq.JValue
	{
		public JRaw(global::Newtonsoft.Json.Linq.JRaw other)
			: base(other)
		{
		}

		public JRaw(object rawJson)
			: base(rawJson, global::Newtonsoft.Json.Linq.JTokenType.Raw)
		{
		}

		public static global::Newtonsoft.Json.Linq.JRaw Create(global::Newtonsoft.Json.JsonReader reader)
		{
			using (global::System.IO.StringWriter stringWriter = new global::System.IO.StringWriter(global::System.Globalization.CultureInfo.InvariantCulture))
			{
				using (global::Newtonsoft.Json.JsonTextWriter jsonTextWriter = new global::Newtonsoft.Json.JsonTextWriter(stringWriter))
				{
					jsonTextWriter.WriteToken(reader);
					return new global::Newtonsoft.Json.Linq.JRaw(stringWriter.ToString());
				}
			}
		}

		internal override global::Newtonsoft.Json.Linq.JToken CloneToken()
		{
			return new global::Newtonsoft.Json.Linq.JRaw(this);
		}
	}
}
