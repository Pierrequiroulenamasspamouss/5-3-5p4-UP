namespace Kampai.UI.View
{
	public class GenericPopupMediator : global::Kampai.UI.View.AbstractGenericPopupMediator<global::Kampai.UI.View.GenericPopupView>
	{
		private global::Kampai.Game.ItemDefinition itemDefinition;

		private bool showGoto;

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.UI.View.GoToResourceButtonClickedSignal gotoSignal { get; set; }

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			base.soundFXSignal.Dispatch("Play_menu_popUp_02");
			itemDefinition = args.Get<global::Kampai.Game.ItemDefinition>();
			global::UnityEngine.Vector3 itemCenter = args.Get<global::UnityEngine.Vector3>();
			showGoto = args.Get<bool>();
			base.view.OnMenuClose.AddListener(OnMenuClose);
			Register(itemDefinition, itemCenter);
		}

		public override void Register(global::Kampai.Game.ItemDefinition itemDef, global::UnityEngine.Vector3 itemCenter)
		{
			base.view.Display(itemCenter);
			string itemOrigin = string.Empty;
			if (itemDef != null)
			{
				base.view.SetName(base.localizationService.GetString(itemDef.LocalizedKey));
				global::Kampai.Game.IngredientsItemDefinition ingredientsItemDefinition = itemDef as global::Kampai.Game.IngredientsItemDefinition;
				if (ingredientsItemDefinition != null)
				{
					base.view.SetTime(IngredientsItemUtil.GetHarvestTimeFromIngredientDefinition(ingredientsItemDefinition, definitionService));
					int buildingDefintionIDFromItemDefintionID = definitionService.GetBuildingDefintionIDFromItemDefintionID(itemDef.ID);
					itemOrigin = base.localizationService.GetString(definitionService.Get<global::Kampai.Game.BuildingDefinition>(buildingDefintionIDFromItemDefintionID).LocalizedKey);
				}
				else
				{
					global::Kampai.Game.DropItemDefinition dropItemDefinition = itemDef as global::Kampai.Game.DropItemDefinition;
					if (dropItemDefinition != null)
					{
						itemOrigin = base.localizationService.GetString("StorageBuildingTooltipRandomDrop");
						base.view.DisableDurationInfo();
					}
				}
			}
			base.view.SetItemOrigin(itemOrigin);
			if (showGoto)
			{
				base.view.ShowGotoButton();
			}
		}

		public override void OnRegister()
		{
			base.OnRegister();
			base.view.OnMenuClose.AddListener(OnMenuClose);
			base.view.gotoButton.ClickedSignal.AddListener(gotoClicked);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			base.view.OnMenuClose.RemoveListener(OnMenuClose);
			base.view.gotoButton.ClickedSignal.RemoveListener(gotoClicked);
		}

		private void gotoClicked()
		{
			base.closeSignal.Dispatch();
			gotoSignal.Dispatch(itemDefinition.ID);
		}
	}
}
