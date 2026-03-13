namespace Kampai.Game
{
	public class CheckUpgradeCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("CheckUpgradeCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IConfigurationsService configService { get; set; }

		[Inject]
		public global::Kampai.Game.IUserSessionService userSessionService { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowClientUpgradeDialogSignal showClientUpgradeDialogSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowForcedClientUpgradeScreenSignal showForcedClientUpgradeScreenSignal { get; set; }

		[Inject]
		public global::Kampai.Util.IClientVersion clientVersionService { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.ConfigurationDefinition configurations = configService.GetConfigurations();
			string clientVersion = clientVersionService.GetClientVersion();
			logger.Info("CheckUpgradeCommand for '{0}'", clientVersion);
			if (global::Kampai.Util.GameConstants.StaticConfig.DEBUG_ENABLED && clientVersion.Equals("0"))
			{
				return;
			}
			if (clientVersion == null)
			{
				logger.Info("CheckUpgradeCommand: Client version is null");
			}
			if (configurations.isAllowed)
			{
				logger.Log(global::Kampai.Util.KampaiLogLevel.Info, "CheckUpgradeCommand: This client version is allowed... carry on");
			}
			else if (configurations.isNudgeAllowed)
			{
				logger.Info("CheckUpgradeCommand: Client version is a nudge upgrade version...");
				int num = configurations.nudgeUpgradePercentage;
				if (num == 0)
				{
					num = 100;
				}
				global::Kampai.Game.UserSession userSession = userSessionService.UserSession;
				if (userSession != null)
				{
					string s = userSession.UserID.Substring(userSession.UserID.Length - 2, 2);
					int result = 0;
					if (!int.TryParse(s, out result))
					{
						result = 0;
					}
					if (result <= num)
					{
						logger.Log(global::Kampai.Util.KampaiLogLevel.Info, "CheckUpgradeCommand: Going to Nudge player to update");
						showClientUpgradeDialogSignal.Dispatch();
					}
				}
			}
			else
			{
				logger.Log(global::Kampai.Util.KampaiLogLevel.Info, "CheckUpgradeCommand: Going to force client to upgrade");
				showForcedClientUpgradeScreenSignal.Dispatch();
			}
		}
	}
}
