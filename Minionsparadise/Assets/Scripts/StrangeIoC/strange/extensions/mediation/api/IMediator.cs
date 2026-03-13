namespace strange.extensions.mediation.api
{
	public interface IMediator
	{
		global::UnityEngine.GameObject contextView { get; set; }

		void PreRegister();

		void OnRegister();

		void OnRemove();
	}
}
