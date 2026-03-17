namespace Kampai.Game.Mignette.WaterSlide.View
{
	public class WaterSlideMignetteManagerMediator : global::Kampai.Game.Mignette.View.MignetteManagerMediator<global::Kampai.Game.Mignette.WaterSlide.View.WaterSlideMignetteManagerView>
	{
		private bool mouseDown;

		[Inject]
		public global::Kampai.UI.View.MignetteDooberSpawnedSignal mignetteDooberSpawnedSignal { get; set; }

		public override string MusicEventName
		{
			get
			{
				return "Play_MUS_waterslide_01";
			}
		}

		public override void OnRegister()
		{
			base.OnRegister();
			mignetteDooberSpawnedSignal.AddListener(TrackMignetteDoober);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			mignetteDooberSpawnedSignal.RemoveListener(TrackMignetteDoober);
		}

		protected override void RequestStopMignette(bool showScore)
		{
			base.view.ResetMignetteObjects();
			base.view.ResetCameraAndStopMignette(showScore);
		}

		protected override void OnPress(global::UnityEngine.Vector3 pos, int input, bool pressed)
		{
			if (pressed && !mouseDown)
			{
				mouseDown = true;
				base.view.OnScreenTapped();
			}
			else if (!pressed && mouseDown)
			{
				mouseDown = false;
			}
		}

		private void TrackMignetteDoober(global::UnityEngine.GameObject gameObject)
		{
			base.view.mignetteDooberGO = gameObject;
		}
	}
}
