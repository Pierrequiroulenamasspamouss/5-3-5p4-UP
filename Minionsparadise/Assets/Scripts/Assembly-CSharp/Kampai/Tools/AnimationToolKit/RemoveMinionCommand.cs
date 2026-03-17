namespace Kampai.Tools.AnimationToolKit
{
	public class RemoveMinionCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.View.BuildingObject BuildingObject { get; set; }

		[Inject]
		public global::Kampai.Game.View.MinionObject MinionObject { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService PlayerService { get; set; }

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
			if (!(taskableBuildingObject == null) || !(leisureBuildingObjectView == null))
			{
				if (taskableBuilding != null)
				{
					taskableBuilding.AddToCompletedMinions(MinionObject.ID, 0);
					taskableBuilding.HarvestFromCompleteMinions();
					taskableBuilding.RemoveMinion(MinionObject.ID, 0);
					taskableBuildingObject.UntrackChild(MinionObject.ID, taskableBuilding);
				}
				else if (leisureBuildingObjectView != null)
				{
					leisureBuilding.CleanMinionQueue();
					leisureBuildingObjectView.FreeAllMinions(MinionObject.ID);
				}
			}
		}
	}
}
