namespace Newtonsoft.Json.Serialization
{
	public class JsonArrayContract : global::Newtonsoft.Json.Serialization.JsonContract
	{
		private readonly bool _isCollectionItemTypeNullableType;

		private readonly global::System.Type _genericCollectionDefinitionType;

		private global::System.Type _genericWrapperType;

		private global::Newtonsoft.Json.Utilities.MethodCall<object, object> _genericWrapperCreator;

		internal global::System.Type CollectionItemType { get; private set; }

		public JsonArrayContract(global::System.Type underlyingType)
			: base(underlyingType)
		{
			if (global::Newtonsoft.Json.Utilities.ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof(global::System.Collections.Generic.ICollection<>), out _genericCollectionDefinitionType))
			{
				CollectionItemType = _genericCollectionDefinitionType.GetGenericArguments()[0];
			}
			else if (underlyingType.IsGenericType && underlyingType.GetGenericTypeDefinition() == typeof(global::System.Collections.Generic.IEnumerable<>))
			{
				_genericCollectionDefinitionType = typeof(global::System.Collections.Generic.IEnumerable<>);
				CollectionItemType = underlyingType.GetGenericArguments()[0];
			}
			else
			{
				CollectionItemType = global::Newtonsoft.Json.Utilities.ReflectionUtils.GetCollectionItemType(base.UnderlyingType);
			}
			if (CollectionItemType != null)
			{
				_isCollectionItemTypeNullableType = global::Newtonsoft.Json.Utilities.ReflectionUtils.IsNullableType(CollectionItemType);
			}
			if (IsTypeGenericCollectionInterface(base.UnderlyingType))
			{
				if (typeof(global::System.Runtime.Serialization.ISerializable).IsAssignableFrom(underlyingType))
				{
					base.CreatedType = global::Newtonsoft.Json.Utilities.ReflectionUtils.MakeGenericType(typeof(global::System.Collections.Generic.HashSet<>), CollectionItemType);
				}
				else
				{
					base.CreatedType = global::Newtonsoft.Json.Utilities.ReflectionUtils.MakeGenericType(typeof(global::System.Collections.Generic.List<>), CollectionItemType);
				}
			}
			else if (typeof(global::System.Collections.Generic.HashSet<>).IsAssignableFrom(base.UnderlyingType))
			{
				base.CreatedType = global::Newtonsoft.Json.Utilities.ReflectionUtils.MakeGenericType(typeof(global::System.Collections.Generic.HashSet<>), CollectionItemType);
			}
		}

		internal global::Newtonsoft.Json.Utilities.IWrappedCollection CreateWrapper(object list)
		{
			if ((list is global::System.Collections.IList && (CollectionItemType == null || !_isCollectionItemTypeNullableType)) || base.UnderlyingType.IsArray)
			{
				return new global::Newtonsoft.Json.Utilities.CollectionWrapper<object>((global::System.Collections.IList)list);
			}
			if (_genericCollectionDefinitionType != null)
			{
				EnsureGenericWrapperCreator();
				return (global::Newtonsoft.Json.Utilities.IWrappedCollection)_genericWrapperCreator(null, list);
			}
			global::System.Collections.IList list2 = global::System.Linq.Enumerable.ToList(global::System.Linq.Enumerable.Cast<object>((global::System.Collections.IEnumerable)list));
			if (CollectionItemType != null)
			{
				global::System.Array array = global::System.Array.CreateInstance(CollectionItemType, list2.Count);
				for (int i = 0; i < list2.Count; i++)
				{
					array.SetValue(list2[i], i);
				}
				list2 = array;
			}
			return new global::Newtonsoft.Json.Utilities.CollectionWrapper<object>(list2);
		}

		private void EnsureGenericWrapperCreator()
		{
			if (_genericWrapperType == null)
			{
				_genericWrapperType = global::Newtonsoft.Json.Utilities.ReflectionUtils.MakeGenericType(typeof(global::Newtonsoft.Json.Utilities.CollectionWrapper<>), CollectionItemType);
				global::System.Type type = ((!global::Newtonsoft.Json.Utilities.ReflectionUtils.InheritsGenericDefinition(_genericCollectionDefinitionType, typeof(global::System.Collections.Generic.List<>)) && _genericCollectionDefinitionType.GetGenericTypeDefinition() != typeof(global::System.Collections.Generic.IEnumerable<>)) ? _genericCollectionDefinitionType : global::Newtonsoft.Json.Utilities.ReflectionUtils.MakeGenericType(typeof(global::System.Collections.Generic.ICollection<>), CollectionItemType));
				global::System.Reflection.ConstructorInfo constructor = _genericWrapperType.GetConstructor(new global::System.Type[1] { type });
				_genericWrapperCreator = global::Newtonsoft.Json.Serialization.JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(constructor);
			}
		}

		private bool IsTypeGenericCollectionInterface(global::System.Type type)
		{
			if (!type.IsGenericType)
			{
				return false;
			}
			global::System.Type genericTypeDefinition = type.GetGenericTypeDefinition();
			if (genericTypeDefinition != typeof(global::System.Collections.Generic.IList<>) && genericTypeDefinition != typeof(global::System.Collections.Generic.ICollection<>))
			{
				return genericTypeDefinition == typeof(global::System.Collections.Generic.IEnumerable<>);
			}
			return true;
		}
	}
}
