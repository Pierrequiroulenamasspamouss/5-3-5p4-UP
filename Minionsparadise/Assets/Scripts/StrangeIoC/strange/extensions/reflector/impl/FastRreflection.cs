namespace strange.extensions.reflector.impl
{
	internal static class FastRreflection
	{
		public static global::System.Action<object, object> CreateSetterDelegate(global::System.Type t, global::System.Reflection.MethodInfo method)
		{
			global::System.Reflection.MethodInfo method2 = typeof(global::strange.extensions.reflector.impl.FastRreflection).GetMethod("SetterHelper", global::System.Reflection.BindingFlags.Static | global::System.Reflection.BindingFlags.NonPublic);
			global::System.Reflection.MethodInfo methodInfo = method2.MakeGenericMethod(t, method.GetParameters()[0].ParameterType);
			object obj = methodInfo.Invoke(null, new object[1] { method });
			return (global::System.Action<object, object>)obj;
		}

		private static global::System.Action<object, object> SetterHelper<TTarget, TParam>(global::System.Reflection.MethodInfo method) where TTarget : class
		{
			global::System.Action<TTarget, TParam> func = (global::System.Action<TTarget, TParam>)global::System.Delegate.CreateDelegate(typeof(global::System.Action<TTarget, TParam>), method);
			return delegate(object _target, object _param)
			{
				TTarget arg = (TTarget)_target;
				TParam arg2 = (TParam)_param;
				func(arg, arg2);
			};
		}

		public static global::System.Action<object> CreatePostConstructDelegate(global::System.Type t, global::System.Reflection.MethodInfo method)
		{
			global::System.Reflection.MethodInfo method2 = typeof(global::strange.extensions.reflector.impl.FastRreflection).GetMethod("PostConstructHelper", global::System.Reflection.BindingFlags.Static | global::System.Reflection.BindingFlags.NonPublic);
			global::System.Reflection.MethodInfo methodInfo = method2.MakeGenericMethod(t);
			object obj = methodInfo.Invoke(null, new object[1] { method });
			return (global::System.Action<object>)obj;
		}

		private static global::System.Action<object> PostConstructHelper<TTarget>(global::System.Reflection.MethodInfo method) where TTarget : class
		{
			global::System.Action<TTarget> func = (global::System.Action<TTarget>)global::System.Delegate.CreateDelegate(typeof(global::System.Action<TTarget>), method);
			return delegate(object _target)
			{
				TTarget obj = (TTarget)_target;
				func(obj);
			};
		}
	}
}
