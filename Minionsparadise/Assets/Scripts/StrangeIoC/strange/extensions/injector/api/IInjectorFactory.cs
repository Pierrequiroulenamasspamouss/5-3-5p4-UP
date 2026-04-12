namespace strange.extensions.injector.api
{
	public interface IInjectorFactory
	{
		object Get(global::strange.extensions.injector.api.IInjector injector, global::strange.extensions.injector.api.IInjectionBinding binding);
	}
}
