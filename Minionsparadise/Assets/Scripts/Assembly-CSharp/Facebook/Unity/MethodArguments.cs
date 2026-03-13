namespace Facebook.Unity
{
	internal class MethodArguments
	{
		private global::System.Collections.Generic.IDictionary<string, object> arguments = new global::System.Collections.Generic.Dictionary<string, object>();

		public MethodArguments()
			: this(new global::System.Collections.Generic.Dictionary<string, object>())
		{
		}

		public MethodArguments(global::Facebook.Unity.MethodArguments methodArgs)
			: this(methodArgs.arguments)
		{
		}

		private MethodArguments(global::System.Collections.Generic.IDictionary<string, object> arguments)
		{
			this.arguments = arguments;
		}

		public void AddPrimative<T>(string argumentName, T value) where T : struct
		{
			arguments[argumentName] = value;
		}

		public void AddNullablePrimitive<T>(string argumentName, T? nullable) where T : struct
		{
			if (nullable.HasValue && nullable.HasValue)
			{
				arguments[argumentName] = nullable.Value;
			}
		}

		public void AddString(string argumentName, string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				arguments[argumentName] = value;
			}
		}

		public void AddCommaSeparatedList(string argumentName, global::System.Collections.Generic.IEnumerable<string> value)
		{
			if (value != null)
			{
				arguments[argumentName] = value.ToCommaSeparateList();
			}
		}

		public void AddDictionary(string argumentName, global::System.Collections.Generic.IDictionary<string, object> dict)
		{
			if (dict != null)
			{
				arguments[argumentName] = ToStringDict(dict);
			}
		}

		public void AddList<T>(string argumentName, global::System.Collections.Generic.IEnumerable<T> list)
		{
			if (list != null)
			{
				arguments[argumentName] = list;
			}
		}

		public void AddUri(string argumentName, global::System.Uri uri)
		{
			if (uri != null && !string.IsNullOrEmpty(uri.AbsoluteUri))
			{
				arguments[argumentName] = uri.ToString();
			}
		}

		public string ToJsonString()
		{
			return global::Facebook.MiniJSON.Json.Serialize(arguments);
		}

		private static global::System.Collections.Generic.Dictionary<string, string> ToStringDict(global::System.Collections.Generic.IDictionary<string, object> dict)
		{
			if (dict == null)
			{
				return null;
			}
			global::System.Collections.Generic.Dictionary<string, string> dictionary = new global::System.Collections.Generic.Dictionary<string, string>();
			foreach (global::System.Collections.Generic.KeyValuePair<string, object> item in dict)
			{
				dictionary[item.Key] = item.Value.ToString();
			}
			return dictionary;
		}
	}
}
