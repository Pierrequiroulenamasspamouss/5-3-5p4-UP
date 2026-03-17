namespace Prime31
{
	public class PocoJsonSerializerStrategy : global::Prime31.IJsonSerializerStrategy
	{
		internal global::Prime31.Reflection.CacheResolver cacheResolver;

		private static readonly string[] Iso8601Format = new string[3] { "yyyy-MM-dd\\THH:mm:ss.FFFFFFF\\Z", "yyyy-MM-dd\\THH:mm:ss\\Z", "yyyy-MM-dd\\THH:mm:ssK" };

		public PocoJsonSerializerStrategy()
		{
			cacheResolver = new global::Prime31.Reflection.CacheResolver(buildMap);
		}

		protected virtual void buildMap(global::System.Type type, global::Prime31.Reflection.SafeDictionary<string, global::Prime31.Reflection.CacheResolver.MemberMap> memberMaps)
		{
			global::System.Reflection.PropertyInfo[] properties = type.GetProperties(global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Public | global::System.Reflection.BindingFlags.NonPublic);
			foreach (global::System.Reflection.PropertyInfo propertyInfo in properties)
			{
				memberMaps.add(propertyInfo.Name, new global::Prime31.Reflection.CacheResolver.MemberMap(propertyInfo));
			}
			global::System.Reflection.FieldInfo[] fields = type.GetFields(global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Public | global::System.Reflection.BindingFlags.NonPublic);
			foreach (global::System.Reflection.FieldInfo fieldInfo in fields)
			{
				memberMaps.add(fieldInfo.Name, new global::Prime31.Reflection.CacheResolver.MemberMap(fieldInfo));
			}
		}

		public virtual bool serializeNonPrimitiveObject(object input, out object output)
		{
			return trySerializeKnownTypes(input, out output) || trySerializeUnknownTypes(input, out output);
		}

		public virtual object deserializeObject(object value, global::System.Type type)
		{
			object obj = null;
			if (value is string)
			{
				string text = value as string;
				obj = ((string.IsNullOrEmpty(text) || (type != typeof(global::System.DateTime) && (!global::Prime31.Reflection.ReflectionUtils.isNullableType(type) || global::System.Nullable.GetUnderlyingType(type) != typeof(global::System.DateTime)))) ? text : ((object)global::System.DateTime.ParseExact(text, Iso8601Format, global::System.Globalization.CultureInfo.InvariantCulture, global::System.Globalization.DateTimeStyles.AdjustToUniversal | global::System.Globalization.DateTimeStyles.AssumeUniversal)));
			}
			else if (value is bool)
			{
				obj = value;
			}
			else if (value == null)
			{
				obj = null;
			}
			else if ((value is long && type == typeof(long)) || (value is double && type == typeof(double)))
			{
				obj = value;
			}
			else
			{
				if ((!(value is double) || type == typeof(double)) && (!(value is long) || type == typeof(long)))
				{
					if (value is global::System.Collections.Generic.IDictionary<string, object>)
					{
						global::System.Collections.Generic.IDictionary<string, object> dictionary = (global::System.Collections.Generic.IDictionary<string, object>)value;
						if (global::Prime31.Reflection.ReflectionUtils.isTypeDictionary(type))
						{
							global::System.Type type2 = type.GetGenericArguments()[0];
							global::System.Type type3 = type.GetGenericArguments()[1];
							global::System.Type type4 = typeof(global::System.Collections.Generic.Dictionary<, >).MakeGenericType(type2, type3);
							global::System.Collections.IDictionary dictionary2 = (global::System.Collections.IDictionary)global::Prime31.Reflection.CacheResolver.getNewInstance(type4);
							foreach (global::System.Collections.Generic.KeyValuePair<string, object> item in dictionary)
							{
								dictionary2.Add(item.Key, deserializeObject(item.Value, type3));
							}
							obj = dictionary2;
						}
						else
						{
							obj = global::Prime31.Reflection.CacheResolver.getNewInstance(type);
							global::Prime31.Reflection.SafeDictionary<string, global::Prime31.Reflection.CacheResolver.MemberMap> safeDictionary = cacheResolver.loadMaps(type);
							if (safeDictionary == null)
							{
								obj = value;
							}
							else
							{
								foreach (global::System.Collections.Generic.KeyValuePair<string, global::Prime31.Reflection.CacheResolver.MemberMap> item2 in safeDictionary)
								{
									global::Prime31.Reflection.CacheResolver.MemberMap value2 = item2.Value;
									if (value2.Setter != null)
									{
										string key = item2.Key;
										if (dictionary.ContainsKey(key))
										{
											object value3 = deserializeObject(dictionary[key], value2.Type);
											value2.Setter(obj, value3);
										}
									}
								}
							}
						}
					}
					else if (value is global::System.Collections.Generic.IList<object>)
					{
						global::System.Collections.Generic.IList<object> list = (global::System.Collections.Generic.IList<object>)value;
						global::System.Collections.IList list2 = null;
						if (type.IsArray)
						{
							list2 = (global::System.Collections.IList)global::System.Activator.CreateInstance(type, list.Count);
							int num = 0;
							foreach (object item3 in list)
							{
								list2[num++] = deserializeObject(item3, type.GetElementType());
							}
						}
						else if (global::Prime31.Reflection.ReflectionUtils.isTypeGenericeCollectionInterface(type) || typeof(global::System.Collections.IList).IsAssignableFrom(type))
						{
							global::System.Type type5 = type.GetGenericArguments()[0];
							global::System.Type type6 = typeof(global::System.Collections.Generic.List<>).MakeGenericType(type5);
							list2 = (global::System.Collections.IList)global::Prime31.Reflection.CacheResolver.getNewInstance(type6);
							foreach (object item4 in list)
							{
								list2.Add(deserializeObject(item4, type5));
							}
						}
						obj = list2;
					}
					return obj;
				}
				obj = ((value is long && type == typeof(global::System.DateTime)) ? ((object)new global::System.DateTime(1970, 1, 1, 0, 0, 0, global::System.DateTimeKind.Utc).AddMilliseconds((long)value)) : ((!type.IsEnum) ? ((!typeof(global::System.IConvertible).IsAssignableFrom(type)) ? value : global::System.Convert.ChangeType(value, type, global::System.Globalization.CultureInfo.InvariantCulture)) : global::System.Enum.ToObject(type, value)));
			}
			if (global::Prime31.Reflection.ReflectionUtils.isNullableType(type))
			{
				return global::Prime31.Reflection.ReflectionUtils.toNullableType(obj, type);
			}
			return obj;
		}

		protected virtual object serializeEnum(global::System.Enum p)
		{
			return global::System.Convert.ToDouble(p, global::System.Globalization.CultureInfo.InvariantCulture);
		}

		protected virtual bool trySerializeKnownTypes(object input, out object output)
		{
			bool result = true;
			if (input is global::System.DateTime)
			{
				output = ((global::System.DateTime)input).ToUniversalTime().ToString(Iso8601Format[0], global::System.Globalization.CultureInfo.InvariantCulture);
			}
			else if (input is global::System.Guid)
			{
				output = ((global::System.Guid)input).ToString("D");
			}
			else if (input is global::System.Uri)
			{
				output = input.ToString();
			}
			else if (input is global::System.Enum)
			{
				output = serializeEnum((global::System.Enum)input);
			}
			else
			{
				result = false;
				output = null;
			}
			return result;
		}

		protected virtual bool trySerializeUnknownTypes(object input, out object output)
		{
			output = null;
			global::System.Type type = input.GetType();
			if (type.FullName == null)
			{
				return false;
			}
			global::System.Collections.Generic.IDictionary<string, object> dictionary = new global::Prime31.JsonObject();
			global::Prime31.Reflection.SafeDictionary<string, global::Prime31.Reflection.CacheResolver.MemberMap> safeDictionary = cacheResolver.loadMaps(type);
			foreach (global::System.Collections.Generic.KeyValuePair<string, global::Prime31.Reflection.CacheResolver.MemberMap> item in safeDictionary)
			{
				if (item.Value.Getter != null)
				{
					dictionary.Add(item.Key, item.Value.Getter(input));
				}
			}
			output = dictionary;
			return true;
		}
	}
}
