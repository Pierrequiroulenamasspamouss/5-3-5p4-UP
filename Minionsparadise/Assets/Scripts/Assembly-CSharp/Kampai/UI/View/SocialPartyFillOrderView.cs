namespace Kampai.UI.View
{
	public class SocialPartyFillOrderView : global::Kampai.UI.View.PopupMenuView
	{
		public global::Kampai.UI.View.ButtonView leaveTeamButton;

		public global::Kampai.UI.View.ButtonView messageAlertButton;

		public global::Kampai.UI.View.ButtonView teamPanelButton;

		public global::Kampai.UI.View.ButtonView closeTeamButton;

		public global::UnityEngine.GameObject SocialFillOrderButtonContainer;

		public global::UnityEngine.GameObject SocialFillTeamPanel;

		public global::UnityEngine.Animator teamPanelAnimator;

		public global::UnityEngine.UI.Text premiumRewardText;

		public global::UnityEngine.UI.Text grindRewardText;

		public global::UnityEngine.UI.Text leaveTeamButtonText;

		public global::UnityEngine.UI.Text teamTitle;

		public global::UnityEngine.UI.Text timeRemaining;

		public global::UnityEngine.UI.Text ordersRemaining;

		public global::UnityEngine.UI.Text teamOrderBoardText;

		public global::UnityEngine.UI.Text descriptionText;

		public global::UnityEngine.UI.Image progressBar;

		public global::UnityEngine.UI.Text teamButtonText;

		public global::UnityEngine.UI.Text questTitle;

		public global::strange.extensions.signal.impl.Signal<bool> FillOrderSignal = new global::strange.extensions.signal.impl.Signal<bool>();

		public override void Init()
		{
			base.Init();
			base.Open();
		}

		internal void OpenTeam()
		{
			teamPanelAnimator.Play("Open");
		}

		internal void CloseTeam()
		{
			teamPanelAnimator.Play("Close");
		}
	}
}
