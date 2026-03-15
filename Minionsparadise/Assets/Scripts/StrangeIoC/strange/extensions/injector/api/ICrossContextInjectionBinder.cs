namespace strange.extensions.injector.api
{
	public interface ICrossContextInjectionBinder : global::strange.extensions.injector.api.IInjectionBinder, global::strange.framework.api.IInstanceProvider
	{
		global::strange.extensions.injector.api.IInjectionBinder CrossContextBinder { get; set; }
	}
}
