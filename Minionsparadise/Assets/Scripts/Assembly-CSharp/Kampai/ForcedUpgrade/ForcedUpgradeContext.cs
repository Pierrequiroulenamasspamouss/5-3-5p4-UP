namespace Kampai.ForcedUpgrade
{
	public class ForcedUpgradeContext : global::Kampai.Util.BaseContext
	{
		public ForcedUpgradeContext()
		{
		}

		public ForcedUpgradeContext(global::UnityEngine.MonoBehaviour view, bool autoStartup)
			: base(view, autoStartup)
		{
		}

		protected override void MapBindings()
		{
			injectionBinder.Bind<global::Kampai.Main.ILocalizationService>().To<global::Kampai.Main.HALService>().ToSingleton();
			injectionBinder.Bind<global::Kampai.Main.PlayGlobalSoundFXSignal>().ToSingleton();
			base.commandBinder.Bind<global::Kampai.Common.StartSignal>().To<global::Kampai.ForcedUpgrade.ForcedUpgradeStartCommand>();
			base.commandBinder.Bind<global::Kampai.Main.InitLocalizationServiceSignal>().To<global::Kampai.Main.InitLocalizationServiceCommand>();
			base.mediationBinder.Bind<global::Kampai.ForcedUpgrade.View.ForcedUpgradeView>().To<global::Kampai.ForcedUpgrade.View.ForcedUpgradeMediator>();
		}
	}
}
