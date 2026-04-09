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
			global::Kampai.UI.View.SettingsLearnSystemsCategoryView[] components = gameObject.GetComponentsInChildren<global::Kampai.UI.View.SettingsLearnSystemsCategoryView>(true);
			global::UnityEngine.Debug.Log(string.Format("ANTIGRAVITY: AddCategory - GameObject: {0}, Components in hierarchy: {1}", gameObject.name, components.Length));
			
			foreach (global::Kampai.UI.View.SettingsLearnSystemsCategoryView comp in components)
			{
				bool selected = false;
				if (!firstCategory)
				{
					firstCategory = true;
					selected = true;
				}
				comp.Init(categoryDefinition, localizationService, selected);
			}
		}

		internal void ClearCategories()
		{
			foreach (global::UnityEngine.Transform child in CategoryParent)
			{
				global::UnityEngine.Object.Destroy(child.gameObject);
			}
			firstCategory = false;
		}
	}
}
