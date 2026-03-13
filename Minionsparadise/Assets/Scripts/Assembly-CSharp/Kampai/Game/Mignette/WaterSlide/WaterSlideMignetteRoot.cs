namespace Kampai.Game.Mignette.WaterSlide
{
	public class WaterSlideMignetteRoot : global::strange.extensions.context.impl.ContextView
	{
		private void Awake()
		{
			context = new global::Kampai.Game.Mignette.WaterSlide.WaterSlideMignetteContext(this, true);
			context.Start();
		}
	}
}
