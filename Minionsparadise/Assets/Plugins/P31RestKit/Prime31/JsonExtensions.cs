namespace Prime31
{
	public static class JsonExtensions
	{
		public static string toJson(this global::System.Collections.IList obj)
		{
			return global::Prime31.Json.encode(obj);
		}

		public static string toJson(this global::System.Collections.IDictionary obj)
		{
			return global::Prime31.Json.encode(obj);
		}

		public static global::System.Collections.Generic.List<object> listFromJson(this string json)
		{
			return global::Prime31.Json.decode(json) as global::System.Collections.Generic.List<object>;
		}

		public static global::System.Collections.Generic.Dictionary<string, object> dictionaryFromJson(this string json)
		{
			return global::Prime31.Json.decode(json) as global::System.Collections.Generic.Dictionary<string, object>;
		}
	}
}
