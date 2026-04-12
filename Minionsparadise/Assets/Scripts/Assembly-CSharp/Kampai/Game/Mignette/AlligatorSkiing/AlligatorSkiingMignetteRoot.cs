namespace Kampai.Game.Mignette.AlligatorSkiing
{
	public class AlligatorSkiingMignetteRoot : global::strange.extensions.context.impl.ContextView
	{
		private void Awake()
		{
			context = new global::Kampai.Game.Mignette.AlligatorSkiing.AlligatorSkiingMignetteContext(this, true);
			context.Start();
		}
	}
}
