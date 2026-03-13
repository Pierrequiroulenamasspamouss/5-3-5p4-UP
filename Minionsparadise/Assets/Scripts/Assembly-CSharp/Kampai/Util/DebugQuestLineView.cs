namespace Kampai.Util
{
	public class DebugQuestLineView : global::Kampai.Util.KampaiView
	{
		public global::UnityEngine.UI.Text QuestLineInfo;

		public global::UnityEngine.UI.Image background;

		public global::UnityEngine.RectTransform Content;

		private global::Kampai.Game.QuestLine questLine;

		private float questWidth = 100f;

		private global::Kampai.Game.IDefinitionService defService;

		private global::System.Collections.Generic.List<global::Kampai.Util.QuestDebugButtonView> views = new global::System.Collections.Generic.List<global::Kampai.Util.QuestDebugButtonView>();

		public void AddQuestLine(global::Kampai.Game.QuestLine questLine, global::UnityEngine.GameObject prefab, global::Kampai.Game.IQuestService questService, global::Kampai.Game.IDefinitionService defService)
		{
			this.defService = defService;
			this.questLine = questLine;
			SetQuestLineState(questLine.state);
			global::System.Collections.Generic.IList<global::Kampai.Game.QuestDefinition> quests = questLine.Quests;
			int count = quests.Count;
			Content.sizeDelta = new global::UnityEngine.Vector2(questWidth * (float)count, 0f);
			for (int i = 0; i < count; i++)
			{
				global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(prefab);
				global::UnityEngine.RectTransform rectTransform = gameObject.transform as global::UnityEngine.RectTransform;
				rectTransform.SetParent(Content, false);
				rectTransform.anchoredPosition = new global::UnityEngine.Vector2((float)i * questWidth, 0f);
				rectTransform.sizeDelta = new global::UnityEngine.Vector2(questWidth, 0f);
				global::Kampai.Util.QuestDebugButtonView component = gameObject.GetComponent<global::Kampai.Util.QuestDebugButtonView>();
				component.AddQuest(quests[count - 1 - i], questService);
				views.Add(component);
			}
			SetQuestLineInfo();
		}

		public void UpdateQuestLine()
		{
			foreach (global::Kampai.Util.QuestDebugButtonView view in views)
			{
				view.UpdateQuestInfo();
			}
			SetQuestLineState(questLine.state);
			SetQuestLineInfo();
		}

		private void SetQuestLineInfo()
		{
			int questLineID = questLine.Quests[0].QuestLineID;
			string text = questLine.state.ToString();
			if (questLine.GivenByCharacterID != 0)
			{
				global::Kampai.Game.PrestigeDefinition prestigeDefinition = defService.Get<global::Kampai.Game.PrestigeDefinition>(questLine.GivenByCharacterID);
				if (prestigeDefinition != null)
				{
					SetText(string.Format("{0}\n{1} {2} \n{3}", questLineID, prestigeDefinition.LocalizedKey, questLine.GivenByCharacterPrestigeLevel, text));
				}
				else
				{
					SetText(string.Format("{0}\n{1}", questLineID, text));
				}
			}
			else
			{
				SetText(string.Format("{0}\n{1}", questLineID, text));
			}
		}

		private void SetQuestLineState(global::Kampai.Game.QuestLineState state)
		{
			switch (state)
			{
			case global::Kampai.Game.QuestLineState.NotStarted:
				background.color = new global::UnityEngine.Color(0f, 0f, 0f, 0.4f);
				break;
			case global::Kampai.Game.QuestLineState.Started:
				background.color = new global::UnityEngine.Color(0f, 1f, 0f, 0.4f);
				break;
			case global::Kampai.Game.QuestLineState.Finished:
				background.color = new global::UnityEngine.Color(1f, 1f, 1f, 0.4f);
				break;
			}
		}

		public void SetText(string text)
		{
			QuestLineInfo.text = text;
		}
	}
}
