namespace Kampai.UI.View
{
	public class MoveBuildingMenuMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.MoveBuildingMenuView>
	{
		[global::System.Flags]
		public enum Buttons
		{
			None = 0,
			Inventory = 1,
			Accept = 4,
			Close = 8,
			All = 0x10
		}

		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("MoveBuildingMenuMediator") as global::Kampai.Util.IKampaiLogger;

		private global::Kampai.Game.Scaffolding currentScaffolding;

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal { get; set; }

		[Inject]
		public global::Kampai.Game.UpdateMovementValidity updateSignal { get; set; }

		[Inject]
		public global::Kampai.UI.IPositionService positionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		[Inject]
		public global::Kampai.UI.View.DisableMoveToInventorySignal disableSignal { get; set; }

		[Inject]
		public global::Kampai.Common.PickControllerModel pickControllerModel { get; set; }

		[Inject]
		public global::Kampai.UI.View.UIModel model { get; set; }

		public override void OnRegister()
		{
			model.AllowMultiTouch = true;
			updateSignal.AddListener(UpdateValidity);
			disableSignal.AddListener(DisableInventory);
			currentScaffolding = gameContext.injectionBinder.GetInstance<global::Kampai.Game.Scaffolding>();
			base.view.Init(positionService, gameContext, logger, playerService, localizationService);
			base.view.InventoryButton.ClickedSignal.AddListener(OnInventory);
			base.view.AcceptButton.ClickedSignal.AddListener(OnAccept);
			base.view.CloseButton.ClickedSignal.AddListener(OnClose);
			UpdateCostPanel();
			base.OnRegister();
		}

		public override void OnRemove()
		{
			model.AllowMultiTouch = false;
			updateSignal.RemoveListener(UpdateValidity);
			base.view.InventoryButton.ClickedSignal.RemoveListener(OnInventory);
			base.view.AcceptButton.ClickedSignal.RemoveListener(OnAccept);
			base.view.CloseButton.ClickedSignal.RemoveListener(OnClose);
			disableSignal.RemoveListener(DisableInventory);
			base.OnRemove();
		}

		private void OnInventory()
		{
			playSFXSignal.Dispatch("Play_low_woosh_01");
			ToInventory();
		}

		private void OnAccept()
		{
			Confirm();
		}

		private void OnClose()
		{
			Cancel();
		}

		private void DisableInventory()
		{
			base.view.DisableInventory();
		}

		private void Cancel()
		{
			global::Kampai.Game.CancelBuildingMovementSignal instance = gameContext.injectionBinder.GetInstance<global::Kampai.Game.CancelBuildingMovementSignal>();
			instance.Dispatch(false);
		}

		private void Confirm()
		{
			global::Kampai.Game.ConfirmBuildingMovementSignal instance = gameContext.injectionBinder.GetInstance<global::Kampai.Game.ConfirmBuildingMovementSignal>();
			instance.Dispatch();
		}

		private void ToInventory()
		{
			global::Kampai.Game.InventoryBuildingMovementSignal instance = gameContext.injectionBinder.GetInstance<global::Kampai.Game.InventoryBuildingMovementSignal>();
			instance.Dispatch();
		}

		private void UpdateValidity(bool enable)
		{
			base.view.UpdateValidity(enable);
		}

		private void UpdateCostPanel()
		{
			int num = 0;
			if (pickControllerModel.SelectedBuilding.HasValue && pickControllerModel.SelectedBuilding != -1)
			{
				num = pickControllerModel.SelectedBuilding.Value;
				global::Kampai.Game.Building byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Building>(num);
				SetInventoryCount(byInstanceId.Definition.ID);
			}
			else if (currentScaffolding.Lifted && currentScaffolding.Definition != null)
			{
				SetInventoryCount(currentScaffolding.Definition.ID);
			}
		}

		private void SetInventoryCount(int buildingDefID)
		{
			int inventoryCountByDefinitionID = playerService.GetInventoryCountByDefinitionID(buildingDefID);
			base.view.SetInventoryCount(inventoryCountByDefinitionID);
		}

		protected override void Close()
		{
		}
	}
}
