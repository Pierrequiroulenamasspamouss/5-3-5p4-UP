namespace Kampai.UI.View
{
	public class DebrisModalItemMediator : global::strange.extensions.mediation.impl.Mediator
	{
		[Inject]
		public global::Kampai.UI.View.DebrisModalItemView DebrisModalItemView { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.CAMERA)]
		public global::UnityEngine.Camera UICamera { get; set; }

		public override void OnRegister()
		{
			DebrisModalItemView.Init(UICamera);
		}
	}
}
