namespace Kampai.Common
{
	public class SetupSupersonicCommand : global::strange.extensions.command.impl.Command
	{
		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SetupSupersonicCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IConfigurationsService configurationsService { get; set; }

		[Inject]
		public global::Kampai.Main.ISupersonicService supersonicService { get; set; }

		[Inject(global::Kampai.Main.MainElement.MANAGER_PARENT)]
		public global::UnityEngine.GameObject managers { get; set; }

		public override void Execute()
		{
			if (configurationsService.isKillSwitchOn(global::Kampai.Game.KillSwitch.SUPERSONIC))
			{
				logger.Info("Supersonic is disabled by kill switch.");
				return;
			}
			global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("Supersonic");
			gameObject.transform.parent = managers.transform;
			logger.Debug("SetupSupersonicCommand.Execute supersonic object added to scene");
			gameObject.AddComponent<SupersonicEvents>();
			supersonicService.Init();
		}
	}
}
