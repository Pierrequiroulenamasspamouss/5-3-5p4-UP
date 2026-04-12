namespace strange.extensions.injector.impl
{
	public class InjectorFactory : global::strange.extensions.injector.api.IInjectorFactory
	{
		public object Get(global::strange.extensions.injector.api.IInjector injector, global::strange.extensions.injector.api.IInjectionBinding binding)
		{
			if (binding == null)
			{
				throw new global::strange.extensions.injector.impl.InjectionException("InjectorFactory cannot act on null binding", global::strange.extensions.injector.api.InjectionExceptionType.NULL_BINDING);
			}
			switch (binding.type)
			{
			case global::strange.extensions.injector.api.InjectionBindingType.SINGLETON:
				return singletonOf(injector, binding);
			case global::strange.extensions.injector.api.InjectionBindingType.VALUE:
				return valueOf(binding);
			default:
				return instanceOf(injector, binding);
			}
		}

		protected object singletonOf(global::strange.extensions.injector.api.IInjector injector, global::strange.extensions.injector.api.IInjectionBinding binding)
		{
			if (binding.value != null)
			{
				if (binding.value.GetType().IsInstanceOfType(typeof(global::System.Type)))
				{
					object obj = createFromValue(injector, binding.value);
					if (obj == null)
					{
						return null;
					}
					binding.SetValue(obj);
				}
			}
			else
			{
				binding.SetValue(generateImplicit(injector, (binding.key as object[])[0]));
			}
			return binding.value;
		}

		protected object generateImplicit(global::strange.extensions.injector.api.IInjector injector, object key)
		{
			global::System.Type type = key as global::System.Type;
			if (!type.IsInterface && !type.IsAbstract)
			{
				return createFromValue(injector, key);
			}
			throw new global::strange.extensions.injector.impl.InjectionException("InjectorFactory can't instantiate an Interface or Abstract Class. Class: " + key.ToString(), global::strange.extensions.injector.api.InjectionExceptionType.NOT_INSTANTIABLE);
		}

		protected object valueOf(global::strange.extensions.injector.api.IInjectionBinding binding)
		{
			return binding.value;
		}

		protected object instanceOf(global::strange.extensions.injector.api.IInjector injector, global::strange.extensions.injector.api.IInjectionBinding binding)
		{
			if (binding.value != null)
			{
				return createFromValue(injector, binding.value);
			}
			object o = generateImplicit(injector, (binding.key as object[])[0]);
			return createFromValue(injector, o);
		}

		protected object createFromValue(global::strange.extensions.injector.api.IInjector injector, object o)
		{
			global::System.Type type = ((o is global::System.Type) ? (o as global::System.Type) : o.GetType());
			object result = null;
			try
			{
				global::strange.extensions.reflector.api.IReflectedClass reflectedClass = injector.reflector.Get(type);
				result = reflectedClass.CreateInstance(injector);
			}
			catch (global::System.Exception ex)
			{
				global::UnityEngine.Debug.LogError(ex.ToString());
			}
			return result;
		}
	}
}
