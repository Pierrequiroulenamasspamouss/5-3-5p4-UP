namespace Kampai.UI.View
{
	public class SettingsLearnSystemsCategoryItemView : global::Kampai.Util.KampaiView
	{
		public global::UnityEngine.UI.Text Title;

		public global::UnityEngine.UI.Image CheckMark;

		public ScrollableButtonView Button;

		public global::Kampai.Game.PlayerTrainingDefinition Definition;

		public void Init(global::Kampai.Game.PlayerTrainingDefinition definition, global::Kampai.Main.ILocalizationService localizationService, bool hasCheckMark)
		{
			Definition = definition;
			Title.text = localizationService.GetString(definition.trainingTitleLocalizedKey);
			CheckMark.enabled = hasCheckMark;
		}
	}
}
