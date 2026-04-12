namespace Newtonsoft.Json.Serialization
{
	internal static class JsonTypeReflector
	{
		public const string IdPropertyName = "$id";

		public const string RefPropertyName = "$ref";

		public const string TypePropertyName = "$type";

		public const string ValuePropertyName = "$value";

		public const string ArrayValuesPropertyName = "$values";

		public const string ShouldSerializePrefix = "ShouldSerialize";

		public const string SpecifiedPostfix = "Specified";

		private const string MetadataTypeAttributeTypeName = "System.ComponentModel.DataAnnotations.MetadataTypeAttribute, System.ComponentModel.DataAnnotations, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35";

		private static readonly global::Newtonsoft.Json.Utilities.ThreadSafeStore<global::System.Reflection.ICustomAttributeProvider, global::System.Type> JsonConverterTypeCache = new global::Newtonsoft.Json.Utilities.ThreadSafeStore<global::System.Reflection.ICustomAttributeProvider, global::System.Type>(GetJsonConverterTypeFromAttribute);

		private static readonly global::Newtonsoft.Json.Utilities.ThreadSafeStore<global::System.Type, global::System.Type> AssociatedMetadataTypesCache = new global::Newtonsoft.Json.Utilities.ThreadSafeStore<global::System.Type, global::System.Type>(GetAssociateMetadataTypeFromAttribute);

		private static global::System.Type _cachedMetadataTypeAttributeType;

		private static bool? _dynamicCodeGeneration;

		public static bool DynamicCodeGeneration
		{
			get
			{
				if (!_dynamicCodeGeneration.HasValue)
				{
					_dynamicCodeGeneration = false;
				}
				return _dynamicCodeGeneration.Value;
			}
		}

		public static global::Newtonsoft.Json.Utilities.ReflectionDelegateFactory ReflectionDelegateFactory
		{
			get
			{
				return global::Newtonsoft.Json.Utilities.LateBoundReflectionDelegateFactory.Instance;
			}
		}

		public static global::Newtonsoft.Json.JsonContainerAttribute GetJsonContainerAttribute(global::System.Type type)
		{
			return global::Newtonsoft.Json.Serialization.CachedAttributeGetter<global::Newtonsoft.Json.JsonContainerAttribute>.GetAttribute(type);
		}

		public static global::Newtonsoft.Json.JsonObjectAttribute GetJsonObjectAttribute(global::System.Type type)
		{
			return GetJsonContainerAttribute(type) as global::Newtonsoft.Json.JsonObjectAttribute;
		}

		public static global::Newtonsoft.Json.JsonArrayAttribute GetJsonArrayAttribute(global::System.Type type)
		{
			return GetJsonContainerAttribute(type) as global::Newtonsoft.Json.JsonArrayAttribute;
		}

		public static global::System.Runtime.Serialization.DataContractAttribute GetDataContractAttribute(global::System.Type type)
		{
			global::System.Runtime.Serialization.DataContractAttribute dataContractAttribute = null;
			global::System.Type type2 = type;
			while (dataContractAttribute == null && type2 != null)
			{
				dataContractAttribute = global::Newtonsoft.Json.Serialization.CachedAttributeGetter<global::System.Runtime.Serialization.DataContractAttribute>.GetAttribute(type2);
				type2 = type2.BaseType;
			}
			return dataContractAttribute;
		}

		public static global::System.Runtime.Serialization.DataMemberAttribute GetDataMemberAttribute(global::System.Reflection.MemberInfo memberInfo)
		{
			if (memberInfo.MemberType == global::System.Reflection.MemberTypes.Field)
			{
				return global::Newtonsoft.Json.Serialization.CachedAttributeGetter<global::System.Runtime.Serialization.DataMemberAttribute>.GetAttribute(memberInfo);
			}
			global::System.Reflection.PropertyInfo propertyInfo = (global::System.Reflection.PropertyInfo)memberInfo;
			global::System.Runtime.Serialization.DataMemberAttribute attribute = global::Newtonsoft.Json.Serialization.CachedAttributeGetter<global::System.Runtime.Serialization.DataMemberAttribute>.GetAttribute(propertyInfo);
			if (attribute == null && global::Newtonsoft.Json.Utilities.ReflectionUtils.IsVirtual(propertyInfo))
			{
				global::System.Type type = propertyInfo.DeclaringType;
				while (attribute == null && type != null)
				{
					global::System.Reflection.PropertyInfo propertyInfo2 = (global::System.Reflection.PropertyInfo)global::Newtonsoft.Json.Utilities.ReflectionUtils.GetMemberInfoFromType(type, propertyInfo);
					if (propertyInfo2 != null && global::Newtonsoft.Json.Utilities.ReflectionUtils.IsVirtual(propertyInfo2))
					{
						attribute = global::Newtonsoft.Json.Serialization.CachedAttributeGetter<global::System.Runtime.Serialization.DataMemberAttribute>.GetAttribute(propertyInfo2);
					}
					type = type.BaseType;
				}
			}
			return attribute;
		}

		public static global::Newtonsoft.Json.MemberSerialization GetObjectMemberSerialization(global::System.Type objectType)
		{
			global::Newtonsoft.Json.JsonObjectAttribute jsonObjectAttribute = GetJsonObjectAttribute(objectType);
			if (jsonObjectAttribute == null)
			{
				global::System.Runtime.Serialization.DataContractAttribute dataContractAttribute = GetDataContractAttribute(objectType);
				if (dataContractAttribute != null)
				{
					return global::Newtonsoft.Json.MemberSerialization.OptIn;
				}
				return global::Newtonsoft.Json.MemberSerialization.OptOut;
			}
			return jsonObjectAttribute.MemberSerialization;
		}

		private static global::System.Type GetJsonConverterType(global::System.Reflection.ICustomAttributeProvider attributeProvider)
		{
			return JsonConverterTypeCache.Get(attributeProvider);
		}

		private static global::System.Type GetJsonConverterTypeFromAttribute(global::System.Reflection.ICustomAttributeProvider attributeProvider)
		{
			global::Newtonsoft.Json.JsonConverterAttribute attribute = GetAttribute<global::Newtonsoft.Json.JsonConverterAttribute>(attributeProvider);
			if (attribute == null)
			{
				return null;
			}
			return attribute.ConverterType;
		}

		public static global::Newtonsoft.Json.JsonConverter GetJsonConverter(global::System.Reflection.ICustomAttributeProvider attributeProvider, global::System.Type targetConvertedType)
		{
			global::System.Type jsonConverterType = GetJsonConverterType(attributeProvider);
			if (jsonConverterType != null)
			{
				global::Newtonsoft.Json.JsonConverter jsonConverter = global::Newtonsoft.Json.JsonConverterAttribute.CreateJsonConverterInstance(jsonConverterType);
				if (!jsonConverter.CanConvert(targetConvertedType))
				{
					throw new global::Newtonsoft.Json.JsonSerializationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("JsonConverter {0} on {1} is not compatible with member type {2}.", global::System.Globalization.CultureInfo.InvariantCulture, jsonConverter.GetType().Name, attributeProvider, targetConvertedType.Name));
				}
				return jsonConverter;
			}
			return null;
		}

		public static global::System.ComponentModel.TypeConverter GetTypeConverter(global::System.Type type)
		{
			return global::System.ComponentModel.TypeDescriptor.GetConverter(type);
		}

		private static global::System.Type GetAssociatedMetadataType(global::System.Type type)
		{
			return AssociatedMetadataTypesCache.Get(type);
		}

		private static global::System.Type GetAssociateMetadataTypeFromAttribute(global::System.Type type)
		{
			global::System.Type metadataTypeAttributeType = GetMetadataTypeAttributeType();
			if (metadataTypeAttributeType == null)
			{
				return null;
			}
			object obj = global::System.Linq.Enumerable.SingleOrDefault(type.GetCustomAttributes(metadataTypeAttributeType, true));
			if (obj == null)
			{
				return null;
			}
			global::Newtonsoft.Json.Serialization.IMetadataTypeAttribute metadataTypeAttribute = new global::Newtonsoft.Json.Serialization.LateBoundMetadataTypeAttribute(obj);
			return metadataTypeAttribute.MetadataClassType;
		}

		private static global::System.Type GetMetadataTypeAttributeType()
		{
			if (_cachedMetadataTypeAttributeType == null)
			{
				global::System.Type type = global::System.Type.GetType("System.ComponentModel.DataAnnotations.MetadataTypeAttribute, System.ComponentModel.DataAnnotations, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
				if (type == null)
				{
					return null;
				}
				_cachedMetadataTypeAttributeType = type;
			}
			return _cachedMetadataTypeAttributeType;
		}

		private static T GetAttribute<T>(global::System.Type type) where T : global::System.Attribute
		{
			global::System.Type associatedMetadataType = GetAssociatedMetadataType(type);
			T attribute;
			if (associatedMetadataType != null)
			{
				attribute = global::Newtonsoft.Json.Utilities.ReflectionUtils.GetAttribute<T>(associatedMetadataType, true);
				if (attribute != null)
				{
					return attribute;
				}
			}
			attribute = global::Newtonsoft.Json.Utilities.ReflectionUtils.GetAttribute<T>(type, true);
			if (attribute != null)
			{
				return attribute;
			}
			global::System.Type[] interfaces = type.GetInterfaces();
			foreach (global::System.Type attributeProvider in interfaces)
			{
				attribute = global::Newtonsoft.Json.Utilities.ReflectionUtils.GetAttribute<T>(attributeProvider, true);
				if (attribute != null)
				{
					return attribute;
				}
			}
			return null;
		}

		private static T GetAttribute<T>(global::System.Reflection.MemberInfo memberInfo) where T : global::System.Attribute
		{
			global::System.Type associatedMetadataType = GetAssociatedMetadataType(memberInfo.DeclaringType);
			T attribute;
			if (associatedMetadataType != null)
			{
				global::System.Reflection.MemberInfo memberInfoFromType = global::Newtonsoft.Json.Utilities.ReflectionUtils.GetMemberInfoFromType(associatedMetadataType, memberInfo);
				if (memberInfoFromType != null)
				{
					attribute = global::Newtonsoft.Json.Utilities.ReflectionUtils.GetAttribute<T>(memberInfoFromType, true);
					if (attribute != null)
					{
						return attribute;
					}
				}
			}
			attribute = global::Newtonsoft.Json.Utilities.ReflectionUtils.GetAttribute<T>(memberInfo, true);
			if (attribute != null)
			{
				return attribute;
			}
			global::System.Type[] interfaces = memberInfo.DeclaringType.GetInterfaces();
			foreach (global::System.Type targetType in interfaces)
			{
				global::System.Reflection.MemberInfo memberInfoFromType2 = global::Newtonsoft.Json.Utilities.ReflectionUtils.GetMemberInfoFromType(targetType, memberInfo);
				if (memberInfoFromType2 != null)
				{
					attribute = global::Newtonsoft.Json.Utilities.ReflectionUtils.GetAttribute<T>(memberInfoFromType2, true);
					if (attribute != null)
					{
						return attribute;
					}
				}
			}
			return null;
		}

		public static T GetAttribute<T>(global::System.Reflection.ICustomAttributeProvider attributeProvider) where T : global::System.Attribute
		{
			global::System.Type type = attributeProvider as global::System.Type;
			if (type != null)
			{
				return GetAttribute<T>(type);
			}
			global::System.Reflection.MemberInfo memberInfo = attributeProvider as global::System.Reflection.MemberInfo;
			if (memberInfo != null)
			{
				return GetAttribute<T>(memberInfo);
			}
			return global::Newtonsoft.Json.Utilities.ReflectionUtils.GetAttribute<T>(attributeProvider, true);
		}
	}
}
