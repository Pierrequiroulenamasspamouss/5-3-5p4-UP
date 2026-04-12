namespace Kampai.Game
{
	public class SetupFlowersCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.ILandExpansionService landExpansionService { get; set; }

		[Inject(global::Kampai.Game.GameElement.FLOWER_PARENT)]
		public global::UnityEngine.GameObject parent { get; set; }

		public override void Execute()
		{
			foreach (global::Kampai.Game.LandExpansionBuilding trackedFlower in landExpansionService.GetTrackedFlowers())
			{
				global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(global::Kampai.Util.KampaiResources.Load<global::UnityEngine.GameObject>(trackedFlower.GetPrefab()));
				gameObject.transform.parent = parent.transform;
				gameObject.transform.position = new global::UnityEngine.Vector3(trackedFlower.Location.x, 0f, trackedFlower.Location.y);
				landExpansionService.AddToFlowerMap(trackedFlower.ExpansionID, gameObject);
			}
		}
	}
}
