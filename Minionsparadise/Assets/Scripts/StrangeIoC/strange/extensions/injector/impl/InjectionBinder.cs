#pragma warning disable 3005
namespace strange.extensions.injector.impl
{

	public class InjectionBinder : global::strange.framework.impl.Binder, global::strange.extensions.injector.api.IInjectionBinder, global::strange.framework.api.IInstanceProvider
	{
		private global::strange.extensions.injector.api.IInjector _injector;

		public global::strange.extensions.injector.api.IInjector injector
		{
			get
			{
				return _injector;
			}
			set
			{
				if (_injector != null)
				{
					_injector.binder = null;
				}
				_injector = value;
				_injector.binder = this;
			}
		}

		public InjectionBinder()
		{
			injector = new global::strange.extensions.injector.impl.Injector();
			injector.binder = this;
			injector.reflector = global::strange.extensions.reflector.impl.ReflectionBinder.Instance;
		}

		public object GetInstance(global::System.Type key)
		{
			return GetInstance(key, null);
		}

		public virtual object GetInstance(global::System.Type key, object name)
		{
			global::strange.extensions.injector.api.IInjectionBinding binding = GetBinding(key, name);
			if (binding == null)
			{
				throw new global::strange.extensions.injector.impl.InjectionException(string.Concat("InjectionBinder has no binding for:\n\tkey: ", key, "\nname: ", name), global::strange.extensions.injector.api.InjectionExceptionType.NULL_BINDING);
			}
			return GetInjectorForBinding(binding).Instantiate(binding);
		}

		protected virtual global::strange.extensions.injector.api.IInjector GetInjectorForBinding(global::strange.extensions.injector.api.IInjectionBinding binding)
		{
			return injector;
		}

		public T GetInstance<T>()
		{
			object instance = GetInstance(typeof(T));
			return (T)instance;
		}

		public T GetInstance<T>(object name)
		{
			object instance = GetInstance(typeof(T), name);
			return (T)instance;
		}

		public override global::strange.framework.api.IBinding GetRawBinding()
		{
			return new global::strange.extensions.injector.impl.InjectionBinding(resolver);
		}

		public new global::strange.extensions.injector.api.IInjectionBinding Bind<T>()
		{
			return base.Bind<T>() as global::strange.extensions.injector.api.IInjectionBinding;
		}

		public global::strange.extensions.injector.api.IInjectionBinding Bind(global::System.Type key)
		{
			return base.Bind(key) as global::strange.extensions.injector.api.IInjectionBinding;
		}

		public new virtual global::strange.extensions.injector.api.IInjectionBinding GetBinding<T>()
		{
			return base.GetBinding<T>() as global::strange.extensions.injector.api.IInjectionBinding;
		}

		public new virtual global::strange.extensions.injector.api.IInjectionBinding GetBinding<T>(object name)
		{
			return base.GetBinding<T>(name) as global::strange.extensions.injector.api.IInjectionBinding;
		}

		public new virtual global::strange.extensions.injector.api.IInjectionBinding GetBinding(object key)
		{
			return base.GetBinding(key) as global::strange.extensions.injector.api.IInjectionBinding;
		}

		public new virtual global::strange.extensions.injector.api.IInjectionBinding GetBinding(object key, object name)
		{
			return base.GetBinding(key, name) as global::strange.extensions.injector.api.IInjectionBinding;
		}

		public int ReflectAll()
		{
			global::System.Collections.Generic.List<global::System.Type> list = new global::System.Collections.Generic.List<global::System.Type>();
			foreach (global::System.Collections.Generic.KeyValuePair<object, global::System.Collections.Generic.Dictionary<object, global::strange.framework.api.IBinding>> binding in bindings)
			{
				global::System.Collections.Generic.Dictionary<object, global::strange.framework.api.IBinding> value = binding.Value;
				foreach (global::System.Collections.Generic.KeyValuePair<object, global::strange.framework.api.IBinding> item2 in value)
				{
					global::strange.framework.api.IBinding value2 = item2.Value;
					global::System.Type item = ((value2.value is global::System.Type) ? ((global::System.Type)value2.value) : value2.value.GetType());
					if (list.IndexOf(item) == -1)
					{
						list.Add(item);
					}
				}
			}
			return Reflect(list);
		}

		public int Reflect(global::System.Collections.Generic.List<global::System.Type> list)
		{
			int num = 0;
			foreach (global::System.Type item in list)
			{
				if (!item.IsPrimitive && item != typeof(decimal) && item != typeof(string))
				{
					num++;
					injector.reflector.Get(item);
				}
			}
			return num;
		}
	}
}
