namespace Kampai.Game.Mignette.BalloonBarrage
{
	public class BalloonBarrageMignetteContext : global::Kampai.Game.Mignette.MignetteContext
	{
		public BalloonBarrageMignetteContext()
		{
		}

		public BalloonBarrageMignetteContext(global::UnityEngine.MonoBehaviour view, bool autoStartup)
			: base(view, autoStartup)
		{
		}

		protected override void mapBindings()
		{
			base.mapBindings();
			base.commandBinder.Bind<global::Kampai.Common.StartSignal>().To<global::Kampai.Game.Mignette.BalloonBarrage.SetupBalloonBarrageManagerViewCommand>();
			base.mediationBinder.Bind<global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageMignetteManagerView>().To<global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageMignetteManagerMediator>();
		}
	}
}
