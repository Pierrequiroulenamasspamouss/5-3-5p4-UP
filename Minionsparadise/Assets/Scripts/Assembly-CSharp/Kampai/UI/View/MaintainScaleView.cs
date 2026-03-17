namespace Kampai.UI.View
{
	public class MaintainScaleView : global::Kampai.Util.KampaiView
	{
		private global::UnityEngine.Vector3 localScale;

		[Inject]
		public global::Kampai.Common.ZoomPercentageSignal zoomSignal { get; set; }

		[Inject]
		public global::Kampai.Common.RequestZoomPercentageSignal requestZoomPercentage { get; set; }

		protected override void Start()
		{
			localScale = base.gameObject.transform.localScale;
			base.Start();
			zoomSignal.AddListener(UpdateScale);
			requestZoomPercentage.Dispatch();
		}

		protected override void OnDestroy()
		{
			zoomSignal.RemoveListener(UpdateScale);
			base.OnDestroy();
		}

		internal void UpdateScale(float percentage)
		{
			if (!(percentage > 0.7f))
			{
				base.gameObject.transform.localScale = localScale * (1f - percentage);
			}
		}
	}
}
