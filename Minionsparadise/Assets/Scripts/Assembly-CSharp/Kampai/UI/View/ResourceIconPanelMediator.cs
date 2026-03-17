namespace Kampai.UI.View
{
	public class ResourceIconPanelMediator : global::strange.extensions.mediation.impl.Mediator
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("ResourceIconPanelMediator") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.UI.View.ResourceIconPanelView View { get; set; }

		[Inject]
		public global::Kampai.UI.View.CreateResourceIconSignal CreateResourceIconSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.RemoveResourceIconSignal RemoveResourceIconSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.RemoveResourceIconsByTrackedIdSignal RemoveResourceIconsByTrackedIdSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateResourceIconCountSignal UpdateResourceIconCountSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowAllResourceIconsSignal ShowAllResourceIconsSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideAllResourceIconsSignal HideAllResourceIconsSignal { get; set; }

		public override void OnRegister()
		{
			View.Init(logger);
			CreateResourceIconSignal.AddListener(CreateResourceIcon);
			RemoveResourceIconSignal.AddListener(RemoveResourceIcon);
			RemoveResourceIconsByTrackedIdSignal.AddListener(RemoveResourceIconsByTrackedId);
			UpdateResourceIconCountSignal.AddListener(UpdateResourceIconCount);
			ShowAllResourceIconsSignal.AddListener(ShowAllResourceIcons);
			HideAllResourceIconsSignal.AddListener(HideAllResourceIcons);
		}

		public override void OnRemove()
		{
			View.Cleanup();
			CreateResourceIconSignal.RemoveListener(CreateResourceIcon);
			RemoveResourceIconSignal.RemoveListener(RemoveResourceIcon);
			RemoveResourceIconsByTrackedIdSignal.RemoveListener(RemoveResourceIconsByTrackedId);
			UpdateResourceIconCountSignal.RemoveListener(UpdateResourceIconCount);
			ShowAllResourceIconsSignal.RemoveListener(ShowAllResourceIcons);
			HideAllResourceIconsSignal.RemoveListener(HideAllResourceIcons);
		}

		private void CreateResourceIcon(global::Kampai.UI.View.ResourceIconSettings resourceIconSettings)
		{
			View.CreateResourceIcon(resourceIconSettings);
		}

		private void RemoveResourceIcon(global::Kampai.Util.Tuple<int, int> tuple)
		{
			View.RemoveResourceIcon(tuple.Item1, tuple.Item2);
		}

		private void RemoveResourceIconsByTrackedId(int trackedId)
		{
			View.RemoveResourceIcon(trackedId);
		}

		private void UpdateResourceIconCount(global::Kampai.Util.Tuple<int, int> tuple, int count)
		{
			View.UpdateResourceIconCount(tuple.Item1, tuple.Item2, count);
		}

		private void HideAllResourceIcons()
		{
			View.HideAllResourceIcons();
		}

		private void ShowAllResourceIcons()
		{
			View.ShowAllResourceIcons();
		}
	}
}
