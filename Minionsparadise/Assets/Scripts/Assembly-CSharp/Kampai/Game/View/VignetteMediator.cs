namespace Kampai.Game.View
{
	public class VignetteMediator : global::strange.extensions.mediation.impl.Mediator
	{
		[Inject]
		public global::Kampai.Game.View.VignetteView view { get; set; }

		[Inject]
		public global::Kampai.Game.ToggleVignetteSignal toggleSignal { get; set; }

		public override void OnRegister()
		{
			toggleSignal.AddListener(Toggle);
			view.gameObject.SetActive(false);
		}

		public override void OnRemove()
		{
			toggleSignal.RemoveListener(Toggle);
		}

		private void Toggle(bool enable, float? size)
		{
			if (view != null)
			{
				if (enable)
				{
					view.SetVignetteSize(size);
				}
				view.gameObject.SetActive(enable);
			}
		}
	}
}
