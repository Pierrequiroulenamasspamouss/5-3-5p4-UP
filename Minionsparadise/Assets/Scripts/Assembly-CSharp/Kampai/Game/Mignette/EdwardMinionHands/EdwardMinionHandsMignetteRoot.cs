namespace Kampai.Game.Mignette.EdwardMinionHands
{
	public class EdwardMinionHandsMignetteRoot : global::strange.extensions.context.impl.ContextView
	{
		private void Awake()
		{
			context = new global::Kampai.Game.Mignette.EdwardMinionHands.EdwardMinionHandsMignetteContext(this, true);
			context.Start();
		}
	}
}
