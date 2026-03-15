namespace Kampai.Util.MiniJSON
{
	public class Serializer
	{
		private global::System.Text.StringBuilder builder;

		private global::System.Collections.Generic.Dictionary<global::System.Type, object> converters;

		public Serializer()
		{
			builder = new global::System.Text.StringBuilder();
		}

		public void AddConverter(global::System.Type type, global::Kampai.Util.MiniJSON.MiniJSONSerializeConverter converter)
		{
			if (converters == null)
			{
				converters = new global::System.Collections.Generic.Dictionary<global::System.Type, object>();
			}
			converters.Add(type, converter);
		}

		public static string Serialize(object obj)
		{
			global::Kampai.Util.MiniJSON.Serializer serializer = new global::Kampai.Util.MiniJSON.Serializer();
			return serializer.SerializeValue(obj).Build();
		}

		public string Build()
		{
			return builder.ToString();
		}

		public global::Kampai.Util.MiniJSON.Serializer SerializeValue(object value)
		{
			if (value != null && converters != null)
			{
				global::System.Type type = value.GetType();
				foreach (global::System.Type key in converters.Keys)
				{
					if (key.IsAssignableFrom(type))
					{
						value = ((global::Kampai.Util.MiniJSON.MiniJSONSerializeConverter)converters[key]).Convert(value);
					}
				}
			}
			string str;
			global::System.Collections.IList anArray;
			global::System.Collections.IDictionary obj;
			if (value == null)
			{
				builder.Append("null");
			}
			else if ((str = value as string) != null)
			{
				SerializeString(str);
			}
			else if (value is bool)
			{
				builder.Append((!(bool)value) ? "false" : "true");
			}
			else if ((anArray = value as global::System.Collections.IList) != null)
			{
				SerializeArray(anArray);
			}
			else if ((obj = value as global::System.Collections.IDictionary) != null)
			{
				SerializeObject(obj);
			}
			else if (value is char)
			{
				SerializeString(new string((char)value, 1));
			}
			else
			{
				SerializeOther(value);
			}
			return this;
		}

		public void SerializeObject(global::System.Collections.IDictionary obj)
		{
			bool flag = true;
			builder.Append('{');
			foreach (object key in obj.Keys)
			{
				if (!flag)
				{
					builder.Append(',');
				}
				SerializeString(key.ToString());
				builder.Append(':');
				SerializeValue(obj[key]);
				flag = false;
			}
			builder.Append('}');
		}

		public void SerializeArray(global::System.Collections.IList anArray)
		{
			builder.Append('[');
			bool flag = true;
			foreach (object item in anArray)
			{
				if (!flag)
				{
					builder.Append(',');
				}
				SerializeValue(item);
				flag = false;
			}
			builder.Append(']');
		}

		public void SerializeString(string str)
		{
			builder.Append('"');
			char[] array = str.ToCharArray();
			char[] array2 = array;
			foreach (char c in array2)
			{
				switch (c)
				{
				case '"':
					builder.Append("\\\"");
					continue;
				case '\\':
					builder.Append("\\\\");
					continue;
				case '\b':
					builder.Append("\\b");
					continue;
				case '\f':
					builder.Append("\\f");
					continue;
				case '\n':
					builder.Append("\\n");
					continue;
				case '\r':
					builder.Append("\\r");
					continue;
				case '\t':
					builder.Append("\\t");
					continue;
				}
				int num = global::System.Convert.ToInt32(c);
				if (num >= 32 && num <= 126)
				{
					builder.Append(c);
					continue;
				}
				builder.Append("\\u");
				builder.Append(num.ToString("x4"));
			}
			builder.Append('"');
		}

		public void SerializeOther(object value)
		{
			if (value is float)
			{
				builder.Append(((float)value).ToString("R"));
			}
			else if (value is int || value is uint || value is long || value is sbyte || value is byte || value is short || value is ushort || value is ulong)
			{
				builder.Append(value);
			}
			else if (value is double || value is decimal)
			{
				builder.Append(global::System.Convert.ToDouble(value).ToString("R"));
			}
			else if (value is global::System.Enum)
			{
				SerializeValue((int)value);
			}
			else
			{
				ReflectObject(value);
			}
		}

		public void ReflectObject(object obj)
		{
			global::System.Collections.Generic.Dictionary<string, object> dictionary = new global::System.Collections.Generic.Dictionary<string, object>();
			global::System.Type type = obj.GetType();
			global::System.Reflection.FieldInfo[] fields = type.GetFields();
			global::System.Reflection.FieldInfo[] array = fields;
			foreach (global::System.Reflection.FieldInfo fieldInfo in array)
			{
				if (fieldInfo.GetCustomAttributes(typeof(global::Newtonsoft.Json.JsonIgnoreAttribute), false).Length <= 0 && fieldInfo.IsPublic)
				{
					string name = fieldInfo.Name;
					object value = fieldInfo.GetValue(obj);
					dictionary.Add(name, value);
				}
			}
			global::System.Reflection.PropertyInfo[] properties = type.GetProperties(global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Public);
			global::System.Reflection.PropertyInfo[] array2 = properties;
			foreach (global::System.Reflection.PropertyInfo propertyInfo in array2)
			{
				if (propertyInfo.GetCustomAttributes(typeof(global::Newtonsoft.Json.JsonIgnoreAttribute), false).Length <= 0 && propertyInfo.CanRead && propertyInfo.GetGetMethod() != null)
				{
					string name = propertyInfo.Name;
					object value = propertyInfo.GetValue(obj, null);
					dictionary.Add(name, value);
				}
			}
			SerializeObject(dictionary);
		}
	}
}
