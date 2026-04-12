namespace Kampai.UI.View
{
	public class SettingsLearnSystemsCategoryView : global::Kampai.Util.KampaiView
	{
		public global::UnityEngine.UI.Text Title;

		public global::Kampai.UI.View.KampaiImage toggleImage;

		public ScrollableButtonView Button;

		public global::Kampai.Game.PlayerTrainingCategoryDefinition Definition;

		public void Init(global::Kampai.Game.PlayerTrainingCategoryDefinition definition, global::Kampai.Main.ILocalizationService localizationService, bool selected)
		{
			Definition = definition;
			// global::UnityEngine.Debug.Log(string.Format("ANTIGRAVITY: SettingsLearnSystemsCategoryView.Init - View: {0}, ID: {1}, Definition: {2}", name, GetInstanceID(), (definition != null) ? definition.categoryTitleLocalizedKey : "NULL"));
			Title.text = localizationService.GetString(definition.categoryTitleLocalizedKey);
			if (selected)
			{
				toggleImage.gameObject.SetActive(true);
			}
			else
			{
				toggleImage.gameObject.SetActive(false);
			}
		}
	}
}
