namespace Kampai.UI.View
{
	public static class MasterPlanComponentTaskDefinitionExtension
	{
		public static string GetTaskImage(this global::Kampai.Game.MasterPlanComponentTaskDefinition task, global::Kampai.Main.ILocalizationService localizationService, global::Kampai.Game.IDefinitionService definitionService, string defaultLocKey, out global::Kampai.Game.DisplayableDefinition displayableDefinition)
		{
			displayableDefinition = null;
			if (task == null)
			{
				return string.Empty;
			}
			string result = string.Empty;
			switch (task.Type)
			{
			case global::Kampai.Game.MasterPlanComponentTaskType.Deliver:
			case global::Kampai.Game.MasterPlanComponentTaskType.Collect:
			{
				global::Kampai.Game.ItemDefinition definition2;
				definitionService.TryGet<global::Kampai.Game.ItemDefinition>(task.requiredItemId, out definition2);
				result = definition2.LocalizedKey;
				displayableDefinition = definition2;
				break;
			}
			case global::Kampai.Game.MasterPlanComponentTaskType.CompleteOrders:
				displayableDefinition = new global::Kampai.Game.DisplayableDefinition
				{
					Image = "img_orderboard_item_fill",
					Mask = "img_orderboard_item_mask"
				};
				break;
			case global::Kampai.Game.MasterPlanComponentTaskType.PlayMiniGame:
			case global::Kampai.Game.MasterPlanComponentTaskType.MiniGameScore:
			case global::Kampai.Game.MasterPlanComponentTaskType.EarnPartyPoints:
			case global::Kampai.Game.MasterPlanComponentTaskType.EarnLeisurePartyPoints:
			case global::Kampai.Game.MasterPlanComponentTaskType.EarnMignettePartyPoints:
			case global::Kampai.Game.MasterPlanComponentTaskType.EarnSandDollars:
			{
				global::Kampai.Game.BuildingDefinition definition;
				bool buildingExists = definitionService.TryGet<global::Kampai.Game.BuildingDefinition>(task.requiredItemId, out definition);
				bool isGrindCurrency = task.Type == global::Kampai.Game.MasterPlanComponentTaskType.EarnSandDollars;
				result = SetBuildingImage(localizationService, definitionService, defaultLocKey, buildingExists, isGrindCurrency, definition, out displayableDefinition);
				break;
			}
			}
			return result;
		}

		public static string GetTaskImage(this global::Kampai.Game.MasterPlanComponentTaskDefinition task, global::Kampai.UI.View.KampaiImage itemImage, global::Kampai.Main.ILocalizationService localizationService, global::Kampai.Game.IDefinitionService definitionService, string defaultLocKey)
		{
			global::Kampai.Game.DisplayableDefinition displayableDefinition;
			string taskImage = task.GetTaskImage(localizationService, definitionService, defaultLocKey, out displayableDefinition);
			UIUtils.SetItemIcon(itemImage, displayableDefinition);
			return taskImage;
		}

		private static string SetBuildingImage(global::Kampai.Main.ILocalizationService localizationService, global::Kampai.Game.IDefinitionService definitionService, string defaultLocKey, bool buildingExists, bool isGrindCurrency, global::Kampai.Game.BuildingDefinition buildingDef, out global::Kampai.Game.DisplayableDefinition displayableDefinition)
		{
			string result;
			if (buildingExists)
			{
				result = ((localizationService != null) ? localizationService.GetString(buildingDef.LocalizedKey) : string.Empty);
				displayableDefinition = buildingDef;
			}
			else
			{
				int id = ((!isGrindCurrency) ? 2 : 0);
				result = ((localizationService != null) ? localizationService.GetString(defaultLocKey) : string.Empty);
				displayableDefinition = definitionService.Get<global::Kampai.Game.DisplayableDefinition>(id);
			}
			return result;
		}
	}
}
