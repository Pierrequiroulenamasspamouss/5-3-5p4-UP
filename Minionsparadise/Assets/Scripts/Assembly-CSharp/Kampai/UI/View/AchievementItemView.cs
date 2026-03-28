using UnityEngine;
using UnityEngine.UI;

namespace Kampai.UI.View
{
	public class AchievementItemView : MonoBehaviour
	{
		public Text taskNameText;
		public Text descriptionText;
		public Image progressFill;
		public Text progressText;
		
		public void Init(global::Kampai.Game.Achievement achievement, global::Kampai.Main.ILocalizationService localizationService)
		{
			// Using names from QuestStepView (cmp_TaskPanel)
			global::UnityEngine.Transform titleTransform = transform.Find("Title") != null ? transform.Find("Title") : transform.Find("titleText");
			if (titleTransform != null) taskNameText = titleTransform.GetComponent<Text>();
			if (taskNameText == null) taskNameText = transform.GetComponentInChildren<Text>(); // Fallback
			
			// Objective/descriptionText
			global::UnityEngine.Transform descTransform = transform.Find("Objective") != null ? transform.Find("Objective") : transform.Find("descriptionText");
			if (descTransform != null) descriptionText = descTransform.GetComponent<Text>();

			// ProgressBar/progressFill
			global::UnityEngine.Transform progressTransform = transform.Find("ProgressBar");
			if (progressTransform != null) {
				global::UnityEngine.Transform fillTransform = progressTransform.Find("Fill"); 
				if (fillTransform != null) progressFill = fillTransform.GetComponent<Image>();
			}
			
			global::UnityEngine.Transform pTextTransform = transform.Find("ProgressText");
			if (pTextTransform != null) progressText = pTextTransform.GetComponent<Text>();

			global::Kampai.Game.AchievementDefinition def = achievement.Definition;
			if (def != null)
			{
				if (taskNameText != null)
				{
					string name = localizationService.GetString(def.LocalizedKey);
					taskNameText.text = (name != null && name != def.LocalizedKey) ? name : def.LocalizedKey;
				}
				
				if (descriptionText != null)
				{
					string descKey = def.LocalizedKey + "_DESC";
					string desc = localizationService.GetString(descKey);
					descriptionText.text = (desc != null && desc != descKey) ? desc : ""; 
				}
				
				if (progressFill != null)
				{
					float num = (float)achievement.Progress / (float)def.Steps;
					progressFill.rectTransform.anchorMax = new Vector2(System.Math.Min(num, 1f), 1f);
				}
				
				if (progressText != null)
				{
					progressText.text = string.Format("{0} / {1}", achievement.Progress, def.Steps);
				}
			}
		}
	}
}
