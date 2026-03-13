namespace Kampai.Game
{
	public class BuildingChangeStateCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int buildingID { get; set; }

		[Inject]
		public global::Kampai.Game.BuildingState newState { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		[Inject]
		public global::Kampai.Game.AddFootprintSignal addFootprintSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.Building byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Building>(buildingID);
			if (byInstanceId == null)
			{
				return;
			}
			global::Kampai.Game.BuildingState state = byInstanceId.State;
			global::Kampai.Game.TaskableBuilding taskableBuilding = byInstanceId as global::Kampai.Game.TaskableBuilding;
			if (taskableBuilding != null && newState == global::Kampai.Game.BuildingState.Working && state == global::Kampai.Game.BuildingState.Harvestable && taskableBuilding.GetNumCompleteMinions() > 0)
			{
				return;
			}
			if (buildingManager != null)
			{
				global::Kampai.Game.View.BuildingManagerView component = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
				global::Kampai.Game.View.BuildingObject buildingObject = component.GetBuildingObject(buildingID);
				if (buildingObject != null)
				{
					buildingObject.UpdateColliderState(newState);
					if (state == global::Kampai.Game.BuildingState.Disabled && newState != state)
					{
						addFootprintSignal.Dispatch(byInstanceId, byInstanceId.Location);
					}
					if ((state == global::Kampai.Game.BuildingState.Disabled || newState == global::Kampai.Game.BuildingState.Disabled) && newState != state)
					{
						if (newState == global::Kampai.Game.BuildingState.Disabled)
						{
							buildingObject.SetMaterialColor(global::UnityEngine.Color.gray);
						}
						else
						{
							buildingObject.SetMaterialColor(global::UnityEngine.Color.white);
						}
					}
					global::Kampai.Game.View.IStartAudio startAudio = buildingObject as global::Kampai.Game.View.IStartAudio;
					if (startAudio != null)
					{
						startAudio.NotifyBuildingState(newState);
					}
				}
			}
			byInstanceId.SetState(newState);
			byInstanceId.StateStartTime = timeService.CurrentTime();
		}
	}
}
