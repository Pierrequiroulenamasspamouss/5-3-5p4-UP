namespace Newtonsoft.Json.Serialization
{
	public class DefaultContractResolver : global::Newtonsoft.Json.Serialization.IContractResolver
	{
		private static readonly global::Newtonsoft.Json.Serialization.IContractResolver _instance = new global::Newtonsoft.Json.Serialization.DefaultContractResolver(true);

		private static readonly global::System.Collections.Generic.IList<global::Newtonsoft.Json.JsonConverter> BuiltInConverters = new global::System.Collections.Generic.List<global::Newtonsoft.Json.JsonConverter>
		{
			new global::Newtonsoft.Json.Converters.KeyValuePairConverter(),
			new global::Newtonsoft.Json.Converters.BsonObjectIdConverter()
		};

		private readonly global::Newtonsoft.Json.Utilities.ThreadSafeStore<global::System.Type, global::Newtonsoft.Json.Serialization.JsonContract> _typeContractCache;

		internal static global::Newtonsoft.Json.Serialization.IContractResolver Instance
		{
			get
			{
				return _instance;
			}
		}

		public bool DynamicCodeGeneration
		{
			get
			{
				return global::Newtonsoft.Json.Serialization.JsonTypeReflector.DynamicCodeGeneration;
			}
		}

		public global::System.Reflection.BindingFlags DefaultMembersSearchFlags { get; set; }

		public bool SerializeCompilerGeneratedMembers { get; set; }

		public DefaultContractResolver()
			: this(false)
		{
		}

		public DefaultContractResolver(bool shareCache)
		{
			DefaultMembersSearchFlags = global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Public;
			_typeContractCache = new global::Newtonsoft.Json.Utilities.ThreadSafeStore<global::System.Type, global::Newtonsoft.Json.Serialization.JsonContract>(CreateContract);
		}

		public virtual global::Newtonsoft.Json.Serialization.JsonContract ResolveContract(global::System.Type type)
		{
			if (type == null)
			{
				throw new global::System.ArgumentNullException("type");
			}
			return _typeContractCache.Get(type);
		}

		protected virtual global::System.Collections.Generic.List<global::System.Reflection.MemberInfo> GetSerializableMembers(global::System.Type objectType)
		{
			global::System.Runtime.Serialization.DataContractAttribute dataContractAttribute = global::Newtonsoft.Json.Serialization.JsonTypeReflector.GetDataContractAttribute(objectType);
			global::System.Collections.Generic.List<global::System.Reflection.MemberInfo> list = global::System.Linq.Enumerable.ToList(global::System.Linq.Enumerable.Where(global::Newtonsoft.Json.Utilities.ReflectionUtils.GetFieldsAndProperties(objectType, DefaultMembersSearchFlags), (global::System.Reflection.MemberInfo m) => !global::Newtonsoft.Json.Utilities.ReflectionUtils.IsIndexedProperty(m)));
			global::System.Collections.Generic.List<global::System.Reflection.MemberInfo> list2 = global::System.Linq.Enumerable.ToList(global::System.Linq.Enumerable.Where(global::Newtonsoft.Json.Utilities.ReflectionUtils.GetFieldsAndProperties(objectType, global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Static | global::System.Reflection.BindingFlags.Public | global::System.Reflection.BindingFlags.NonPublic), (global::System.Reflection.MemberInfo m) => !global::Newtonsoft.Json.Utilities.ReflectionUtils.IsIndexedProperty(m)));
			global::System.Collections.Generic.List<global::System.Reflection.MemberInfo> list3 = new global::System.Collections.Generic.List<global::System.Reflection.MemberInfo>();
			foreach (global::System.Reflection.MemberInfo item in list2)
			{
				if (SerializeCompilerGeneratedMembers || !item.IsDefined(typeof(global::System.Runtime.CompilerServices.CompilerGeneratedAttribute), true))
				{
					if (list.Contains(item))
					{
						list3.Add(item);
					}
					else if (global::Newtonsoft.Json.Serialization.JsonTypeReflector.GetAttribute<global::Newtonsoft.Json.JsonPropertyAttribute>(item) != null)
					{
						list3.Add(item);
					}
					else if (dataContractAttribute != null && global::Newtonsoft.Json.Serialization.JsonTypeReflector.GetAttribute<global::System.Runtime.Serialization.DataMemberAttribute>(item) != null)
					{
						list3.Add(item);
					}
				}
			}
			global::System.Type match;
			if (global::Newtonsoft.Json.Utilities.ReflectionUtils.AssignableToTypeName(objectType, "System.Data.Objects.DataClasses.EntityObject", out match))
			{
				list3 = global::System.Linq.Enumerable.ToList(global::System.Linq.Enumerable.Where(list3, ShouldSerializeEntityMember));
			}
			return list3;
		}

		private bool ShouldSerializeEntityMember(global::System.Reflection.MemberInfo memberInfo)
		{
			global::System.Reflection.PropertyInfo propertyInfo = memberInfo as global::System.Reflection.PropertyInfo;
			if (propertyInfo != null && propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition().FullName == "System.Data.Objects.DataClasses.EntityReference`1")
			{
				return false;
			}
			return true;
		}

		protected virtual global::Newtonsoft.Json.Serialization.JsonObjectContract CreateObjectContract(global::System.Type objectType)
		{
			global::Newtonsoft.Json.Serialization.JsonObjectContract jsonObjectContract = new global::Newtonsoft.Json.Serialization.JsonObjectContract(objectType);
			InitializeContract(jsonObjectContract);
			jsonObjectContract.MemberSerialization = global::Newtonsoft.Json.Serialization.JsonTypeReflector.GetObjectMemberSerialization(objectType);
			global::Newtonsoft.Json.Utilities.CollectionUtils.AddRange(jsonObjectContract.Properties, CreateProperties(jsonObjectContract.UnderlyingType, jsonObjectContract.MemberSerialization));
			if (global::System.Linq.Enumerable.Any(objectType.GetConstructors(global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Public | global::System.Reflection.BindingFlags.NonPublic), (global::System.Reflection.ConstructorInfo c) => c.IsDefined(typeof(global::Newtonsoft.Json.JsonConstructorAttribute), true)))
			{
				global::System.Reflection.ConstructorInfo attributeConstructor = GetAttributeConstructor(objectType);
				if (attributeConstructor != null)
				{
					jsonObjectContract.OverrideConstructor = attributeConstructor;
					global::Newtonsoft.Json.Utilities.CollectionUtils.AddRange(jsonObjectContract.ConstructorParameters, CreateConstructorParameters(attributeConstructor, jsonObjectContract.Properties));
				}
			}
			else if (jsonObjectContract.DefaultCreator == null || jsonObjectContract.DefaultCreatorNonPublic)
			{
				global::System.Reflection.ConstructorInfo parametrizedConstructor = GetParametrizedConstructor(objectType);
				if (parametrizedConstructor != null)
				{
					jsonObjectContract.ParametrizedConstructor = parametrizedConstructor;
					global::Newtonsoft.Json.Utilities.CollectionUtils.AddRange(jsonObjectContract.ConstructorParameters, CreateConstructorParameters(parametrizedConstructor, jsonObjectContract.Properties));
				}
			}
			return jsonObjectContract;
		}

		private global::System.Reflection.ConstructorInfo GetAttributeConstructor(global::System.Type objectType)
		{
			global::System.Collections.Generic.IList<global::System.Reflection.ConstructorInfo> list = global::System.Linq.Enumerable.ToList(global::System.Linq.Enumerable.Where(objectType.GetConstructors(global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Public | global::System.Reflection.BindingFlags.NonPublic), (global::System.Reflection.ConstructorInfo c) => c.IsDefined(typeof(global::Newtonsoft.Json.JsonConstructorAttribute), true)));
			if (list.Count > 1)
			{
				throw new global::System.Exception("Multiple constructors with the JsonConstructorAttribute.");
			}
			if (list.Count == 1)
			{
				return list[0];
			}
			return null;
		}

		private global::System.Reflection.ConstructorInfo GetParametrizedConstructor(global::System.Type objectType)
		{
			global::System.Collections.Generic.IList<global::System.Reflection.ConstructorInfo> constructors = objectType.GetConstructors(global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Public);
			if (constructors.Count == 1)
			{
				return constructors[0];
			}
			return null;
		}

		protected virtual global::System.Collections.Generic.IList<global::Newtonsoft.Json.Serialization.JsonProperty> CreateConstructorParameters(global::System.Reflection.ConstructorInfo constructor, global::Newtonsoft.Json.Serialization.JsonPropertyCollection memberProperties)
		{
			global::System.Reflection.ParameterInfo[] parameters = constructor.GetParameters();
			global::Newtonsoft.Json.Serialization.JsonPropertyCollection jsonPropertyCollection = new global::Newtonsoft.Json.Serialization.JsonPropertyCollection(constructor.DeclaringType);
			global::System.Reflection.ParameterInfo[] array = parameters;
			foreach (global::System.Reflection.ParameterInfo parameterInfo in array)
			{
				global::Newtonsoft.Json.Serialization.JsonProperty jsonProperty = memberProperties.GetClosestMatchProperty(parameterInfo.Name);
				if (jsonProperty != null && jsonProperty.PropertyType != parameterInfo.ParameterType)
				{
					jsonProperty = null;
				}
				global::Newtonsoft.Json.Serialization.JsonProperty jsonProperty2 = CreatePropertyFromConstructorParameter(jsonProperty, parameterInfo);
				if (jsonProperty2 != null)
				{
					jsonPropertyCollection.AddProperty(jsonProperty2);
				}
			}
			return jsonPropertyCollection;
		}

		protected virtual global::Newtonsoft.Json.Serialization.JsonProperty CreatePropertyFromConstructorParameter(global::Newtonsoft.Json.Serialization.JsonProperty matchingMemberProperty, global::System.Reflection.ParameterInfo parameterInfo)
		{
			global::Newtonsoft.Json.Serialization.JsonProperty jsonProperty = new global::Newtonsoft.Json.Serialization.JsonProperty();
			jsonProperty.PropertyType = parameterInfo.ParameterType;
			bool allowNonPublicAccess;
			bool hasExplicitAttribute;
			SetPropertySettingsFromAttributes(jsonProperty, parameterInfo, parameterInfo.Name, parameterInfo.Member.DeclaringType, global::Newtonsoft.Json.MemberSerialization.OptOut, out allowNonPublicAccess, out hasExplicitAttribute);
			jsonProperty.Readable = false;
			jsonProperty.Writable = true;
			if (matchingMemberProperty != null)
			{
				jsonProperty.PropertyName = ((jsonProperty.PropertyName != parameterInfo.Name) ? jsonProperty.PropertyName : matchingMemberProperty.PropertyName);
				jsonProperty.Converter = jsonProperty.Converter ?? matchingMemberProperty.Converter;
				jsonProperty.MemberConverter = jsonProperty.MemberConverter ?? matchingMemberProperty.MemberConverter;
				jsonProperty.DefaultValue = jsonProperty.DefaultValue ?? matchingMemberProperty.DefaultValue;
				jsonProperty.Required = ((jsonProperty.Required != global::Newtonsoft.Json.Required.Default) ? jsonProperty.Required : matchingMemberProperty.Required);
				jsonProperty.IsReference = jsonProperty.IsReference ?? matchingMemberProperty.IsReference;
				jsonProperty.NullValueHandling = jsonProperty.NullValueHandling ?? matchingMemberProperty.NullValueHandling;
				jsonProperty.DefaultValueHandling = jsonProperty.DefaultValueHandling ?? matchingMemberProperty.DefaultValueHandling;
				jsonProperty.ReferenceLoopHandling = jsonProperty.ReferenceLoopHandling ?? matchingMemberProperty.ReferenceLoopHandling;
				jsonProperty.ObjectCreationHandling = jsonProperty.ObjectCreationHandling ?? matchingMemberProperty.ObjectCreationHandling;
				jsonProperty.TypeNameHandling = jsonProperty.TypeNameHandling ?? matchingMemberProperty.TypeNameHandling;
			}
			return jsonProperty;
		}

		protected virtual global::Newtonsoft.Json.JsonConverter ResolveContractConverter(global::System.Type objectType)
		{
			return global::Newtonsoft.Json.Serialization.JsonTypeReflector.GetJsonConverter(objectType, objectType);
		}

		private global::System.Func<object> GetDefaultCreator(global::System.Type createdType)
		{
			return global::Newtonsoft.Json.Serialization.JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(createdType);
		}

		private void InitializeContract(global::Newtonsoft.Json.Serialization.JsonContract contract)
		{
			global::Newtonsoft.Json.JsonContainerAttribute jsonContainerAttribute = global::Newtonsoft.Json.Serialization.JsonTypeReflector.GetJsonContainerAttribute(contract.UnderlyingType);
			if (jsonContainerAttribute != null)
			{
				contract.IsReference = jsonContainerAttribute._isReference;
			}
			else
			{
				global::System.Runtime.Serialization.DataContractAttribute dataContractAttribute = global::Newtonsoft.Json.Serialization.JsonTypeReflector.GetDataContractAttribute(contract.UnderlyingType);
				if (dataContractAttribute != null && dataContractAttribute.IsReference)
				{
					contract.IsReference = true;
				}
			}
			contract.Converter = ResolveContractConverter(contract.UnderlyingType);
			contract.InternalConverter = global::Newtonsoft.Json.JsonSerializer.GetMatchingConverter(BuiltInConverters, contract.UnderlyingType);
			if (global::Newtonsoft.Json.Utilities.ReflectionUtils.HasDefaultConstructor(contract.CreatedType, true) || contract.CreatedType.IsValueType)
			{
				contract.DefaultCreator = GetDefaultCreator(contract.CreatedType);
				contract.DefaultCreatorNonPublic = !contract.CreatedType.IsValueType && global::Newtonsoft.Json.Utilities.ReflectionUtils.GetDefaultConstructor(contract.CreatedType) == null;
			}
			ResolveCallbackMethods(contract, contract.UnderlyingType);
		}

		private void ResolveCallbackMethods(global::Newtonsoft.Json.Serialization.JsonContract contract, global::System.Type t)
		{
			if (t.BaseType != null)
			{
				ResolveCallbackMethods(contract, t.BaseType);
			}
			global::System.Reflection.MethodInfo onSerializing;
			global::System.Reflection.MethodInfo onSerialized;
			global::System.Reflection.MethodInfo onDeserializing;
			global::System.Reflection.MethodInfo onDeserialized;
			global::System.Reflection.MethodInfo onError;
			GetCallbackMethodsForType(t, out onSerializing, out onSerialized, out onDeserializing, out onDeserialized, out onError);
			if (onSerializing != null)
			{
				contract.OnSerializing = onSerializing;
			}
			if (onSerialized != null)
			{
				contract.OnSerialized = onSerialized;
			}
			if (onDeserializing != null)
			{
				contract.OnDeserializing = onDeserializing;
			}
			if (onDeserialized != null)
			{
				contract.OnDeserialized = onDeserialized;
			}
			if (onError != null)
			{
				contract.OnError = onError;
			}
		}

		private void GetCallbackMethodsForType(global::System.Type type, out global::System.Reflection.MethodInfo onSerializing, out global::System.Reflection.MethodInfo onSerialized, out global::System.Reflection.MethodInfo onDeserializing, out global::System.Reflection.MethodInfo onDeserialized, out global::System.Reflection.MethodInfo onError)
		{
			onSerializing = null;
			onSerialized = null;
			onDeserializing = null;
			onDeserialized = null;
			onError = null;
			global::System.Reflection.MethodInfo[] methods = type.GetMethods(global::System.Reflection.BindingFlags.DeclaredOnly | global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Public | global::System.Reflection.BindingFlags.NonPublic);
			foreach (global::System.Reflection.MethodInfo methodInfo in methods)
			{
				if (!methodInfo.ContainsGenericParameters)
				{
					global::System.Type prevAttributeType = null;
					global::System.Reflection.ParameterInfo[] parameters = methodInfo.GetParameters();
					if (IsValidCallback(methodInfo, parameters, typeof(global::System.Runtime.Serialization.OnSerializingAttribute), onSerializing, ref prevAttributeType))
					{
						onSerializing = methodInfo;
					}
					if (IsValidCallback(methodInfo, parameters, typeof(global::System.Runtime.Serialization.OnSerializedAttribute), onSerialized, ref prevAttributeType))
					{
						onSerialized = methodInfo;
					}
					if (IsValidCallback(methodInfo, parameters, typeof(global::System.Runtime.Serialization.OnDeserializingAttribute), onDeserializing, ref prevAttributeType))
					{
						onDeserializing = methodInfo;
					}
					if (IsValidCallback(methodInfo, parameters, typeof(global::System.Runtime.Serialization.OnDeserializedAttribute), onDeserialized, ref prevAttributeType))
					{
						onDeserialized = methodInfo;
					}
					if (IsValidCallback(methodInfo, parameters, typeof(global::Newtonsoft.Json.Serialization.OnErrorAttribute), onError, ref prevAttributeType))
					{
						onError = methodInfo;
					}
				}
			}
		}

		protected virtual global::Newtonsoft.Json.Serialization.JsonDictionaryContract CreateDictionaryContract(global::System.Type objectType)
		{
			global::Newtonsoft.Json.Serialization.JsonDictionaryContract jsonDictionaryContract = new global::Newtonsoft.Json.Serialization.JsonDictionaryContract(objectType);
			InitializeContract(jsonDictionaryContract);
			jsonDictionaryContract.PropertyNameResolver = ResolvePropertyName;
			return jsonDictionaryContract;
		}

		protected virtual global::Newtonsoft.Json.Serialization.JsonArrayContract CreateArrayContract(global::System.Type objectType)
		{
			global::Newtonsoft.Json.Serialization.JsonArrayContract jsonArrayContract = new global::Newtonsoft.Json.Serialization.JsonArrayContract(objectType);
			InitializeContract(jsonArrayContract);
			return jsonArrayContract;
		}

		protected virtual global::Newtonsoft.Json.Serialization.JsonPrimitiveContract CreatePrimitiveContract(global::System.Type objectType)
		{
			global::Newtonsoft.Json.Serialization.JsonPrimitiveContract jsonPrimitiveContract = new global::Newtonsoft.Json.Serialization.JsonPrimitiveContract(objectType);
			InitializeContract(jsonPrimitiveContract);
			return jsonPrimitiveContract;
		}

		protected virtual global::Newtonsoft.Json.Serialization.JsonLinqContract CreateLinqContract(global::System.Type objectType)
		{
			global::Newtonsoft.Json.Serialization.JsonLinqContract jsonLinqContract = new global::Newtonsoft.Json.Serialization.JsonLinqContract(objectType);
			InitializeContract(jsonLinqContract);
			return jsonLinqContract;
		}

		protected virtual global::Newtonsoft.Json.Serialization.JsonISerializableContract CreateISerializableContract(global::System.Type objectType)
		{
			global::Newtonsoft.Json.Serialization.JsonISerializableContract jsonISerializableContract = new global::Newtonsoft.Json.Serialization.JsonISerializableContract(objectType);
			InitializeContract(jsonISerializableContract);
			global::System.Reflection.ConstructorInfo constructor = objectType.GetConstructor(global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Public | global::System.Reflection.BindingFlags.NonPublic, null, new global::System.Type[2]
			{
				typeof(global::System.Runtime.Serialization.SerializationInfo),
				typeof(global::System.Runtime.Serialization.StreamingContext)
			}, null);
			if (constructor != null)
			{
				global::Newtonsoft.Json.Utilities.MethodCall<object, object> methodCall = global::Newtonsoft.Json.Serialization.JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(constructor);
				jsonISerializableContract.ISerializableCreator = (object[] args) => methodCall(null, args);
			}
			return jsonISerializableContract;
		}

		protected virtual global::Newtonsoft.Json.Serialization.JsonStringContract CreateStringContract(global::System.Type objectType)
		{
			global::Newtonsoft.Json.Serialization.JsonStringContract jsonStringContract = new global::Newtonsoft.Json.Serialization.JsonStringContract(objectType);
			InitializeContract(jsonStringContract);
			return jsonStringContract;
		}

		protected virtual global::Newtonsoft.Json.Serialization.JsonContract CreateContract(global::System.Type objectType)
		{
			global::System.Type type = global::Newtonsoft.Json.Utilities.ReflectionUtils.EnsureNotNullableType(objectType);
			if (global::Newtonsoft.Json.JsonConvert.IsJsonPrimitiveType(type))
			{
				return CreatePrimitiveContract(type);
			}
			if (global::Newtonsoft.Json.Serialization.JsonTypeReflector.GetJsonObjectAttribute(type) != null)
			{
				return CreateObjectContract(type);
			}
			if (global::Newtonsoft.Json.Serialization.JsonTypeReflector.GetJsonArrayAttribute(type) != null)
			{
				return CreateArrayContract(type);
			}
			if (type == typeof(global::Newtonsoft.Json.Linq.JToken) || type.IsSubclassOf(typeof(global::Newtonsoft.Json.Linq.JToken)))
			{
				return CreateLinqContract(type);
			}
			if (global::Newtonsoft.Json.Utilities.CollectionUtils.IsDictionaryType(type))
			{
				return CreateDictionaryContract(type);
			}
			if (typeof(global::System.Collections.IEnumerable).IsAssignableFrom(type))
			{
				return CreateArrayContract(type);
			}
			if (CanConvertToString(type))
			{
				return CreateStringContract(type);
			}
			if (typeof(global::System.Runtime.Serialization.ISerializable).IsAssignableFrom(type))
			{
				return CreateISerializableContract(type);
			}
			return CreateObjectContract(type);
		}

		internal static bool CanConvertToString(global::System.Type type)
		{
			global::System.ComponentModel.TypeConverter converter = global::Newtonsoft.Json.Utilities.ConvertUtils.GetConverter(type);
			if (converter != null && !(converter is global::System.ComponentModel.ComponentConverter) && !(converter is global::System.ComponentModel.ReferenceConverter) && converter.GetType() != typeof(global::System.ComponentModel.TypeConverter) && converter.CanConvertTo(typeof(string)))
			{
				return true;
			}
			if (type == typeof(global::System.Type) || type.IsSubclassOf(typeof(global::System.Type)))
			{
				return true;
			}
			return false;
		}

		private static bool IsValidCallback(global::System.Reflection.MethodInfo method, global::System.Reflection.ParameterInfo[] parameters, global::System.Type attributeType, global::System.Reflection.MethodInfo currentCallback, ref global::System.Type prevAttributeType)
		{
			if (!method.IsDefined(attributeType, false))
			{
				return false;
			}
			if (currentCallback != null)
			{
				throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Invalid attribute. Both '{0}' and '{1}' in type '{2}' have '{3}'.", global::System.Globalization.CultureInfo.InvariantCulture, method, currentCallback, GetClrTypeFullName(method.DeclaringType), attributeType));
			}
			if (prevAttributeType != null)
			{
				throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Invalid Callback. Method '{3}' in type '{2}' has both '{0}' and '{1}'.", global::System.Globalization.CultureInfo.InvariantCulture, prevAttributeType, attributeType, GetClrTypeFullName(method.DeclaringType), method));
			}
			if (method.IsVirtual)
			{
				throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Virtual Method '{0}' of type '{1}' cannot be marked with '{2}' attribute.", global::System.Globalization.CultureInfo.InvariantCulture, method, GetClrTypeFullName(method.DeclaringType), attributeType));
			}
			if (method.ReturnType != typeof(void))
			{
				throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Serialization Callback '{1}' in type '{0}' must return void.", global::System.Globalization.CultureInfo.InvariantCulture, GetClrTypeFullName(method.DeclaringType), method));
			}
			if (attributeType == typeof(global::Newtonsoft.Json.Serialization.OnErrorAttribute))
			{
				if (parameters == null || parameters.Length != 2 || parameters[0].ParameterType != typeof(global::System.Runtime.Serialization.StreamingContext) || parameters[1].ParameterType != typeof(global::Newtonsoft.Json.Serialization.ErrorContext))
				{
					throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Serialization Error Callback '{1}' in type '{0}' must have two parameters of type '{2}' and '{3}'.", global::System.Globalization.CultureInfo.InvariantCulture, GetClrTypeFullName(method.DeclaringType), method, typeof(global::System.Runtime.Serialization.StreamingContext), typeof(global::Newtonsoft.Json.Serialization.ErrorContext)));
				}
			}
			else if (parameters == null || parameters.Length != 1 || parameters[0].ParameterType != typeof(global::System.Runtime.Serialization.StreamingContext))
			{
				throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Serialization Callback '{1}' in type '{0}' must have a single parameter of type '{2}'.", global::System.Globalization.CultureInfo.InvariantCulture, GetClrTypeFullName(method.DeclaringType), method, typeof(global::System.Runtime.Serialization.StreamingContext)));
			}
			prevAttributeType = attributeType;
			return true;
		}

		internal static string GetClrTypeFullName(global::System.Type type)
		{
			if (type.IsGenericTypeDefinition || !type.ContainsGenericParameters)
			{
				return type.FullName;
			}
			return string.Format(global::System.Globalization.CultureInfo.InvariantCulture, "{0}.{1}", type.Namespace, type.Name);
		}

		protected virtual global::System.Collections.Generic.IList<global::Newtonsoft.Json.Serialization.JsonProperty> CreateProperties(global::System.Type type, global::Newtonsoft.Json.MemberSerialization memberSerialization)
		{
			global::System.Collections.Generic.List<global::System.Reflection.MemberInfo> serializableMembers = GetSerializableMembers(type);
			if (serializableMembers == null)
			{
				throw new global::Newtonsoft.Json.JsonSerializationException("Null collection of seralizable members returned.");
			}
			global::Newtonsoft.Json.Serialization.JsonPropertyCollection jsonPropertyCollection = new global::Newtonsoft.Json.Serialization.JsonPropertyCollection(type);
			foreach (global::System.Reflection.MemberInfo item in serializableMembers)
			{
				global::Newtonsoft.Json.Serialization.JsonProperty jsonProperty = CreateProperty(item, memberSerialization);
				if (jsonProperty != null)
				{
					jsonPropertyCollection.AddProperty(jsonProperty);
				}
			}
			return global::System.Linq.Enumerable.ToList(global::System.Linq.Enumerable.OrderBy(jsonPropertyCollection, (global::Newtonsoft.Json.Serialization.JsonProperty p) => p.Order ?? (-1)));
		}

		protected virtual global::Newtonsoft.Json.Serialization.IValueProvider CreateMemberValueProvider(global::System.Reflection.MemberInfo member)
		{
			return new global::Newtonsoft.Json.Serialization.ReflectionValueProvider(member);
		}

		protected virtual global::Newtonsoft.Json.Serialization.JsonProperty CreateProperty(global::System.Reflection.MemberInfo member, global::Newtonsoft.Json.MemberSerialization memberSerialization)
		{
			global::Newtonsoft.Json.Serialization.JsonProperty jsonProperty = new global::Newtonsoft.Json.Serialization.JsonProperty();
			jsonProperty.PropertyType = global::Newtonsoft.Json.Utilities.ReflectionUtils.GetMemberUnderlyingType(member);
			jsonProperty.ValueProvider = CreateMemberValueProvider(member);
			bool allowNonPublicAccess;
			bool hasExplicitAttribute;
			SetPropertySettingsFromAttributes(jsonProperty, member, member.Name, member.DeclaringType, memberSerialization, out allowNonPublicAccess, out hasExplicitAttribute);
			jsonProperty.Readable = global::Newtonsoft.Json.Utilities.ReflectionUtils.CanReadMemberValue(member, allowNonPublicAccess);
			jsonProperty.Writable = global::Newtonsoft.Json.Utilities.ReflectionUtils.CanSetMemberValue(member, allowNonPublicAccess, hasExplicitAttribute);
			jsonProperty.ShouldSerialize = CreateShouldSerializeTest(member);
			SetIsSpecifiedActions(jsonProperty, member, allowNonPublicAccess);
			return jsonProperty;
		}

		private void SetPropertySettingsFromAttributes(global::Newtonsoft.Json.Serialization.JsonProperty property, global::System.Reflection.ICustomAttributeProvider attributeProvider, string name, global::System.Type declaringType, global::Newtonsoft.Json.MemberSerialization memberSerialization, out bool allowNonPublicAccess, out bool hasExplicitAttribute)
		{
			hasExplicitAttribute = false;
			global::System.Runtime.Serialization.DataContractAttribute dataContractAttribute = global::Newtonsoft.Json.Serialization.JsonTypeReflector.GetDataContractAttribute(declaringType);
			global::System.Runtime.Serialization.DataMemberAttribute dataMemberAttribute = ((dataContractAttribute == null || !(attributeProvider is global::System.Reflection.MemberInfo)) ? null : global::Newtonsoft.Json.Serialization.JsonTypeReflector.GetDataMemberAttribute((global::System.Reflection.MemberInfo)attributeProvider));
			global::Newtonsoft.Json.JsonPropertyAttribute attribute = global::Newtonsoft.Json.Serialization.JsonTypeReflector.GetAttribute<global::Newtonsoft.Json.JsonPropertyAttribute>(attributeProvider);
			if (attribute != null)
			{
				hasExplicitAttribute = true;
			}
			bool flag = global::Newtonsoft.Json.Serialization.JsonTypeReflector.GetAttribute<global::Newtonsoft.Json.JsonIgnoreAttribute>(attributeProvider) != null;
			string propertyName = ((attribute != null && attribute.PropertyName != null) ? attribute.PropertyName : ((dataMemberAttribute == null || dataMemberAttribute.Name == null) ? name : dataMemberAttribute.Name));
			property.PropertyName = ResolvePropertyName(propertyName);
			property.UnderlyingName = name;
			if (attribute != null)
			{
				property.Required = attribute.Required;
				property.Order = attribute._order;
			}
			else if (dataMemberAttribute != null)
			{
				property.Required = (dataMemberAttribute.IsRequired ? global::Newtonsoft.Json.Required.AllowNull : global::Newtonsoft.Json.Required.Default);
				property.Order = ((dataMemberAttribute.Order != -1) ? new int?(dataMemberAttribute.Order) : ((int?)null));
			}
			else
			{
				property.Required = global::Newtonsoft.Json.Required.Default;
			}
			property.Ignored = flag || (memberSerialization == global::Newtonsoft.Json.MemberSerialization.OptIn && attribute == null && dataMemberAttribute == null);
			property.Converter = global::Newtonsoft.Json.Serialization.JsonTypeReflector.GetJsonConverter(attributeProvider, property.PropertyType);
			property.MemberConverter = global::Newtonsoft.Json.Serialization.JsonTypeReflector.GetJsonConverter(attributeProvider, property.PropertyType);
			global::System.ComponentModel.DefaultValueAttribute attribute2 = global::Newtonsoft.Json.Serialization.JsonTypeReflector.GetAttribute<global::System.ComponentModel.DefaultValueAttribute>(attributeProvider);
			property.DefaultValue = ((attribute2 != null) ? attribute2.Value : null);
			property.NullValueHandling = ((attribute != null) ? attribute._nullValueHandling : ((global::Newtonsoft.Json.NullValueHandling?)null));
			property.DefaultValueHandling = ((attribute != null) ? attribute._defaultValueHandling : ((global::Newtonsoft.Json.DefaultValueHandling?)null));
			property.ReferenceLoopHandling = ((attribute != null) ? attribute._referenceLoopHandling : ((global::Newtonsoft.Json.ReferenceLoopHandling?)null));
			property.ObjectCreationHandling = ((attribute != null) ? attribute._objectCreationHandling : ((global::Newtonsoft.Json.ObjectCreationHandling?)null));
			property.TypeNameHandling = ((attribute != null) ? attribute._typeNameHandling : ((global::Newtonsoft.Json.TypeNameHandling?)null));
			property.IsReference = ((attribute != null) ? attribute._isReference : ((bool?)null));
			allowNonPublicAccess = false;
			if ((DefaultMembersSearchFlags & global::System.Reflection.BindingFlags.NonPublic) == global::System.Reflection.BindingFlags.NonPublic)
			{
				allowNonPublicAccess = true;
			}
			if (attribute != null)
			{
				allowNonPublicAccess = true;
			}
			if (dataMemberAttribute != null)
			{
				allowNonPublicAccess = true;
				hasExplicitAttribute = true;
			}
		}

		private global::System.Predicate<object> CreateShouldSerializeTest(global::System.Reflection.MemberInfo member)
		{
			global::System.Reflection.MethodInfo method = member.DeclaringType.GetMethod("ShouldSerialize" + member.Name, new global::System.Type[0]);
			if (method == null || method.ReturnType != typeof(bool))
			{
				return null;
			}
			global::Newtonsoft.Json.Utilities.MethodCall<object, object> shouldSerializeCall = global::Newtonsoft.Json.Serialization.JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method);
			return (object o) => (bool)shouldSerializeCall(o);
		}

		private void SetIsSpecifiedActions(global::Newtonsoft.Json.Serialization.JsonProperty property, global::System.Reflection.MemberInfo member, bool allowNonPublicAccess)
		{
			global::System.Reflection.MemberInfo memberInfo = member.DeclaringType.GetProperty(member.Name + "Specified");
			if (memberInfo == null)
			{
				memberInfo = member.DeclaringType.GetField(member.Name + "Specified");
			}
			if (memberInfo != null && global::Newtonsoft.Json.Utilities.ReflectionUtils.GetMemberUnderlyingType(memberInfo) == typeof(bool))
			{
				global::System.Func<object, object> specifiedPropertyGet = global::Newtonsoft.Json.Serialization.JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(memberInfo);
				property.GetIsSpecified = (object o) => (bool)specifiedPropertyGet(o);
				if (global::Newtonsoft.Json.Utilities.ReflectionUtils.CanSetMemberValue(memberInfo, allowNonPublicAccess, false))
				{
					property.SetIsSpecified = global::Newtonsoft.Json.Serialization.JsonTypeReflector.ReflectionDelegateFactory.CreateSet<object>(memberInfo);
				}
			}
		}

		protected internal virtual string ResolvePropertyName(string propertyName)
		{
			return propertyName;
		}
	}
}
