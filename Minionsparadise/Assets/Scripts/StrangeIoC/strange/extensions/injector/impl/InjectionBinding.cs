namespace strange.extensions.injector.impl
{
	public class InjectionBinding : global::strange.framework.impl.Binding, global::strange.extensions.injector.api.IInjectionBinding, global::strange.framework.api.IBinding
	{
		private global::strange.extensions.injector.api.InjectionBindingType _type;

		private bool _toInject = true;

		private bool _isCrossContext;

		public global::strange.extensions.injector.api.InjectionBindingType type
		{
			get
			{
				return _type;
			}
			set
			{
				_type = value;
			}
		}

		public bool toInject
		{
			get
			{
				return _toInject;
			}
		}

		public bool isCrossContext
		{
			get
			{
				return _isCrossContext;
			}
		}

		public InjectionBinding(global::strange.framework.impl.Binder.BindingResolver resolver)
		{
			base.resolver = resolver;
			base.keyConstraint = global::strange.framework.api.BindingConstraintType.MANY;
			base.valueConstraint = global::strange.framework.api.BindingConstraintType.ONE;
		}

		public global::strange.extensions.injector.api.IInjectionBinding ToInject(bool value)
		{
			_toInject = value;
			return this;
		}

		public global::strange.extensions.injector.api.IInjectionBinding ToSingleton()
		{
			if (type == global::strange.extensions.injector.api.InjectionBindingType.VALUE)
			{
				return this;
			}
			type = global::strange.extensions.injector.api.InjectionBindingType.SINGLETON;
			if (resolver != null)
			{
				resolver(this);
			}
			return this;
		}

		public global::strange.extensions.injector.api.IInjectionBinding ToValue(object o)
		{
			type = global::strange.extensions.injector.api.InjectionBindingType.VALUE;
			SetValue(o);
			return this;
		}

		public global::strange.extensions.injector.api.IInjectionBinding SetValue(object o)
		{
			global::System.Type type = o.GetType();
			object[] array = base.key as object[];
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				object obj = array[i];
				global::System.Type type2 = ((obj is global::System.Type) ? (obj as global::System.Type) : obj.GetType());
				if (!type2.IsAssignableFrom(type) && !HasGenericAssignableFrom(type2, type))
				{
					throw new global::strange.extensions.injector.impl.InjectionException("Injection cannot bind a value that does not extend or implement the binding type.", global::strange.extensions.injector.api.InjectionExceptionType.ILLEGAL_BINDING_VALUE);
				}
			}
			To(o);
			return this;
		}

		protected bool HasGenericAssignableFrom(global::System.Type keyType, global::System.Type objType)
		{
			if (!keyType.IsGenericType)
			{
				return false;
			}
			return true;
		}

		protected bool IsGenericTypeAssignable(global::System.Type givenType, global::System.Type genericType)
		{
			global::System.Type[] interfaces = givenType.GetInterfaces();
			global::System.Type[] array = interfaces;
			foreach (global::System.Type type in array)
			{
				if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
				{
					return true;
				}
			}
			if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
			{
				return true;
			}
			global::System.Type baseType = givenType.BaseType;
			if (baseType == null)
			{
				return false;
			}
			return IsGenericTypeAssignable(baseType, genericType);
		}

		public global::strange.extensions.injector.api.IInjectionBinding CrossContext()
		{
			_isCrossContext = true;
			if (resolver != null)
			{
				resolver(this);
			}
			return this;
		}

		public new global::strange.extensions.injector.api.IInjectionBinding Bind<T>()
		{
			return base.Bind<T>() as global::strange.extensions.injector.api.IInjectionBinding;
		}

		public new global::strange.extensions.injector.api.IInjectionBinding Bind(object key)
		{
			return base.Bind(key) as global::strange.extensions.injector.api.IInjectionBinding;
		}

		public new global::strange.extensions.injector.api.IInjectionBinding To<T>()
		{
			return base.To<T>() as global::strange.extensions.injector.api.IInjectionBinding;
		}

		public new global::strange.extensions.injector.api.IInjectionBinding To(object o)
		{
			return base.To(o) as global::strange.extensions.injector.api.IInjectionBinding;
		}

		public new global::strange.extensions.injector.api.IInjectionBinding ToName<T>()
		{
			return base.ToName<T>() as global::strange.extensions.injector.api.IInjectionBinding;
		}

		public new global::strange.extensions.injector.api.IInjectionBinding ToName(object o)
		{
			return base.ToName(o) as global::strange.extensions.injector.api.IInjectionBinding;
		}

		public new global::strange.extensions.injector.api.IInjectionBinding Named<T>()
		{
			return base.Named<T>() as global::strange.extensions.injector.api.IInjectionBinding;
		}

		public new global::strange.extensions.injector.api.IInjectionBinding Named(object o)
		{
			return base.Named(o) as global::strange.extensions.injector.api.IInjectionBinding;
		}
	}
}
