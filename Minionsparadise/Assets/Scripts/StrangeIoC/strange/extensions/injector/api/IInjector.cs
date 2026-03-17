namespace strange.extensions.injector.api
{
	public interface IInjector
	{
		global::strange.extensions.injector.api.IInjectorFactory factory { get; set; }

		global::strange.extensions.injector.api.IInjectionBinder binder { get; set; }

		global::strange.extensions.reflector.api.IReflectionBinder reflector { get; set; }

		object Instantiate(global::strange.extensions.injector.api.IInjectionBinding binding);

		object Inject(object target);

		object Inject(object target, bool attemptConstructorInjection);

		void Uninject(object target);

		object GetValueInjection(global::System.Type t, object name, object target);
	}
}
