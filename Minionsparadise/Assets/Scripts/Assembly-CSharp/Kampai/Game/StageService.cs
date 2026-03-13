namespace Kampai.Game
{
	public class StageService
	{
		private global::UnityEngine.GameObject stageBackdrop;

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		public void ShowStageBackdrop()
		{
			if (stageBackdrop == null)
			{
				global::Kampai.Game.StageBuilding firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.StageBuilding>(3054);
				string backdropPrefabName = firstInstanceByDefinitionId.Definition.backdropPrefabName;
				global::UnityEngine.GameObject original = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.GameObject>(backdropPrefabName);
				global::UnityEngine.Vector3 position = new global::UnityEngine.Vector3(firstInstanceByDefinitionId.Location.x, 0f, firstInstanceByDefinitionId.Location.y);
				stageBackdrop = global::UnityEngine.Object.Instantiate(original);
				stageBackdrop.transform.position = position;
			}
			stageBackdrop.SetActive(true);
		}

		public void HideStageBackdrop()
		{
			if (stageBackdrop != null)
			{
				stageBackdrop.SetActive(false);
			}
		}
	}
}
