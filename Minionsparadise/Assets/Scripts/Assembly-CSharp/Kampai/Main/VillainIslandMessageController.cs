namespace Kampai.Main
{
	public class VillainIslandMessageController : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public bool showMessage { get; set; }

		[Inject]
		public global::Kampai.UI.View.UIModel model { get; set; }

		[Inject]
		public global::Kampai.UI.View.PopupMessageSignal popupMessageSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal sfxSignal { get; set; }

		[Inject]
		public global::Kampai.Game.EnableVillainIslandCollidersSignal enableVillainIslandCollidersSignal { get; set; }

		public override void Execute()
		{
			int quantity = (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID);
			int num = 0;
			global::Kampai.Game.QuestDefinition questDefinition = definitionService.Get<global::Kampai.Game.QuestDefinition>(3849290);
			if (questDefinition != null)
			{
				num = questDefinition.UnlockLevel;
			}
			global::Kampai.Game.IQuestController questControllerByDefinitionID = questService.GetQuestControllerByDefinitionID(3849290);
			if (quantity < num && showMessage)
			{
				PopupMessage(localService.GetString("AspirationalMessageVillainIsland", num));
			}
			else if ((questControllerByDefinitionID != null && questControllerByDefinitionID.State != global::Kampai.Game.QuestState.Notstarted) || questService.IsQuestCompleted(3849290))
			{
				enableVillainIslandCollidersSignal.Dispatch(false);
			}
			else if (showMessage)
			{
				PopupMessage(localService.GetString("UnlockKevinForVillainIsland", num));
			}
		}

		private void PopupMessage(string message)
		{
			if (!model.BuildingDragMode)
			{
				popupMessageSignal.Dispatch(message, global::Kampai.UI.View.PopupMessageType.NORMAL);
				sfxSignal.Dispatch("Play_action_locked_01");
			}
		}
	}
}
