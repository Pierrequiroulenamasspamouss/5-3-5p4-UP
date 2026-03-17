namespace Kampai.Game.View
{
	public class UnlockVillainLairCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.VillainLairEntranceBuilding portal { get; set; }

		[Inject]
		public int villainLairDefinitionID { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Game.LoadVillainLairAssetsSignal loadVillainLairAssetsSignal { get; set; }

		public override void Execute()
		{
			if (!portal.IsUnlocked)
			{
				global::Kampai.Game.VillainLairDefinition villainLairDefinition = definitionService.Get<global::Kampai.Game.VillainLairDefinition>(villainLairDefinitionID);
				global::Kampai.Game.VillainLair villainLair = (global::Kampai.Game.VillainLair)villainLairDefinition.Build();
				playerService.Add(villainLair);
				portal.unlockVillainLair(villainLair.ID);
				CreateResourcePlots(villainLairDefinition, villainLair);
				routineRunner.StartCoroutine(LoadLairAssets());
			}
		}

		private global::System.Collections.IEnumerator LoadLairAssets()
		{
			yield return null;
			loadVillainLairAssetsSignal.Dispatch(portal.VillainLairInstanceID);
		}

		private void CreateResourcePlots(global::Kampai.Game.VillainLairDefinition lairDefinition, global::Kampai.Game.VillainLair lair)
		{
			for (int i = 0; i < lairDefinition.ResourcePlots.Count; i++)
			{
				global::Kampai.Game.ResourcePlotDefinition resourcePlotDefinition = lairDefinition.ResourcePlots[i];
				global::Kampai.Game.VillainLairResourcePlotDefinition villainLairResourcePlotDefinition = definitionService.Get<global::Kampai.Game.VillainLairResourcePlotDefinition>(lair.Definition.ResourceBuildingDefID);
				global::Kampai.Game.VillainLairResourcePlot villainLairResourcePlot = villainLairResourcePlotDefinition.Build() as global::Kampai.Game.VillainLairResourcePlot;
				villainLairResourcePlot.Location = new global::Kampai.Game.Location(resourcePlotDefinition.location.x + lair.Definition.Location.y, resourcePlotDefinition.location.y + lair.Definition.Location.y);
				villainLairResourcePlot.rotation = resourcePlotDefinition.rotation;
				villainLairResourcePlot.parentLair = lair;
				villainLairResourcePlot.indexInLairResourcePlots = i;
				villainLairResourcePlot.State = ((!resourcePlotDefinition.isAutomaticallyUnlocked) ? global::Kampai.Game.BuildingState.Inaccessible : global::Kampai.Game.BuildingState.Idle);
				villainLairResourcePlot.unlockTransactionID = resourcePlotDefinition.unlockTransactionID;
				playerService.Add(villainLairResourcePlot);
				lair.resourcePlotInstanceIDs.Add(villainLairResourcePlot.ID);
				lair.portalInstanceID = portal.ID;
			}
		}
	}
}
