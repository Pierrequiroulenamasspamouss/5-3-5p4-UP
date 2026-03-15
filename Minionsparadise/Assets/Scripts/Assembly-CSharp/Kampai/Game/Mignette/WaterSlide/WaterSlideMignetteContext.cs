namespace Kampai.Game.Mignette.WaterSlide
{
	public class WaterSlideMignetteContext : global::Kampai.Game.Mignette.MignetteContext
	{
		public WaterSlideMignetteContext()
		{
		}

		public WaterSlideMignetteContext(global::UnityEngine.MonoBehaviour view, bool autoStartup)
			: base(view, autoStartup)
		{
		}

		protected override void mapBindings()
		{
			base.mapBindings();
			base.commandBinder.Bind<global::Kampai.Common.StartSignal>().To<global::Kampai.Game.Mignette.WaterSlide.WaterSlideMignetteSetupCommand>();
			base.mediationBinder.Bind<global::Kampai.Game.Mignette.WaterSlide.View.WaterSlideMignetteManagerView>().To<global::Kampai.Game.Mignette.WaterSlide.View.WaterSlideMignetteManagerMediator>();
			injectionBinder.Bind<global::Kampai.Game.Mignette.WaterSlide.WaterSlideMignetteJumpLandedSignal>().ToSingleton();
			injectionBinder.Bind<global::Kampai.Game.Mignette.WaterSlide.WaterSlideMignetteMinionHitObstacleSignal>().ToSingleton();
			injectionBinder.Bind<global::Kampai.Game.Mignette.WaterSlide.WaterSlideMignetteMinionHitCollectableSignal>().ToSingleton();
			injectionBinder.Bind<global::Kampai.Game.Mignette.WaterSlide.WaterSlideMignettePathCompletedSignal>().ToSingleton();
			injectionBinder.Bind<global::Kampai.Game.Mignette.WaterSlide.WaterslideMignetteOnDiveTriggerSignal>().ToSingleton();
			injectionBinder.Bind<global::Kampai.Game.Mignette.WaterSlide.WaterslideMignetteOnPlayDiveTriggerSignal>().ToSingleton();
		}
	}
}
