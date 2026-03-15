namespace Kampai.UI.View
{
	public class CabanaParentWayFinderMediator : global::Kampai.UI.View.AbstractParentWayFinderMediator
	{
		[Inject]
		public global::Kampai.UI.View.CabanaParentWayFinderView CabanaParentWayFinderView { get; set; }

		[Inject]
		public global::Kampai.Game.CameraAutoMoveToPositionSignal CameraAutoMoveToPositionSignal { get; set; }

		public override global::Kampai.UI.View.IWayFinderView View
		{
			get
			{
				return CabanaParentWayFinderView;
			}
		}

		protected override void PanToInstance()
		{
			CameraAutoMoveToPositionSignal.Dispatch(View.GetIndicatorPosition() + global::Kampai.Util.GameConstants.CAMERA_OFFSET_WAYFINDER_CABANA, 0.3f, false);
		}
	}
}
