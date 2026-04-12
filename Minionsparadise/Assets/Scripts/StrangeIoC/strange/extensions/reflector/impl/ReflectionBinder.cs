namespace strange.extensions.reflector.impl
{
	public class ReflectionBinder : global::strange.framework.impl.Binder, global::strange.extensions.reflector.api.IReflectionBinder
	{
		public static global::strange.extensions.reflector.impl.ReflectionBinder Instance = new global::strange.extensions.reflector.impl.ReflectionBinder();

		private ReflectionBinder()
		{
		}

		private bool SupportsJIT()
		{
			return false;
		}

		public global::strange.extensions.reflector.api.IReflectedClass Get<T>()
		{
			return Get(typeof(T));
		}

		public global::strange.extensions.reflector.api.IReflectedClass Get(global::System.Type type)
		{
			global::strange.framework.api.IBinding binding = GetBinding(type);
			global::strange.extensions.reflector.api.IReflectedClass reflectedClass3;
			if (binding == null)
			{
				binding = GetRawBinding();
				global::strange.extensions.reflector.api.IReflectedClass reflectedClass = global::strange.extensions.reflector.api.PreReflectedTypesDatabase.Instance.GetClass(type);
				if (reflectedClass == null)
				{
					global::strange.extensions.reflector.impl.ReflectedClass reflectedClass2 = new global::strange.extensions.reflector.impl.ReflectedClass();
					mapPreferredConstructor(reflectedClass2, binding, type);
					mapPostConstructors(reflectedClass2, binding, type);
					mapSetters(reflectedClass2, binding, type);
					reflectedClass = reflectedClass2;
				}
				binding.Bind(type).To(reflectedClass);
				reflectedClass3 = binding.value as global::strange.extensions.reflector.api.IReflectedClass;
				reflectedClass3.PreGenerated = false;
			}
			else
			{
				reflectedClass3 = binding.value as global::strange.extensions.reflector.api.IReflectedClass;
				reflectedClass3.PreGenerated = true;
			}
			return reflectedClass3;
		}

		public override global::strange.framework.api.IBinding GetRawBinding()
		{
			global::strange.framework.api.IBinding rawBinding = base.GetRawBinding();
			rawBinding.valueConstraint = global::strange.framework.api.BindingConstraintType.ONE;
			return rawBinding;
		}

		private void mapPreferredConstructor(global::strange.extensions.reflector.impl.ReflectedClass reflected, global::strange.framework.api.IBinding binding, global::System.Type type)
		{
			global::System.Reflection.ConstructorInfo constructor = findPreferredConstructor(type);
			if (constructor == null)
			{
				throw new global::strange.extensions.reflector.impl.ReflectionException(string.Concat("The reflector requires concrete classes.\nType ", type, " has no constructor. Is it an interface?"), global::strange.extensions.reflector.api.ReflectionExceptionType.CANNOT_REFLECT_INTERFACE);
			}
			global::System.Reflection.ParameterInfo[] parameters = constructor.GetParameters();
			global::System.Type[] array = new global::System.Type[parameters.Length];
			int num = 0;
			global::System.Reflection.ParameterInfo[] array2 = parameters;
			foreach (global::System.Reflection.ParameterInfo parameterInfo in array2)
			{
				global::System.Type parameterType = parameterInfo.ParameterType;
				array[num] = parameterType;
				num++;
			}
			reflected.Constructor = (object[] p) => constructor.Invoke(p);
			reflected.ConstructorParameters = array;
		}

		private global::System.Reflection.ConstructorInfo findPreferredConstructor(global::System.Type type)
		{
			global::System.Reflection.ConstructorInfo[] constructors = type.GetConstructors(global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Public | global::System.Reflection.BindingFlags.FlattenHierarchy | global::System.Reflection.BindingFlags.InvokeMethod);
			if (constructors.Length == 1)
			{
				return constructors[0];
			}
			int num = int.MaxValue;
			global::System.Reflection.ConstructorInfo result = null;
			global::System.Reflection.ConstructorInfo[] array = constructors;
			foreach (global::System.Reflection.ConstructorInfo constructorInfo in array)
			{
				object[] customAttributes = constructorInfo.GetCustomAttributes(typeof(Construct), true);
				if (customAttributes.Length > 0)
				{
					return constructorInfo;
				}
				int num2 = constructorInfo.GetParameters().Length;
				if (num2 < num)
				{
					num = num2;
					result = constructorInfo;
				}
			}
			return result;
		}

		private void mapPostConstructors(global::strange.extensions.reflector.impl.ReflectedClass reflected, global::strange.framework.api.IBinding binding, global::System.Type type)
		{
			global::System.Reflection.MethodInfo[] methods = type.GetMethods(global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Public | global::System.Reflection.BindingFlags.FlattenHierarchy | global::System.Reflection.BindingFlags.InvokeMethod);
			global::System.Collections.Generic.List<global::System.Reflection.MethodInfo> list = new global::System.Collections.Generic.List<global::System.Reflection.MethodInfo>(methods.Length);
			global::System.Reflection.MethodInfo[] array = methods;
			foreach (global::System.Reflection.MethodInfo methodInfo in array)
			{
				object[] customAttributes = methodInfo.GetCustomAttributes(typeof(PostConstruct), true);
				if (customAttributes.Length > 0)
				{
					list.Add(methodInfo);
				}
			}
			list.Sort(new global::strange.extensions.reflector.impl.PriorityComparer());
			global::System.Action<object>[] array2 = new global::System.Action<object>[list.Count];
			for (int j = 0; j < list.Count; j++)
			{
				global::System.Reflection.MethodInfo mi = list[j];
				if (SupportsJIT())
				{
					array2[j] = global::strange.extensions.reflector.impl.FastRreflection.CreatePostConstructDelegate(type, mi);
					continue;
				}
				array2[j] = delegate(object target)
				{
					mi.Invoke(target, null);
				};
			}
			reflected.PostConstructors = array2;
		}

		private void mapSetters(global::strange.extensions.reflector.impl.ReflectedClass reflected, global::strange.framework.api.IBinding binding, global::System.Type type)
		{
			global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<global::System.Type, global::System.Action<object, object>>> list = new global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<global::System.Type, global::System.Action<object, object>>>();
			global::System.Collections.Generic.List<object> list2 = new global::System.Collections.Generic.List<object>();
			global::System.Collections.Generic.List<bool> list3 = new global::System.Collections.Generic.List<bool>();
			global::System.Reflection.MemberInfo[] array = type.FindMembers(global::System.Reflection.MemberTypes.Property, global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.NonPublic | global::System.Reflection.BindingFlags.FlattenHierarchy | global::System.Reflection.BindingFlags.SetProperty, null, null);
			global::System.Reflection.MemberInfo[] array2 = array;
			foreach (global::System.Reflection.MemberInfo memberInfo in array2)
			{
				object[] customAttributes = memberInfo.GetCustomAttributes(typeof(Inject), true);
				if (customAttributes.Length > 0)
				{
					throw new global::strange.extensions.reflector.impl.ReflectionException("The class " + type.Name + " has a non-public Injection setter " + memberInfo.Name + ". Make the setter public to allow injection.", global::strange.extensions.reflector.api.ReflectionExceptionType.CANNOT_INJECT_INTO_NONPUBLIC_SETTER);
				}
			}
			global::System.Reflection.MemberInfo[] array3 = type.FindMembers(global::System.Reflection.MemberTypes.Property, global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Public | global::System.Reflection.BindingFlags.FlattenHierarchy | global::System.Reflection.BindingFlags.SetProperty, null, null);
			list.Capacity = array3.Length;
			list2.Capacity = array3.Length;
			list3.Capacity = array3.Length;
			global::System.Reflection.MemberInfo[] array4 = array3;
			foreach (global::System.Reflection.MemberInfo memberInfo2 in array4)
			{
				object[] customAttributes2 = memberInfo2.GetCustomAttributes(typeof(Inject), true);
				if (customAttributes2 == null || customAttributes2.Length <= 0)
				{
					continue;
				}
				Inject inject = customAttributes2[0] as Inject;
				global::System.Reflection.PropertyInfo propertyInfo = memberInfo2 as global::System.Reflection.PropertyInfo;
				global::System.Reflection.MethodInfo setter = propertyInfo.GetSetMethod();
				global::System.Action<object, object> value;
				if (SupportsJIT())
				{
					value = global::strange.extensions.reflector.impl.FastRreflection.CreateSetterDelegate(type, setter);
				}
				else
				{
					object[] arr = new object[1];
					value = delegate(object target, object obj)
					{
						arr[0] = obj;
						setter.Invoke(target, arr);
					};
				}
				global::System.Type propertyType = propertyInfo.PropertyType;
				global::System.Collections.Generic.KeyValuePair<global::System.Type, global::System.Action<object, object>> item = new global::System.Collections.Generic.KeyValuePair<global::System.Type, global::System.Action<object, object>>(propertyType, value);
				list.Add(item);
				object name = inject.name;
				list2.Add(name);
				list3.Add(inject.optional);
			}
			reflected.Setters = list.ToArray();
			reflected.SetterNames = list2.ToArray();
			reflected.SetterOptional = list3.ToArray();
		}
	}
}
