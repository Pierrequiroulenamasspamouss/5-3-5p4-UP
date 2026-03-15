namespace Kampai.UI.View
{
	public class FloatingTextPanelMediator : global::strange.extensions.mediation.impl.Mediator
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("FloatingTextPanelMediator") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.UI.View.FloatingTextPanelView view { get; set; }

		[Inject]
		public global::Kampai.UI.View.DisplayFloatingTextSignal displayFloatingTextSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.RemoveFloatingTextSignal removeFloatingTextSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ToggleAllFloatingTextSignal toggleAllFloatingTextSignal { get; set; }

		public override void OnRegister()
		{
			view.Init(logger);
			displayFloatingTextSignal.AddListener(CreateFloatingText);
			removeFloatingTextSignal.AddListener(RemoveFloatingText);
			toggleAllFloatingTextSignal.AddListener(ToggleAllFloatingText);
		}

		public override void OnRemove()
		{
			view.Cleanup();
			displayFloatingTextSignal.RemoveListener(CreateFloatingText);
			removeFloatingTextSignal.RemoveListener(RemoveFloatingText);
			toggleAllFloatingTextSignal.RemoveListener(ToggleAllFloatingText);
		}

		private void CreateFloatingText(global::Kampai.UI.View.FloatingTextSettings settings)
		{
			view.CreateFloatingText(settings);
		}

		private void RemoveFloatingText(int trackedId)
		{
			view.RemoveFloatingText(trackedId);
		}

		private void ToggleAllFloatingText(bool show)
		{
			view.ToggleAllFloatingText(show);
		}
	}
}
