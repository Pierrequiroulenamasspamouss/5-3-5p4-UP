namespace Kampai.UI.View
{
	public class SettingsLearnSystemsCategoryItemPanelView : global::Kampai.Util.KampaiView
	{
		public global::UnityEngine.Transform PlayerTrainingParent;

		private global::System.Collections.Generic.Dictionary<int, global::Kampai.UI.View.SettingsLearnSystemsCategoryItemView> children = new global::System.Collections.Generic.Dictionary<int, global::Kampai.UI.View.SettingsLearnSystemsCategoryItemView>();

		internal void AddPlayerTraining(global::Kampai.Game.PlayerTrainingDefinition definition, global::Kampai.Main.ILocalizationService localizationService, bool hasSeen)
		{
			int iD = definition.ID;
			if (children.ContainsKey(iD))
			{
				children[iD].Init(definition, localizationService, hasSeen);
				return;
			}
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(global::Kampai.Util.KampaiResources.Load<global::UnityEngine.GameObject>("cmp_SettingsPlayerTrainingCategoryItem"));
			gameObject.transform.SetParent(PlayerTrainingParent, false);
			global::Kampai.UI.View.SettingsLearnSystemsCategoryItemView component = gameObject.GetComponent<global::Kampai.UI.View.SettingsLearnSystemsCategoryItemView>();
			component.Init(definition, localizationService, hasSeen);
			children[definition.ID] = component;
		}

		internal void ClearPlayerTraining()
		{
			foreach (global::Kampai.UI.View.SettingsLearnSystemsCategoryItemView value in children.Values)
			{
				global::UnityEngine.Object.Destroy(value.gameObject);
			}
			children.Clear();
		}
	}
}
