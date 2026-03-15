namespace Newtonsoft.Json.Serialization
{
	public class JsonDictionaryContract : global::Newtonsoft.Json.Serialization.JsonContract
	{
		private readonly bool _isDictionaryValueTypeNullableType;

		private readonly global::System.Type _genericCollectionDefinitionType;

		private global::System.Type _genericWrapperType;

		private global::Newtonsoft.Json.Utilities.MethodCall<object, object> _genericWrapperCreator;

		public global::System.Func<string, string> PropertyNameResolver { get; set; }

		internal global::System.Type DictionaryKeyType { get; private set; }

		internal global::System.Type DictionaryValueType { get; private set; }

		public JsonDictionaryContract(global::System.Type underlyingType)
			: base(underlyingType)
		{
			global::System.Type keyType;
			global::System.Type valueType;
			if (global::Newtonsoft.Json.Utilities.ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof(global::System.Collections.Generic.IDictionary<, >), out _genericCollectionDefinitionType))
			{
				keyType = _genericCollectionDefinitionType.GetGenericArguments()[0];
				valueType = _genericCollectionDefinitionType.GetGenericArguments()[1];
			}
			else
			{
				global::Newtonsoft.Json.Utilities.ReflectionUtils.GetDictionaryKeyValueTypes(base.UnderlyingType, out keyType, out valueType);
			}
			DictionaryKeyType = keyType;
			DictionaryValueType = valueType;
			if (DictionaryValueType != null)
			{
				_isDictionaryValueTypeNullableType = global::Newtonsoft.Json.Utilities.ReflectionUtils.IsNullableType(DictionaryValueType);
			}
			if (IsTypeGenericDictionaryInterface(base.UnderlyingType))
			{
				base.CreatedType = global::Newtonsoft.Json.Utilities.ReflectionUtils.MakeGenericType(typeof(global::System.Collections.Generic.Dictionary<, >), keyType, valueType);
			}
			else if (base.UnderlyingType == typeof(global::System.Collections.IDictionary))
			{
				base.CreatedType = typeof(global::System.Collections.Generic.Dictionary<object, object>);
			}
		}

		internal global::Newtonsoft.Json.Utilities.IWrappedDictionary CreateWrapper(object dictionary)
		{
			if (dictionary is global::System.Collections.IDictionary && (DictionaryValueType == null || !_isDictionaryValueTypeNullableType))
			{
				return new global::Newtonsoft.Json.Utilities.DictionaryWrapper<object, object>((global::System.Collections.IDictionary)dictionary);
			}
			if (_genericWrapperType == null)
			{
				_genericWrapperType = global::Newtonsoft.Json.Utilities.ReflectionUtils.MakeGenericType(typeof(global::Newtonsoft.Json.Utilities.DictionaryWrapper<, >), DictionaryKeyType, DictionaryValueType);
				global::System.Reflection.ConstructorInfo constructor = _genericWrapperType.GetConstructor(new global::System.Type[1] { _genericCollectionDefinitionType });
				_genericWrapperCreator = global::Newtonsoft.Json.Serialization.JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(constructor);
			}
			return (global::Newtonsoft.Json.Utilities.IWrappedDictionary)_genericWrapperCreator(null, dictionary);
		}

		private bool IsTypeGenericDictionaryInterface(global::System.Type type)
		{
			if (!type.IsGenericType)
			{
				return false;
			}
			global::System.Type genericTypeDefinition = type.GetGenericTypeDefinition();
			return genericTypeDefinition == typeof(global::System.Collections.Generic.IDictionary<, >);
		}
	}
}
