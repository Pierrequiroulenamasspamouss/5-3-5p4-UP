namespace Kampai.Game
{
	public class AddStuartToStageCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.StuartStageAnimationType animType { get; set; }

		[Inject]
		public global::Kampai.Game.StuartAddToStageSignal addToStageSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.StageBuilding firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.StageBuilding>(3054);
			if (animType == global::Kampai.Game.StuartStageAnimationType.CELEBRATE)
			{
				base.injectionBinder.GetInstance<global::Kampai.Game.BuildingZoomSignal>().Dispatch(new global::Kampai.Game.BuildingZoomSettings(global::Kampai.Game.ZoomType.IN, global::Kampai.Game.BuildingZoomType.STAGE, null, false));
			}
			global::Kampai.Game.View.BuildingManagerView component = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::Kampai.Game.View.BuildingObject buildingObject = component.GetBuildingObject(firstInstanceByDefinitionId.ID);
			global::Kampai.Game.View.StageBuildingObject stageBuildingObject = buildingObject as global::Kampai.Game.View.StageBuildingObject;
			if (stageBuildingObject != null)
			{
				global::UnityEngine.Transform stageTransform = stageBuildingObject.GetStageTransform();
				addToStageSignal.Dispatch(stageTransform.position, stageTransform.localRotation, animType);
			}
		}
	}
}
