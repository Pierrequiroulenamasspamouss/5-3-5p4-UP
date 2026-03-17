namespace Kampai.Game
{
	public class RelocateTaskedMinionsCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.Building building { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		[Inject(global::Kampai.Game.GameElement.MINION_MANAGER)]
		public global::UnityEngine.GameObject minionManager { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.View.MinionManagerView component = minionManager.GetComponent<global::Kampai.Game.View.MinionManagerView>();
			global::Kampai.Game.View.BuildingManagerView component2 = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::Kampai.Game.TaskableBuilding taskableBuilding = building as global::Kampai.Game.TaskableBuilding;
			global::Kampai.Game.View.TaskableBuildingObject taskableBuildingObject = null;
			if (taskableBuilding != null)
			{
				taskableBuildingObject = component2.GetBuildingObject(building.ID) as global::Kampai.Game.View.TaskableBuildingObject;
			}
			global::Kampai.Game.LeisureBuilding leisureBuilding = building as global::Kampai.Game.LeisureBuilding;
			global::Kampai.Game.View.LeisureBuildingObjectView leisureBuildingObjectView = null;
			if (leisureBuilding != null)
			{
				leisureBuildingObjectView = component2.GetBuildingObject(building.ID) as global::Kampai.Game.View.LeisureBuildingObjectView;
			}
			global::System.Collections.Generic.IList<int> list3;
			if (taskableBuilding == null)
			{
				global::System.Collections.Generic.IList<int> list2;
				global::System.Collections.Generic.IList<int> list;
				if (leisureBuilding == null)
				{
					list = null;
					list2 = list;
				}
				else
				{
					list2 = leisureBuilding.MinionList;
				}
				list = list2;
				list3 = list;
			}
			else
			{
				list3 = taskableBuilding.MinionList;
			}
			global::System.Collections.Generic.IList<int> list4 = list3;
			if (list4 == null)
			{
				return;
			}
			for (int i = 0; i < list4.Count; i++)
			{
				global::Kampai.Game.View.MinionObject characterObject = component.Get(list4[i]);
				if (taskableBuildingObject != null)
				{
					taskableBuildingObject.MoveToRoutingPosition(characterObject, i);
				}
				if (leisureBuildingObjectView != null)
				{
					leisureBuildingObjectView.MoveToRoutingPosition(characterObject, i);
				}
			}
		}
	}
}
