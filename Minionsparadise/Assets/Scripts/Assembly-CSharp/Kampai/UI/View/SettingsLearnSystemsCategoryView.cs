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
