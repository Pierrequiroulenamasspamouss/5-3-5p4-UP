namespace Newtonsoft.Json.Utilities
{
	internal abstract class ReflectionDelegateFactory
	{
		public global::System.Func<T, object> CreateGet<T>(global::System.Reflection.MemberInfo memberInfo)
		{
			global::System.Reflection.PropertyInfo propertyInfo = memberInfo as global::System.Reflection.PropertyInfo;
			if (propertyInfo != null)
			{
				return CreateGet<T>(propertyInfo);
			}
			global::System.Reflection.FieldInfo fieldInfo = memberInfo as global::System.Reflection.FieldInfo;
			if (fieldInfo != null)
			{
				return CreateGet<T>(fieldInfo);
			}
			throw new global::System.Exception("Could not create getter for {0}.".FormatWith(global::System.Globalization.CultureInfo.InvariantCulture, memberInfo));
		}

		public global::System.Action<T, object> CreateSet<T>(global::System.Reflection.MemberInfo memberInfo)
		{
			global::System.Reflection.PropertyInfo propertyInfo = memberInfo as global::System.Reflection.PropertyInfo;
			if (propertyInfo != null)
			{
				return CreateSet<T>(propertyInfo);
			}
			global::System.Reflection.FieldInfo fieldInfo = memberInfo as global::System.Reflection.FieldInfo;
			if (fieldInfo != null)
			{
				return CreateSet<T>(fieldInfo);
			}
			throw new global::System.Exception("Could not create setter for {0}.".FormatWith(global::System.Globalization.CultureInfo.InvariantCulture, memberInfo));
		}

		public abstract global::Newtonsoft.Json.Utilities.MethodCall<T, object> CreateMethodCall<T>(global::System.Reflection.MethodBase method);

		public abstract global::System.Func<T> CreateDefaultConstructor<T>(global::System.Type type);

		public abstract global::System.Func<T, object> CreateGet<T>(global::System.Reflection.PropertyInfo propertyInfo);

		public abstract global::System.Func<T, object> CreateGet<T>(global::System.Reflection.FieldInfo fieldInfo);

		public abstract global::System.Action<T, object> CreateSet<T>(global::System.Reflection.FieldInfo fieldInfo);

		public abstract global::System.Action<T, object> CreateSet<T>(global::System.Reflection.PropertyInfo propertyInfo);
	}
}
