namespace Kampai.UI.View
{
	public class MIBRewardMediator : global::strange.extensions.mediation.impl.Mediator
	{
		[Inject]
		public global::Kampai.UI.View.MIBRewardView view { get; set; }

		[Inject]
		public global::Kampai.Game.GrantMIBRewardsSignal grantMIBRewardsSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSkrimSignal { get; set; }

		public override void OnRegister()
		{
			view.mibRewardAnimationCompleteSignal.AddListener(OnAnimationComplete);
		}

		public override void OnRemove()
		{
			view.mibRewardAnimationCompleteSignal.RemoveListener(OnAnimationComplete);
		}

		private void OnAnimationComplete(global::Kampai.Game.Transaction.TransactionDefinition pickedTransactionDef, global::UnityEngine.Vector3 tweenLocation)
		{
			grantMIBRewardsSignal.Dispatch(global::Kampai.Game.MIBRewardType.ON_RETURN, pickedTransactionDef, tweenLocation);
			hideSkrimSignal.Dispatch("MIBRewardScreenSkrim");
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "screen_MessageInABottle");
		}
	}
}
