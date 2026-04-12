namespace Newtonsoft.Json.Utilities
{
	internal class LateBoundReflectionDelegateFactory : global::Newtonsoft.Json.Utilities.ReflectionDelegateFactory
	{
		private static readonly global::Newtonsoft.Json.Utilities.LateBoundReflectionDelegateFactory _instance = new global::Newtonsoft.Json.Utilities.LateBoundReflectionDelegateFactory();

		internal static global::Newtonsoft.Json.Utilities.ReflectionDelegateFactory Instance
		{
			get
			{
				return _instance;
			}
		}

		public override global::Newtonsoft.Json.Utilities.MethodCall<T, object> CreateMethodCall<T>(global::System.Reflection.MethodBase method)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(method, "method");
			global::System.Reflection.ConstructorInfo c = method as global::System.Reflection.ConstructorInfo;
			if (c != null)
			{
				return (T o, object[] a) => c.Invoke(a);
			}
			return (T o, object[] a) => method.Invoke(o, a);
		}

		public override global::System.Func<T> CreateDefaultConstructor<T>(global::System.Type type)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(type, "type");
			if (type.IsValueType)
			{
				return () => (T)global::Newtonsoft.Json.Utilities.ReflectionUtils.CreateInstance(type);
			}
			global::System.Reflection.ConstructorInfo constructorInfo = global::Newtonsoft.Json.Utilities.ReflectionUtils.GetDefaultConstructor(type, true);
			return () => (T)constructorInfo.Invoke(null);
		}

		public override global::System.Func<T, object> CreateGet<T>(global::System.Reflection.PropertyInfo propertyInfo)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
			return (T o) => propertyInfo.GetValue(o, null);
		}

		public override global::System.Func<T, object> CreateGet<T>(global::System.Reflection.FieldInfo fieldInfo)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(fieldInfo, "fieldInfo");
			return (T o) => fieldInfo.GetValue(o);
		}

		public override global::System.Action<T, object> CreateSet<T>(global::System.Reflection.FieldInfo fieldInfo)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(fieldInfo, "fieldInfo");
			return delegate(T o, object v)
			{
				fieldInfo.SetValue(o, v);
			};
		}

		public override global::System.Action<T, object> CreateSet<T>(global::System.Reflection.PropertyInfo propertyInfo)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
			return delegate(T o, object v)
			{
				propertyInfo.SetValue(o, v, null);
			};
		}
	}
}
