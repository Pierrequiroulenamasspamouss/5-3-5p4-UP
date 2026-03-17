namespace Kampai.Game
{
	public class GameRoot : global::strange.extensions.context.impl.ContextView
	{
		private void Awake()
		{
			context = new global::Kampai.Game.GameContext(this, true);
			context.Start();
		}
	}
}
