#pragma warning disable 3005
namespace strange.extensions.injector.api
{

	public interface IInjectionBinding : global::strange.framework.api.IBinding
	{
		bool isCrossContext { get; }

		bool toInject { get; }

		global::strange.extensions.injector.api.InjectionBindingType type { get; set; }

		new object key { get; }

		new object name { get; }

		new object value { get; }

		new global::strange.framework.api.BindingConstraintType keyConstraint { get; set; }

		new global::strange.framework.api.BindingConstraintType valueConstraint { get; set; }

		global::strange.extensions.injector.api.IInjectionBinding ToSingleton();

		global::strange.extensions.injector.api.IInjectionBinding ToValue(object o);

		global::strange.extensions.injector.api.IInjectionBinding SetValue(object o);

		global::strange.extensions.injector.api.IInjectionBinding CrossContext();

		global::strange.extensions.injector.api.IInjectionBinding ToInject(bool value);

		new global::strange.extensions.injector.api.IInjectionBinding Bind<T>();

		new global::strange.extensions.injector.api.IInjectionBinding Bind(object key);

		new global::strange.extensions.injector.api.IInjectionBinding To<T>();

		new global::strange.extensions.injector.api.IInjectionBinding To(object o);

		new global::strange.extensions.injector.api.IInjectionBinding ToName<T>();

		new global::strange.extensions.injector.api.IInjectionBinding ToName(object o);

		new global::strange.extensions.injector.api.IInjectionBinding Named<T>();

		new global::strange.extensions.injector.api.IInjectionBinding Named(object o);
	}
}
