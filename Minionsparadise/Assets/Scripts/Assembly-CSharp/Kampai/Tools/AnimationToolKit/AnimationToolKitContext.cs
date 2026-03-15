namespace Kampai.Tools.AnimationToolKit
{
	public class AnimationToolKitContext : global::strange.extensions.context.impl.MVCSContext
	{
		public AnimationToolKitContext()
		{
		}

		public AnimationToolKitContext(global::UnityEngine.MonoBehaviour view, bool autoStartup)
			: base(view, autoStartup)
		{
		}

		public override void Launch()
		{
			base.Launch();
			injectionBinder.GetInstance<global::Kampai.Common.StartSignal>().Dispatch();
		}

		protected override void addCoreComponents()
		{
			base.addCoreComponents();
			injectionBinder.Unbind<global::strange.extensions.command.api.ICommandBinder>();
			injectionBinder.Bind<global::strange.extensions.command.api.ICommandBinder>().To<global::strange.extensions.command.impl.SignalCommandBinder>().ToSingleton();
		}

		protected override void mapBindings()
		{
			base.mapBindings();
			injectionBinder.Bind<global::strange.extensions.context.api.ICrossContextCapable>().ToValue(this).ToName(global::Kampai.Util.BaseElement.CONTEXT);
			injectionBinder.Bind<global::strange.extensions.context.api.ICrossContextCapable>().ToValue(this).ToName(global::Kampai.Tools.AnimationToolKit.AnimationToolKitElement.CONTEXT);
			mapGameBindings();
			mapAnimationToolKitBindings();
		}

		private void mapGameBindings()
		{
			global::strange.extensions.injector.api.ICrossContextInjectionBinder crossContextInjectionBinder = injectionBinder;
			crossContextInjectionBinder.Bind<global::Kampai.Main.ILocalizationService>().To<global::Kampai.Main.HALService>();
			crossContextInjectionBinder.Bind<global::Kampai.Game.IDefinitionService>().To<global::Kampai.Game.DefinitionService>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Util.IRoutineRunner>().To<global::Kampai.Util.RoutineRunner>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Util.IInvokerService>().To<global::Kampai.Util.InvokerService>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Util.IMinionBuilder>().To<global::Kampai.Util.MinionBuilder>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Util.INamedCharacterBuilder>().To<global::Kampai.Util.NamedCharacterBuilder>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Game.IPlayerService>().To<global::Kampai.Tools.AnimationToolKit.AnimationToolKitPlayerService>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Common.IManifestService>().To<global::Kampai.Common.ManifestService>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Main.IAssetBundlesService>().To<global::Kampai.Main.AssetBundlesService>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Main.ILocalContentService>().To<global::Kampai.Main.LocalContentService>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Game.View.MinionIdleNotifier>().To<global::Kampai.Tools.AnimationToolKit.AnimationToolKitMinionManagerView>().ToSingleton();
			crossContextInjectionBinder.Bind<ILocalPersistanceService>().To<LocalPersistanceService>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Util.ICoroutineProgressMonitor>().To<global::Kampai.Util.CoroutineProgressMonitor>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Util.IUpdateRunner>().To<global::Kampai.Util.UpdateRunner>().ToSingleton();
			crossContextInjectionBinder.Bind<IEncryptionService>().To<EncryptionService>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Splash.IDownloadService>().To<global::Kampai.Tools.AnimationToolKit.AnimationToolKitDownloadService>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Game.IDLCService>().To<global::Kampai.Tools.AnimationToolKit.AnimationToolKitDLCService>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Common.ITelemetryService>().To<global::Kampai.Common.TelemetryService>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Common.ICoppaService>().To<global::Kampai.Common.CoppaService>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Common.IRandomService>().ToValue(new global::Kampai.Common.RandomService(0L));
			crossContextInjectionBinder.Bind<global::Kampai.Game.ITimeService>().To<global::Kampai.Game.TimeService>().ToSingleton();
			injectionBinder.Bind<string>().ToValue(global::Kampai.Util.GameConstants.Server.SERVER_ENVIRONMENT).ToName("game.server.environment");
			crossContextInjectionBinder.Bind<global::Kampai.Common.Service.Audio.IFMODService>().To<global::Kampai.Common.Service.Audio.FMODService>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Main.PlayGlobalSoundFXSignal>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Main.PlayGlobalSoundFXOneShotSignal>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Splash.SplashProgressUpdateSignal>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.UI.View.LogToScreenSignal>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Game.DefinitionsChangedSignal>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Tools.AnimationToolKit.SetMinionCountSignal>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Tools.AnimationToolKit.PlayMinionAnimationSignal>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Tools.AnimationToolKit.EnableInterfaceSignal>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Main.MainCompleteSignal>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Game.LoginUserSignal>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Tools.AnimationToolKit.MinionCreatedSignal>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Tools.AnimationToolKit.VillainCreatedSignal>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Tools.AnimationToolKit.CharacterCreatedSignal>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Game.TeleportCharacterToTikiBarSignal>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Game.MinionPartyAnimationSignal>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Main.PlayLocalAudioSignal>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Main.StopLocalAudioSignal>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Main.PlayLocalAudioOneShotSignal>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Main.PlayMinionStateAudioSignal>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Main.QueueLocalAudioCommandSignal>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Main.StartLoopingAudioSignal>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Game.PlayLocalAudioCommand>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Game.StopLocalAudioCommand>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Game.PlayLocalAudioOneShotCommand>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Game.Controller.Audio.PlayMinionStateAudioCommand>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Game.QueueLocalAudioCommand>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Game.StartLoopingAudioCommand>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Game.QuestScriptKernel>().ToSingleton();
			base.commandBinder.Bind<global::Kampai.Game.LoadDefinitionsSignal>().To<LoadDefinitionsCommand>();
			base.commandBinder.Bind<global::Kampai.Common.SetupManifestSignal>().To<global::Kampai.Tools.AnimationToolKit.LoadAnimationToolKitManifestCommand>();
			base.commandBinder.Bind<global::Kampai.Main.ReloadGameSignal>().To<global::Kampai.Main.ReloadGameCommand>();
			global::Kampai.Game.PlayLocalAudioCommand playLocalAudioCommand = injectionBinder.GetInstance<global::Kampai.Game.PlayLocalAudioCommand>();
			global::Kampai.Main.PlayLocalAudioSignal instance = injectionBinder.GetInstance<global::Kampai.Main.PlayLocalAudioSignal>();
			instance.AddListener(delegate(CustomFMOD_StudioEventEmitter emitter, string name, global::System.Collections.Generic.Dictionary<string, float> evtParams)
			{
				playLocalAudioCommand.Execute(emitter, name, evtParams);
			});
			global::Kampai.Game.StopLocalAudioCommand stopLocalAudioCommand = injectionBinder.GetInstance<global::Kampai.Game.StopLocalAudioCommand>();
			global::Kampai.Main.StopLocalAudioSignal instance2 = injectionBinder.GetInstance<global::Kampai.Main.StopLocalAudioSignal>();
			instance2.AddListener(delegate(CustomFMOD_StudioEventEmitter emitter)
			{
				stopLocalAudioCommand.Execute(emitter);
			});
			global::Kampai.Game.Controller.Audio.PlayMinionStateAudioCommand playMinionStateAudioCommand = injectionBinder.GetInstance<global::Kampai.Game.Controller.Audio.PlayMinionStateAudioCommand>();
			global::Kampai.Main.PlayMinionStateAudioSignal instance3 = injectionBinder.GetInstance<global::Kampai.Main.PlayMinionStateAudioSignal>();
			instance3.AddListener(delegate(MinionStateAudioArgs args)
			{
				playMinionStateAudioCommand.Execute(args);
			});
			global::Kampai.Game.PlayLocalAudioOneShotCommand playLocalAudioOneShotCommand = injectionBinder.GetInstance<global::Kampai.Game.PlayLocalAudioOneShotCommand>();
			global::Kampai.Main.PlayLocalAudioOneShotSignal instance4 = injectionBinder.GetInstance<global::Kampai.Main.PlayLocalAudioOneShotSignal>();
			instance4.AddListener(delegate(CustomFMOD_StudioEventEmitter emitter, string audioClip)
			{
				playLocalAudioOneShotCommand.Execute(emitter, audioClip);
			});
			global::Kampai.Main.QueueLocalAudioCommandSignal instance5 = injectionBinder.GetInstance<global::Kampai.Main.QueueLocalAudioCommandSignal>();
			global::Kampai.Game.QueueLocalAudioCommand queueLocalAudioCommand = injectionBinder.GetInstance<global::Kampai.Game.QueueLocalAudioCommand>();
			instance5.AddListener(delegate(CustomFMOD_StudioEventEmitter emitter, string audioClip)
			{
				queueLocalAudioCommand.Execute(emitter, audioClip);
			});
			global::Kampai.Game.StartLoopingAudioCommand startLoopingAudioCommand = injectionBinder.GetInstance<global::Kampai.Game.StartLoopingAudioCommand>();
			global::Kampai.Main.StartLoopingAudioSignal instance6 = injectionBinder.GetInstance<global::Kampai.Main.StartLoopingAudioSignal>();
			instance6.AddListener(delegate(CustomFMOD_StudioEventEmitter emitter, string name, global::System.Collections.Generic.Dictionary<string, float> evtParams)
			{
				startLoopingAudioCommand.Execute(emitter, name, evtParams);
			});
		}

		private void mapAnimationToolKitBindings()
		{
			global::strange.extensions.injector.api.ICrossContextInjectionBinder crossContextInjectionBinder = injectionBinder;
			crossContextInjectionBinder.Bind<global::Kampai.Tools.AnimationToolKit.AnimationToolKit>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Tools.AnimationToolKit.InitToggleSignal>().ToSingleton();
			crossContextInjectionBinder.Bind<global::Kampai.Tools.AnimationToolKit.AnimationToolkitModel>().ToSingleton();
			base.commandBinder.Bind<global::Kampai.Common.StartSignal>().To<global::Kampai.Tools.AnimationToolKit.LoadAnimationToolKitCommand>();
			base.commandBinder.Bind<global::Kampai.Tools.AnimationToolKit.LoadCameraSignal>().To<global::Kampai.Tools.AnimationToolKit.LoadCameraCommand>();
			base.commandBinder.Bind<global::Kampai.Tools.AnimationToolKit.LoadCanvasSignal>().To<global::Kampai.Tools.AnimationToolKit.LoadCanvasCommand>();
			base.commandBinder.Bind<global::Kampai.Tools.AnimationToolKit.LoadEventSystemSignal>().To<global::Kampai.Tools.AnimationToolKit.LoadEventSystemCommand>();
			base.commandBinder.Bind<global::Kampai.Tools.AnimationToolKit.LoadInterfaceSignal>().To<global::Kampai.Tools.AnimationToolKit.LoadInterfaceCommand>();
			base.commandBinder.Bind<global::Kampai.Tools.AnimationToolKit.LoadToggleGroupSignal>().To<global::Kampai.Tools.AnimationToolKit.LoadToggleGroupCommand>();
			base.commandBinder.Bind<global::Kampai.Tools.AnimationToolKit.LoadToggleSignal>().To<global::Kampai.Tools.AnimationToolKit.LoadToggleCommand>();
			base.commandBinder.Bind<global::Kampai.Tools.AnimationToolKit.GenerateMinionSignal>().To<global::Kampai.Tools.AnimationToolKit.GenerateMinionCommand>();
			base.commandBinder.Bind<global::Kampai.Tools.AnimationToolKit.GenerateVillainSignal>().To<global::Kampai.Tools.AnimationToolKit.GenerateVillainCommand>();
			base.commandBinder.Bind<global::Kampai.Tools.AnimationToolKit.GenerateCharacterSignal>().To<global::Kampai.Tools.AnimationToolKit.GenerateCharacterCommand>();
			base.commandBinder.Bind<global::Kampai.Tools.AnimationToolKit.AddMinionSignal>().To<global::Kampai.Tools.AnimationToolKit.AddMinionCommand>();
			base.commandBinder.Bind<global::Kampai.Tools.AnimationToolKit.AddCharacterSignal>().To<global::Kampai.Tools.AnimationToolKit.AddCharacterCommand>();
			base.commandBinder.Bind<global::Kampai.Tools.AnimationToolKit.RemoveMinionSignal>().To<global::Kampai.Tools.AnimationToolKit.RemoveMinionCommand>();
			base.commandBinder.Bind<global::Kampai.Tools.AnimationToolKit.RemoveCharacterSignal>().To<global::Kampai.Tools.AnimationToolKit.RemoveCharacterCommand>();
			base.commandBinder.Bind<global::Kampai.Tools.AnimationToolKit.ToggleInterfaceSignal>().To<global::Kampai.Tools.AnimationToolKit.ToggleInterfaceCommand>();
			base.commandBinder.Bind<global::Kampai.Tools.AnimationToolKit.ToggleMeshSignal>().To<global::Kampai.Tools.AnimationToolKit.ToggleMeshCommand>();
			base.mediationBinder.Bind<global::Kampai.Tools.AnimationToolKit.ToggleView>().To<global::Kampai.Tools.AnimationToolKit.ToggleMediator>();
			base.mediationBinder.Bind<global::Kampai.Tools.AnimationToolKit.AnimationToolKitButtonView>().To<global::Kampai.Tools.AnimationToolKit.AnimationToolKitButtonMediator>();
			base.mediationBinder.Bind<global::Kampai.Tools.AnimationToolKit.GachaButtonPanelView>().To<global::Kampai.Tools.AnimationToolKit.GachaButtonPanelMediator>();
		}
	}
}
