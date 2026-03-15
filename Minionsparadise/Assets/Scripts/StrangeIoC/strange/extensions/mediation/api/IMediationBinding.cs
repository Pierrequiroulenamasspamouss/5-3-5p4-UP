namespace strange.extensions.mediation.api
{
	public interface IMediationBinding : global::strange.framework.api.IBinding
	{
		object abstraction { get; }

		global::strange.extensions.mediation.api.IMediationBinding ToMediator<T>();

		global::strange.extensions.mediation.api.IMediationBinding ToAbstraction<T>();

		new global::strange.extensions.mediation.api.IMediationBinding Bind<T>();

		new global::strange.extensions.mediation.api.IMediationBinding Bind(object key);

		new global::strange.extensions.mediation.api.IMediationBinding To<T>();

		new global::strange.extensions.mediation.api.IMediationBinding To(object o);
	}
}
