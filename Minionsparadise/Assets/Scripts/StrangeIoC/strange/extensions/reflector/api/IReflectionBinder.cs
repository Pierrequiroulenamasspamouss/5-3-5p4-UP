namespace strange.extensions.reflector.api
{
	public interface IReflectionBinder
	{
		global::strange.extensions.reflector.api.IReflectedClass Get(global::System.Type type);

		global::strange.extensions.reflector.api.IReflectedClass Get<T>();
	}
}
