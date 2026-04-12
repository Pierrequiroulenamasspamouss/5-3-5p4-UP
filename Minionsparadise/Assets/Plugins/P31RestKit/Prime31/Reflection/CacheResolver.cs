namespace Prime31.Reflection
{
	public class CacheResolver
	{
		private delegate object CtorDelegate();

		public sealed class MemberMap
		{
			public readonly global::System.Reflection.MemberInfo MemberInfo;

			public readonly global::System.Type Type;

			public readonly global::Prime31.Reflection.GetHandler Getter;

			public readonly global::Prime31.Reflection.SetHandler Setter;

			public MemberMap(global::System.Reflection.PropertyInfo propertyInfo)
			{
				MemberInfo = propertyInfo;
				Type = propertyInfo.PropertyType;
				Getter = createGetHandler(propertyInfo);
				Setter = createSetHandler(propertyInfo);
			}

			public MemberMap(global::System.Reflection.FieldInfo fieldInfo)
			{
				MemberInfo = fieldInfo;
				Type = fieldInfo.FieldType;
				Getter = createGetHandler(fieldInfo);
				Setter = createSetHandler(fieldInfo);
			}
		}

		private readonly global::Prime31.Reflection.MemberMapLoader _memberMapLoader;

		private readonly global::Prime31.Reflection.SafeDictionary<global::System.Type, global::Prime31.Reflection.SafeDictionary<string, global::Prime31.Reflection.CacheResolver.MemberMap>> _memberMapsCache = new global::Prime31.Reflection.SafeDictionary<global::System.Type, global::Prime31.Reflection.SafeDictionary<string, global::Prime31.Reflection.CacheResolver.MemberMap>>();

		private static readonly global::Prime31.Reflection.SafeDictionary<global::System.Type, global::Prime31.Reflection.CacheResolver.CtorDelegate> constructorCache = new global::Prime31.Reflection.SafeDictionary<global::System.Type, global::Prime31.Reflection.CacheResolver.CtorDelegate>();

		public CacheResolver(global::Prime31.Reflection.MemberMapLoader memberMapLoader)
		{
			_memberMapLoader = memberMapLoader;
		}

		public static object getNewInstance(global::System.Type type)
		{
			global::Prime31.Reflection.CacheResolver.CtorDelegate value;
			if (constructorCache.tryGetValue(type, out value))
			{
				return value();
			}
			global::System.Reflection.ConstructorInfo constructorInfo = type.GetConstructor(global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Public | global::System.Reflection.BindingFlags.NonPublic, null, global::System.Type.EmptyTypes, null);
			value = () => constructorInfo.Invoke(null);
			constructorCache.add(type, value);
			return value();
		}

		public global::Prime31.Reflection.SafeDictionary<string, global::Prime31.Reflection.CacheResolver.MemberMap> loadMaps(global::System.Type type)
		{
			if (type == null || type == typeof(object))
			{
				return null;
			}
			global::Prime31.Reflection.SafeDictionary<string, global::Prime31.Reflection.CacheResolver.MemberMap> value;
			if (_memberMapsCache.tryGetValue(type, out value))
			{
				return value;
			}
			value = new global::Prime31.Reflection.SafeDictionary<string, global::Prime31.Reflection.CacheResolver.MemberMap>();
			_memberMapLoader(type, value);
			_memberMapsCache.add(type, value);
			return value;
		}

		private static global::Prime31.Reflection.GetHandler createGetHandler(global::System.Reflection.FieldInfo fieldInfo)
		{
			return (object instance) => fieldInfo.GetValue(instance);
		}

		private static global::Prime31.Reflection.SetHandler createSetHandler(global::System.Reflection.FieldInfo fieldInfo)
		{
			if (fieldInfo.IsInitOnly || fieldInfo.IsLiteral)
			{
				return null;
			}
			return delegate(object instance, object value)
			{
				fieldInfo.SetValue(instance, value);
			};
		}

		private static global::Prime31.Reflection.GetHandler createGetHandler(global::System.Reflection.PropertyInfo propertyInfo)
		{
			global::System.Reflection.MethodInfo getMethodInfo = propertyInfo.GetGetMethod(true);
			if (getMethodInfo == null)
			{
				return null;
			}
			return (object instance) => getMethodInfo.Invoke(instance, global::System.Type.EmptyTypes);
		}

		private static global::Prime31.Reflection.SetHandler createSetHandler(global::System.Reflection.PropertyInfo propertyInfo)
		{
			global::System.Reflection.MethodInfo setMethodInfo = propertyInfo.GetSetMethod(true);
			if (setMethodInfo == null)
			{
				return null;
			}
			return delegate(object instance, object value)
			{
				setMethodInfo.Invoke(instance, new object[1] { value });
			};
		}
	}
}
