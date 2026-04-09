namespace strange.extensions.mediation.impl
{
	public class MediationBinder : global::strange.framework.impl.Binder, global::strange.extensions.mediation.api.IMediationBinder, global::strange.framework.api.IBinder
	{
		[Inject]
		public global::strange.extensions.injector.api.IInjectionBinder injectionBinder { get; set; }

		public override global::strange.framework.api.IBinding GetRawBinding()
		{
			return new global::strange.extensions.mediation.impl.MediationBinding(resolver);
		}

		public void Trigger(global::strange.extensions.mediation.api.MediationEvent evt, global::strange.extensions.mediation.api.IView view)
		{
			global::System.Type type = view.GetType();
			global::strange.extensions.mediation.api.IMediationBinding mediationBinding = GetBinding(type) as global::strange.extensions.mediation.api.IMediationBinding;
			if (mediationBinding != null)
			{
				switch (evt)
				{
				case global::strange.extensions.mediation.api.MediationEvent.AWAKE:
					injectViewAndChildren(view);
					mapView(view, mediationBinding);
					break;
				case global::strange.extensions.mediation.api.MediationEvent.DESTROYED:
					unmapView(view, mediationBinding);
					break;
				}
			}
			else if (evt == global::strange.extensions.mediation.api.MediationEvent.AWAKE)
			{
				injectViewAndChildren(view);
			}
		}

		protected virtual void injectViewAndChildren(global::strange.extensions.mediation.api.IView view)
		{
			global::UnityEngine.MonoBehaviour target = view as global::UnityEngine.MonoBehaviour;
			injectionBinder.injector.Inject(target, false);
		}

		public new global::strange.extensions.mediation.api.IMediationBinding Bind<T>()
		{
			return base.Bind<T>() as global::strange.extensions.mediation.api.IMediationBinding;
		}

		public global::strange.extensions.mediation.api.IMediationBinding BindView<T>() where T : global::UnityEngine.MonoBehaviour
		{
			return base.Bind<T>() as global::strange.extensions.mediation.api.IMediationBinding;
		}

		protected virtual void mapView(global::strange.extensions.mediation.api.IView view, global::strange.extensions.mediation.api.IMediationBinding binding)
		{
			UnityEngine.Debug.Log(string.Format("ANTIGRAVITY: MediationBinder.mapView for {0} (Binding: {1})", view.GetType().Name, binding.key));
			global::System.Type type = view.GetType();
			if (!bindings.ContainsKey(type))
			{
				UnityEngine.Debug.LogWarning(string.Format("ANTIGRAVITY: MediationBinder.mapView NO BINDING for {0}", type.Name));
				return;
			}
			object[] array = binding.value as object[];
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				global::UnityEngine.MonoBehaviour monoBehaviour = view as global::UnityEngine.MonoBehaviour;
				global::System.Type type2 = array[i] as global::System.Type;
				if (type2 == type)
				{
					throw new global::strange.extensions.mediation.impl.MediationException(string.Concat(type, "mapped to itself. The result would be a stack overflow."), global::strange.extensions.mediation.api.MediationExceptionType.MEDIATOR_VIEW_STACK_OVERFLOW);
				}
				global::UnityEngine.MonoBehaviour monoBehaviour2 = monoBehaviour.gameObject.AddComponent(type2) as global::UnityEngine.MonoBehaviour;
				if (monoBehaviour2 == null)
				{
					throw new global::strange.extensions.mediation.impl.MediationException("The view: " + type.ToString() + " is mapped to mediator: " + type2.ToString() + ". AddComponent resulted in null, which probably means " + type2.ToString().Substring(type2.ToString().LastIndexOf(".") + 1) + " is not a MonoBehaviour.", global::strange.extensions.mediation.api.MediationExceptionType.NULL_MEDIATOR);
				}
				if (monoBehaviour2 is global::strange.extensions.mediation.api.IMediator)
				{
					((global::strange.extensions.mediation.api.IMediator)monoBehaviour2).PreRegister();
				}
				global::System.Type key = ((binding.abstraction == null || binding.abstraction.Equals(global::strange.framework.api.BindingConst.NULLOID)) ? type : (binding.abstraction as global::System.Type));
				injectionBinder.Bind(key).ToValue(view).ToInject(false);
				injectionBinder.injector.Inject(monoBehaviour2);
				injectionBinder.Unbind(key);
				if (monoBehaviour2 is global::strange.extensions.mediation.api.IMediator)
				{
					UnityEngine.Debug.Log(string.Format("ANTIGRAVITY: MediationBinder.mapView CALLING OnRegister for {0}", type2.Name));
					((global::strange.extensions.mediation.api.IMediator)monoBehaviour2).OnRegister();
				}
			}
		}

		protected virtual void unmapView(global::strange.extensions.mediation.api.IView view, global::strange.extensions.mediation.api.IMediationBinding binding)
		{
			global::System.Type type = view.GetType();
			if (!bindings.ContainsKey(type))
			{
				return;
			}
			object[] array = binding.value as object[];
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				global::System.Type type2 = array[i] as global::System.Type;
				global::UnityEngine.MonoBehaviour monoBehaviour = view as global::UnityEngine.MonoBehaviour;
				global::strange.extensions.mediation.api.IMediator mediator = monoBehaviour.GetComponent(type2) as global::strange.extensions.mediation.api.IMediator;
				if (mediator != null)
				{
					mediator.OnRemove();
				}
			}
		}

		private void enableView(global::strange.extensions.mediation.api.IView view)
		{
		}

		private void disableView(global::strange.extensions.mediation.api.IView view)
		{
		}
	}
}
