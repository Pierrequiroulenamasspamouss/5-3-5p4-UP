#pragma warning disable 3005
namespace strange.extensions.injector.api
{

	public interface IInjectionBinder : global::strange.framework.api.IInstanceProvider
	{
		global::strange.extensions.injector.api.IInjector injector { get; set; }

		object GetInstance(global::System.Type key, object name);

		T GetInstance<T>(object name);

		int Reflect(global::System.Collections.Generic.List<global::System.Type> list);

		int ReflectAll();

		void ResolveBinding(global::strange.framework.api.IBinding binding, object key);

		global::strange.extensions.injector.api.IInjectionBinding Bind<T>();

		global::strange.extensions.injector.api.IInjectionBinding Bind(global::System.Type key);

		global::strange.framework.api.IBinding Bind(object key);

		global::strange.extensions.injector.api.IInjectionBinding GetBinding<T>();

		global::strange.extensions.injector.api.IInjectionBinding GetBinding<T>(object name);

		global::strange.extensions.injector.api.IInjectionBinding GetBinding(object key);

		global::strange.extensions.injector.api.IInjectionBinding GetBinding(object key, object name);

		void Unbind<T>();

		void Unbind<T>(object name);

		void Unbind(object key);

		void Unbind(object key, object name);

		void Unbind(global::strange.framework.api.IBinding binding);
	}
}
