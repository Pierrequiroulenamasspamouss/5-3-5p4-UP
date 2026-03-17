namespace strange.extensions.mediation.api
{
	public interface IMediationBinder : global::strange.framework.api.IBinder
	{
		void Trigger(global::strange.extensions.mediation.api.MediationEvent evt, global::strange.extensions.mediation.api.IView view);

		new global::strange.extensions.mediation.api.IMediationBinding Bind<T>();

		global::strange.extensions.mediation.api.IMediationBinding BindView<T>() where T : global::UnityEngine.MonoBehaviour;
	}
}
