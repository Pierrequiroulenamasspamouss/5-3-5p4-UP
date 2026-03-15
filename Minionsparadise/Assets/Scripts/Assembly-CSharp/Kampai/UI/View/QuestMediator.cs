namespace Kampai.UI.View
{
	public class QuestMediator : global::strange.extensions.mediation.impl.Mediator
	{
		[Inject]
		public global::Kampai.UI.View.QuestView view { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateQuestPanelWithNewQuestSignal updateQuestPanelSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal globalSFX { get; set; }

		[Inject]
		public global::Kampai.UI.View.QuestUIModel questUIModel { get; set; }

		[Inject]
		public global::Kampai.UI.IFancyUIService fancyUIService { get; set; }

		public override void OnRegister()
		{
			view.button.ClickedSignal.AddListener(Clicked);
			view.Init(fancyUIService);
		}

		public override void OnRemove()
		{
			view.button.ClickedSignal.RemoveListener(Clicked);
		}

		private void Clicked()
		{
			if (questUIModel.lastSelectedQuestID != view.quest.ID)
			{
				StartCoroutine(WaitAFrame());
			}
		}

		private global::System.Collections.IEnumerator WaitAFrame()
		{
			yield return null;
			globalSFX.Dispatch("Play_button_click_01");
			updateQuestPanelSignal.Dispatch(view.quest.ID);
		}
	}
}
