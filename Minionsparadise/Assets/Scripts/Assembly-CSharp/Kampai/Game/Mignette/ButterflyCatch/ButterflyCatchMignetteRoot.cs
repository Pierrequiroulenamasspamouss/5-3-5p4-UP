namespace Kampai.Game.Mignette.ButterflyCatch
{
	public class ButterflyCatchMignetteRoot : global::strange.extensions.context.impl.ContextView
	{
		private void Awake()
		{
			context = new global::Kampai.Game.Mignette.ButterflyCatch.ButterflyCatchMignetteContext(this, true);
			context.Start();
		}
	}
}
