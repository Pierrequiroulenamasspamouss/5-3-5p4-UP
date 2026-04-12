namespace Kampai.Game
{
	internal sealed class ConnectableBuildingPickedUpCommand : global::strange.extensions.command.impl.Command
	{
		private global::Kampai.Game.View.BuildingManagerView bmv;

		[Inject]
		public int buildingId { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.ConnectableBuilding connectableBuilding = playerService.GetByInstanceId<global::Kampai.Game.Building>(buildingId) as global::Kampai.Game.ConnectableBuilding;
			if (connectableBuilding != null)
			{
				bmv = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
				global::Kampai.Game.View.BuildingObject buildingObject = bmv.GetBuildingObject(buildingId);
				global::UnityEngine.Vector3 position = buildingObject.transform.position;
				global::UnityEngine.Vector3 localEulerAngles = buildingObject.transform.localEulerAngles;
				bmv.RemoveBuilding(connectableBuilding.ID);
				global::UnityEngine.GameObject gameObject = bmv.CreateBuilding(connectableBuilding, 2);
				global::Kampai.Game.View.BuildingObject component = gameObject.GetComponent<global::Kampai.Game.View.BuildingObject>();
				component.transform.localEulerAngles = localEulerAngles;
				component.transform.position = position;
				component.ID = connectableBuilding.ID;
			}
		}
	}
}
