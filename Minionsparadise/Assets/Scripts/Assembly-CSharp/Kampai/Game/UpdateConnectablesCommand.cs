namespace Kampai.Game
{
	internal sealed class UpdateConnectablesCommand : global::strange.extensions.command.impl.Command
	{
		private global::Kampai.Game.View.BuildingManagerView bmv;

		[Inject]
		public global::Kampai.Game.Location location { get; set; }

		[Inject]
		public int connectableType { get; set; }

		[Inject]
		public global::Kampai.Game.Environment environment { get; set; }

		[Inject]
		public global::Kampai.Game.DecoGridModel decoGridModel { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		public override void Execute()
		{
			int[] array = new int[2] { -1, 1 };
			bmv = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			for (int i = 0; i < array.Length; i++)
			{
				global::Kampai.Game.Building building = environment.GetBuilding(location.x, location.y + array[i]);
				if (building != null && building is global::Kampai.Game.ConnectableBuilding)
				{
					UpdateConnectable(building);
				}
			}
			for (int j = 0; j < array.Length; j++)
			{
				global::Kampai.Game.Building building2 = environment.GetBuilding(location.x + array[j], location.y);
				if (building2 != null && building2 is global::Kampai.Game.ConnectableBuilding)
				{
					UpdateConnectable(building2);
				}
			}
			global::Kampai.Game.Building building3 = environment.GetBuilding(location.x, location.y);
			if (building3 != null && building3 is global::Kampai.Game.ConnectableBuilding)
			{
				UpdateConnectable(building3);
			}
		}

		private void UpdateConnectable(global::Kampai.Game.Building building)
		{
			global::Kampai.Game.ConnectableBuildingDefinition connectableBuildingDefinition = building.Definition as global::Kampai.Game.ConnectableBuildingDefinition;
			if (connectableType == connectableBuildingDefinition.connectableType)
			{
				int outDirection;
				global::Kampai.Game.ConnectableBuildingPieceType connectablePieceType = decoGridModel.GetConnectablePieceType(building.Location.x, building.Location.y, connectableBuildingDefinition.connectableType, out outDirection);
				global::Kampai.Game.ConnectableBuilding connectableBuilding = building as global::Kampai.Game.ConnectableBuilding;
				connectableBuilding.pieceType = connectablePieceType;
				connectableBuilding.rotation = outDirection;
				bmv.RemoveBuilding(connectableBuilding.ID);
				global::UnityEngine.GameObject gameObject = bmv.CreateBuilding(building, (int)connectablePieceType);
				global::Kampai.Game.View.BuildingObject component = gameObject.GetComponent<global::Kampai.Game.View.BuildingObject>();
				component.transform.localEulerAngles = new global::UnityEngine.Vector3(0f, outDirection, 0f);
				component.ID = building.ID;
			}
		}
	}
}
