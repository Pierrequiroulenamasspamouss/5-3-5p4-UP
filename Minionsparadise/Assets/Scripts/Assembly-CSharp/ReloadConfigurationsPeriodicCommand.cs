public class ReloadConfigurationsPeriodicCommand : global::strange.extensions.command.impl.Command
{
	private static int CONFIG_RELOAD_INTERVAL = 120;

	public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("ReloadConfigurationsPeriodicCommand") as global::Kampai.Util.IKampaiLogger;

	[Inject]
	public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

	[Inject]
	public global::Kampai.Game.LoadConfigurationSignal loadConfigurationSignal { get; set; }

	public override void Execute()
	{
		logger.Debug("Scheduling reload of configs");
		routineRunner.StartCoroutine(PeriodicReloadConfigs());
	}

	private global::System.Collections.IEnumerator PeriodicReloadConfigs()
	{
		while (true)
		{
			yield return new global::UnityEngine.WaitForSeconds(CONFIG_RELOAD_INTERVAL);
			logger.Info("ReloadConfigurationsPeriodicCommand: Reloading configurations!");
			loadConfigurationSignal.Dispatch(false);
		}
	}
}
