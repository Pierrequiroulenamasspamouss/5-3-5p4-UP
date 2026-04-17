namespace Kampai.UI.View
{
	public class SettingsLearnSystemsCategoryPanelMediator : global::strange.extensions.mediation.impl.Mediator
	{
		[Inject]
		public global::Kampai.UI.View.SettingsLearnSystemsCategoryPanelView view { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		[Inject]
		public global::Kampai.UI.View.SettingsLearnSystemsCategorySelectedSignal categorySelectedSignal { get; set; }

		public override void OnRegister()
		{
			SetupCategories();
		}

		private void SetupCategories()
		{
			view.ClearCategories();
			global::System.Collections.Generic.List<global::Kampai.Game.PlayerTrainingCategoryDefinition> all = definitionService.GetAll<global::Kampai.Game.PlayerTrainingCategoryDefinition>();
			if (all == null || all.Count < 1)
			{
				return;
			}
			foreach (global::Kampai.Game.PlayerTrainingCategoryDefinition item in all)
			{
				if (item.ID == 19200001)
				{
					continue;
				}
				view.AddCategory(item, localizationService);
			}
			categorySelectedSignal.Dispatch(all[0].ID);
		}
	}
}
