namespace strange.extensions.mediation.impl
{
	public class MediationBinding : global::strange.framework.impl.Binding, global::strange.extensions.mediation.api.IMediationBinding, global::strange.framework.api.IBinding
	{
		protected global::strange.framework.api.ISemiBinding _abstraction;

		public object abstraction
		{
			get
			{
				if (_abstraction.value != null)
				{
					return _abstraction.value;
				}
				return global::strange.framework.api.BindingConst.NULLOID;
			}
		}

		public MediationBinding(global::strange.framework.impl.Binder.BindingResolver resolver)
			: base(resolver)
		{
			_abstraction = new global::strange.framework.impl.SemiBinding();
			_abstraction.constraint = global::strange.framework.api.BindingConstraintType.ONE;
		}

		global::strange.extensions.mediation.api.IMediationBinding global::strange.extensions.mediation.api.IMediationBinding.ToMediator<T>()
		{
			return base.To(typeof(T)) as global::strange.extensions.mediation.api.IMediationBinding;
		}

		global::strange.extensions.mediation.api.IMediationBinding global::strange.extensions.mediation.api.IMediationBinding.ToAbstraction<T>()
		{
			global::System.Type typeFromHandle = typeof(T);
			if (base.key != null)
			{
				global::System.Type c = base.key as global::System.Type;
				if (!typeFromHandle.IsAssignableFrom(c))
				{
					throw new global::strange.extensions.mediation.impl.MediationException("The View " + base.key.ToString() + " has been bound to the abstraction " + typeof(T).ToString() + " which the View neither extends nor implements. ", global::strange.extensions.mediation.api.MediationExceptionType.VIEW_NOT_ASSIGNABLE);
				}
			}
			_abstraction.Add(typeFromHandle);
			return this;
		}

		public new global::strange.extensions.mediation.api.IMediationBinding Bind<T>()
		{
			return base.Bind<T>() as global::strange.extensions.mediation.api.IMediationBinding;
		}

		public new global::strange.extensions.mediation.api.IMediationBinding Bind(object key)
		{
			return base.Bind(key) as global::strange.extensions.mediation.api.IMediationBinding;
		}

		public new global::strange.extensions.mediation.api.IMediationBinding To<T>()
		{
			return base.To<T>() as global::strange.extensions.mediation.api.IMediationBinding;
		}

		public new global::strange.extensions.mediation.api.IMediationBinding To(object o)
		{
			return base.To(o) as global::strange.extensions.mediation.api.IMediationBinding;
		}
	}
}
