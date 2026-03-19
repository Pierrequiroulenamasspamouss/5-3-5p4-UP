namespace Kampai.Common
{
	public class ShowSocialPartyInviteCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("ShowSocialPartyInviteCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimedSocialEventService timedSocialEventService { get; set; }

		[Inject(global::Kampai.Game.SocialServices.FACEBOOK)]
		public global::Kampai.Game.ISocialService facebookService { get; set; }

		[Inject]
		public global::Kampai.Common.NetworkConnectionLostSignal networkConnectionLostSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SocialPartyFillOrderSetupUISignal socialPartyFillOrderSetupUISignal { get; set; }

		public override void Execute()
		{
			if (facebookService.isLoggedIn)
			{
				global::strange.extensions.signal.impl.Signal<global::System.Collections.Generic.List<string>> signal = new global::strange.extensions.signal.impl.Signal<global::System.Collections.Generic.List<string>>();
				global::strange.extensions.signal.impl.Signal<global::System.Collections.Generic.List<string>> signal2 = new global::strange.extensions.signal.impl.Signal<global::System.Collections.Generic.List<string>>();
				signal.AddListener(OnFBInviteSuccess);
				signal2.AddListener(OnFBInviteFailure);
				((global::Kampai.Game.FacebookService)facebookService).FriendInvite(localService.GetString("socialpartyjointeamdescription"), localService.GetString("socialpartyjointeamtitle"), string.Empty, 4, signal, signal2);
			}
		}

		private void OnFBInviteFailure(global::System.Collections.Generic.IList<string> to)
		{
			logger.Debug("OnFBInviteFailure");
		}

		private void OnFBInviteSuccess(global::System.Collections.Generic.IList<string> to)
		{
			logger.Debug("OnFBInviteSuccess");
			if (to == null)
			{
				logger.Error("No List to send to");
				return;
			}
			int eventId = timedSocialEventService.GetSocialEventStateCached(timedSocialEventService.GetCurrentSocialEvent().ID).EventId;
			long iD = timedSocialEventService.GetSocialEventStateCached(timedSocialEventService.GetCurrentSocialEvent().ID).Team.ID;
			global::strange.extensions.signal.impl.Signal<global::Kampai.Game.SocialTeamResponse, global::Kampai.Game.ErrorResponse> signal = new global::strange.extensions.signal.impl.Signal<global::Kampai.Game.SocialTeamResponse, global::Kampai.Game.ErrorResponse>();
			signal.AddListener(OnInviteSuccess);
			timedSocialEventService.InviteFriends(eventId, iD, global::Kampai.Game.IdentityType.discord, to, signal);
		}

		public void OnInviteSuccess(global::Kampai.Game.SocialTeamResponse response, global::Kampai.Game.ErrorResponse error)
		{
			if (error != null)
			{
				networkConnectionLostSignal.Dispatch();
				return;
			}
			if (response == null || response.Team == null)
			{
				logger.Warning("OnInviteSuccess has no team in the response");
			}
			socialPartyFillOrderSetupUISignal.Dispatch(response.Team);
		}
	}
}
