namespace Kampai.Game
{
	public class ShuffleCompositeBuildingPiecesCommand : global::strange.extensions.command.impl.Command
	{
		private const float MINION_REACT_RADIUS = 15f;

		[Inject]
		public int buildingID { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Common.MinionReactInRadiusSignal minionReactInRadiusSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.CompositeBuilding byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.CompositeBuilding>(buildingID);
			byInstanceId.ShufflePieceIDs();
			global::Kampai.Game.View.BuildingManagerView component = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::Kampai.Game.View.BuildingObject buildingObject = component.GetBuildingObject(buildingID);
			global::Kampai.Game.View.CompositeBuildingMediator component2 = buildingObject.GetComponent<global::Kampai.Game.View.CompositeBuildingMediator>();
			component2.PlayShuffleSequence(byInstanceId.AttachedCompositePieceIDs);
			minionReactInRadiusSignal.Dispatch(15f, buildingObject.transform.position);
		}
	}
}
