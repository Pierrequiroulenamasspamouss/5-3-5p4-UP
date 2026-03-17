namespace strange.framework.impl
{
	public class Binding : global::strange.framework.api.IBinding
	{
		public global::strange.framework.impl.Binder.BindingResolver resolver;

		protected global::strange.framework.api.ISemiBinding _key;

		protected global::strange.framework.api.ISemiBinding _value;

		protected global::strange.framework.api.ISemiBinding _name;

		protected bool _isWeak;

		public object key
		{
			get
			{
				return _key.value;
			}
		}

		public object value
		{
			get
			{
				return _value.value;
			}
		}

		public object name
		{
			get
			{
				if (_name.value != null)
				{
					return _name.value;
				}
				return global::strange.framework.api.BindingConst.NULLOID;
			}
		}

		public global::strange.framework.api.BindingConstraintType keyConstraint
		{
			get
			{
				return _key.constraint;
			}
			set
			{
				_key.constraint = value;
			}
		}

		public global::strange.framework.api.BindingConstraintType valueConstraint
		{
			get
			{
				return _value.constraint;
			}
			set
			{
				_value.constraint = value;
			}
		}

		public global::strange.framework.api.BindingConstraintType nameConstraint
		{
			get
			{
				return _name.constraint;
			}
			set
			{
				_name.constraint = value;
			}
		}

		public bool isWeak
		{
			get
			{
				return _isWeak;
			}
		}

		public Binding(global::strange.framework.impl.Binder.BindingResolver resolver)
		{
			this.resolver = resolver;
			_key = new global::strange.framework.impl.SemiBinding();
			_value = new global::strange.framework.impl.SemiBinding();
			_name = new global::strange.framework.impl.SemiBinding();
			keyConstraint = global::strange.framework.api.BindingConstraintType.ONE;
			nameConstraint = global::strange.framework.api.BindingConstraintType.ONE;
			valueConstraint = global::strange.framework.api.BindingConstraintType.MANY;
		}

		public Binding()
			: this(null)
		{
		}

		public virtual global::strange.framework.api.IBinding Bind<T>()
		{
			return Bind(typeof(T));
		}

		public virtual global::strange.framework.api.IBinding Bind(object o)
		{
			_key.Add(o);
			return this;
		}

		public virtual global::strange.framework.api.IBinding To<T>()
		{
			return To(typeof(T));
		}

		public virtual global::strange.framework.api.IBinding To(object o)
		{
			_value.Add(o);
			if (resolver != null)
			{
				resolver(this);
			}
			return this;
		}

		public virtual global::strange.framework.api.IBinding ToName<T>()
		{
			return ToName(typeof(T));
		}

		public virtual global::strange.framework.api.IBinding ToName(object o)
		{
			object obj = ((o == null) ? global::strange.framework.api.BindingConst.NULLOID : o);
			_name.Add(obj);
			if (resolver != null)
			{
				resolver(this);
			}
			return this;
		}

		public virtual global::strange.framework.api.IBinding Named<T>()
		{
			return Named(typeof(T));
		}

		public virtual global::strange.framework.api.IBinding Named(object o)
		{
			if (_name.value != o)
			{
				return null;
			}
			return this;
		}

		public virtual void RemoveKey(object o)
		{
			_key.Remove(o);
		}

		public virtual void RemoveValue(object o)
		{
			_value.Remove(o);
		}

		public virtual void RemoveName(object o)
		{
			_name.Remove(o);
		}

		public virtual global::strange.framework.api.IBinding Weak()
		{
			_isWeak = true;
			return this;
		}
	}
}
