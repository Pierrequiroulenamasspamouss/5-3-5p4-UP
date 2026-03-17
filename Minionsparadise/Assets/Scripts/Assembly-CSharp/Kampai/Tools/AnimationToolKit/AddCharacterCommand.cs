namespace Kampai.Tools.AnimationToolKit
{
	public class AddCharacterCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.IPlayerService PlayerService { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService PrestigeService { get; set; }

		[Inject]
		public global::Kampai.Game.View.BuildingObject BuildingObject { get; set; }

		[Inject]
		public global::Kampai.Game.View.CharacterObject CharacterObject { get; set; }

		[Inject]
		public global::Kampai.Game.GetNewQuestSignal GetNewQuestSignal { get; set; }

		[Inject]
		public int RouteIndex { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.Building byInstanceId = PlayerService.GetByInstanceId<global::Kampai.Game.Building>(BuildingObject.ID);
			global::Kampai.Game.StageBuilding stageBuilding = byInstanceId as global::Kampai.Game.StageBuilding;
			global::Kampai.Game.TikiBarBuilding tikiBarBuilding = byInstanceId as global::Kampai.Game.TikiBarBuilding;
			if (stageBuilding == null && tikiBarBuilding == null)
			{
				return;
			}
			global::Kampai.Game.View.StageBuildingObject stageBuildingObject = BuildingObject as global::Kampai.Game.View.StageBuildingObject;
			global::Kampai.Game.View.TikiBarBuildingObjectView tikiBarBuildingObjectView = BuildingObject as global::Kampai.Game.View.TikiBarBuildingObjectView;
			if (stageBuildingObject == null && tikiBarBuildingObjectView == null)
			{
				return;
			}
			global::Kampai.Game.View.TaskingCharacterObject taskingCharacterObject = null;
			taskingCharacterObject = new global::Kampai.Game.View.TaskingCharacterObject(CharacterObject, RouteIndex);
			taskingCharacterObject.RoutingIndex = RouteIndex;
			global::Kampai.Game.AnimatingBuildingDefinition animatingBuildingDefinition = null;
			if (stageBuilding != null)
			{
				animatingBuildingDefinition = stageBuilding.Definition;
			}
			else if (tikiBarBuilding != null)
			{
				animatingBuildingDefinition = tikiBarBuilding.Definition;
			}
			global::UnityEngine.RuntimeAnimatorController animController = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>(animatingBuildingDefinition.AnimationDefinitions[0].MinionController);
			CharacterObject.SetAnimController(animController);
			global::Kampai.Game.TaskableBuilding taskableBuilding = byInstanceId as global::Kampai.Game.TaskableBuilding;
			if (taskableBuilding != null)
			{
				taskableBuilding.AddMinion(CharacterObject.ID, 0);
			}
			if (stageBuildingObject != null)
			{
				stageBuildingObject.MoveToRoutingPosition(CharacterObject, RouteIndex);
			}
			else if (tikiBarBuildingObjectView != null)
			{
				global::Kampai.Game.View.PhilView philView = CharacterObject as global::Kampai.Game.View.PhilView;
				if (philView != null)
				{
					tikiBarBuildingObjectView.MoveToRoutingPosition(CharacterObject, RouteIndex);
				}
				else
				{
					tikiBarBuildingObjectView.AddCharacterToBuildingActions(CharacterObject, PlayerService, RouteIndex, PrestigeService, GetNewQuestSignal);
				}
			}
		}
	}
}
