namespace strange.extensions.reflector.api
{
	public interface IReflectedClass
	{
		bool PreGenerated { get; set; }

		bool HasNonTrivialConstructor();

		object CreateInstance(global::strange.extensions.injector.api.IInjector injector);

		void CallPostConstructors(object instance);

		void PerformInjection(object instance, global::strange.extensions.injector.api.IInjector injector);

		void PerformUninjection(object instance, global::strange.extensions.injector.api.IInjector injector);
	}
}
