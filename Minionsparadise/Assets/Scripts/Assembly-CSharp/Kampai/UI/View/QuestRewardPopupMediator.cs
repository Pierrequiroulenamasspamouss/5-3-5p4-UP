namespace Kampai.UI.View
{
	public class QuestRewardPopupMediator : global::strange.extensions.mediation.impl.Mediator
	{
		[Inject]
		public global::Kampai.UI.View.QuestRewardPopupView view { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.UI.View.QuestRewardPopupContentsSignal popupContentsSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideRewardDisplaySignal hideRewardDisplaySignal { get; set; }

		public override void OnRegister()
		{
			view.ConfirmButton.ClickedSignal.AddListener(Close);
			popupContentsSignal.AddListener(PopulateView);
			hideRewardDisplaySignal.AddListener(Close);
			ToggleOpen(false);
		}

		public override void OnRemove()
		{
			view.ConfirmButton.ClickedSignal.RemoveListener(Close);
			popupContentsSignal.RemoveListener(PopulateView);
			hideRewardDisplaySignal.RemoveListener(Close);
		}

		private void ToggleOpen(bool open)
		{
			view.PlayAnim(open);
		}

		private void PopulateView(global::System.Collections.Generic.List<global::Kampai.Game.DisplayableDefinition> itemDefs)
		{
			ToggleOpen(true);
			view.Init(itemDefs, localService);
		}

		private void Close()
		{
			ToggleOpen(false);
		}
	}
}
