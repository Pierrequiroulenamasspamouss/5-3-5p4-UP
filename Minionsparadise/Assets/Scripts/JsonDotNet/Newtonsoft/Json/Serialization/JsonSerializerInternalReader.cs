namespace Newtonsoft.Json.Serialization
{
	internal class JsonSerializerInternalReader : global::Newtonsoft.Json.Serialization.JsonSerializerInternalBase
	{
		internal enum PropertyPresence
		{
			None = 0,
			Null = 1,
			Value = 2
		}

		private global::Newtonsoft.Json.Serialization.JsonSerializerProxy _internalSerializer;

		private global::Newtonsoft.Json.Serialization.JsonFormatterConverter _formatterConverter;

		public JsonSerializerInternalReader(global::Newtonsoft.Json.JsonSerializer serializer)
			: base(serializer)
		{
		}

		public void Populate(global::Newtonsoft.Json.JsonReader reader, object target)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(target, "target");
			global::System.Type type = target.GetType();
			global::Newtonsoft.Json.Serialization.JsonContract jsonContract = base.Serializer.ContractResolver.ResolveContract(type);
			if (reader.TokenType == global::Newtonsoft.Json.JsonToken.None)
			{
				reader.Read();
			}
			if (reader.TokenType == global::Newtonsoft.Json.JsonToken.StartArray)
			{
				if (jsonContract is global::Newtonsoft.Json.Serialization.JsonArrayContract)
				{
					PopulateList(global::Newtonsoft.Json.Utilities.CollectionUtils.CreateCollectionWrapper(target), reader, null, (global::Newtonsoft.Json.Serialization.JsonArrayContract)jsonContract);
					return;
				}
				throw new global::Newtonsoft.Json.JsonSerializationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Cannot populate JSON array onto type '{0}'.", global::System.Globalization.CultureInfo.InvariantCulture, type));
			}
			if (reader.TokenType == global::Newtonsoft.Json.JsonToken.StartObject)
			{
				CheckedRead(reader);
				string id = null;
				if (reader.TokenType == global::Newtonsoft.Json.JsonToken.PropertyName && string.Equals(reader.Value.ToString(), "$id", global::System.StringComparison.Ordinal))
				{
					CheckedRead(reader);
					id = ((reader.Value != null) ? reader.Value.ToString() : null);
					CheckedRead(reader);
				}
				if (jsonContract is global::Newtonsoft.Json.Serialization.JsonDictionaryContract)
				{
					PopulateDictionary(global::Newtonsoft.Json.Utilities.CollectionUtils.CreateDictionaryWrapper(target), reader, (global::Newtonsoft.Json.Serialization.JsonDictionaryContract)jsonContract, id);
					return;
				}
				if (jsonContract is global::Newtonsoft.Json.Serialization.JsonObjectContract)
				{
					PopulateObject(target, reader, (global::Newtonsoft.Json.Serialization.JsonObjectContract)jsonContract, id);
					return;
				}
				throw new global::Newtonsoft.Json.JsonSerializationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Cannot populate JSON object onto type '{0}'.", global::System.Globalization.CultureInfo.InvariantCulture, type));
			}
			throw new global::Newtonsoft.Json.JsonSerializationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Unexpected initial token '{0}' when populating object. Expected JSON object or array.", global::System.Globalization.CultureInfo.InvariantCulture, reader.TokenType));
		}

		private global::Newtonsoft.Json.Serialization.JsonContract GetContractSafe(global::System.Type type)
		{
			if (type == null)
			{
				return null;
			}
			return base.Serializer.ContractResolver.ResolveContract(type);
		}

		private global::Newtonsoft.Json.Serialization.JsonContract GetContractSafe(global::System.Type type, object value)
		{
			if (value == null)
			{
				return GetContractSafe(type);
			}
			return base.Serializer.ContractResolver.ResolveContract(value.GetType());
		}

		public object Deserialize(global::Newtonsoft.Json.JsonReader reader, global::System.Type objectType)
		{
			if (reader == null)
			{
				throw new global::System.ArgumentNullException("reader");
			}
			if (reader.TokenType == global::Newtonsoft.Json.JsonToken.None && !ReadForType(reader, objectType, null))
			{
				return null;
			}
			return CreateValueNonProperty(reader, objectType, GetContractSafe(objectType));
		}

		private global::Newtonsoft.Json.Serialization.JsonSerializerProxy GetInternalSerializer()
		{
			if (_internalSerializer == null)
			{
				_internalSerializer = new global::Newtonsoft.Json.Serialization.JsonSerializerProxy(this);
			}
			return _internalSerializer;
		}

		private global::Newtonsoft.Json.Serialization.JsonFormatterConverter GetFormatterConverter()
		{
			if (_formatterConverter == null)
			{
				_formatterConverter = new global::Newtonsoft.Json.Serialization.JsonFormatterConverter(GetInternalSerializer());
			}
			return _formatterConverter;
		}

		private global::Newtonsoft.Json.Linq.JToken CreateJToken(global::Newtonsoft.Json.JsonReader reader, global::Newtonsoft.Json.Serialization.JsonContract contract)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(reader, "reader");
			if (contract != null && contract.UnderlyingType == typeof(global::Newtonsoft.Json.Linq.JRaw))
			{
				return global::Newtonsoft.Json.Linq.JRaw.Create(reader);
			}
			using (global::Newtonsoft.Json.Linq.JTokenWriter jTokenWriter = new global::Newtonsoft.Json.Linq.JTokenWriter())
			{
				jTokenWriter.WriteToken(reader);
				return jTokenWriter.Token;
			}
		}

		private global::Newtonsoft.Json.Linq.JToken CreateJObject(global::Newtonsoft.Json.JsonReader reader)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(reader, "reader");
			using (global::Newtonsoft.Json.Linq.JTokenWriter jTokenWriter = new global::Newtonsoft.Json.Linq.JTokenWriter())
			{
				jTokenWriter.WriteStartObject();
				if (reader.TokenType == global::Newtonsoft.Json.JsonToken.PropertyName)
				{
					jTokenWriter.WriteToken(reader, reader.Depth - 1);
				}
				else
				{
					jTokenWriter.WriteEndObject();
				}
				return jTokenWriter.Token;
			}
		}

		private object CreateValueProperty(global::Newtonsoft.Json.JsonReader reader, global::Newtonsoft.Json.Serialization.JsonProperty property, object target, bool gottenCurrentValue, object currentValue)
		{
			global::Newtonsoft.Json.Serialization.JsonContract contractSafe = GetContractSafe(property.PropertyType, currentValue);
			global::System.Type propertyType = property.PropertyType;
			global::Newtonsoft.Json.JsonConverter converter = GetConverter(contractSafe, property.MemberConverter);
			if (converter != null && converter.CanRead)
			{
				if (!gottenCurrentValue && target != null && property.Readable)
				{
					currentValue = property.ValueProvider.GetValue(target);
				}
				return converter.ReadJson(reader, propertyType, currentValue, GetInternalSerializer());
			}
			return CreateValueInternal(reader, propertyType, contractSafe, property, currentValue);
		}

		private object CreateValueNonProperty(global::Newtonsoft.Json.JsonReader reader, global::System.Type objectType, global::Newtonsoft.Json.Serialization.JsonContract contract)
		{
			global::Newtonsoft.Json.JsonConverter converter = GetConverter(contract, null);
			if (converter != null && converter.CanRead)
			{
				return converter.ReadJson(reader, objectType, null, GetInternalSerializer());
			}
			return CreateValueInternal(reader, objectType, contract, null, null);
		}

		private object CreateValueInternal(global::Newtonsoft.Json.JsonReader reader, global::System.Type objectType, global::Newtonsoft.Json.Serialization.JsonContract contract, global::Newtonsoft.Json.Serialization.JsonProperty member, object existingValue)
		{
			if (contract is global::Newtonsoft.Json.Serialization.JsonLinqContract)
			{
				return CreateJToken(reader, contract);
			}
			do
			{
				switch (reader.TokenType)
				{
				case global::Newtonsoft.Json.JsonToken.StartObject:
					return CreateObject(reader, objectType, contract, member, existingValue);
				case global::Newtonsoft.Json.JsonToken.StartArray:
					return CreateList(reader, objectType, contract, member, existingValue, null);
				case global::Newtonsoft.Json.JsonToken.Integer:
				case global::Newtonsoft.Json.JsonToken.Float:
				case global::Newtonsoft.Json.JsonToken.Boolean:
				case global::Newtonsoft.Json.JsonToken.Date:
				case global::Newtonsoft.Json.JsonToken.Bytes:
					return EnsureType(reader.Value, global::System.Globalization.CultureInfo.InvariantCulture, objectType);
				case global::Newtonsoft.Json.JsonToken.String:
					if (string.IsNullOrEmpty((string)reader.Value) && objectType != null && global::Newtonsoft.Json.Utilities.ReflectionUtils.IsNullableType(objectType))
					{
						return null;
					}
					if (objectType == typeof(byte[]))
					{
						return global::System.Convert.FromBase64String((string)reader.Value);
					}
					return EnsureType(reader.Value, global::System.Globalization.CultureInfo.InvariantCulture, objectType);
				case global::Newtonsoft.Json.JsonToken.StartConstructor:
				case global::Newtonsoft.Json.JsonToken.EndConstructor:
					return reader.Value.ToString();
				case global::Newtonsoft.Json.JsonToken.Null:
				case global::Newtonsoft.Json.JsonToken.Undefined:
					if (objectType == typeof(global::System.DBNull))
					{
						return global::System.DBNull.Value;
					}
					return EnsureType(reader.Value, global::System.Globalization.CultureInfo.InvariantCulture, objectType);
				case global::Newtonsoft.Json.JsonToken.Raw:
					return new global::Newtonsoft.Json.Linq.JRaw((string)reader.Value);
				default:
					throw new global::Newtonsoft.Json.JsonSerializationException("Unexpected token while deserializing object: " + reader.TokenType);
				case global::Newtonsoft.Json.JsonToken.Comment:
					break;
				}
			}
			while (reader.Read());
			throw new global::Newtonsoft.Json.JsonSerializationException("Unexpected end when deserializing object.");
		}

		private global::Newtonsoft.Json.JsonConverter GetConverter(global::Newtonsoft.Json.Serialization.JsonContract contract, global::Newtonsoft.Json.JsonConverter memberConverter)
		{
			global::Newtonsoft.Json.JsonConverter result = null;
			if (memberConverter != null)
			{
				result = memberConverter;
			}
			else if (contract != null)
			{
				global::Newtonsoft.Json.JsonConverter matchingConverter;
				if (contract.Converter != null)
				{
					result = contract.Converter;
				}
				else if ((matchingConverter = base.Serializer.GetMatchingConverter(contract.UnderlyingType)) != null)
				{
					result = matchingConverter;
				}
				else if (contract.InternalConverter != null)
				{
					result = contract.InternalConverter;
				}
			}
			return result;
		}

		private object CreateObject(global::Newtonsoft.Json.JsonReader reader, global::System.Type objectType, global::Newtonsoft.Json.Serialization.JsonContract contract, global::Newtonsoft.Json.Serialization.JsonProperty member, object existingValue)
		{
			CheckedRead(reader);
			string text = null;
			if (reader.TokenType == global::Newtonsoft.Json.JsonToken.PropertyName)
			{
				bool flag;
				do
				{
					string a = reader.Value.ToString();
					if (string.Equals(a, "$ref", global::System.StringComparison.Ordinal))
					{
						CheckedRead(reader);
						if (reader.TokenType != global::Newtonsoft.Json.JsonToken.String && reader.TokenType != global::Newtonsoft.Json.JsonToken.Null)
						{
							throw new global::Newtonsoft.Json.JsonSerializationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("JSON reference {0} property must have a string or null value.", global::System.Globalization.CultureInfo.InvariantCulture, "$ref"));
						}
						string text2 = ((reader.Value != null) ? reader.Value.ToString() : null);
						CheckedRead(reader);
						if (text2 != null)
						{
							if (reader.TokenType == global::Newtonsoft.Json.JsonToken.PropertyName)
							{
								throw new global::Newtonsoft.Json.JsonSerializationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Additional content found in JSON reference object. A JSON reference object should only have a {0} property.", global::System.Globalization.CultureInfo.InvariantCulture, "$ref"));
							}
							return base.Serializer.ReferenceResolver.ResolveReference(this, text2);
						}
						flag = true;
					}
					else if (string.Equals(a, "$type", global::System.StringComparison.Ordinal))
					{
						CheckedRead(reader);
						string text3 = reader.Value.ToString();
						CheckedRead(reader);
						if ((((member != null) ? member.TypeNameHandling : ((global::Newtonsoft.Json.TypeNameHandling?)null)) ?? base.Serializer.TypeNameHandling) != global::Newtonsoft.Json.TypeNameHandling.None)
						{
							string typeName;
							string assemblyName;
							global::Newtonsoft.Json.Utilities.ReflectionUtils.SplitFullyQualifiedTypeName(text3, out typeName, out assemblyName);
							global::System.Type type;
							try
							{
								type = base.Serializer.Binder.BindToType(assemblyName, typeName);
							}
							catch (global::System.Exception innerException)
							{
								throw new global::Newtonsoft.Json.JsonSerializationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Error resolving type specified in JSON '{0}'.", global::System.Globalization.CultureInfo.InvariantCulture, text3), innerException);
							}
							if (type == null)
							{
								throw new global::Newtonsoft.Json.JsonSerializationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Type specified in JSON '{0}' was not resolved.", global::System.Globalization.CultureInfo.InvariantCulture, text3));
							}
							if (objectType != null && !objectType.IsAssignableFrom(type))
							{
								throw new global::Newtonsoft.Json.JsonSerializationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Type specified in JSON '{0}' is not compatible with '{1}'.", global::System.Globalization.CultureInfo.InvariantCulture, type.AssemblyQualifiedName, objectType.AssemblyQualifiedName));
							}
							objectType = type;
							contract = GetContractSafe(type);
						}
						flag = true;
					}
					else if (string.Equals(a, "$id", global::System.StringComparison.Ordinal))
					{
						CheckedRead(reader);
						text = ((reader.Value != null) ? reader.Value.ToString() : null);
						CheckedRead(reader);
						flag = true;
					}
					else
					{
						if (string.Equals(a, "$values", global::System.StringComparison.Ordinal))
						{
							CheckedRead(reader);
							object result = CreateList(reader, objectType, contract, member, existingValue, text);
							CheckedRead(reader);
							return result;
						}
						flag = false;
					}
				}
				while (flag && reader.TokenType == global::Newtonsoft.Json.JsonToken.PropertyName);
			}
			if (!HasDefinedType(objectType))
			{
				return CreateJObject(reader);
			}
			if (contract == null)
			{
				throw new global::Newtonsoft.Json.JsonSerializationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Could not resolve type '{0}' to a JsonContract.", global::System.Globalization.CultureInfo.InvariantCulture, objectType));
			}
			global::Newtonsoft.Json.Serialization.JsonDictionaryContract jsonDictionaryContract = contract as global::Newtonsoft.Json.Serialization.JsonDictionaryContract;
			if (jsonDictionaryContract != null)
			{
				if (existingValue == null)
				{
					return CreateAndPopulateDictionary(reader, jsonDictionaryContract, text);
				}
				return PopulateDictionary(jsonDictionaryContract.CreateWrapper(existingValue), reader, jsonDictionaryContract, text);
			}
			global::Newtonsoft.Json.Serialization.JsonObjectContract jsonObjectContract = contract as global::Newtonsoft.Json.Serialization.JsonObjectContract;
			if (jsonObjectContract != null)
			{
				if (existingValue == null)
				{
					return CreateAndPopulateObject(reader, jsonObjectContract, text);
				}
				return PopulateObject(existingValue, reader, jsonObjectContract, text);
			}
			global::Newtonsoft.Json.Serialization.JsonPrimitiveContract jsonPrimitiveContract = contract as global::Newtonsoft.Json.Serialization.JsonPrimitiveContract;
			if (jsonPrimitiveContract != null && reader.TokenType == global::Newtonsoft.Json.JsonToken.PropertyName && string.Equals(reader.Value.ToString(), "$value", global::System.StringComparison.Ordinal))
			{
				CheckedRead(reader);
				object result2 = CreateValueInternal(reader, objectType, jsonPrimitiveContract, member, existingValue);
				CheckedRead(reader);
				return result2;
			}
			global::Newtonsoft.Json.Serialization.JsonISerializableContract jsonISerializableContract = contract as global::Newtonsoft.Json.Serialization.JsonISerializableContract;
			if (jsonISerializableContract != null)
			{
				return CreateISerializable(reader, jsonISerializableContract, text);
			}
			throw new global::Newtonsoft.Json.JsonSerializationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Cannot deserialize JSON object into type '{0}'.", global::System.Globalization.CultureInfo.InvariantCulture, objectType));
		}

		private global::Newtonsoft.Json.Serialization.JsonArrayContract EnsureArrayContract(global::System.Type objectType, global::Newtonsoft.Json.Serialization.JsonContract contract)
		{
			if (contract == null)
			{
				throw new global::Newtonsoft.Json.JsonSerializationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Could not resolve type '{0}' to a JsonContract.", global::System.Globalization.CultureInfo.InvariantCulture, objectType));
			}
			global::Newtonsoft.Json.Serialization.JsonArrayContract jsonArrayContract = contract as global::Newtonsoft.Json.Serialization.JsonArrayContract;
			if (jsonArrayContract == null)
			{
				throw new global::Newtonsoft.Json.JsonSerializationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Cannot deserialize JSON array into type '{0}'.", global::System.Globalization.CultureInfo.InvariantCulture, objectType));
			}
			return jsonArrayContract;
		}

		private void CheckedRead(global::Newtonsoft.Json.JsonReader reader)
		{
			if (!reader.Read())
			{
				throw new global::Newtonsoft.Json.JsonSerializationException("Unexpected end when deserializing object.");
			}
		}

		private object CreateList(global::Newtonsoft.Json.JsonReader reader, global::System.Type objectType, global::Newtonsoft.Json.Serialization.JsonContract contract, global::Newtonsoft.Json.Serialization.JsonProperty member, object existingValue, string reference)
		{
			if (HasDefinedType(objectType))
			{
				global::Newtonsoft.Json.Serialization.JsonArrayContract jsonArrayContract = EnsureArrayContract(objectType, contract);
				if (existingValue == null)
				{
					return CreateAndPopulateList(reader, reference, jsonArrayContract);
				}
				return PopulateList(jsonArrayContract.CreateWrapper(existingValue), reader, reference, jsonArrayContract);
			}
			return CreateJToken(reader, contract);
		}

		private bool HasDefinedType(global::System.Type type)
		{
			if (type != null && type != typeof(object))
			{
				return !typeof(global::Newtonsoft.Json.Linq.JToken).IsAssignableFrom(type);
			}
			return false;
		}

		private object EnsureType(object value, global::System.Globalization.CultureInfo culture, global::System.Type targetType)
		{
			if (targetType == null)
			{
				return value;
			}
			global::System.Type objectType = global::Newtonsoft.Json.Utilities.ReflectionUtils.GetObjectType(value);
			if (objectType != targetType)
			{
				try
				{
					return global::Newtonsoft.Json.Utilities.ConvertUtils.ConvertOrCast(value, culture, targetType);
				}
				catch (global::System.Exception innerException)
				{
					throw new global::Newtonsoft.Json.JsonSerializationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Error converting value {0} to type '{1}'.", global::System.Globalization.CultureInfo.InvariantCulture, FormatValueForPrint(value), targetType), innerException);
				}
			}
			return value;
		}

		private string FormatValueForPrint(object value)
		{
			if (value == null)
			{
				return "{null}";
			}
			if (value is string)
			{
				return string.Concat("\"", value, "\"");
			}
			return value.ToString();
		}

		private void SetPropertyValue(global::Newtonsoft.Json.Serialization.JsonProperty property, global::Newtonsoft.Json.JsonReader reader, object target)
		{
			if (property.Ignored)
			{
				reader.Skip();
				return;
			}
			object obj = null;
			bool flag = false;
			bool gottenCurrentValue = false;
			global::Newtonsoft.Json.ObjectCreationHandling valueOrDefault = property.ObjectCreationHandling.GetValueOrDefault(base.Serializer.ObjectCreationHandling);
			if ((valueOrDefault == global::Newtonsoft.Json.ObjectCreationHandling.Auto || valueOrDefault == global::Newtonsoft.Json.ObjectCreationHandling.Reuse) && (reader.TokenType == global::Newtonsoft.Json.JsonToken.StartArray || reader.TokenType == global::Newtonsoft.Json.JsonToken.StartObject) && property.Readable)
			{
				obj = property.ValueProvider.GetValue(target);
				gottenCurrentValue = true;
				flag = obj != null && !property.PropertyType.IsArray && !global::Newtonsoft.Json.Utilities.ReflectionUtils.InheritsGenericDefinition(property.PropertyType, typeof(global::System.Collections.ObjectModel.ReadOnlyCollection<>)) && !property.PropertyType.IsValueType;
			}
			if (!property.Writable && !flag)
			{
				reader.Skip();
				return;
			}
			if (property.NullValueHandling.GetValueOrDefault(base.Serializer.NullValueHandling) == global::Newtonsoft.Json.NullValueHandling.Ignore && reader.TokenType == global::Newtonsoft.Json.JsonToken.Null)
			{
				reader.Skip();
				return;
			}
			if (HasFlag(property.DefaultValueHandling.GetValueOrDefault(base.Serializer.DefaultValueHandling), global::Newtonsoft.Json.DefaultValueHandling.Ignore) && global::Newtonsoft.Json.JsonReader.IsPrimitiveToken(reader.TokenType) && global::Newtonsoft.Json.Utilities.MiscellaneousUtils.ValueEquals(reader.Value, property.DefaultValue))
			{
				reader.Skip();
				return;
			}
			object currentValue = (flag ? obj : null);
			object obj2 = CreateValueProperty(reader, property, target, gottenCurrentValue, currentValue);
			if ((!flag || obj2 != obj) && ShouldSetPropertyValue(property, obj2))
			{
				property.ValueProvider.SetValue(target, obj2);
				if (property.SetIsSpecified != null)
				{
					property.SetIsSpecified(target, true);
				}
			}
		}

		private bool HasFlag(global::Newtonsoft.Json.DefaultValueHandling value, global::Newtonsoft.Json.DefaultValueHandling flag)
		{
			return (value & flag) == flag;
		}

		private bool ShouldSetPropertyValue(global::Newtonsoft.Json.Serialization.JsonProperty property, object value)
		{
			if (property.NullValueHandling.GetValueOrDefault(base.Serializer.NullValueHandling) == global::Newtonsoft.Json.NullValueHandling.Ignore && value == null)
			{
				return false;
			}
			if (HasFlag(property.DefaultValueHandling.GetValueOrDefault(base.Serializer.DefaultValueHandling), global::Newtonsoft.Json.DefaultValueHandling.Ignore) && global::Newtonsoft.Json.Utilities.MiscellaneousUtils.ValueEquals(value, property.DefaultValue))
			{
				return false;
			}
			if (!property.Writable)
			{
				return false;
			}
			return true;
		}

		private object CreateAndPopulateDictionary(global::Newtonsoft.Json.JsonReader reader, global::Newtonsoft.Json.Serialization.JsonDictionaryContract contract, string id)
		{
			if (contract.DefaultCreator != null && (!contract.DefaultCreatorNonPublic || base.Serializer.ConstructorHandling == global::Newtonsoft.Json.ConstructorHandling.AllowNonPublicDefaultConstructor))
			{
				object dictionary = contract.DefaultCreator();
				global::Newtonsoft.Json.Utilities.IWrappedDictionary wrappedDictionary = contract.CreateWrapper(dictionary);
				PopulateDictionary(wrappedDictionary, reader, contract, id);
				return wrappedDictionary.UnderlyingDictionary;
			}
			throw new global::Newtonsoft.Json.JsonSerializationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Unable to find a default constructor to use for type {0}.", global::System.Globalization.CultureInfo.InvariantCulture, contract.UnderlyingType));
		}

		private object PopulateDictionary(global::Newtonsoft.Json.Utilities.IWrappedDictionary dictionary, global::Newtonsoft.Json.JsonReader reader, global::Newtonsoft.Json.Serialization.JsonDictionaryContract contract, string id)
		{
			if (id != null)
			{
				base.Serializer.ReferenceResolver.AddReference(this, id, dictionary.UnderlyingDictionary);
			}
			contract.InvokeOnDeserializing(dictionary.UnderlyingDictionary, base.Serializer.Context);
			int depth = reader.Depth;
			do
			{
				switch (reader.TokenType)
				{
				case global::Newtonsoft.Json.JsonToken.PropertyName:
				{
					object obj = reader.Value;
					try
					{
						try
						{
							obj = EnsureType(obj, global::System.Globalization.CultureInfo.InvariantCulture, contract.DictionaryKeyType);
						}
						catch (global::System.Exception innerException)
						{
							throw new global::Newtonsoft.Json.JsonSerializationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Could not convert string '{0}' to dictionary key type '{1}'. Create a TypeConverter to convert from the string to the key type object.", global::System.Globalization.CultureInfo.InvariantCulture, reader.Value, contract.DictionaryKeyType), innerException);
						}
						if (!ReadForType(reader, contract.DictionaryValueType, null))
						{
							throw new global::Newtonsoft.Json.JsonSerializationException("Unexpected end when deserializing object.");
						}
						dictionary[obj] = CreateValueNonProperty(reader, contract.DictionaryValueType, GetContractSafe(contract.DictionaryValueType));
					}
					catch (global::System.Exception ex)
					{
						if (IsErrorHandled(dictionary, contract, obj, ex))
						{
							HandleError(reader, depth);
							break;
						}
						throw;
					}
					break;
				}
				case global::Newtonsoft.Json.JsonToken.EndObject:
					contract.InvokeOnDeserialized(dictionary.UnderlyingDictionary, base.Serializer.Context);
					return dictionary.UnderlyingDictionary;
				default:
					throw new global::Newtonsoft.Json.JsonSerializationException("Unexpected token when deserializing object: " + reader.TokenType);
				case global::Newtonsoft.Json.JsonToken.Comment:
					break;
				}
			}
			while (reader.Read());
			throw new global::Newtonsoft.Json.JsonSerializationException("Unexpected end when deserializing object.");
		}

		private object CreateAndPopulateList(global::Newtonsoft.Json.JsonReader reader, string reference, global::Newtonsoft.Json.Serialization.JsonArrayContract contract)
		{
			return global::Newtonsoft.Json.Utilities.CollectionUtils.CreateAndPopulateList(contract.CreatedType, delegate(global::System.Collections.IList l, bool isTemporaryListReference)
			{
				if (reference != null && isTemporaryListReference)
				{
					throw new global::Newtonsoft.Json.JsonSerializationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Cannot preserve reference to array or readonly list: {0}", global::System.Globalization.CultureInfo.InvariantCulture, contract.UnderlyingType));
				}
				if (contract.OnSerializing != null && isTemporaryListReference)
				{
					throw new global::Newtonsoft.Json.JsonSerializationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Cannot call OnSerializing on an array or readonly list: {0}", global::System.Globalization.CultureInfo.InvariantCulture, contract.UnderlyingType));
				}
				if (contract.OnError != null && isTemporaryListReference)
				{
					throw new global::Newtonsoft.Json.JsonSerializationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Cannot call OnError on an array or readonly list: {0}", global::System.Globalization.CultureInfo.InvariantCulture, contract.UnderlyingType));
				}
				PopulateList(contract.CreateWrapper(l), reader, reference, contract);
			});
		}

		private bool ReadForTypeArrayHack(global::Newtonsoft.Json.JsonReader reader, global::System.Type t)
		{
			try
			{
				return ReadForType(reader, t, null);
			}
			catch (global::Newtonsoft.Json.JsonReaderException)
			{
				if (reader.TokenType == global::Newtonsoft.Json.JsonToken.EndArray)
				{
					return true;
				}
				throw;
			}
		}

		private object PopulateList(global::Newtonsoft.Json.Utilities.IWrappedCollection wrappedList, global::Newtonsoft.Json.JsonReader reader, string reference, global::Newtonsoft.Json.Serialization.JsonArrayContract contract)
		{
			object underlyingCollection = wrappedList.UnderlyingCollection;
			if (wrappedList.IsFixedSize)
			{
				reader.Skip();
				return wrappedList.UnderlyingCollection;
			}
			if (reference != null)
			{
				base.Serializer.ReferenceResolver.AddReference(this, reference, underlyingCollection);
			}
			contract.InvokeOnDeserializing(underlyingCollection, base.Serializer.Context);
			int depth = reader.Depth;
			while (ReadForTypeArrayHack(reader, contract.CollectionItemType))
			{
				switch (reader.TokenType)
				{
				case global::Newtonsoft.Json.JsonToken.EndArray:
					contract.InvokeOnDeserialized(underlyingCollection, base.Serializer.Context);
					return wrappedList.UnderlyingCollection;
				case global::Newtonsoft.Json.JsonToken.Comment:
					continue;
				}
				try
				{
					object value = CreateValueNonProperty(reader, contract.CollectionItemType, GetContractSafe(contract.CollectionItemType));
					wrappedList.Add(value);
				}
				catch (global::System.Exception ex)
				{
					if (IsErrorHandled(underlyingCollection, contract, wrappedList.Count, ex))
					{
						HandleError(reader, depth);
						continue;
					}
					throw;
				}
			}
			throw new global::Newtonsoft.Json.JsonSerializationException("Unexpected end when deserializing array.");
		}

		private object CreateISerializable(global::Newtonsoft.Json.JsonReader reader, global::Newtonsoft.Json.Serialization.JsonISerializableContract contract, string id)
		{
			global::System.Type underlyingType = contract.UnderlyingType;
			global::System.Runtime.Serialization.SerializationInfo serializationInfo = new global::System.Runtime.Serialization.SerializationInfo(contract.UnderlyingType, GetFormatterConverter());
			bool flag = false;
			do
			{
				switch (reader.TokenType)
				{
				case global::Newtonsoft.Json.JsonToken.PropertyName:
				{
					string text = reader.Value.ToString();
					if (!reader.Read())
					{
						throw new global::Newtonsoft.Json.JsonSerializationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Unexpected end when setting {0}'s value.", global::System.Globalization.CultureInfo.InvariantCulture, text));
					}
					serializationInfo.AddValue(text, global::Newtonsoft.Json.Linq.JToken.ReadFrom(reader));
					break;
				}
				case global::Newtonsoft.Json.JsonToken.EndObject:
					flag = true;
					break;
				default:
					throw new global::Newtonsoft.Json.JsonSerializationException("Unexpected token when deserializing object: " + reader.TokenType);
				case global::Newtonsoft.Json.JsonToken.Comment:
					break;
				}
			}
			while (!flag && reader.Read());
			if (contract.ISerializableCreator == null)
			{
				throw new global::Newtonsoft.Json.JsonSerializationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("ISerializable type '{0}' does not have a valid constructor. To correctly implement ISerializable a constructor that takes SerializationInfo and StreamingContext parameters should be present.", global::System.Globalization.CultureInfo.InvariantCulture, underlyingType));
			}
			object obj = contract.ISerializableCreator(serializationInfo, base.Serializer.Context);
			if (id != null)
			{
				base.Serializer.ReferenceResolver.AddReference(this, id, obj);
			}
			contract.InvokeOnDeserializing(obj, base.Serializer.Context);
			contract.InvokeOnDeserialized(obj, base.Serializer.Context);
			return obj;
		}

		private object CreateAndPopulateObject(global::Newtonsoft.Json.JsonReader reader, global::Newtonsoft.Json.Serialization.JsonObjectContract contract, string id)
		{
			object obj = null;
			if (contract.UnderlyingType.IsInterface || contract.UnderlyingType.IsAbstract)
			{
				throw new global::Newtonsoft.Json.JsonSerializationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Could not create an instance of type {0}. Type is an interface or abstract class and cannot be instantated.", global::System.Globalization.CultureInfo.InvariantCulture, contract.UnderlyingType));
			}
			if (contract.OverrideConstructor != null)
			{
				if (contract.OverrideConstructor.GetParameters().Length > 0)
				{
					return CreateObjectFromNonDefaultConstructor(reader, contract, contract.OverrideConstructor, id);
				}
				obj = contract.OverrideConstructor.Invoke(null);
			}
			else if (contract.DefaultCreator != null && (!contract.DefaultCreatorNonPublic || base.Serializer.ConstructorHandling == global::Newtonsoft.Json.ConstructorHandling.AllowNonPublicDefaultConstructor))
			{
				obj = contract.DefaultCreator();
			}
			else if (contract.ParametrizedConstructor != null)
			{
				return CreateObjectFromNonDefaultConstructor(reader, contract, contract.ParametrizedConstructor, id);
			}
			if (obj == null)
			{
				throw new global::Newtonsoft.Json.JsonSerializationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Unable to find a constructor to use for type {0}. A class should either have a default constructor, one constructor with arguments or a constructor marked with the JsonConstructor attribute.", global::System.Globalization.CultureInfo.InvariantCulture, contract.UnderlyingType));
			}
			PopulateObject(obj, reader, contract, id);
			return obj;
		}

		private object CreateObjectFromNonDefaultConstructor(global::Newtonsoft.Json.JsonReader reader, global::Newtonsoft.Json.Serialization.JsonObjectContract contract, global::System.Reflection.ConstructorInfo constructorInfo, string id)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(constructorInfo, "constructorInfo");
			global::System.Type underlyingType = contract.UnderlyingType;
			global::System.Collections.Generic.IDictionary<global::Newtonsoft.Json.Serialization.JsonProperty, object> enumerable = ResolvePropertyAndConstructorValues(contract, reader, underlyingType);
			global::System.Collections.Generic.IDictionary<global::System.Reflection.ParameterInfo, object> constructorParameters = global::System.Linq.Enumerable.ToDictionary(constructorInfo.GetParameters(), (global::System.Reflection.ParameterInfo p) => p, (global::System.Reflection.ParameterInfo p) => (object)null);
			global::System.Collections.Generic.IDictionary<global::Newtonsoft.Json.Serialization.JsonProperty, object> remainingPropertyValues = new global::System.Collections.Generic.Dictionary<global::Newtonsoft.Json.Serialization.JsonProperty, object>();
			global::Newtonsoft.Json.Aot.EnumerationExtension.ForEach(enumerable, delegate(global::System.Collections.Generic.KeyValuePair<global::Newtonsoft.Json.Serialization.JsonProperty, object> propertyValue)
			{
				global::System.Reflection.ParameterInfo key = global::Newtonsoft.Json.Utilities.StringUtils.ForgivingCaseSensitiveFind(constructorParameters, (global::System.Collections.Generic.KeyValuePair<global::System.Reflection.ParameterInfo, object> kv) => kv.Key.Name, propertyValue.Key.UnderlyingName).Key;
				if (key != null)
				{
					constructorParameters[key] = propertyValue.Value;
				}
				else
				{
					remainingPropertyValues.Add(propertyValue);
				}
			});
			object createdObject = constructorInfo.Invoke(global::System.Linq.Enumerable.ToArray(constructorParameters.Values));
			if (id != null)
			{
				base.Serializer.ReferenceResolver.AddReference(this, id, createdObject);
			}
			contract.InvokeOnDeserializing(createdObject, base.Serializer.Context);
			global::Newtonsoft.Json.Aot.EnumerationExtension.ForEach(remainingPropertyValues, delegate(global::System.Collections.Generic.KeyValuePair<global::Newtonsoft.Json.Serialization.JsonProperty, object> remainingPropertyValue)
			{
				global::Newtonsoft.Json.Serialization.JsonProperty key = remainingPropertyValue.Key;
				object value = remainingPropertyValue.Value;
				if (ShouldSetPropertyValue(remainingPropertyValue.Key, remainingPropertyValue.Value))
				{
					key.ValueProvider.SetValue(createdObject, value);
				}
				else if (!key.Writable && value != null)
				{
					global::Newtonsoft.Json.Serialization.JsonContract jsonContract = base.Serializer.ContractResolver.ResolveContract(key.PropertyType);
					if (jsonContract is global::Newtonsoft.Json.Serialization.JsonArrayContract)
					{
						global::Newtonsoft.Json.Serialization.JsonArrayContract jsonArrayContract = jsonContract as global::Newtonsoft.Json.Serialization.JsonArrayContract;
						object value2 = key.ValueProvider.GetValue(createdObject);
						if (value2 != null)
						{
							global::Newtonsoft.Json.Utilities.IWrappedCollection wrappedCollection = jsonArrayContract.CreateWrapper(value2);
							global::Newtonsoft.Json.Utilities.IWrappedCollection wrappedCollection2 = jsonArrayContract.CreateWrapper(value);
							foreach (object item in wrappedCollection2)
							{
								wrappedCollection.Add(item);
							}
						}
					}
					else if (jsonContract is global::Newtonsoft.Json.Serialization.JsonDictionaryContract)
					{
						global::Newtonsoft.Json.Serialization.JsonDictionaryContract jsonDictionaryContract = jsonContract as global::Newtonsoft.Json.Serialization.JsonDictionaryContract;
						object value3 = key.ValueProvider.GetValue(createdObject);
						if (value3 != null)
						{
							global::Newtonsoft.Json.Utilities.IWrappedDictionary wrappedDictionary = jsonDictionaryContract.CreateWrapper(value3);
							global::Newtonsoft.Json.Utilities.IWrappedDictionary wrappedDictionary2 = jsonDictionaryContract.CreateWrapper(value);
							foreach (global::System.Collections.DictionaryEntry item2 in wrappedDictionary2)
							{
								wrappedDictionary.Add(item2.Key, item2.Value);
							}
						}
					}
				}
			});
			contract.InvokeOnDeserialized(createdObject, base.Serializer.Context);
			return createdObject;
		}

		private global::System.Collections.Generic.IDictionary<global::Newtonsoft.Json.Serialization.JsonProperty, object> ResolvePropertyAndConstructorValues(global::Newtonsoft.Json.Serialization.JsonObjectContract contract, global::Newtonsoft.Json.JsonReader reader, global::System.Type objectType)
		{
			global::System.Collections.Generic.IDictionary<global::Newtonsoft.Json.Serialization.JsonProperty, object> dictionary = new global::System.Collections.Generic.Dictionary<global::Newtonsoft.Json.Serialization.JsonProperty, object>();
			bool flag = false;
			do
			{
				switch (reader.TokenType)
				{
				case global::Newtonsoft.Json.JsonToken.PropertyName:
				{
					string text = reader.Value.ToString();
					global::Newtonsoft.Json.Serialization.JsonProperty jsonProperty = contract.ConstructorParameters.GetClosestMatchProperty(text) ?? contract.Properties.GetClosestMatchProperty(text);
					if (jsonProperty != null)
					{
						if (!ReadForType(reader, jsonProperty.PropertyType, jsonProperty.Converter))
						{
							throw new global::Newtonsoft.Json.JsonSerializationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Unexpected end when setting {0}'s value.", global::System.Globalization.CultureInfo.InvariantCulture, text));
						}
						if (!jsonProperty.Ignored)
						{
							dictionary[jsonProperty] = CreateValueProperty(reader, jsonProperty, null, true, null);
						}
						else
						{
							reader.Skip();
						}
						break;
					}
					if (!reader.Read())
					{
						throw new global::Newtonsoft.Json.JsonSerializationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Unexpected end when setting {0}'s value.", global::System.Globalization.CultureInfo.InvariantCulture, text));
					}
					if (base.Serializer.MissingMemberHandling == global::Newtonsoft.Json.MissingMemberHandling.Error)
					{
						throw new global::Newtonsoft.Json.JsonSerializationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Could not find member '{0}' on object of type '{1}'", global::System.Globalization.CultureInfo.InvariantCulture, text, objectType.Name));
					}
					reader.Skip();
					break;
				}
				case global::Newtonsoft.Json.JsonToken.EndObject:
					flag = true;
					break;
				default:
					throw new global::Newtonsoft.Json.JsonSerializationException("Unexpected token when deserializing object: " + reader.TokenType);
				case global::Newtonsoft.Json.JsonToken.Comment:
					break;
				}
			}
			while (!flag && reader.Read());
			return dictionary;
		}

		private bool ReadForType(global::Newtonsoft.Json.JsonReader reader, global::System.Type t, global::Newtonsoft.Json.JsonConverter propertyConverter)
		{
			if (GetConverter(GetContractSafe(t), propertyConverter) != null)
			{
				return reader.Read();
			}
			if (t == typeof(byte[]))
			{
				reader.ReadAsBytes();
				return true;
			}
			if (t == typeof(decimal) || t == typeof(decimal?))
			{
				reader.ReadAsDecimal();
				return true;
			}
			if (t == typeof(global::System.DateTimeOffset) || t == typeof(global::System.DateTimeOffset?))
			{
				reader.ReadAsDateTimeOffset();
				return true;
			}
			do
			{
				if (!reader.Read())
				{
					return false;
				}
			}
			while (reader.TokenType == global::Newtonsoft.Json.JsonToken.Comment);
			return true;
		}

		private object PopulateObject(object newObject, global::Newtonsoft.Json.JsonReader reader, global::Newtonsoft.Json.Serialization.JsonObjectContract contract, string id)
		{
			contract.InvokeOnDeserializing(newObject, base.Serializer.Context);
			global::System.Collections.Generic.Dictionary<global::Newtonsoft.Json.Serialization.JsonProperty, global::Newtonsoft.Json.Serialization.JsonSerializerInternalReader.PropertyPresence> dictionary = global::System.Linq.Enumerable.ToDictionary(contract.Properties, (global::Newtonsoft.Json.Serialization.JsonProperty m) => m, (global::Newtonsoft.Json.Serialization.JsonProperty m) => global::Newtonsoft.Json.Serialization.JsonSerializerInternalReader.PropertyPresence.None);
			if (id != null)
			{
				base.Serializer.ReferenceResolver.AddReference(this, id, newObject);
			}
			int depth = reader.Depth;
			do
			{
				switch (reader.TokenType)
				{
				case global::Newtonsoft.Json.JsonToken.PropertyName:
				{
					string text = reader.Value.ToString();
					try
					{
						global::Newtonsoft.Json.Serialization.JsonProperty closestMatchProperty = contract.Properties.GetClosestMatchProperty(text);
						if (closestMatchProperty == null)
						{
							if (base.Serializer.MissingMemberHandling == global::Newtonsoft.Json.MissingMemberHandling.Error)
							{
								throw new global::Newtonsoft.Json.JsonSerializationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Could not find member '{0}' on object of type '{1}'", global::System.Globalization.CultureInfo.InvariantCulture, text, contract.UnderlyingType.Name));
							}
							reader.Skip();
						}
						else
						{
							if (!ReadForType(reader, closestMatchProperty.PropertyType, closestMatchProperty.Converter))
							{
								throw new global::Newtonsoft.Json.JsonSerializationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Unexpected end when setting {0}'s value.", global::System.Globalization.CultureInfo.InvariantCulture, text));
							}
							SetPropertyPresence(reader, closestMatchProperty, dictionary);
							SetPropertyValue(closestMatchProperty, reader, newObject);
						}
					}
					catch (global::System.Exception ex)
					{
						if (IsErrorHandled(newObject, contract, text, ex))
						{
							HandleError(reader, depth);
							break;
						}
						throw;
					}
					break;
				}
				case global::Newtonsoft.Json.JsonToken.EndObject:
					foreach (global::System.Collections.Generic.KeyValuePair<global::Newtonsoft.Json.Serialization.JsonProperty, global::Newtonsoft.Json.Serialization.JsonSerializerInternalReader.PropertyPresence> item in dictionary)
					{
						global::Newtonsoft.Json.Serialization.JsonProperty key = item.Key;
						switch (item.Value)
						{
						case global::Newtonsoft.Json.Serialization.JsonSerializerInternalReader.PropertyPresence.None:
							if (key.Required == global::Newtonsoft.Json.Required.AllowNull || key.Required == global::Newtonsoft.Json.Required.Always)
							{
								throw new global::Newtonsoft.Json.JsonSerializationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Required property '{0}' not found in JSON.", global::System.Globalization.CultureInfo.InvariantCulture, key.PropertyName));
							}
							if (HasFlag(key.DefaultValueHandling.GetValueOrDefault(base.Serializer.DefaultValueHandling), global::Newtonsoft.Json.DefaultValueHandling.Populate) && key.Writable)
							{
								key.ValueProvider.SetValue(newObject, EnsureType(key.DefaultValue, global::System.Globalization.CultureInfo.InvariantCulture, key.PropertyType));
							}
							break;
						case global::Newtonsoft.Json.Serialization.JsonSerializerInternalReader.PropertyPresence.Null:
							if (key.Required == global::Newtonsoft.Json.Required.Always)
							{
								throw new global::Newtonsoft.Json.JsonSerializationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Required property '{0}' expects a value but got null.", global::System.Globalization.CultureInfo.InvariantCulture, key.PropertyName));
							}
							break;
						}
					}
					contract.InvokeOnDeserialized(newObject, base.Serializer.Context);
					return newObject;
				default:
					throw new global::Newtonsoft.Json.JsonSerializationException("Unexpected token when deserializing object: " + reader.TokenType);
				case global::Newtonsoft.Json.JsonToken.Comment:
					break;
				}
			}
			while (reader.Read());
			throw new global::Newtonsoft.Json.JsonSerializationException("Unexpected end when deserializing object.");
		}

		private void SetPropertyPresence(global::Newtonsoft.Json.JsonReader reader, global::Newtonsoft.Json.Serialization.JsonProperty property, global::System.Collections.Generic.Dictionary<global::Newtonsoft.Json.Serialization.JsonProperty, global::Newtonsoft.Json.Serialization.JsonSerializerInternalReader.PropertyPresence> requiredProperties)
		{
			if (property != null)
			{
				requiredProperties[property] = ((reader.TokenType == global::Newtonsoft.Json.JsonToken.Null || reader.TokenType == global::Newtonsoft.Json.JsonToken.Undefined) ? global::Newtonsoft.Json.Serialization.JsonSerializerInternalReader.PropertyPresence.Null : global::Newtonsoft.Json.Serialization.JsonSerializerInternalReader.PropertyPresence.Value);
			}
		}

		private void HandleError(global::Newtonsoft.Json.JsonReader reader, int initialDepth)
		{
			ClearErrorContext();
			reader.Skip();
			while (reader.Depth > initialDepth + 1)
			{
				reader.Read();
			}
		}
	}
}
