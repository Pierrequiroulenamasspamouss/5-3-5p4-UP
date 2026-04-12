namespace Kampai.Game
{
	public class SocialInitSuccessCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SocialInitSuccessCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IUserSessionService userSessionService { get; set; }

		[Inject]
		public global::Kampai.Game.ISocialService socialService { get; set; }

		[Inject]
		public global::Kampai.Game.LinkAccountSignal linkAccountSignal { get; set; }

		[Inject]
		public global::Kampai.Common.Service.HealthMetrics.IClientHealthService clientHealth { get; set; }

		[Inject]
		public global::Kampai.Main.DisplayHindsightContentSignal displayHindsightContentSignal { get; set; }

		[Inject]
		public ILocalPersistanceService localPersistanceService { get; set; }

		[Inject]
		public global::Kampai.Game.UpdateMarketplaceSlotStateSignal updateMarketplaceSlotSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.SocialServices type = socialService.type;
			switch (type)
			{
			case global::Kampai.Game.SocialServices.FACEBOOK:
				clientHealth.MarkMeterEvent("External.Discord.Login");
				break;
			case global::Kampai.Game.SocialServices.GAMECENTER:
				clientHealth.MarkMeterEvent("External.GameCenter.Login");
				break;
			case global::Kampai.Game.SocialServices.GOOGLEPLAY:
				clientHealth.MarkMeterEvent("External.Google.Login");
				break;
			}
			logger.Debug("In {0} Init Success", type.ToString());
			if (userSessionService.UserSession != null)
			{
				updateMarketplaceSlotSignal.Dispatch();
				CheckLoggedIn(type);
				if (!localPersistanceService.GetData("HindsightTriggeredAtGameLaunch").Equals("True"))
				{
					displayHindsightContentSignal.Dispatch(global::Kampai.Main.HindsightCampaign.Scope.game_launch);
					localPersistanceService.PutData("HindsightTriggeredAtGameLaunch", "True");
				}
			}
		}

		private void CheckLoggedIn(global::Kampai.Game.SocialServices socialType)
		{
			bool flag = false;
			if (!socialService.isLoggedIn)
			{
				return;
			}
			if (string.IsNullOrEmpty(socialService.LoginSource))
			{
				socialService.SendLoginTelemetry("Automatic");
			}
			else
			{
				socialService.SendLoginTelemetry(socialService.LoginSource);
			}
			logger.Debug("{0} Logged into looking into links", socialType.ToString());
			foreach (global::Kampai.Game.UserIdentity socialIdentity in userSessionService.UserSession.SocialIdentities)
			{
				if (socialIdentity.ExternalID == socialService.userID)
				{
					return;
				}
			}
			foreach (global::Kampai.Game.UserIdentity socialIdentity2 in userSessionService.UserSession.SocialIdentities)
			{
				if (socialIdentity2.Type.ToString().ToLower().Equals(socialType.ToString().ToLower()))
				{
					flag = true;
					if (socialIdentity2.ExternalID != socialService.userID)
					{
						LinkAccount(socialType);
					}
					return;
				}
			}
			if (!flag)
			{
				LinkAccount(socialType);
			}
		}

		private void LinkAccount(global::Kampai.Game.SocialServices socialType)
		{
			logger.Debug("Calling link from {0} Init", socialType.ToString());
			// If LoginSource is not empty, it's a manual login from settings/social menu, so we should reload
			bool shouldReload = !string.IsNullOrEmpty(socialService.LoginSource);
			linkAccountSignal.Dispatch(socialService, shouldReload);
		}
	}
}
