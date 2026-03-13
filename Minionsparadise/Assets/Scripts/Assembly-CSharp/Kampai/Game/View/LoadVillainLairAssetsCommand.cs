namespace Kampai.Game.View
{
	public class LoadVillainLairAssetsCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("LoadVillainLairAssetsCommand") as global::Kampai.Util.IKampaiLogger;

		private global::Kampai.Game.VillainLairResourcePlotDefinition resourcePlotDefinition;

		private global::Kampai.Game.VillainLair currentLair;

		[Inject]
		public int villainLairInstanceId { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.VillainLairModel villainLairModel { get; set; }

		[Inject]
		public global::Kampai.Game.VillainLairAssetsLoadedSignal loadingCompleteSignal { get; set; }

		public override void Execute()
		{
			currentLair = playerService.GetByInstanceId<global::Kampai.Game.VillainLair>(villainLairInstanceId);
			if (currentLair == null)
			{
				logger.Info("Lair ID {0} is not in player service: aborting load of lair and all instances associated with it.", villainLairInstanceId);
			}
			else
			{
				resourcePlotDefinition = definitionService.Get<global::Kampai.Game.VillainLairResourcePlotDefinition>(currentLair.Definition.ResourceBuildingDefID);
				LoadNextPrefab(0);
			}
		}

		public void LoadNextPrefab(int indexIntoPrefabs)
		{
			if (!villainLairModel.areLairAssetsLoaded)
			{
				switch (indexIntoPrefabs)
				{
				case 0:
					global::Kampai.Util.KampaiResources.LoadAsync(currentLair.Definition.Prefab, routineRunner, LoadLairPrefab);
					break;
				case 1:
					global::Kampai.Util.KampaiResources.LoadAsync(resourcePlotDefinition.brokenPrefab_loaded, routineRunner, LoadLockedResourcesPrefab);
					break;
				case 2:
					global::Kampai.Util.KampaiResources.LoadAsync(resourcePlotDefinition.prefab_loaded, routineRunner, LoadUnlockedResourcesPrefab);
					break;
				default:
					logger.Error("Villain lair prefabs aren't fully loaded, but we're attempting to use an invalid index ({0}) into the prefab types", indexIntoPrefabs);
					break;
				}
			}
			else
			{
				LoadingFinished();
			}
		}

		private void LoadingFinished()
		{
			loadingCompleteSignal.Dispatch(true);
		}

		private void LoadUnlockedResourcesPrefab(global::UnityEngine.Object prefabObj)
		{
			global::Kampai.Game.VillainLairModel.LairPrefabType lairPrefabType = global::Kampai.Game.VillainLairModel.LairPrefabType.UNLOCKED_PLOT;
			villainLairModel.asyncLoadedPrefabs[(int)lairPrefabType] = prefabObj as global::UnityEngine.GameObject;
			LoadNextPrefab((int)(lairPrefabType + 1));
		}

		private void LoadLockedResourcesPrefab(global::UnityEngine.Object prefabObj)
		{
			global::Kampai.Game.VillainLairModel.LairPrefabType lairPrefabType = global::Kampai.Game.VillainLairModel.LairPrefabType.LOCKED_PLOT;
			villainLairModel.asyncLoadedPrefabs[(int)lairPrefabType] = prefabObj as global::UnityEngine.GameObject;
			LoadNextPrefab((int)(lairPrefabType + 1));
		}

		private void LoadLairPrefab(global::UnityEngine.Object prefabObj)
		{
			global::Kampai.Game.VillainLairModel.LairPrefabType lairPrefabType = global::Kampai.Game.VillainLairModel.LairPrefabType.LAIR;
			villainLairModel.asyncLoadedPrefabs[(int)lairPrefabType] = prefabObj as global::UnityEngine.GameObject;
			LoadNextPrefab((int)(lairPrefabType + 1));
		}
	}
}
