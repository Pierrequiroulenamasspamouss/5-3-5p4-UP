namespace Kampai.UI.View
{
	public class RewardReminderHUDMediator : global::strange.extensions.mediation.impl.EventMediator
	{
		[Inject]
		public global::Kampai.UI.View.RewardReminderHUDView view { get; set; }

		[Inject]
		public global::Kampai.UI.IPositionService positionService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		[Inject]
		public global::Kampai.UI.View.PopupMessageSignal popupMessageSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowHUDReminderSignal showReminderSignal { get; set; }

		public override void OnRegister()
		{
			view.Init(positionService);
			view.imageButton.ClickedSignal.AddListener(ClickedReminder);
			showReminderSignal.AddListener(view.ShowReminder);
		}

		public override void OnRemove()
		{
			view.imageButton.ClickedSignal.RemoveListener(ClickedReminder);
			showReminderSignal.RemoveListener(view.ShowReminder);
		}

		private void ClickedReminder()
		{
			if (view.pendingRewardDef != null)
			{
				popupMessageSignal.Dispatch(localizationService.GetString(view.pendingRewardDef.aspirationalLocKey, view.pendingRewardDef.awardAtLevel), global::Kampai.UI.View.PopupMessageType.NORMAL);
			}
		}
	}
}
