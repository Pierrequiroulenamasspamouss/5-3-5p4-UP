namespace Kampai.Game.Mignette
{
	public class MignetteContext : global::strange.extensions.context.impl.MVCSContext
	{
		public MignetteContext()
		{
		}

		public MignetteContext(global::UnityEngine.MonoBehaviour view, bool autoStartup)
			: base(view, autoStartup)
		{
		}

		protected override void addCoreComponents()
		{
			base.addCoreComponents();
			injectionBinder.Unbind<global::strange.extensions.command.api.ICommandBinder>();
			injectionBinder.Bind<global::strange.extensions.command.api.ICommandBinder>().To<global::strange.extensions.command.impl.SignalCommandBinder>().ToSingleton();
		}

		public override void Launch()
		{
			base.Launch();
			injectionBinder.GetInstance<global::Kampai.Common.StartSignal>().Dispatch();
		}

		protected override void mapBindings()
		{
			base.mapBindings();
			base.commandBinder.Bind<global::Kampai.Game.Mignette.ChangeMignetteScoreSignal>().To<global::Kampai.Game.Mignette.ChangeCurrentMignetteScore>();
			base.commandBinder.Bind<global::Kampai.Game.Mignette.DestroyMignetteContextSignal>().To<global::Kampai.Game.Mignette.DestroyMignetteContextCommand>();
		}
	}
}
