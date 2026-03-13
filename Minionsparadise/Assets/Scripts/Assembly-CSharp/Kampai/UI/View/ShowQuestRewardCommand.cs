namespace Kampai.UI.View
{
	public class ShowQuestRewardCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int questInstanceID { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseAllOtherMenuSignal closeSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowHUDSignal showHUDSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowStoreSignal showStoreSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IMasterPlanQuestService masterPlanQuestService { get; set; }

		public override void Execute()
		{
			closeSignal.Dispatch(null);
			global::Kampai.Game.Quest questByInstanceId = masterPlanQuestService.GetQuestByInstanceId(questInstanceID);
			if (questByInstanceId != null)
			{
				global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Queue, "popup_QuestReward");
				iGUICommand.skrimScreen = "QuestRewardSkrim";
				iGUICommand.darkSkrim = true;
				iGUICommand.Args.Add(questInstanceID);
				guiService.Execute(iGUICommand);
				playSFXSignal.Dispatch("Play_menu_popUp_01");
				showHUDSignal.Dispatch(true);
				showStoreSignal.Dispatch(true);
				if ((questByInstanceId != null && questByInstanceId.state == global::Kampai.Game.QuestState.Complete) || questByInstanceId.state == global::Kampai.Game.QuestState.Harvestable)
				{
					gameContext.injectionBinder.GetInstance<global::Kampai.Game.RemoveQuestWorldIconSignal>().Dispatch(questByInstanceId);
				}
			}
		}
	}
}
