namespace Kampai.Game.Mignette.ButterflyCatch.View
{
	public class ButterflyCatchMignetteManagerMediator : global::Kampai.Game.Mignette.View.MignetteManagerMediator<global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchMignetteManagerView>
	{
		private const float MINION_REACT_RADIUS = 15f;

		private bool previousPressed;

		public override string MusicEventName
		{
			get
			{
				return "Play_MUS_butterflyCatch_01";
			}
		}

		protected override void RequestStopMignette(bool showScore)
		{
			base.view.CleanupMignette();
			base.view.ResetCameraAndStopMignette(showScore);
		}

		protected override void OnPress(global::UnityEngine.Vector3 pos, int input, bool pressed)
		{
			if (pressed && !previousPressed)
			{
				base.view.OnInputDown(pos);
			}
			previousPressed = pressed;
		}
	}
}
