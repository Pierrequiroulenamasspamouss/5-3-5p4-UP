namespace Kampai.Game.View
{
	public class StorageBuildingObject : global::Kampai.Game.View.AnimatingBuildingObject
	{
		public global::Kampai.Game.StorageBuilding storageBuilding { get; set; }

		internal override void Init(global::Kampai.Game.Building building, global::Kampai.Util.IKampaiLogger logger, global::System.Collections.Generic.IDictionary<string, global::UnityEngine.RuntimeAnimatorController> controllers, global::Kampai.Game.IDefinitionService definitionService)
		{
			base.Init(building, logger, controllers, definitionService);
			storageBuilding = (global::Kampai.Game.StorageBuilding)building;
			base.gameObject.AddComponent<global::Kampai.Game.View.StorageBuildingView>();
			SetupWaterEmitter();
		}

		private void SetupWaterEmitter()
		{
			CustomFMOD_StudioEventEmitter customFMOD_StudioEventEmitter = base.gameObject.AddComponent<CustomFMOD_StudioEventEmitter>();
			customFMOD_StudioEventEmitter.id = "StorageWaterEmitter";
			customFMOD_StudioEventEmitter.shiftPosition = false;
			customFMOD_StudioEventEmitter.staticSound = false;
			customFMOD_StudioEventEmitter.path = base.fmodService.GetGuid("Play_water_stream_light_01");
			customFMOD_StudioEventEmitter.Play();
		}

		internal void SetOpen()
		{
			if (!IsInAnimatorState(GetHashAnimationState("Base Layer.Open")) || !IsInAnimatorState(GetHashAnimationState("Base Layer.Opening")))
			{
				EnqueueAction(new global::Kampai.Game.View.TriggerBuildingAnimationAction(this, "OnOpen", logger));
			}
		}

		internal void SetClose()
		{
			if (!IsInAnimatorState(GetHashAnimationState("Base Layer.Closing")) || !IsInAnimatorState(GetHashAnimationState("Base Layer.Idle")))
			{
				EnqueueAction(new global::Kampai.Game.View.TriggerBuildingAnimationAction(this, "OnClose", logger));
			}
		}

		public override void SetState(global::Kampai.Game.BuildingState newState)
		{
			base.SetState(newState);
			if (buildingState != newState)
			{
				buildingState = newState;
				switch (newState)
				{
				case global::Kampai.Game.BuildingState.Inactive:
				case global::Kampai.Game.BuildingState.Idle:
					SetClose();
					break;
				case global::Kampai.Game.BuildingState.Working:
					SetOpen();
					break;
				case global::Kampai.Game.BuildingState.Construction:
				case global::Kampai.Game.BuildingState.Complete:
					break;
				}
			}
		}
	}
}
