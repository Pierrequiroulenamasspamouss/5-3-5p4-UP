namespace Prime31
{
	public class DTOBase
	{
		public static global::System.Collections.Generic.List<T> listFromJson<T>(string json) where T : global::Prime31.DTOBase
		{
			global::System.Collections.Generic.List<object> list = json.listFromJson();
			global::System.Collections.Generic.List<T> list2 = new global::System.Collections.Generic.List<T>();
			foreach (object item2 in list)
			{
				T item = global::System.Activator.CreateInstance<T>();
				item.setDataFromDictionary(item2 as global::System.Collections.Generic.Dictionary<string, object>);
				list2.Add(item);
			}
			return list2;
		}

		public void setDataFromJson(string json)
		{
			setDataFromDictionary(json.dictionaryFromJson());
		}

		public void setDataFromDictionary(global::System.Collections.Generic.Dictionary<string, object> dict)
		{
			global::System.Collections.Generic.Dictionary<string, global::System.Action<object>> membersWithSetters = getMembersWithSetters();
			foreach (global::System.Collections.Generic.KeyValuePair<string, object> item in dict)
			{
				if (membersWithSetters.ContainsKey(item.Key))
				{
					try
					{
						membersWithSetters[item.Key](item.Value);
					}
					catch (global::System.Exception obj)
					{
						global::Prime31.Utils.logObject(obj);
					}
				}
			}
		}

		private bool shouldIncludeTypeWithSetters(global::System.Type type)
		{
			if (type.IsGenericType)
			{
				return false;
			}
			if (type.Namespace.StartsWith("System"))
			{
				return true;
			}
			return false;
		}

		protected global::System.Collections.Generic.Dictionary<string, global::System.Action<object>> getMembersWithSetters()
		{
			global::System.Collections.Generic.Dictionary<string, global::System.Action<object>> dictionary = new global::System.Collections.Generic.Dictionary<string, global::System.Action<object>>();
			global::System.Reflection.FieldInfo[] fields = GetType().GetFields();
			foreach (global::System.Reflection.FieldInfo fieldInfo in fields)
			{
				if (shouldIncludeTypeWithSetters(fieldInfo.FieldType))
				{
					global::System.Reflection.FieldInfo theInfo = fieldInfo;
					dictionary[fieldInfo.Name] = delegate(object val)
					{
						theInfo.SetValue(this, val);
					};
				}
			}
			global::System.Reflection.PropertyInfo[] properties = GetType().GetProperties();
			foreach (global::System.Reflection.PropertyInfo propertyInfo in properties)
			{
				if (shouldIncludeTypeWithSetters(propertyInfo.PropertyType) && propertyInfo.CanWrite && propertyInfo.GetSetMethod() != null)
				{
					global::System.Reflection.PropertyInfo theInfo2 = propertyInfo;
					dictionary[propertyInfo.Name] = delegate(object val)
					{
						theInfo2.SetValue(this, val, null);
					};
				}
			}
			return dictionary;
		}

		public override string ToString()
		{
			global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
			stringBuilder.AppendFormat("[{0}]:", GetType());
			global::System.Reflection.FieldInfo[] fields = GetType().GetFields();
			foreach (global::System.Reflection.FieldInfo fieldInfo in fields)
			{
				stringBuilder.AppendFormat(", {0}: {1}", fieldInfo.Name, fieldInfo.GetValue(this));
			}
			global::System.Reflection.PropertyInfo[] properties = GetType().GetProperties();
			foreach (global::System.Reflection.PropertyInfo propertyInfo in properties)
			{
				stringBuilder.AppendFormat(", {0}: {1}", propertyInfo.Name, propertyInfo.GetValue(this, null));
			}
			return stringBuilder.ToString();
		}
	}
}
