namespace Kampai.Game.Mignette.AlligatorSkiing
{
	public class AlligatorSkiingMignetteContext : global::Kampai.Game.Mignette.MignetteContext
	{
		public AlligatorSkiingMignetteContext()
		{
		}

		public AlligatorSkiingMignetteContext(global::UnityEngine.MonoBehaviour view, bool autoStartup)
			: base(view, autoStartup)
		{
		}

		protected override void mapBindings()
		{
			base.mapBindings();
			base.commandBinder.Bind<global::Kampai.Common.StartSignal>().To<global::Kampai.Game.Mignette.AlligatorSkiing.AlligatorSkiingMignetteSetupCommand>();
			base.mediationBinder.Bind<global::Kampai.Game.Mignette.AlligatorSkiing.View.AlligatorSkiingMignetteManagerView>().To<global::Kampai.Game.Mignette.AlligatorSkiing.View.AlligatorSkiingMignetteManagerMediator>();
			injectionBinder.Bind<global::Kampai.Game.Mignette.AlligatorSkiing.AlligatorMignettePathCompletedSignal>().ToSingleton();
			injectionBinder.Bind<global::Kampai.Game.Mignette.AlligatorSkiing.AlligatorMignetteMinionHitObstacleSignal>().ToSingleton();
			injectionBinder.Bind<global::Kampai.Game.Mignette.AlligatorSkiing.AlligatorMignetteMinionHitCollectableSignal>().ToSingleton();
			injectionBinder.Bind<global::Kampai.Game.Mignette.AlligatorSkiing.AlligatorMignetteJumpLandedSignal>().ToSingleton();
		}
	}
}
