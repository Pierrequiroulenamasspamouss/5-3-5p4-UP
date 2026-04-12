namespace Kampai.BuildingsSizeToolbox
{
	public class BuildingsSizeToolboxContext : global::strange.extensions.context.impl.MVCSContext
	{
		public BuildingsSizeToolboxContext()
		{
		}

		public BuildingsSizeToolboxContext(global::UnityEngine.MonoBehaviour view, bool autoStartup)
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
			injectionBinder.GetInstance<global::Kampai.Main.ILocalizationService>().Initialize("EN-US");
			global::UnityEngine.TextAsset textAsset = global::UnityEngine.Resources.Load("dev_definitions") as global::UnityEngine.TextAsset;
			using (global::System.IO.StringReader textReader = new global::System.IO.StringReader(textAsset.text))
			{
				injectionBinder.GetInstance<global::Kampai.Game.IDefinitionService>().DeserializeJson(textReader);
			}
		}

		protected override void mapBindings()
		{
			global::strange.extensions.injector.api.ICrossContextInjectionBinder crossContextInjectionBinder = injectionBinder;
			crossContextInjectionBinder.Bind<global::strange.extensions.context.api.ICrossContextCapable>().ToValue(this).ToName(global::Kampai.Util.BaseElement.CONTEXT);
			crossContextInjectionBinder.Bind<global::Kampai.Main.ILocalizationService>().To<global::Kampai.Main.HALService>().ToSingleton();
			crossContextInjectionBinder.Bind<ILocalPersistanceService>().To<LocalPersistanceService>();
			crossContextInjectionBinder.Bind<global::Kampai.Game.IDefinitionService>().To<global::Kampai.Game.DefinitionService>().ToSingleton();
			crossContextInjectionBinder.Bind<string>().ToValue(global::Kampai.Util.GameConstants.Server.SERVER_ENVIRONMENT).ToName("game.server.environment");
			crossContextInjectionBinder.Bind<global::Kampai.Game.IPlayerService>().To<global::Kampai.Tools.AnimationToolKit.AnimationToolKitPlayerService>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.UI.IFancyUIService>().To<global::Kampai.UI.FancyUIService>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.UI.IGhostComponentService>().To<global::Kampai.UI.GhostComponentService>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Game.IDLCService>().To<global::Kampai.Tools.AnimationToolKit.AnimationToolKitDLCService>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Splash.IDownloadService>().To<global::Kampai.Tools.AnimationToolKit.AnimationToolKitDownloadService>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Util.IMinionBuilder>().To<global::Kampai.Util.MinionBuilder>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Common.IRandomService>().ToValue(new global::Kampai.Common.RandomService(0L));
			crossContextInjectionBinder.Bind<global::Kampai.Game.IPrestigeService>().To<global::Kampai.BuildingsSizeToolbox.BuildingsSizeToolboxPrestigeService>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Util.IDummyCharacterBuilder>().To<global::Kampai.Util.DummyCharacterBuilder>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Common.Service.Audio.IFMODService>().To<global::Kampai.BuildingsSizeToolbox.BuildingsSizeToolboxFMODService>().ToSingleton();
			bindSignals(crossContextInjectionBinder);
		}

		private void bindSignals(global::strange.extensions.injector.api.ICrossContextInjectionBinder localInjectionBinder)
		{
			localInjectionBinder.Bind<global::Kampai.UI.View.LogToScreenSignal>().ToSingleton();
			localInjectionBinder.Bind<global::Kampai.Main.PlayGlobalSoundFXSignal>().ToSingleton();
			localInjectionBinder.Bind<global::Kampai.BuildingsSizeToolbox.NewUpsellScreenSelectedSignal>().ToSingleton();
			localInjectionBinder.Bind<global::Kampai.Main.PlayLocalAudioSignal>().ToSingleton();
			localInjectionBinder.Bind<global::Kampai.Main.StartLoopingAudioSignal>().ToSingleton();
			localInjectionBinder.Bind<global::Kampai.Main.StopLocalAudioSignal>().ToSingleton();
			localInjectionBinder.Bind<global::Kampai.Main.PlayMinionStateAudioSignal>().ToSingleton();
			localInjectionBinder.Bind<global::Kampai.Main.MoveAudioListenerSignal>().ToSingleton();
			localInjectionBinder.Bind<global::Kampai.BuildingsSizeToolbox.BuildingSelectedSignal>().ToSingleton();
			localInjectionBinder.Bind<global::Kampai.BuildingsSizeToolbox.BuildingModifiedSignal>().ToSingleton();
			localInjectionBinder.Bind<global::Kampai.BuildingsSizeToolbox.BuildingsStateSavedSignal>().ToSingleton();
		}
	}
}
