namespace Prime31
{
	public static class DeserializationExtensions
	{
		public static global::System.Collections.Generic.List<T> toList<T>(this global::System.Collections.IList self)
		{
			global::System.Collections.Generic.List<T> list = new global::System.Collections.Generic.List<T>();
			foreach (global::System.Collections.Generic.Dictionary<string, object> item in self)
			{
				list.Add(item.toClass<T>());
			}
			return list;
		}

		public static T toClass<T>(this global::System.Collections.IDictionary self)
		{
			object obj = global::System.Activator.CreateInstance(typeof(T));
			global::System.Reflection.FieldInfo[] fields = typeof(T).GetFields(global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Public | global::System.Reflection.BindingFlags.NonPublic);
			foreach (global::System.Reflection.FieldInfo fieldInfo in fields)
			{
				object[] customAttributes = fieldInfo.GetCustomAttributes(typeof(global::Prime31.P31DeserializeableFieldAttribute), true);
				foreach (object obj2 in customAttributes)
				{
					global::Prime31.P31DeserializeableFieldAttribute p31DeserializeableFieldAttribute = obj2 as global::Prime31.P31DeserializeableFieldAttribute;
					if (!self.Contains(p31DeserializeableFieldAttribute.key))
					{
						continue;
					}
					object obj3 = self[p31DeserializeableFieldAttribute.key];
					if (obj3 is global::System.Collections.IDictionary)
					{
						global::System.Reflection.MethodInfo methodInfo = typeof(global::Prime31.DeserializationExtensions).GetMethod("toClass").MakeGenericMethod(p31DeserializeableFieldAttribute.type);
						object value = methodInfo.Invoke(null, new object[1] { obj3 });
						fieldInfo.SetValue(obj, value);
						self.Remove(p31DeserializeableFieldAttribute.key);
					}
					else if (obj3 is global::System.Collections.IList)
					{
						if (!p31DeserializeableFieldAttribute.isCollection)
						{
							global::UnityEngine.Debug.LogError("found an IList but the field is not a collection: " + p31DeserializeableFieldAttribute.key);
							continue;
						}
						global::System.Reflection.MethodInfo methodInfo2 = typeof(global::Prime31.DeserializationExtensions).GetMethod("toList").MakeGenericMethod(p31DeserializeableFieldAttribute.type);
						object value2 = methodInfo2.Invoke(null, new object[1] { obj3 });
						fieldInfo.SetValue(obj, value2);
						self.Remove(p31DeserializeableFieldAttribute.key);
					}
					else
					{
						fieldInfo.SetValue(obj, global::System.Convert.ChangeType(obj3, fieldInfo.FieldType));
						self.Remove(p31DeserializeableFieldAttribute.key);
					}
				}
			}
			return (T)obj;
		}

		public static global::System.Collections.Generic.Dictionary<string, object> toDictionary(this object self)
		{
			global::System.Collections.Generic.Dictionary<string, object> dictionary = new global::System.Collections.Generic.Dictionary<string, object>();
			global::System.Reflection.FieldInfo[] fields = self.GetType().GetFields(global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Public | global::System.Reflection.BindingFlags.NonPublic);
			foreach (global::System.Reflection.FieldInfo fieldInfo in fields)
			{
				object[] customAttributes = fieldInfo.GetCustomAttributes(typeof(global::Prime31.P31DeserializeableFieldAttribute), true);
				foreach (object obj in customAttributes)
				{
					global::Prime31.P31DeserializeableFieldAttribute p31DeserializeableFieldAttribute = obj as global::Prime31.P31DeserializeableFieldAttribute;
					if (p31DeserializeableFieldAttribute.isCollection)
					{
						global::System.Collections.IEnumerable enumerable = fieldInfo.GetValue(self) as global::System.Collections.IEnumerable;
						global::System.Collections.ArrayList arrayList = new global::System.Collections.ArrayList();
						foreach (object item in enumerable)
						{
							arrayList.Add(item.toDictionary());
						}
						dictionary[p31DeserializeableFieldAttribute.key] = arrayList;
					}
					else if (p31DeserializeableFieldAttribute.type != null)
					{
						dictionary[p31DeserializeableFieldAttribute.key] = fieldInfo.GetValue(self).toDictionary();
					}
					else
					{
						dictionary[p31DeserializeableFieldAttribute.key] = fieldInfo.GetValue(self);
					}
				}
			}
			return dictionary;
		}

		[global::System.Obsolete("Use the toDictionary method to get a proper generic Dictionary returned. Hashtables are obsolute.")]
		public static global::System.Collections.Hashtable toHashtable(this object self)
		{
			global::System.Collections.Hashtable hashtable = new global::System.Collections.Hashtable();
			global::System.Reflection.FieldInfo[] fields = self.GetType().GetFields(global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Public | global::System.Reflection.BindingFlags.NonPublic);
			foreach (global::System.Reflection.FieldInfo fieldInfo in fields)
			{
				object[] customAttributes = fieldInfo.GetCustomAttributes(typeof(global::Prime31.P31DeserializeableFieldAttribute), true);
				foreach (object obj in customAttributes)
				{
					global::Prime31.P31DeserializeableFieldAttribute p31DeserializeableFieldAttribute = obj as global::Prime31.P31DeserializeableFieldAttribute;
					if (p31DeserializeableFieldAttribute.isCollection)
					{
						global::System.Collections.IEnumerable enumerable = fieldInfo.GetValue(self) as global::System.Collections.IEnumerable;
						global::System.Collections.ArrayList arrayList = new global::System.Collections.ArrayList();
						foreach (object item in enumerable)
						{
							arrayList.Add(item.toHashtable());
						}
						hashtable[p31DeserializeableFieldAttribute.key] = arrayList;
					}
					else if (p31DeserializeableFieldAttribute.type != null)
					{
						hashtable[p31DeserializeableFieldAttribute.key] = fieldInfo.GetValue(self).toHashtable();
					}
					else
					{
						hashtable[p31DeserializeableFieldAttribute.key] = fieldInfo.GetValue(self);
					}
				}
			}
			return hashtable;
		}
	}
}
