namespace Kampai.Game.Mignette.EdwardMinionHands
{
	public class EdwardMinionHandsMignetteContext : global::Kampai.Game.Mignette.MignetteContext
	{
		public EdwardMinionHandsMignetteContext()
		{
		}

		public EdwardMinionHandsMignetteContext(global::UnityEngine.MonoBehaviour view, bool autoStartup)
			: base(view, autoStartup)
		{
		}

		protected override void mapBindings()
		{
			base.mapBindings();
			base.commandBinder.Bind<global::Kampai.Common.StartSignal>().To<global::Kampai.Game.Mignette.EdwardMinionHands.SetupEdwardMinionHandsManagerViewCommand>();
			base.mediationBinder.Bind<global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsMignetteManagerView>().To<global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsMignetteManagerMediator>();
		}
	}
}
