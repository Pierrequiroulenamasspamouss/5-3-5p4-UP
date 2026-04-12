namespace Kampai.UI.View
{
	public abstract class KampaiMediator : global::strange.extensions.mediation.impl.Mediator
	{
		public string PrefabName;

		public string guiLabel;

		public virtual void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
		}

		public virtual void SetActive(bool isActive)
		{
			base.gameObject.SetActive(isActive);
		}

		protected virtual void Update()
		{
		}
	}
}
