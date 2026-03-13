namespace Kampai.UI.View
{
	public class CraftingPopupMediator : global::Kampai.UI.View.AbstractGenericPopupMediator<global::Kampai.UI.View.CraftingPopupView>
	{
		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject(global::Kampai.Main.MainElement.UI_GLASSCANVAS)]
		public global::UnityEngine.GameObject glassCanvas { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseAllOtherMenuSignal closeAllOtherMenuSignal { get; set; }

		public override void Register(global::Kampai.Game.ItemDefinition itemDef, global::UnityEngine.Vector3 itemCenter)
		{
			base.view.Display(itemCenter, playerService, definitionService, base.localizationService, glassCanvas);
			base.view.SetName(base.localizationService.GetString(itemDef.LocalizedKey));
			global::Kampai.Game.IngredientsItemDefinition ingredientsItemDefinition = itemDef as global::Kampai.Game.IngredientsItemDefinition;
			if (ingredientsItemDefinition != null)
			{
				base.view.SetTime((int)ingredientsItemDefinition.TimeToHarvest);
				base.view.PopulateIngredients(ingredientsItemDefinition);
			}
		}

		public override void OnRegister()
		{
			base.OnRegister();
			base.view.OnMenuClose.AddListener(OnMenuClose);
			closeAllOtherMenuSignal.AddListener(Close);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			base.view.OnMenuClose.RemoveListener(OnMenuClose);
			closeAllOtherMenuSignal.RemoveListener(Close);
		}

		private void Close(global::UnityEngine.GameObject exception)
		{
			OnMenuClose();
		}
	}
}
