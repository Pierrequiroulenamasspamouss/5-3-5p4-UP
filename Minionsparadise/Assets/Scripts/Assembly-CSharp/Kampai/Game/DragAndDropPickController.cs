namespace Kampai.Game
{
	public class DragAndDropPickController : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int pickEvent { get; set; }

		[Inject]
		public global::UnityEngine.Vector3 inputPosition { get; set; }

		[Inject]
		public global::Kampai.Game.DragOffsetType dragOffsetType { get; set; }

		[Inject]
		public global::Kampai.Common.PickControllerModel model { get; set; }

		[Inject]
		public global::Kampai.Game.DisableCameraBehaviourSignal disableCameraSignal { get; set; }

		[Inject]
		public global::Kampai.Game.EnableCameraBehaviourSignal enableCameraSignal { get; set; }

		[Inject]
		public global::Kampai.Game.MoveBuildingSignal moveBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.MoveScaffoldingSignal moveScaffoldingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.View.CameraUtils cameraUtils { get; set; }

		[Inject]
		public global::Kampai.Game.UpdateMovementValidity updateSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IBuildingUtilities buildingUtilies { get; set; }

		[Inject]
		public global::Kampai.Game.Scaffolding currentScaffolding { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowMoveBuildingMenuSignal showMoveBuildingMenuSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		public override void Execute()
		{
			switch (pickEvent)
			{
			case 1:
				PickEventStart();
				break;
			case 2:
				PickEventHold();
				break;
			case 3:
				PickEventEnd();
				break;
			}
		}

		private void PickEventStart()
		{
			disableCameraSignal.Dispatch(1);
			enableCameraSignal.Dispatch(8);
			model.OffsetType = dragOffsetType;
		}

		private void PickEventHold()
		{
			global::UnityEngine.Vector3 vector = cameraUtils.GroundPlaneRaycast(inputPosition);
			vector.x = global::UnityEngine.Mathf.Clamp(vector.x, 15f, 223f);
			vector.z = global::UnityEngine.Mathf.Clamp(vector.z, 0f, 214f);
			if (model.DragPreviousPosition == global::UnityEngine.Vector3.zero)
			{
				model.DragPreviousPosition = vector;
			}
			if (model.OffsetType == global::Kampai.Game.DragOffsetType.ONE_X_ONE)
			{
				vector = new global::UnityEngine.Vector3(vector.x + -1.4f, vector.y, vector.z + 1.2f);
			}
			global::UnityEngine.Vector3 moveToPosition = vector;
			Drag(moveToPosition);
			model.DragPreviousPosition = new global::UnityEngine.Vector3(global::UnityEngine.Mathf.Round(vector.x), global::UnityEngine.Mathf.Round(vector.y), global::UnityEngine.Mathf.Round(vector.z));
		}

		private void Drag(global::UnityEngine.Vector3 moveToPosition)
		{
			if (!model.SelectedBuilding.HasValue)
			{
				return;
			}
			int value = model.SelectedBuilding.Value;
			if (value == -1)
			{
				moveToPosition = anchorToCornerBuilding(moveToPosition, currentScaffolding.Definition);
				global::Kampai.Game.Location location = new global::Kampai.Game.Location(moveToPosition);
				currentScaffolding.Location = location;
				bool flag = buildingUtilies.ValidateScaffoldingPlacement(currentScaffolding.Definition, location);
				moveScaffoldingSignal.Dispatch(moveToPosition, flag);
				updateSignal.Dispatch(flag);
				return;
			}
			global::Kampai.Game.Building byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Building>(value);
			if (byInstanceId != null && byInstanceId.Definition.Movable)
			{
				moveToPosition = anchorToCornerBuilding(moveToPosition, byInstanceId.Definition);
				global::Kampai.Game.Location location2 = new global::Kampai.Game.Location(moveToPosition);
				bool flag2 = buildingUtilies.ValidateLocation(byInstanceId, location2);
				moveBuildingSignal.Dispatch(value, moveToPosition, flag2);
				updateSignal.Dispatch(flag2);
			}
		}

		private global::UnityEngine.Vector3 anchorToCornerBuilding(global::UnityEngine.Vector3 originalPosition, global::Kampai.Game.BuildingDefinition buildingDef)
		{
			global::UnityEngine.Vector3 result = originalPosition;
			global::Kampai.Game.FootprintDefinition footprintDefinition = definitionService.Get<global::Kampai.Game.FootprintDefinition>(buildingDef.FootprintID);
			if (footprintDefinition != null)
			{
				int num = BuildingUtil.GetFootprintDepth(footprintDefinition.Footprint) / 2;
				int num2 = BuildingUtil.GetFootprintWidth(footprintDefinition.Footprint) / 2;
				result = new global::UnityEngine.Vector3(result.x - (float)num2, result.y, result.z + (float)num);
			}
			return result;
		}

		private void PickEventEnd()
		{
			disableCameraSignal.Dispatch(8);
			enableCameraSignal.Dispatch(1);
			if (model.SelectedBuilding == -1)
			{
				showMoveBuildingMenuSignal.Dispatch(true, new global::Kampai.UI.View.MoveBuildingSetting(-1, 1, questService.ShouldPulseMoveButtonAccept(), true));
				bool type = buildingUtilies.ValidateScaffoldingPlacement(currentScaffolding.Definition, currentScaffolding.Location);
				updateSignal.Dispatch(type);
			}
			Reset();
		}

		private void Reset()
		{
			model.DragPreviousPosition = global::UnityEngine.Vector3.zero;
		}
	}
}
