namespace Kampai.Game
{
	public class CreditCollectionRewardCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.MignetteCollectionService collectionService { get; set; }

		[Inject]
		public global::Kampai.UI.View.OpenStoreHighlightItemSignal highlightStoreItemSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CameraAutoMoveToBuildingSignal cameraAutoMoveToBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CompositeBuildingPieceAddedSignal compositeBuildingPieceAddedSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.Transaction.TransactionDefinition pendingRewardTransaction = collectionService.pendingRewardTransaction;
			foreach (global::Kampai.Util.QuantityItem output in pendingRewardTransaction.Outputs)
			{
				int iD = output.ID;
				global::Kampai.Game.CompositeBuildingPieceDefinition definition = null;
				if (!definitionService.TryGet<global::Kampai.Game.CompositeBuildingPieceDefinition>(iD, out definition))
				{
					continue;
				}
				global::Kampai.Game.CompositeBuilding firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.CompositeBuilding>(definition.BuildingDefinitionID);
				if (firstInstanceByDefinitionId != null)
				{
					if (firstInstanceByDefinitionId.State == global::Kampai.Game.BuildingState.Inventory)
					{
						highlightStoreItemSignal.Dispatch(definition.BuildingDefinitionID, true);
					}
					else
					{
						cameraAutoMoveToBuildingSignal.Dispatch(firstInstanceByDefinitionId, new global::Kampai.Game.PanInstructions(firstInstanceByDefinitionId));
					}
					compositeBuildingPieceAddedSignal.Dispatch(firstInstanceByDefinitionId);
				}
			}
		}
	}
}
