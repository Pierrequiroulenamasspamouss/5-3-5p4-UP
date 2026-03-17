namespace Kampai.Util
{
	public class QuestDebugButtonView : global::Kampai.UI.View.ButtonView
	{
		public global::UnityEngine.UI.Text ButtonName;

		public global::UnityEngine.UI.Image Background;

		private global::Kampai.Game.IQuestService questService;

		private global::Kampai.Game.QuestDefinition questDefinition;

		private global::Kampai.Game.IQuestController questController;

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		public override void OnClickEvent()
		{
			if (questController != null)
			{
				questController.GoToQuestState(global::Kampai.Game.QuestState.Complete);
				return;
			}
			global::Kampai.Game.Quest quest = new global::Kampai.Game.Quest(questDefinition);
			quest.Initialize();
			quest.state = global::Kampai.Game.QuestState.Complete;
			questController = questService.AddQuest(quest);
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.GetNewQuestSignal>().Dispatch();
			SetState(questController);
		}

		public void UpdateQuestInfo()
		{
			global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.IQuestController> questMap = questService.GetQuestMap();
			int iD = questDefinition.ID;
			if (!questMap.ContainsKey(iD))
			{
				Background.color = new global::UnityEngine.Color(0f, 0f, 0f, 0.4f);
				SetText(string.Format("{0}\n{1}", iD.ToString(), "Doesn't exist"));
			}
			else
			{
				questController = questMap[iD];
				SetState(questController);
			}
		}

		public void AddQuest(global::Kampai.Game.QuestDefinition questDefinition, global::Kampai.Game.IQuestService questService)
		{
			this.questService = questService;
			this.questDefinition = questDefinition;
			global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.IQuestController> questMap = questService.GetQuestMap();
			int iD = questDefinition.ID;
			if (!questMap.ContainsKey(iD))
			{
				Background.color = new global::UnityEngine.Color(0f, 0f, 0f, 0.4f);
				SetText(string.Format("{0}\n{1}", iD.ToString(), "Doesn't exist"));
			}
			else
			{
				questController = questMap[iD];
				SetState(questController);
			}
		}

		private void SetState(global::Kampai.Game.IQuestController questCon)
		{
			switch (questCon.State)
			{
			case global::Kampai.Game.QuestState.Notstarted:
				Background.color = global::UnityEngine.Color.gray;
				break;
			case global::Kampai.Game.QuestState.RunningStartScript:
			case global::Kampai.Game.QuestState.RunningTasks:
			case global::Kampai.Game.QuestState.RunningCompleteScript:
				Background.color = global::UnityEngine.Color.green;
				break;
			case global::Kampai.Game.QuestState.Complete:
				Background.color = global::UnityEngine.Color.blue;
				break;
			}
			SetText(string.Format("{0}\n{1}", questCon.Definition.ID.ToString(), questCon.State.ToString()));
		}

		public void SetText(string text)
		{
			ButtonName.text = text;
		}
	}
}
