namespace Kampai.Tools.AnimationToolKit
{
	public class AddMinionCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.IPlayerService PlayerService { get; set; }

		[Inject]
		public global::Kampai.Game.View.BuildingObject BuildingObject { get; set; }

		[Inject]
		public global::Kampai.Game.View.MinionObject MinionObject { get; set; }

		[Inject]
		public int RouteIndex { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.Building byInstanceId = PlayerService.GetByInstanceId<global::Kampai.Game.Building>(BuildingObject.ID);
			global::Kampai.Game.TaskableBuilding taskableBuilding = byInstanceId as global::Kampai.Game.TaskableBuilding;
			global::Kampai.Game.LeisureBuilding leisureBuilding = byInstanceId as global::Kampai.Game.LeisureBuilding;
			if (taskableBuilding == null && leisureBuilding == null)
			{
				return;
			}
			global::Kampai.Game.View.TaskableBuildingObject taskableBuildingObject = BuildingObject as global::Kampai.Game.View.TaskableBuildingObject;
			global::Kampai.Game.View.LeisureBuildingObjectView leisureBuildingObjectView = BuildingObject as global::Kampai.Game.View.LeisureBuildingObjectView;
			if (taskableBuildingObject == null && leisureBuildingObjectView == null)
			{
				return;
			}
			global::Kampai.Game.View.TaskingMinionObject taskingMinionObject = new global::Kampai.Game.View.TaskingMinionObject(MinionObject, RouteIndex);
			if (taskingMinionObject != null)
			{
				taskingMinionObject.RoutingIndex = RouteIndex;
				global::Kampai.Game.AnimatingBuildingDefinition animatingBuildingDefinition = null;
				if (taskableBuilding != null)
				{
					taskableBuilding.AddMinion(MinionObject.ID, 0);
					animatingBuildingDefinition = taskableBuilding.Definition;
				}
				else if (leisureBuilding != null)
				{
					leisureBuilding.AddMinion(MinionObject.ID, 0);
					animatingBuildingDefinition = leisureBuilding.Definition;
				}
				global::UnityEngine.RuntimeAnimatorController runtimeAnimatorController = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>(animatingBuildingDefinition.AnimationDefinitions[0].MinionController);
				MinionObject.SetAnimController(runtimeAnimatorController);
				if (taskableBuildingObject != null)
				{
					taskableBuildingObject.MoveToRoutingPosition(MinionObject, RouteIndex);
					taskableBuildingObject.TrackChild(MinionObject, runtimeAnimatorController, false);
				}
				else if (leisureBuildingObjectView != null)
				{
					leisureBuildingObjectView.MoveToRoutingPosition(MinionObject, RouteIndex);
					leisureBuildingObjectView.TrackChild(MinionObject, runtimeAnimatorController, RouteIndex);
				}
			}
		}
	}
}
