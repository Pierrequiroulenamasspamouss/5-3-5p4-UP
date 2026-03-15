namespace Kampai.Game.Mignette.BalloonBarrage
{
	public class BalloonBarrageMignetteRoot : global::strange.extensions.context.impl.ContextView
	{
		private void Awake()
		{
			context = new global::Kampai.Game.Mignette.BalloonBarrage.BalloonBarrageMignetteContext(this, true);
			context.Start();
		}
	}
}
