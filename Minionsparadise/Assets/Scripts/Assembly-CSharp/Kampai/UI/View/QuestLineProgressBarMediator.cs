namespace Kampai.UI.View
{
	public class QuestLineProgressBarMediator : global::strange.extensions.mediation.impl.Mediator
	{
		[Inject]
		public global::Kampai.UI.View.QuestLineProgressBarView view { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateQuestPanelWithNewQuestSignal updateQuestPanelSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateQuestLineProgressSignal updateQuestLineProgressSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.QuestDetailIDSignal idSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IMasterPlanQuestService masterPlanQuestService { get; set; }

		public override void OnRegister()
		{
			updateQuestLineProgressSignal.AddListener(OnQuestSelected);
			updateQuestPanelSignal.AddListener(OnQuestSelected);
			idSignal.AddListener(OnQuestSelected);
		}

		public override void OnRemove()
		{
			updateQuestLineProgressSignal.RemoveListener(OnQuestSelected);
			updateQuestPanelSignal.RemoveListener(OnQuestSelected);
			idSignal.RemoveListener(OnQuestSelected);
		}

		private void OnQuestSelected(int questId)
		{
			global::Kampai.Game.Quest questByInstanceId = masterPlanQuestService.GetQuestByInstanceId(questId);
			int questLineID = questByInstanceId.GetActiveDefinition().QuestLineID;
			string title = localService.GetString("QuestLineTitle3");
			if (questLineID == 0)
			{
				view.UpdateProgress(0, 1);
				view.SetTitle(title);
				return;
			}
			global::Kampai.Game.PrestigeDefinition definition = null;
			if (definitionService.TryGet<global::Kampai.Game.PrestigeDefinition>(questByInstanceId.GetActiveDefinition().SurfaceID, out definition))
			{
				string text = localService.GetString(definition.LocalizedKey);
				title = localService.GetString("QuestLineTitle2", text);
			}
			view.SetTitle(title);
			global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.QuestLine> questLines = questService.GetQuestLines();
			global::Kampai.Game.QuestLine questLine = questLines[questLineID];
			int totalCount = 0;
			int completeCount = 0;
			CheckQuestProgress(questLine, questByInstanceId.GetActiveDefinition().ID, out completeCount, out totalCount);
			completeCount += ((questByInstanceId.state == global::Kampai.Game.QuestState.Complete) ? 1 : 0);
			view.UpdateProgress(completeCount, totalCount);
		}

		private void CheckQuestProgress(global::Kampai.Game.QuestLine questLine, int questDefinitionID, out int completeCount, out int totalCount)
		{
			bool flag = false;
			totalCount = 0;
			completeCount = 0;
			for (int num = questLine.Quests.Count - 1; num >= 0; num--)
			{
				global::Kampai.Game.QuestDefinition questDefinition = questLine.Quests[num];
				if (questDefinition.ID == questDefinitionID)
				{
					flag = true;
				}
				if (questDefinition.QuestVersion != -1 && questDefinition.SurfaceType != global::Kampai.Game.QuestSurfaceType.Automatic)
				{
					if (!flag)
					{
						completeCount++;
					}
					totalCount++;
				}
			}
		}
	}
}
