namespace strange.extensions.injector.impl
{
	public class CrossContextInjectionBinder : global::strange.extensions.injector.impl.InjectionBinder, global::strange.extensions.injector.api.ICrossContextInjectionBinder, global::strange.extensions.injector.api.IInjectionBinder, global::strange.framework.api.IInstanceProvider
	{
		public global::strange.extensions.injector.api.IInjectionBinder CrossContextBinder { get; set; }

		public override global::strange.extensions.injector.api.IInjectionBinding GetBinding<T>()
		{
			return GetBinding(typeof(T), null);
		}

		public override global::strange.extensions.injector.api.IInjectionBinding GetBinding<T>(object name)
		{
			return GetBinding(typeof(T), name);
		}

		public override global::strange.extensions.injector.api.IInjectionBinding GetBinding(object key)
		{
			return GetBinding(key, null);
		}

		public override global::strange.extensions.injector.api.IInjectionBinding GetBinding(object key, object name)
		{
			global::strange.extensions.injector.api.IInjectionBinding binding = base.GetBinding(key, name);
			if (binding == null && CrossContextBinder != null)
			{
				binding = CrossContextBinder.GetBinding(key, name);
			}
			return binding;
		}

		public override void ResolveBinding(global::strange.framework.api.IBinding binding, object key)
		{
			if (!(binding is global::strange.extensions.injector.api.IInjectionBinding))
			{
				return;
			}
			global::strange.extensions.injector.impl.InjectionBinding injectionBinding = (global::strange.extensions.injector.impl.InjectionBinding)binding;
			if (injectionBinding.isCrossContext)
			{
				if (CrossContextBinder == null)
				{
					base.ResolveBinding(binding, key);
					return;
				}
				Unbind(key, binding.name);
				CrossContextBinder.ResolveBinding(binding, key);
			}
			else
			{
				base.ResolveBinding(binding, key);
			}
		}

		protected override global::strange.extensions.injector.api.IInjector GetInjectorForBinding(global::strange.extensions.injector.api.IInjectionBinding binding)
		{
			if (binding.isCrossContext && CrossContextBinder != null)
			{
				return CrossContextBinder.injector;
			}
			return base.injector;
		}
	}
}
