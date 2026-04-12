namespace Kampai.Game.Mignette.BalloonBarrage.View
{
	public class BalloonBarrageMignetteManagerMediator : global::Kampai.Game.Mignette.View.MignetteManagerMediator<global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageMignetteManagerView>
	{
		private const float MINION_REACT_RADIUS = 15f;

		private bool prevPressed;

		public override string MusicEventName
		{
			get
			{
				return "Play_MUS_balloonBarrage_01";
			}
		}

		protected override void RequestStopMignette(bool showScore)
		{
			base.view.ResetMignetteObjects();
			base.view.ResetCameraAndStopMignette(showScore);
			base.view.SilencePilots();
		}

		protected override void OnPress(global::UnityEngine.Vector3 pos, int input, bool pressed)
		{
			if (prevPressed != pressed)
			{
				base.view.OnPress(pos, pressed);
			}
			if (pressed)
			{
				base.view.OnPressed(pos);
			}
			prevPressed = pressed;
		}
	}
}
