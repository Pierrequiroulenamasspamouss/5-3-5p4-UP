namespace Kampai.Game.View
{
	public class CleanupDebrisCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("CleanupDebrisCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public int buildingID { get; set; }

		[Inject]
		public bool showVFX { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Game.RemoveBuildingSignal removeBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		public override void Execute()
		{
			routineRunner.StartCoroutine(WaitAFrame());
		}

		private global::System.Collections.IEnumerator WaitAFrame()
		{
			global::Kampai.Game.View.BuildingManagerView buildingManagerView = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::Kampai.Game.View.BuildingObject buildObj = buildingManagerView.GetBuildingObject(buildingID);
			global::Kampai.Game.DebrisBuilding building = playerService.GetByInstanceId<global::Kampai.Game.DebrisBuilding>(buildingID);
			logger.Info("Cleaning up debris: {0}", buildingID);
			if (showVFX)
			{
				yield return new global::UnityEngine.WaitForEndOfFrame();
				if (!(buildObj == null) && building != null)
				{
					global::Kampai.Game.DebrisBuildingDefinition def = building.Definition;
					for (int i = 0; i < def.VFXPrefabs.Count; i++)
					{
						global::UnityEngine.GameObject clearingGO = global::UnityEngine.Object.Instantiate(global::Kampai.Util.KampaiResources.Load<global::UnityEngine.GameObject>(def.VFXPrefabs[i]));
						clearingGO.transform.parent = buildObj.gameObject.transform;
						clearingGO.transform.localPosition = global::UnityEngine.Vector3.zero;
					}
					global::Kampai.Game.View.DebrisBuildingObject debrisObject = buildObj as global::Kampai.Game.View.DebrisBuildingObject;
					if (debrisObject != null)
					{
						debrisObject.FadeDebris();
					}
				}
			}
			else
			{
				buildingManagerView.RemoveBuilding(buildingID);
				removeBuildingSignal.Dispatch(building.Location, definitionService.GetBuildingFootprint(building.Definition.FootprintID));
				playerService.Remove(building);
			}
		}
	}
}
