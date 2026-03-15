namespace Kampai.Game
{
	public class SocialLogoutCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SocialLogoutCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.ISocialService socialService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.UI.View.PopupMessageSignal popupMessageSignal { get; set; }

		[Inject]
		public global::Kampai.Game.UpdateMarketplaceSlotStateSignal updateSlotStateSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.SocialServices type = socialService.type;
			logger.Debug("Social Logout Command Called With {0}", type.ToString());
			socialService.Logout();
			switch (type)
			{
			case global::Kampai.Game.SocialServices.FACEBOOK:
			{
				string type3 = localService.GetString("fbLogoutSuccess");
				popupMessageSignal.Dispatch(type3, global::Kampai.UI.View.PopupMessageType.NORMAL);
				updateSlotStateSignal.Dispatch();
				break;
			}
			case global::Kampai.Game.SocialServices.GOOGLEPLAY:
			{
				string type2 = localService.GetString("googleplaylogoutsuccess");
				popupMessageSignal.Dispatch(type2, global::Kampai.UI.View.PopupMessageType.NORMAL);
				break;
			}
			}
		}
	}
}
