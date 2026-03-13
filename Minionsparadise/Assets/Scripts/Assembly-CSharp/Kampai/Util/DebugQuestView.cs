namespace Kampai.Util
{
	public class DebugQuestView : global::Kampai.Util.KampaiView
	{
		public global::UnityEngine.RectTransform content;

		private global::UnityEngine.GameObject QuestLinePrefab;

		private global::UnityEngine.GameObject QuestButtonPrefab;

		private float questLineHeight = 80f;

		private global::System.Collections.Generic.List<global::Kampai.Util.DebugQuestLineView> views = new global::System.Collections.Generic.List<global::Kampai.Util.DebugQuestLineView>();

		private bool show;

		private bool questLineSetup;

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService defService { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		protected override void Start()
		{
			base.Start();
			global::UnityEngine.RectTransform rectTransform = base.transform as global::UnityEngine.RectTransform;
			rectTransform.offsetMin = global::UnityEngine.Vector2.zero;
			rectTransform.offsetMax = global::UnityEngine.Vector2.zero;
			QuestLinePrefab = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.GameObject>("QuestLineDebugPanel");
			QuestButtonPrefab = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.GameObject>("QuestDebugButton");
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.UpdateQuestWorldIconsSignal>().AddListener(UpdateQuestState);
			base.gameObject.SetActive(false);
		}

		public void Toggle()
		{
			if (!questLineSetup)
			{
				SetupQuestLines();
				questLineSetup = true;
			}
			if (show)
			{
				gameContext.injectionBinder.GetInstance<global::Kampai.Game.UpdateQuestWorldIconsSignal>().RemoveListener(UpdateQuestState);
				base.gameObject.SetActive(false);
				show = false;
			}
			else
			{
				gameContext.injectionBinder.GetInstance<global::Kampai.Game.UpdateQuestWorldIconsSignal>().AddListener(UpdateQuestState);
				base.gameObject.SetActive(true);
				show = true;
			}
		}

		private void UpdateQuestState(global::Kampai.Game.Quest quest)
		{
			UpdateQuestLines();
		}

		private void UpdateQuestLines()
		{
			foreach (global::Kampai.Util.DebugQuestLineView view in views)
			{
				view.UpdateQuestLine();
			}
		}

		private void SetupQuestLines()
		{
			global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.QuestLine> questLines = questService.GetQuestLines();
			int count = questLines.Count;
			content.sizeDelta = new global::UnityEngine.Vector2(0f, questLineHeight * (float)count);
			int num = 0;
			foreach (global::System.Collections.Generic.KeyValuePair<int, global::Kampai.Game.QuestLine> item in questLines)
			{
				global::Kampai.Game.QuestLine value = item.Value;
				global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(QuestLinePrefab);
				global::UnityEngine.RectTransform rectTransform = gameObject.transform as global::UnityEngine.RectTransform;
				rectTransform.SetParent(content, false);
				rectTransform.anchoredPosition = new global::UnityEngine.Vector2(0f, (0f - questLineHeight) * (float)num);
				rectTransform.sizeDelta = new global::UnityEngine.Vector2(0f, questLineHeight);
				global::Kampai.Util.DebugQuestLineView component = gameObject.GetComponent<global::Kampai.Util.DebugQuestLineView>();
				component.AddQuestLine(value, QuestButtonPrefab, questService, defService);
				views.Add(component);
				num++;
			}
		}
	}
}
