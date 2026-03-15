namespace Kampai.UI
{
	public class DragFromStoreCommand : global::strange.extensions.command.impl.Command
	{
		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("DragFromStoreCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.Definition definition { get; set; }

		[Inject]
		public global::Kampai.Game.Transaction.TransactionDefinition transactionDef { get; set; }

		[Inject]
		public global::UnityEngine.Vector3 eventPosition { get; set; }

		[Inject]
		public bool isFromStore { get; set; }

		[Inject]
		public global::Kampai.Common.PickControllerModel pickControllerModel { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject(global::Kampai.Main.MainElement.CAMERA)]
		public global::UnityEngine.Camera mainCamera { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.DecoGridModel decoGridModel { get; set; }

		[Inject]
		public global::Kampai.Common.DragAndDropPickSignal dragAndDropSignal { get; set; }

		public override void Execute()
		{
			global::strange.extensions.injector.api.ICrossContextInjectionBinder crossContextInjectionBinder = gameContext.injectionBinder;
			global::Kampai.Game.BuildingDefinition buildingDefinition = definition as global::Kampai.Game.BuildingDefinition;
			if (buildingDefinition == null)
			{
				return;
			}
			pickControllerModel.LastBuildingStorePosition = eventPosition;
			global::UnityEngine.Vector3 positioningBasedOnBuildingType = GetPositioningBasedOnBuildingType(buildingDefinition);
			global::Kampai.Game.Scaffolding instance = crossContextInjectionBinder.GetInstance<global::Kampai.Game.Scaffolding>();
			instance.Definition = buildingDefinition;
			instance.Location = new global::Kampai.Game.Location(positioningBasedOnBuildingType);
			instance.Transaction = transactionDef;
			instance.Building = null;
			instance.Lifted = true;
			logger.Debug(transactionDef.ToString());
			bool type = false;
			global::System.Collections.Generic.ICollection<global::Kampai.Game.Instance> byDefinitionId = playerService.GetByDefinitionId<global::Kampai.Game.Instance>(buildingDefinition.ID);
			if (byDefinitionId.Count != 0)
			{
				foreach (global::Kampai.Game.Instance item in byDefinitionId)
				{
					global::Kampai.Game.Building building = item as global::Kampai.Game.Building;
					if (building != null && building.State == global::Kampai.Game.BuildingState.Inventory)
					{
						instance.Building = building;
						type = true;
						break;
					}
				}
			}
			global::Kampai.Game.DragOffsetType offsetType = global::Kampai.Game.DragOffsetType.NONE;
			if (buildingDefinition.FootprintID == 300000)
			{
				offsetType = global::Kampai.Game.DragOffsetType.ONE_X_ONE;
			}
			global::Kampai.Game.CreateDummyBuildingSignal instance2 = crossContextInjectionBinder.GetInstance<global::Kampai.Game.CreateDummyBuildingSignal>();
			positioningBasedOnBuildingType = new global::UnityEngine.Vector3(positioningBasedOnBuildingType.x, 0f, positioningBasedOnBuildingType.z);
			instance2.Dispatch(buildingDefinition, positioningBasedOnBuildingType, type);
			SetPickController(-1, offsetType);
		}

		private global::UnityEngine.Vector3 GetPositioningBasedOnBuildingType(global::Kampai.Game.BuildingDefinition definition)
		{
			global::strange.extensions.injector.api.ICrossContextInjectionBinder crossContextInjectionBinder = gameContext.injectionBinder;
			global::Kampai.Game.DecorationBuildingDefinition decorationBuildingDefinition = definition as global::Kampai.Game.DecorationBuildingDefinition;
			global::Kampai.Game.ConnectableBuildingDefinition connectableBuildingDefinition = definition as global::Kampai.Game.ConnectableBuildingDefinition;
			global::UnityEngine.Vector3 result = default(global::UnityEngine.Vector3);
			if (isFromStore)
			{
				return BuildingUtil.UIToWorldCoords(mainCamera, eventPosition);
			}
			if (connectableBuildingDefinition != null)
			{
				global::UnityEngine.Vector3 newPieceLocation = decoGridModel.GetNewPieceLocation((int)eventPosition.x, (int)eventPosition.z, connectableBuildingDefinition.connectableType, crossContextInjectionBinder.GetInstance<global::Kampai.Game.Environment>());
				return eventPosition + newPieceLocation;
			}
			if (decorationBuildingDefinition != null)
			{
				global::UnityEngine.Vector3 newPieceLocation2 = decoGridModel.GetNewPieceLocation((int)eventPosition.x, (int)eventPosition.z, 0, crossContextInjectionBinder.GetInstance<global::Kampai.Game.Environment>());
				return eventPosition + newPieceLocation2;
			}
			return result;
		}

		private void SetPickController(int? selectedBuildingID, global::Kampai.Game.DragOffsetType offsetType)
		{
			pickControllerModel.InvalidateMovement = false;
			pickControllerModel.StartHitObject = null;
			pickControllerModel.CurrentMode = global::Kampai.Common.PickControllerModel.Mode.DragAndDrop;
			pickControllerModel.SelectedBuilding = selectedBuildingID;
			dragAndDropSignal.Dispatch(isFromStore ? 1 : 3, eventPosition, offsetType);
		}
	}
}
