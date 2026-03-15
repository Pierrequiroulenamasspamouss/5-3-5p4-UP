namespace Kampai.Game
{
	public class EnableBuildingAnimatorsCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public bool enable { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.View.BuildingManagerView component = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::System.Collections.Generic.ICollection<int> animatingBuildingIDs = playerService.GetAnimatingBuildingIDs();
			foreach (int item in animatingBuildingIDs)
			{
				global::Kampai.Game.View.AnimatingBuildingObject animatingBuildingObject = component.GetBuildingObject(item) as global::Kampai.Game.View.AnimatingBuildingObject;
				if (animatingBuildingObject != null)
				{
					animatingBuildingObject.EnableAnimators(enable);
				}
			}
		}
	}
}
