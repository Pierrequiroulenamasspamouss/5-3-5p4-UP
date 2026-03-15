namespace Kampai.Game.Mignette.ButterflyCatch
{
	public class ButterflyCatchMignetteContext : global::Kampai.Game.Mignette.MignetteContext
	{
		public ButterflyCatchMignetteContext()
		{
		}

		public ButterflyCatchMignetteContext(global::UnityEngine.MonoBehaviour view, bool autoStartup)
			: base(view, autoStartup)
		{
		}

		protected override void mapBindings()
		{
			base.mapBindings();
			base.commandBinder.Bind<global::Kampai.Common.StartSignal>().To<global::Kampai.Game.Mignette.ButterflyCatch.SetupButterflyCatchManagerViewCommand>();
			base.mediationBinder.Bind<global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchMignetteManagerView>().To<global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchMignetteManagerMediator>();
		}
	}
}
