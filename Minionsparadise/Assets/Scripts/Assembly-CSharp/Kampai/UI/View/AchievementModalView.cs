using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kampai.UI.View
{
	public class AchievementModalView : global::Kampai.UI.View.PopupMenuView
	{
		public RectTransform taskScrollViewTransform;
		public Text questName; // Will be used for "Achievements" title
		
		private GameObject taskPanelPrefab;
		private List<GameObject> achievementViews = new List<GameObject>();
		private float taskPanelWidth = 320f; // Approx width from QuestPanelView

		public override void Init()
		{
			base.Init();
			
			// Find components from the screen_QuestPanel layout using the project's recursive FindChild extension
			GameObject titleGO = gameObject.FindChild("txt_QuestName");
			if (titleGO != null) questName = titleGO.GetComponent<Text>();
			
			GameObject scrollGO = gameObject.FindChild("TaskScrollView");
			if (scrollGO != null) taskScrollViewTransform = scrollGO.GetComponent<RectTransform>();

			this.taskPanelPrefab = global::Kampai.Util.KampaiResources.Load("cmp_TaskPanel") as GameObject;
			
			// Deactivate things we don't need from the original layout
			string[] toHide = { "QuestPanelProgressBar", "QuestLineProgressBar", "CurrentQuestPanel", "QuestTabScrollView" };
			foreach (string name in toHide)
			{
				GameObject t = gameObject.FindChild(name);
				if (t != null) t.SetActive(false);
			}
		}

		public void CreateAchievements(IList<global::Kampai.Game.Achievement> achievements, global::Kampai.Main.ILocalizationService localizationService)
		{
			foreach (GameObject view in achievementViews)
			{
				Destroy(view);
			}
			achievementViews.Clear();

			if (questName != null)
			{
				string val = localizationService.GetString("ACHIEVEMENTS_TITLE");
				questName.text = (val != null) ? val : "Achievements";
			}

			if (achievements == null) return;

			for (int i = 0; i < achievements.Count; i++)
			{
				GameObject gameObject = Instantiate(taskPanelPrefab);
				Transform transform = gameObject.transform;
				transform.SetParent(taskScrollViewTransform, false);
				RectTransform rectTransform = transform as RectTransform;
				rectTransform.localPosition = Vector3.zero;
				rectTransform.localScale = Vector3.one;
				
				// Standard horizontal layout logic from QuestPanelView
				rectTransform.offsetMin = new Vector2(taskPanelWidth * (float)i, 0f);
				rectTransform.offsetMax = new Vector2(taskPanelWidth * (float)(i + 1), 0f);
				
				// We will attach AchievementItemView to this instantiated prefab
				AchievementItemView itemView = gameObject.AddComponent<AchievementItemView>();
				itemView.Init(achievements[i], localizationService);
				
				achievementViews.Add(gameObject);
			}

			if (taskScrollViewTransform != null)
			{
				taskScrollViewTransform.offsetMin = new Vector2(0f, 0f);
				taskScrollViewTransform.offsetMax = new Vector2((int)((float)achievements.Count * taskPanelWidth), 0f);
			}
		}
	}
}
