namespace Kampai.UI.View
{
	public class SettingsLearnSystemsCategoryPanelView : global::Kampai.Util.KampaiView
	{
		public global::UnityEngine.Transform CategoryParent;

		public global::strange.extensions.signal.impl.Signal<int> OnSelectCategorySignal = new global::strange.extensions.signal.impl.Signal<int>();

		private bool firstCategory;

		internal void AddCategory(global::Kampai.Game.PlayerTrainingCategoryDefinition categoryDefinition, global::Kampai.Main.ILocalizationService localizationService)
		{
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(global::Kampai.Util.KampaiResources.Load<global::UnityEngine.GameObject>("cmp_SettingsPlayerTrainingCategory"));
			gameObject.transform.SetParent(CategoryParent, false);
			global::Kampai.UI.View.SettingsLearnSystemsCategoryView component = gameObject.GetComponent<global::Kampai.UI.View.SettingsLearnSystemsCategoryView>();
			bool selected = false;
			if (!firstCategory)
			{
				firstCategory = true;
				selected = true;
			}
			component.Init(categoryDefinition, localizationService, selected);
		}
	}
}
