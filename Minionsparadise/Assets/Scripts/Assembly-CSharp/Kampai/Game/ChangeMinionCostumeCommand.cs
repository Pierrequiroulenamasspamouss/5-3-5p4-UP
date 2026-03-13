namespace Kampai.Game
{
	public class ChangeMinionCostumeCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.View.MinionObject minionObject { get; set; }

		[Inject]
		public global::Kampai.Game.CostumeItemDefinition newCostume { get; set; }

		[Inject]
		public global::Kampai.Game.IDLCService dlcService { get; set; }

		[Inject]
		public global::Kampai.Util.IMinionBuilder minionBuilder { get; set; }

		public override void Execute()
		{
			global::UnityEngine.SkinnedMeshRenderer[] componentsInChildren = minionObject.gameObject.GetComponentsInChildren<global::UnityEngine.SkinnedMeshRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].transform.parent = null;
				componentsInChildren[i].enabled = false;
				global::UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
			}
			string targetLOD = dlcService.GetDownloadQualityLevel().ToUpper();
			global::Kampai.Util.SkinnedMeshAggregator.AddSubModels(newCostume.MeshList, minionObject.transform, targetLOD);
			minionObject.RefreshRenderers();
			minionBuilder.RebuildMinion(minionObject.gameObject);
		}
	}
}
