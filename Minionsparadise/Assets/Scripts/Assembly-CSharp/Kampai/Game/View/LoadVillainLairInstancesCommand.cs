namespace Kampai.Game.View
{
	public class LoadVillainLairInstancesCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("LoadVillainLairInstancesCommand") as global::Kampai.Util.IKampaiLogger;

		private int lairDefinitionID;

		private global::Kampai.Game.VillainLair currentLair;

		[Inject]
		public int villainLairInstanceID { get; set; }

		[Inject]
		public global::System.Action callback { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IMasterPlanService masterPlanService { get; set; }

		[Inject]
		public global::Kampai.Game.VillainLairModel villainLairModel { get; set; }

		[Inject]
		public global::Kampai.Game.ParentLairResourcePlotSignal parentLairResourcePlotSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.VILLAIN_LAIR_PARENT)]
		public global::UnityEngine.GameObject villainLairParent { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		public override void Execute()
		{
			currentLair = playerService.GetByInstanceId<global::Kampai.Game.VillainLair>(villainLairInstanceID);
			if (currentLair == null)
			{
				logger.Error("Trying to load instanves for a  villain lair that doesn't exist: {0}", villainLairInstanceID);
				return;
			}
			lairDefinitionID = currentLair.Definition.ID;
			if (!villainLairModel.areLairAssetsLoaded)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.CMD_INCOMPLETE_VILLAIN_LAIR_ASSETS_RESOURCES_UI, "Assets for Villain Lair {0} are not loaded", currentLair.ID);
			}
			LoadInstances();
		}

		private void LoadInstances()
		{
			if (villainLairModel.villainLairInstances.Keys.Contains(villainLairInstanceID))
			{
				return;
			}
			foreach (int value in global::System.Enum.GetValues(typeof(global::Kampai.Game.VillainLairModel.LairPrefabType)))
			{
				LoadInstances((global::Kampai.Game.VillainLairModel.LairPrefabType)value);
			}
			InstanceLoadingComplete();
		}

		private void InstanceLoadingComplete()
		{
			callback();
		}

		private void LoadInstances(global::Kampai.Game.VillainLairModel.LairPrefabType prefabType)
		{
			switch (prefabType)
			{
			case global::Kampai.Game.VillainLairModel.LairPrefabType.LAIR:
				LoadLairInstance(prefabType);
				break;
			case global::Kampai.Game.VillainLairModel.LairPrefabType.LOCKED_PLOT:
			case global::Kampai.Game.VillainLairModel.LairPrefabType.UNLOCKED_PLOT:
				LoadPlotInstances(prefabType);
				break;
			default:
				logger.Error("Trying to load an unaccounted instance {0}: need a method to load this for the villain lair.");
				break;
			}
		}

		private void LoadPlotInstances(global::Kampai.Game.VillainLairModel.LairPrefabType prefabType)
		{
			bool flag = prefabType == global::Kampai.Game.VillainLairModel.LairPrefabType.LOCKED_PLOT;
			global::Kampai.Game.View.BuildingManagerView component = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			for (int i = 0; i < currentLair.resourcePlotInstanceIDs.Count; i++)
			{
				global::Kampai.Game.VillainLairResourcePlot byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.VillainLairResourcePlot>(currentLair.resourcePlotInstanceIDs[i]);
				if (flag != (byInstanceId.State == global::Kampai.Game.BuildingState.Inaccessible))
				{
					continue;
				}
				global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(villainLairModel.asyncLoadedPrefabs[(int)prefabType], (global::UnityEngine.Vector3)byInstanceId.Location, global::UnityEngine.Quaternion.Euler(0f, byInstanceId.rotation, 0f)) as global::UnityEngine.GameObject;
				if (gameObject == null)
				{
					logger.Fatal(global::Kampai.Util.FatalCode.CMD_NULL_PREFAB, "Villain Lair Island Resources prefab is null: {0}", currentLair.Definition.ResourceBuildingDefID);
					break;
				}
				global::Kampai.Game.View.VillainLairResourcePlotObjectView villainLairResourcePlotObjectView = component.GetBuildingObject(byInstanceId.ID) as global::Kampai.Game.View.VillainLairResourcePlotObjectView;
				if (villainLairResourcePlotObjectView != null)
				{
					parentLairResourcePlotSignal.Dispatch(byInstanceId, gameObject);
				}
				else
				{
					component.CreateBuilding(byInstanceId);
					villainLairResourcePlotObjectView = component.GetBuildingObject(byInstanceId.ID) as global::Kampai.Game.View.VillainLairResourcePlotObjectView;
					if (villainLairResourcePlotObjectView != null)
					{
						parentLairResourcePlotSignal.Dispatch(byInstanceId, gameObject);
					}
				}
				villainLairResourcePlotObjectView.InitializeAnimators();
			}
		}

		private void LoadLairInstance(global::Kampai.Game.VillainLairModel.LairPrefabType prefabType)
		{
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(villainLairModel.asyncLoadedPrefabs[(int)prefabType]);
			if (gameObject == null)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.CMD_NULL_PREFAB, "Villain Lair Island prefab is null: {0}", lairDefinitionID);
				return;
			}
			villainLairModel.villainLairInstances[currentLair.ID] = gameObject;
			gameObject.transform.parent = villainLairParent.transform;
			global::Kampai.Game.View.VillainLairLocationView componentInChildren = gameObject.GetComponentInChildren<global::Kampai.Game.View.VillainLairLocationView>();
			global::Kampai.Game.MasterPlan currentMasterPlan = masterPlanService.CurrentMasterPlan;
			if (currentMasterPlan == null)
			{
				logger.Error("Master Plan Instance is null on creation of the Villain Lair!");
				return;
			}
			global::Kampai.Game.MasterPlanDefinition definition = currentMasterPlan.Definition;
			componentInChildren.SetUpInstanceIDs(definition, playerService, logger);
			gameObject.transform.position = (global::UnityEngine.Vector3)currentLair.Definition.Location;
			global::Kampai.Game.View.MasterPlanObject masterPlanObject = gameObject.AddComponent<global::Kampai.Game.View.MasterPlanObject>();
			masterPlanObject.Init(currentMasterPlan.ID);
			masterPlanService.AddMasterPlanObject(masterPlanObject);
		}
	}
}
