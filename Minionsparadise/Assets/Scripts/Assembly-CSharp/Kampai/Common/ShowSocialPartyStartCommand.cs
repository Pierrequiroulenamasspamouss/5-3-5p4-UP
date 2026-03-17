namespace Kampai.Common
{
	public class ShowSocialPartyStartCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.ITimedSocialEventService timedSocialEventService { get; set; }

		[Inject]
		public global::Kampai.Common.NetworkConnectionLostSignal networkConnectionLostSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowSocialPartyFillOrderSignal showSocialPartyFillOrderSignal { get; set; }

		public override void Execute()
		{
			global::strange.extensions.signal.impl.Signal<global::Kampai.Game.SocialTeamResponse, global::Kampai.Game.ErrorResponse> signal = new global::strange.extensions.signal.impl.Signal<global::Kampai.Game.SocialTeamResponse, global::Kampai.Game.ErrorResponse>();
			signal.AddListener(OnCreateTeamResponse);
			timedSocialEventService.CreateSocialTeam(timedSocialEventService.GetCurrentSocialEvent().ID, signal);
		}

		public void OnCreateTeamResponse(global::Kampai.Game.SocialTeamResponse response, global::Kampai.Game.ErrorResponse error)
		{
			if (error != null)
			{
				networkConnectionLostSignal.Dispatch();
			}
			else
			{
				showSocialPartyFillOrderSignal.Dispatch(0);
			}
		}
	}
}
