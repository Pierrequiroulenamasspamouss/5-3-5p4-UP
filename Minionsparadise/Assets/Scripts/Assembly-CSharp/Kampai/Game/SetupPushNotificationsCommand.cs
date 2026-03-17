namespace Kampai.Game
{
	public class SetupPushNotificationsCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SetupPushNotificationsCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IPushNotificationService pushNotificationService { get; set; }

		[Inject]
		public global::Kampai.Game.IUserSessionService userSessionService { get; set; }

		[Inject]
		public global::Kampai.Common.ICoppaService coppaService { get; set; }

		public override void Execute()
		{
			logger.Debug("[PN] SetupPushNotificationsCommand: prepare args for PN service");
			global::Kampai.Game.UserSession userSession = userSessionService.UserSession;
			if (userSession == null)
			{
				logger.Error("[PN] User session is null");
				return;
			}
			string userID = userSession.UserID;
			if (string.IsNullOrEmpty(userID))
			{
				logger.Error("[PN] User alias is invalid");
				return;
			}
			global::System.DateTime birthdate;
			if (!coppaService.GetBirthdate(out birthdate))
			{
				logger.Info("[PN] Coppa birthdate is unknown at the moment");
				return;
			}
			logger.Debug("[PN] Start push notification service.");
			pushNotificationService.Start(userID, birthdate.Year, birthdate.Month);
		}
	}
}
