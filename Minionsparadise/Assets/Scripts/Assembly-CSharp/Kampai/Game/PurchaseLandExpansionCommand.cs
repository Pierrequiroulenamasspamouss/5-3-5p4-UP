namespace Kampai.Game
{
	public class PurchaseLandExpansionCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("PurchaseLandExpansionCommand") as global::Kampai.Util.IKampaiLogger;

		private global::System.Collections.Generic.List<int> generatedDebris;

		private global::System.Collections.Generic.List<int> generatedAspirationalBuildings;

		private global::Kampai.Game.CommonLandExpansionDefinition commonDefinition;

		private global::Kampai.Game.LandExpansionConfig config;

		private global::UnityEngine.GameObject flamethrowerMinion;

		private global::UnityEngine.Animator flamethrowerAnimator;

		private global::Kampai.Game.BobCharacter bob;

		private global::Kampai.Game.View.BuildingManagerView buildingManagerView;

		private global::System.Collections.Generic.List<global::Kampai.Game.LandExpansionBuilding> expansionBuildings;

		[Inject]
		public int expansionID { get; set; }

		[Inject]
		public bool playFX { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Common.IRandomService randomService { get; set; }

		[Inject]
		public global::Kampai.Game.ILandExpansionService landExpansionService { get; set; }

		[Inject]
		public global::Kampai.Game.ILandExpansionConfigService landExpansionConfigService { get; set; }

		[Inject]
		public global::Kampai.Game.BurnLandExpansionSignal burnLandSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.CreateInventoryBuildingSignal createInventoryBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.CreateForSaleSignSignal createForSaleSignSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal audioSignal { get; set; }

		[Inject]
		public global::Kampai.Game.PlaceBuildingSignal placeBuilding { get; set; }

		[Inject]
		public global::Kampai.Game.RemoveBuildingSignal removeFootprintSignal { get; set; }

		[Inject]
		public global::Kampai.Game.BobCelebrateLandExpansionSignal celebrateLandExpansionSignal { get; set; }

		[Inject]
		public global::Kampai.Game.BobCelebrateLandExpansionCompleteSignal celebrateLandExpansionCompleteSignal { get; set; }

		[Inject]
		public global::Kampai.Game.PointBobLandExpansionSignal pointBobLandExpansionSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService prestigeService { get; set; }

		[Inject]
		public global::Kampai.UI.View.RemoveWayFinderSignal removeWayFinderSignal { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		public override void Execute()
		{
			config = landExpansionConfigService.GetExpansionConfig(expansionID);
			buildingManagerView = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			expansionBuildings = landExpansionService.GetBuildingsByExpansionID(expansionID) as global::System.Collections.Generic.List<global::Kampai.Game.LandExpansionBuilding>;
			if (expansionBuildings == null || expansionBuildings.Count == 0)
			{
				logger.Error("No expansion buildings for {0}", expansionID);
				return;
			}
			bob = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.BobCharacter>(70002);
			if (bob != null && prestigeService.GetPrestigeFromMinionInstance(bob).state != global::Kampai.Game.PrestigeState.Questing)
			{
				removeWayFinderSignal.Dispatch(bob.ID);
			}
			int quantity = (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID);
			foreach (int adjacentExpansionId in config.adjacentExpansionIds)
			{
				if (!landExpansionService.IsLevelGated(adjacentExpansionId, quantity))
				{
					createForSaleSignSignal.Dispatch(adjacentExpansionId, true);
				}
			}
			generatedDebris = new global::System.Collections.Generic.List<int>();
			generatedAspirationalBuildings = new global::System.Collections.Generic.List<int>();
			commonDefinition = definitionService.Get<global::Kampai.Game.CommonLandExpansionDefinition>();
			if (commonDefinition == null)
			{
				logger.Log(global::Kampai.Util.KampaiLogLevel.Error, "Unable to find the common expansion definition.");
			}
			GenerateDebris();
			HandleAspirationalBuildings();
			HandleBob();
		}

		private void HandleAspirationalBuildings()
		{
			foreach (int containedAspirationalBuilding in config.containedAspirationalBuildings)
			{
				global::Kampai.Game.Building aspirationalBuilding = landExpansionService.GetAspirationalBuilding(containedAspirationalBuilding);
				if (aspirationalBuilding != null && playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Building>(aspirationalBuilding.Definition.ID) == null)
				{
					playerService.Add(aspirationalBuilding);
					generatedAspirationalBuildings.Add(containedAspirationalBuilding);
					aspirationalBuilding.SetState(global::Kampai.Game.BuildingState.Idle);
				}
			}
		}

		private void HandleBob()
		{
			if (playerService.GetTargetExpansion() == expansionID)
			{
				celebrateLandExpansionCompleteSignal.AddListener(BobCelebrationComplete);
				celebrateLandExpansionSignal.Dispatch();
				return;
			}
			if (bob != null && generatedAspirationalBuildings.Count == 0)
			{
				pointBobLandExpansionSignal.Dispatch();
			}
			routineRunner.StartCoroutine(BurnGrass());
		}

		public void BobCelebrationComplete()
		{
			celebrateLandExpansionCompleteSignal.RemoveListener(BobCelebrationComplete);
			pointBobLandExpansionSignal.Dispatch();
			routineRunner.StartCoroutine(BurnGrass());
		}

		private void GenerateDebris()
		{
			foreach (int containedDebri in config.containedDebris)
			{
				global::Kampai.Game.Building debris = landExpansionService.GetDebris(containedDebri);
				playerService.Add(debris);
				generatedDebris.Add(containedDebri);
				debris.SetState(global::Kampai.Game.BuildingState.Idle);
			}
		}

		private global::System.Collections.IEnumerator BurnGrass()
		{
			if (expansionBuildings.Count == 0)
			{
				yield break;
			}
			int startBuildingIndex = randomService.NextInt(expansionBuildings.Count);
			global::Kampai.Game.LandExpansionBuilding startBuilding = expansionBuildings[startBuildingIndex];
			global::System.Collections.Generic.List<global::Kampai.Game.LandExpansionBuilding> burningBuildings = new global::System.Collections.Generic.List<global::Kampai.Game.LandExpansionBuilding>(expansionBuildings.Count);
			global::System.Collections.Generic.List<global::Kampai.Game.LandExpansionBuilding> freshBurnList = new global::System.Collections.Generic.List<global::Kampai.Game.LandExpansionBuilding>(expansionBuildings.Count);
			audioSignal.Dispatch("Play_grass_clear_01");
			BurnBuilding(startBuilding, buildingManagerView);
			burningBuildings.Add(startBuilding);
			yield return new global::UnityEngine.WaitForSeconds(0.25f);
			landExpansionService.RemoveForSaleSign(expansionID);
			while (burningBuildings.Count < expansionBuildings.Count)
			{
				foreach (global::Kampai.Game.LandExpansionBuilding burningBuilding in burningBuildings)
				{
					foreach (global::Kampai.Game.LandExpansionBuilding building in expansionBuildings)
					{
						if (!burningBuildings.Contains(building) && !freshBurnList.Contains(building) && IsAdjacentBuilding(building, burningBuilding))
						{
							BurnBuilding(building, buildingManagerView);
							freshBurnList.Add(building);
						}
					}
				}
				burningBuildings.AddRange(freshBurnList);
				freshBurnList.Clear();
				yield return new global::UnityEngine.WaitForSeconds(0.25f);
			}
			UpdateDebrisState();
			UpdateAspirationalBuildingState();
			string landExpansionName = new global::System.Text.StringBuilder().Append("LandExpansion").Append(config.ID).ToString();
			telemetryService.Send_Telemetry_EVT_GP_ACHIEVEMENTS_CHECKPOINTS_EAL(landExpansionName, global::Kampai.Common.Service.Telemetry.TelemetryAchievementType.LandExpansion, 0, string.Empty);
		}

		internal void BurnBuilding(global::Kampai.Game.LandExpansionBuilding building, global::Kampai.Game.View.BuildingManagerView buildingManagerView)
		{
			removeFootprintSignal.Dispatch(building.Location, definitionService.GetBuildingFootprint(building.Definition.FootprintID));
			global::Kampai.Game.View.LandExpansionBuildingObject landExpansionBuildingObject = buildingManagerView.GetBuildingObject(building.ID) as global::Kampai.Game.View.LandExpansionBuildingObject;
			if (!(landExpansionBuildingObject == null))
			{
				if (playFX && landExpansionBuildingObject != null)
				{
					landExpansionBuildingObject.Burn(burnLandSignal, building.ID, commonDefinition.VFXGrassClearing);
				}
				else
				{
					burnLandSignal.Dispatch(building.ID);
				}
			}
		}

		internal bool IsAdjacentBuilding(global::Kampai.Game.LandExpansionBuilding building, global::Kampai.Game.LandExpansionBuilding burningBuilding)
		{
			if (global::System.Math.Abs(building.Location.x - burningBuilding.Location.x) <= 3 && global::System.Math.Abs(building.Location.y - burningBuilding.Location.y) <= 3)
			{
				return true;
			}
			return false;
		}

		internal void UpdateDebrisState()
		{
			global::Kampai.Game.View.BuildingManagerView component = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			foreach (int generatedDebri in generatedDebris)
			{
				global::Kampai.Game.Building debris = landExpansionService.GetDebris(generatedDebri);
				global::Kampai.Game.DebrisDefinition debrisDefinition = definitionService.Get<global::Kampai.Game.DebrisDefinition>(generatedDebri);
				component.CleanupBuilding(-debrisDefinition.ID);
				createInventoryBuildingSignal.Dispatch(debris, debris.Location);
				placeBuilding.Dispatch(debris.ID, debris.Location);
			}
		}

		internal void UpdateAspirationalBuildingState()
		{
			global::Kampai.Game.View.BuildingManagerView component = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			foreach (int generatedAspirationalBuilding in generatedAspirationalBuildings)
			{
				global::Kampai.Game.Building aspirationalBuilding = landExpansionService.GetAspirationalBuilding(generatedAspirationalBuilding);
				global::Kampai.Game.AspirationalBuildingDefinition aspirationalBuildingDefinition = definitionService.Get<global::Kampai.Game.AspirationalBuildingDefinition>(generatedAspirationalBuilding);
				component.CleanupBuilding(-aspirationalBuildingDefinition.ID);
				createInventoryBuildingSignal.Dispatch(aspirationalBuilding, aspirationalBuilding.Location);
			}
		}
	}
}
