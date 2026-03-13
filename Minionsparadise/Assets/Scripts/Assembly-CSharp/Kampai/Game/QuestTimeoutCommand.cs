namespace Kampai.Game
{
	public class QuestTimeoutCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateQuestBookBadgeSignal updateBadgeSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.PopupMessageSignal messageSignal { get; set; }

		[Inject]
		public global::Kampai.Game.RemoveQuestWorldIconSignal removeQuestIconSignal { get; set; }

		[Inject]
		public global::Kampai.Game.UpdateQuestWorldIconsSignal updateQuestWorldIconSignal { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public int questId { get; set; }

		public override void Execute()
		{
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "screen_QuestPanel");
			updateBadgeSignal.Dispatch();
			string type = localService.GetString("QuestTimeout");
			messageSignal.Dispatch(type, global::Kampai.UI.View.PopupMessageType.NORMAL);
			global::Kampai.Game.Quest byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Quest>(questId);
			if (byInstanceId == null)
			{
				return;
			}
			global::Kampai.Game.TimedQuestDefinition timedQuestDefinition = byInstanceId.GetActiveDefinition() as global::Kampai.Game.TimedQuestDefinition;
			if (timedQuestDefinition != null)
			{
				if (timedQuestDefinition.Repeat)
				{
					byInstanceId.Clear();
					updateQuestWorldIconSignal.Dispatch(byInstanceId);
				}
				else
				{
					global::Kampai.Game.IQuestController questControllerByDefinitionID = questService.GetQuestControllerByDefinitionID(byInstanceId.GetActiveDefinition().ID);
					if (questControllerByDefinitionID != null)
					{
						questControllerByDefinitionID.GoToQuestState(global::Kampai.Game.QuestState.Complete);
					}
					removeQuestIconSignal.Dispatch(byInstanceId);
				}
			}
			global::Kampai.Game.LimitedQuestDefinition limitedQuestDefinition = byInstanceId.GetActiveDefinition() as global::Kampai.Game.LimitedQuestDefinition;
			if (limitedQuestDefinition != null)
			{
				questService.RemoveQuest(byInstanceId.GetActiveDefinition().ID);
				removeQuestIconSignal.Dispatch(byInstanceId);
			}
		}
	}
}
