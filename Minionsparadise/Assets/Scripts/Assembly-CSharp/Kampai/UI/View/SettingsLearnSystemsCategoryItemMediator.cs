namespace Kampai.UI.View
{
	public class SettingsLearnSystemsCategoryItemMediator : global::strange.extensions.mediation.impl.Mediator
	{
		[Inject]
		public global::Kampai.UI.View.SettingsLearnSystemsCategoryItemView view { get; set; }

		[Inject]
		public global::Kampai.UI.View.SettingsLearnSystemsCategoryItemSelectedSignal categoryItemSelectedSignal { get; set; }

		public override void OnRegister()
		{
			view.Button.ClickedSignal.AddListener(OnClick);
		}

		public override void OnRemove()
		{
			view.Button.ClickedSignal.RemoveListener(OnClick);
		}

		private void OnClick()
		{
			categoryItemSelectedSignal.Dispatch(view.Definition.ID);
		}
	}
}
