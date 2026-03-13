namespace Kampai.UI.View
{
	public class MignetteWayFinderMediator : global::Kampai.UI.View.AbstractWayFinderMediator
	{
		[Inject]
		public global::Kampai.UI.View.MignetteWayFinderView mignetteWayFinderView { get; set; }

		[Inject(global::Kampai.Main.MainElement.CAMERA)]
		public global::UnityEngine.GameObject cameraGO { get; set; }

		public override global::Kampai.UI.View.IWayFinderView View
		{
			get
			{
				return mignetteWayFinderView;
			}
		}

		protected override void PanToInstance()
		{
			if (base.ZoomCameraModel.ZoomedIn)
			{
				return;
			}
			global::Kampai.Game.Building byInstanceId = base.PlayerService.GetByInstanceId<global::Kampai.Game.Building>(GetTrackedId());
			if (byInstanceId == null)
			{
				return;
			}
			global::Kampai.Game.PanInstructions panInstructions = new global::Kampai.Game.PanInstructions(byInstanceId);
			if (byInstanceId.Definition.ID == 3502)
			{
				float currentPercentage = cameraGO.GetComponent<global::Kampai.Game.View.ZoomView>().GetCurrentPercentage();
				float zoom = byInstanceId.Definition.ScreenPosition.zoom;
				float num = ((!(zoom < 0f)) ? zoom : currentPercentage);
				if (num > 0.01f)
				{
					panInstructions.Offset = new global::Kampai.Util.Boxed<global::UnityEngine.Vector3>(global::Kampai.Util.GameConstants.Mignettes.ALLIGATOR_SKIING_WAYFINDER_CAMERA_OFFSET * num);
				}
			}
			base.CameraAutoMoveToInstanceSignal.Dispatch(panInstructions, new global::Kampai.Util.Boxed<global::Kampai.Game.ScreenPosition>(new global::Kampai.Game.ScreenPosition()));
		}
	}
}
