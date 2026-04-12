namespace Newtonsoft.Json.Serialization
{
	internal class JsonSerializerInternalWriter : global::Newtonsoft.Json.Serialization.JsonSerializerInternalBase
	{
		private global::Newtonsoft.Json.Serialization.JsonSerializerProxy _internalSerializer;

		private global::System.Collections.Generic.List<object> _serializeStack;

		private global::System.Collections.Generic.List<object> SerializeStack
		{
			get
			{
				if (_serializeStack == null)
				{
					_serializeStack = new global::System.Collections.Generic.List<object>();
				}
				return _serializeStack;
			}
		}

		public JsonSerializerInternalWriter(global::Newtonsoft.Json.JsonSerializer serializer)
			: base(serializer)
		{
		}

		public void Serialize(global::Newtonsoft.Json.JsonWriter jsonWriter, object value)
		{
			if (jsonWriter == null)
			{
				throw new global::System.ArgumentNullException("jsonWriter");
			}
			SerializeValue(jsonWriter, value, GetContractSafe(value), null, null);
		}

		private global::Newtonsoft.Json.Serialization.JsonSerializerProxy GetInternalSerializer()
		{
			if (_internalSerializer == null)
			{
				_internalSerializer = new global::Newtonsoft.Json.Serialization.JsonSerializerProxy(this);
			}
			return _internalSerializer;
		}

		private global::Newtonsoft.Json.Serialization.JsonContract GetContractSafe(object value)
		{
			if (value == null)
			{
				return null;
			}
			return base.Serializer.ContractResolver.ResolveContract(value.GetType());
		}

		private void SerializePrimitive(global::Newtonsoft.Json.JsonWriter writer, object value, global::Newtonsoft.Json.Serialization.JsonPrimitiveContract contract, global::Newtonsoft.Json.Serialization.JsonProperty member, global::Newtonsoft.Json.Serialization.JsonContract collectionValueContract)
		{
			if (contract.UnderlyingType == typeof(byte[]) && ShouldWriteType(global::Newtonsoft.Json.TypeNameHandling.Objects, contract, member, collectionValueContract))
			{
				writer.WriteStartObject();
				WriteTypeProperty(writer, contract.CreatedType);
				writer.WritePropertyName("$value");
				writer.WriteValue(value);
				writer.WriteEndObject();
			}
			else
			{
				writer.WriteValue(value);
			}
		}

		private void SerializeValue(global::Newtonsoft.Json.JsonWriter writer, object value, global::Newtonsoft.Json.Serialization.JsonContract valueContract, global::Newtonsoft.Json.Serialization.JsonProperty member, global::Newtonsoft.Json.Serialization.JsonContract collectionValueContract)
		{
			global::Newtonsoft.Json.JsonConverter jsonConverter = ((member != null) ? member.Converter : null);
			if (value == null)
			{
				writer.WriteNull();
			}
			else if ((jsonConverter != null || (jsonConverter = valueContract.Converter) != null || (jsonConverter = base.Serializer.GetMatchingConverter(valueContract.UnderlyingType)) != null || (jsonConverter = valueContract.InternalConverter) != null) && jsonConverter.CanWrite)
			{
				SerializeConvertable(writer, jsonConverter, value, valueContract);
			}
			else if (valueContract is global::Newtonsoft.Json.Serialization.JsonPrimitiveContract)
			{
				SerializePrimitive(writer, value, (global::Newtonsoft.Json.Serialization.JsonPrimitiveContract)valueContract, member, collectionValueContract);
			}
			else if (valueContract is global::Newtonsoft.Json.Serialization.JsonStringContract)
			{
				SerializeString(writer, value, (global::Newtonsoft.Json.Serialization.JsonStringContract)valueContract);
			}
			else if (valueContract is global::Newtonsoft.Json.Serialization.JsonObjectContract)
			{
				SerializeObject(writer, value, (global::Newtonsoft.Json.Serialization.JsonObjectContract)valueContract, member, collectionValueContract);
			}
			else if (valueContract is global::Newtonsoft.Json.Serialization.JsonDictionaryContract)
			{
				global::Newtonsoft.Json.Serialization.JsonDictionaryContract jsonDictionaryContract = (global::Newtonsoft.Json.Serialization.JsonDictionaryContract)valueContract;
				SerializeDictionary(writer, jsonDictionaryContract.CreateWrapper(value), jsonDictionaryContract, member, collectionValueContract);
			}
			else if (valueContract is global::Newtonsoft.Json.Serialization.JsonArrayContract)
			{
				global::Newtonsoft.Json.Serialization.JsonArrayContract jsonArrayContract = (global::Newtonsoft.Json.Serialization.JsonArrayContract)valueContract;
				SerializeList(writer, jsonArrayContract.CreateWrapper(value), jsonArrayContract, member, collectionValueContract);
			}
			else if (valueContract is global::Newtonsoft.Json.Serialization.JsonLinqContract)
			{
				((global::Newtonsoft.Json.Linq.JToken)value).WriteTo(writer, (base.Serializer.Converters != null) ? global::System.Linq.Enumerable.ToArray(base.Serializer.Converters) : null);
			}
			else if (valueContract is global::Newtonsoft.Json.Serialization.JsonISerializableContract)
			{
				SerializeISerializable(writer, (global::System.Runtime.Serialization.ISerializable)value, (global::Newtonsoft.Json.Serialization.JsonISerializableContract)valueContract);
			}
		}

		private bool ShouldWriteReference(object value, global::Newtonsoft.Json.Serialization.JsonProperty property, global::Newtonsoft.Json.Serialization.JsonContract contract)
		{
			if (value == null)
			{
				return false;
			}
			if (contract is global::Newtonsoft.Json.Serialization.JsonPrimitiveContract)
			{
				return false;
			}
			bool? flag = null;
			if (property != null)
			{
				flag = property.IsReference;
			}
			if (!flag.HasValue)
			{
				flag = contract.IsReference;
			}
			if (!flag.HasValue)
			{
				flag = ((!(contract is global::Newtonsoft.Json.Serialization.JsonArrayContract)) ? new bool?(HasFlag(base.Serializer.PreserveReferencesHandling, global::Newtonsoft.Json.PreserveReferencesHandling.Objects)) : new bool?(HasFlag(base.Serializer.PreserveReferencesHandling, global::Newtonsoft.Json.PreserveReferencesHandling.Arrays)));
			}
			if (!flag.Value)
			{
				return false;
			}
			return base.Serializer.ReferenceResolver.IsReferenced(this, value);
		}

		private void WriteMemberInfoProperty(global::Newtonsoft.Json.JsonWriter writer, object memberValue, global::Newtonsoft.Json.Serialization.JsonProperty property, global::Newtonsoft.Json.Serialization.JsonContract contract)
		{
			string propertyName = property.PropertyName;
			object defaultValue = property.DefaultValue;
			if ((property.NullValueHandling.GetValueOrDefault(base.Serializer.NullValueHandling) == global::Newtonsoft.Json.NullValueHandling.Ignore && memberValue == null) || (HasFlag(property.DefaultValueHandling.GetValueOrDefault(base.Serializer.DefaultValueHandling), global::Newtonsoft.Json.DefaultValueHandling.Ignore) && global::Newtonsoft.Json.Utilities.MiscellaneousUtils.ValueEquals(memberValue, defaultValue)))
			{
				return;
			}
			if (ShouldWriteReference(memberValue, property, contract))
			{
				writer.WritePropertyName(propertyName);
				WriteReference(writer, memberValue);
			}
			else if (CheckForCircularReference(memberValue, property.ReferenceLoopHandling, contract))
			{
				if (memberValue == null && property.Required == global::Newtonsoft.Json.Required.Always)
				{
					throw new global::Newtonsoft.Json.JsonSerializationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Cannot write a null value for property '{0}'. Property requires a value.", global::System.Globalization.CultureInfo.InvariantCulture, property.PropertyName));
				}
				writer.WritePropertyName(propertyName);
				SerializeValue(writer, memberValue, contract, property, null);
			}
		}

		private bool CheckForCircularReference(object value, global::Newtonsoft.Json.ReferenceLoopHandling? referenceLoopHandling, global::Newtonsoft.Json.Serialization.JsonContract contract)
		{
			if (value == null || contract is global::Newtonsoft.Json.Serialization.JsonPrimitiveContract)
			{
				return true;
			}
			if (SerializeStack.IndexOf(value) != -1)
			{
				switch ((value is global::UnityEngine.Vector2 || value is global::UnityEngine.Vector3 || value is global::UnityEngine.Vector4 || value is global::UnityEngine.Color || value is global::UnityEngine.Color32) ? global::Newtonsoft.Json.ReferenceLoopHandling.Ignore : referenceLoopHandling.GetValueOrDefault(base.Serializer.ReferenceLoopHandling))
				{
				case global::Newtonsoft.Json.ReferenceLoopHandling.Error:
					throw new global::Newtonsoft.Json.JsonSerializationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Self referencing loop detected for type '{0}'.", global::System.Globalization.CultureInfo.InvariantCulture, value.GetType()));
				case global::Newtonsoft.Json.ReferenceLoopHandling.Ignore:
					return false;
				case global::Newtonsoft.Json.ReferenceLoopHandling.Serialize:
					return true;
				default:
					throw new global::System.InvalidOperationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Unexpected ReferenceLoopHandling value: '{0}'", global::System.Globalization.CultureInfo.InvariantCulture, base.Serializer.ReferenceLoopHandling));
				}
			}
			return true;
		}

		private void WriteReference(global::Newtonsoft.Json.JsonWriter writer, object value)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("$ref");
			writer.WriteValue(base.Serializer.ReferenceResolver.GetReference(this, value));
			writer.WriteEndObject();
		}

		internal static bool TryConvertToString(object value, global::System.Type type, out string s)
		{
			global::System.ComponentModel.TypeConverter converter = global::Newtonsoft.Json.Utilities.ConvertUtils.GetConverter(type);
			if (converter != null && !(converter is global::System.ComponentModel.ComponentConverter) && converter.GetType() != typeof(global::System.ComponentModel.TypeConverter) && converter.CanConvertTo(typeof(string)))
			{
				s = converter.ConvertToInvariantString(value);
				return true;
			}
			if (value is global::System.Type)
			{
				s = ((global::System.Type)value).AssemblyQualifiedName;
				return true;
			}
			s = null;
			return false;
		}

		private void SerializeString(global::Newtonsoft.Json.JsonWriter writer, object value, global::Newtonsoft.Json.Serialization.JsonStringContract contract)
		{
			contract.InvokeOnSerializing(value, base.Serializer.Context);
			string s;
			TryConvertToString(value, contract.UnderlyingType, out s);
			writer.WriteValue(s);
			contract.InvokeOnSerialized(value, base.Serializer.Context);
		}

		private void SerializeObject(global::Newtonsoft.Json.JsonWriter writer, object value, global::Newtonsoft.Json.Serialization.JsonObjectContract contract, global::Newtonsoft.Json.Serialization.JsonProperty member, global::Newtonsoft.Json.Serialization.JsonContract collectionValueContract)
		{
			contract.InvokeOnSerializing(value, base.Serializer.Context);
			SerializeStack.Add(value);
			writer.WriteStartObject();
			if (contract.IsReference ?? HasFlag(base.Serializer.PreserveReferencesHandling, global::Newtonsoft.Json.PreserveReferencesHandling.Objects))
			{
				writer.WritePropertyName("$id");
				writer.WriteValue(base.Serializer.ReferenceResolver.GetReference(this, value));
			}
			if (ShouldWriteType(global::Newtonsoft.Json.TypeNameHandling.Objects, contract, member, collectionValueContract))
			{
				WriteTypeProperty(writer, contract.UnderlyingType);
			}
			int top = writer.Top;
			foreach (global::Newtonsoft.Json.Serialization.JsonProperty property in contract.Properties)
			{
				try
				{
					if (!property.Ignored && property.Readable && ShouldSerialize(property, value) && IsSpecified(property, value))
					{
						object value2 = property.ValueProvider.GetValue(value);
						global::Newtonsoft.Json.Serialization.JsonContract contractSafe = GetContractSafe(value2);
						WriteMemberInfoProperty(writer, value2, property, contractSafe);
					}
				}
				catch (global::System.Exception ex)
				{
					if (IsErrorHandled(value, contract, property.PropertyName, ex))
					{
						HandleError(writer, top);
						continue;
					}
					throw;
				}
			}
			writer.WriteEndObject();
			SerializeStack.RemoveAt(SerializeStack.Count - 1);
			contract.InvokeOnSerialized(value, base.Serializer.Context);
		}

		private void WriteTypeProperty(global::Newtonsoft.Json.JsonWriter writer, global::System.Type type)
		{
			writer.WritePropertyName("$type");
			writer.WriteValue(global::Newtonsoft.Json.Utilities.ReflectionUtils.GetTypeName(type, base.Serializer.TypeNameAssemblyFormat, base.Serializer.Binder));
		}

		private bool HasFlag(global::Newtonsoft.Json.DefaultValueHandling value, global::Newtonsoft.Json.DefaultValueHandling flag)
		{
			return (value & flag) == flag;
		}

		private bool HasFlag(global::Newtonsoft.Json.PreserveReferencesHandling value, global::Newtonsoft.Json.PreserveReferencesHandling flag)
		{
			return (value & flag) == flag;
		}

		private bool HasFlag(global::Newtonsoft.Json.TypeNameHandling value, global::Newtonsoft.Json.TypeNameHandling flag)
		{
			return (value & flag) == flag;
		}

		private void SerializeConvertable(global::Newtonsoft.Json.JsonWriter writer, global::Newtonsoft.Json.JsonConverter converter, object value, global::Newtonsoft.Json.Serialization.JsonContract contract)
		{
			if (ShouldWriteReference(value, null, contract))
			{
				WriteReference(writer, value);
			}
			else if (CheckForCircularReference(value, null, contract))
			{
				SerializeStack.Add(value);
				converter.WriteJson(writer, value, GetInternalSerializer());
				SerializeStack.RemoveAt(SerializeStack.Count - 1);
			}
		}

		private void SerializeList(global::Newtonsoft.Json.JsonWriter writer, global::Newtonsoft.Json.Utilities.IWrappedCollection values, global::Newtonsoft.Json.Serialization.JsonArrayContract contract, global::Newtonsoft.Json.Serialization.JsonProperty member, global::Newtonsoft.Json.Serialization.JsonContract collectionValueContract)
		{
			contract.InvokeOnSerializing(values.UnderlyingCollection, base.Serializer.Context);
			SerializeStack.Add(values.UnderlyingCollection);
			bool flag = contract.IsReference ?? HasFlag(base.Serializer.PreserveReferencesHandling, global::Newtonsoft.Json.PreserveReferencesHandling.Arrays);
			bool flag2 = ShouldWriteType(global::Newtonsoft.Json.TypeNameHandling.Arrays, contract, member, collectionValueContract);
			if (flag || flag2)
			{
				writer.WriteStartObject();
				if (flag)
				{
					writer.WritePropertyName("$id");
					writer.WriteValue(base.Serializer.ReferenceResolver.GetReference(this, values.UnderlyingCollection));
				}
				if (flag2)
				{
					WriteTypeProperty(writer, values.UnderlyingCollection.GetType());
				}
				writer.WritePropertyName("$values");
			}
			global::Newtonsoft.Json.Serialization.JsonContract collectionValueContract2 = base.Serializer.ContractResolver.ResolveContract(contract.CollectionItemType ?? typeof(object));
			writer.WriteStartArray();
			int top = writer.Top;
			int num = 0;
			foreach (object value in values)
			{
				try
				{
					global::Newtonsoft.Json.Serialization.JsonContract contractSafe = GetContractSafe(value);
					if (ShouldWriteReference(value, null, contractSafe))
					{
						WriteReference(writer, value);
					}
					else if (CheckForCircularReference(value, null, contract))
					{
						SerializeValue(writer, value, contractSafe, null, collectionValueContract2);
					}
				}
				catch (global::System.Exception ex)
				{
					if (IsErrorHandled(values.UnderlyingCollection, contract, num, ex))
					{
						HandleError(writer, top);
						continue;
					}
					throw;
				}
				finally
				{
					num++;
				}
			}
			writer.WriteEndArray();
			if (flag || flag2)
			{
				writer.WriteEndObject();
			}
			SerializeStack.RemoveAt(SerializeStack.Count - 1);
			contract.InvokeOnSerialized(values.UnderlyingCollection, base.Serializer.Context);
		}

		[global::System.Security.SecuritySafeCritical]
		private void SerializeISerializable(global::Newtonsoft.Json.JsonWriter writer, global::System.Runtime.Serialization.ISerializable value, global::Newtonsoft.Json.Serialization.JsonISerializableContract contract)
		{
			contract.InvokeOnSerializing(value, base.Serializer.Context);
			SerializeStack.Add(value);
			writer.WriteStartObject();
			global::System.Runtime.Serialization.SerializationInfo serializationInfo = new global::System.Runtime.Serialization.SerializationInfo(contract.UnderlyingType, new global::System.Runtime.Serialization.FormatterConverter());
			value.GetObjectData(serializationInfo, base.Serializer.Context);
			global::System.Runtime.Serialization.SerializationInfoEnumerator enumerator = serializationInfo.GetEnumerator();
			while (enumerator.MoveNext())
			{
				global::System.Runtime.Serialization.SerializationEntry current = enumerator.Current;
				writer.WritePropertyName(current.Name);
				SerializeValue(writer, current.Value, GetContractSafe(current.Value), null, null);
			}
			writer.WriteEndObject();
			SerializeStack.RemoveAt(SerializeStack.Count - 1);
			contract.InvokeOnSerialized(value, base.Serializer.Context);
		}

		private bool ShouldWriteType(global::Newtonsoft.Json.TypeNameHandling typeNameHandlingFlag, global::Newtonsoft.Json.Serialization.JsonContract contract, global::Newtonsoft.Json.Serialization.JsonProperty member, global::Newtonsoft.Json.Serialization.JsonContract collectionValueContract)
		{
			if (HasFlag(((member != null) ? member.TypeNameHandling : ((global::Newtonsoft.Json.TypeNameHandling?)null)) ?? base.Serializer.TypeNameHandling, typeNameHandlingFlag))
			{
				return true;
			}
			if (member != null)
			{
				if ((member.TypeNameHandling ?? base.Serializer.TypeNameHandling) == global::Newtonsoft.Json.TypeNameHandling.Auto && contract.UnderlyingType != member.PropertyType)
				{
					global::Newtonsoft.Json.Serialization.JsonContract jsonContract = base.Serializer.ContractResolver.ResolveContract(member.PropertyType);
					if (contract.UnderlyingType != jsonContract.CreatedType)
					{
						return true;
					}
				}
			}
			else if (collectionValueContract != null && base.Serializer.TypeNameHandling == global::Newtonsoft.Json.TypeNameHandling.Auto && contract.UnderlyingType != collectionValueContract.UnderlyingType)
			{
				return true;
			}
			return false;
		}

		private void SerializeDictionary(global::Newtonsoft.Json.JsonWriter writer, global::Newtonsoft.Json.Utilities.IWrappedDictionary values, global::Newtonsoft.Json.Serialization.JsonDictionaryContract contract, global::Newtonsoft.Json.Serialization.JsonProperty member, global::Newtonsoft.Json.Serialization.JsonContract collectionValueContract)
		{
			contract.InvokeOnSerializing(values.UnderlyingDictionary, base.Serializer.Context);
			SerializeStack.Add(values.UnderlyingDictionary);
			writer.WriteStartObject();
			if (contract.IsReference ?? HasFlag(base.Serializer.PreserveReferencesHandling, global::Newtonsoft.Json.PreserveReferencesHandling.Objects))
			{
				writer.WritePropertyName("$id");
				writer.WriteValue(base.Serializer.ReferenceResolver.GetReference(this, values.UnderlyingDictionary));
			}
			if (ShouldWriteType(global::Newtonsoft.Json.TypeNameHandling.Objects, contract, member, collectionValueContract))
			{
				WriteTypeProperty(writer, values.UnderlyingDictionary.GetType());
			}
			global::Newtonsoft.Json.Serialization.JsonContract collectionValueContract2 = base.Serializer.ContractResolver.ResolveContract(contract.DictionaryValueType ?? typeof(object));
			int top = writer.Top;
			global::System.Collections.IDictionaryEnumerator enumerator = values.GetEnumerator();
			while (enumerator.MoveNext())
			{
				global::System.Collections.DictionaryEntry value2 = enumerator.Entry;
				string propertyName = GetPropertyName(value2);
				propertyName = ((contract.PropertyNameResolver != null) ? contract.PropertyNameResolver(propertyName) : propertyName);
				try
				{
					object value = value2.Value;
					global::Newtonsoft.Json.Serialization.JsonContract contractSafe = GetContractSafe(value);
					if (ShouldWriteReference(value, null, contractSafe))
					{
						writer.WritePropertyName(propertyName);
						WriteReference(writer, value);
					}
					else if (CheckForCircularReference(value, null, contract))
					{
						writer.WritePropertyName(propertyName);
						SerializeValue(writer, value, contractSafe, null, collectionValueContract2);
					}
				}
				catch (global::System.Exception ex)
				{
					if (IsErrorHandled(values.UnderlyingDictionary, contract, propertyName, ex))
					{
						HandleError(writer, top);
						continue;
					}
					throw;
				}
			}
			writer.WriteEndObject();
			SerializeStack.RemoveAt(SerializeStack.Count - 1);
			contract.InvokeOnSerialized(values.UnderlyingDictionary, base.Serializer.Context);
		}

		private string GetPropertyName(global::System.Collections.DictionaryEntry entry)
		{
			if (entry.Key is global::System.IConvertible)
			{
				return global::System.Convert.ToString(entry.Key, global::System.Globalization.CultureInfo.InvariantCulture);
			}
			string s;
			if (TryConvertToString(entry.Key, entry.Key.GetType(), out s))
			{
				return s;
			}
			return entry.Key.ToString();
		}

		private void HandleError(global::Newtonsoft.Json.JsonWriter writer, int initialDepth)
		{
			ClearErrorContext();
			while (writer.Top > initialDepth)
			{
				writer.WriteEnd();
			}
		}

		private bool ShouldSerialize(global::Newtonsoft.Json.Serialization.JsonProperty property, object target)
		{
			if (property.ShouldSerialize == null)
			{
				return true;
			}
			return property.ShouldSerialize(target);
		}

		private bool IsSpecified(global::Newtonsoft.Json.Serialization.JsonProperty property, object target)
		{
			if (property.GetIsSpecified == null)
			{
				return true;
			}
			return property.GetIsSpecified(target);
		}
	}
}
