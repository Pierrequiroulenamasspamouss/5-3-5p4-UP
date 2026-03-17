namespace Kampai.Game
{
	internal sealed class MarketplaceItemSoldCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IMarketplaceService marketplaceService { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateSaleSlotSignal updateSaleSlot { get; set; }

		[Inject]
		public global::Kampai.Game.MarketplaceUpdateSoldItemsSignal updateSoldItemsSignal { get; set; }

		[Inject]
		public int saleItemId { get; set; }

		[Inject]
		public global::Kampai.Main.PlayLocalAudioSignal playLocalAudioSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.MarketplaceSaleItem byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.MarketplaceSaleItem>(saleItemId);
			if (byInstanceId != null)
			{
				byInstanceId.state = global::Kampai.Game.MarketplaceSaleItem.State.SOLD;
				global::Kampai.Game.MarketplaceSaleSlot slotByItem = marketplaceService.GetSlotByItem(byInstanceId);
				updateSaleSlot.Dispatch(slotByItem.ID);
				global::Kampai.Game.StorageBuilding firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.StorageBuilding>(3018);
				global::Kampai.Game.View.BuildingManagerView component = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
				global::Kampai.Game.View.BuildingObject buildingObject = component.GetBuildingObject(firstInstanceByDefinitionId.ID);
				CustomFMOD_StudioEventEmitter type = global::Kampai.Util.Audio.GetAudioEmitter.Get(buildingObject.gameObject, "LocalAudio");
				playLocalAudioSignal.Dispatch(type, "Play_marketplace_bagDrop_01", null);
				updateSoldItemsSignal.Dispatch(true);
			}
		}
	}
}
