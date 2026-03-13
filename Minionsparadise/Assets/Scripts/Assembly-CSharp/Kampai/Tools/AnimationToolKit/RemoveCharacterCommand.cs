namespace Kampai.Tools.AnimationToolKit
{
	public class RemoveCharacterCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.IPlayerService PlayerService { get; set; }

		[Inject]
		public global::Kampai.Game.View.BuildingObject BuildingObject { get; set; }

		[Inject]
		public global::Kampai.Game.View.CharacterObject CharacterObject { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.Building byInstanceId = PlayerService.GetByInstanceId<global::Kampai.Game.Building>(BuildingObject.ID);
			global::Kampai.Game.TaskableBuilding taskableBuilding = byInstanceId as global::Kampai.Game.TaskableBuilding;
			if (taskableBuilding != null)
			{
				taskableBuilding.RemoveMinion(CharacterObject.ID, 0);
			}
			global::Kampai.Game.View.TikiBarBuildingObjectView tikiBarBuildingObjectView = BuildingObject as global::Kampai.Game.View.TikiBarBuildingObjectView;
			if (tikiBarBuildingObjectView != null)
			{
				tikiBarBuildingObjectView.UnlinkChild(CharacterObject.ID);
			}
		}
	}
}
