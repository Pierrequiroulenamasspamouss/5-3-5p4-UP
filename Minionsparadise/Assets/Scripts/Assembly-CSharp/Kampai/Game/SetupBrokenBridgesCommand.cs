namespace Kampai.Game
{
	public class SetupBrokenBridgesCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.CreateInventoryBuildingSignal createInventroyBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.BuildingChangeStateSignal changeState { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.Game.RepairBridgeSignal repairSignal { get; set; }

		[Inject]
		public global::Kampai.Game.Environment environment { get; set; }

		[Inject]
		public global::Kampai.Game.DebugUpdateGridSignal gridSignal { get; set; }

		public override void Execute()
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.BridgeDefinition> all = definitionService.GetAll<global::Kampai.Game.BridgeDefinition>();
			foreach (global::Kampai.Game.BridgeDefinition item in all)
			{
				global::Kampai.Game.Building building = environment.GetBuilding(item.location.x, item.location.y);
				global::Kampai.Game.BridgeBuilding bridgeBuilding = building as global::Kampai.Game.BridgeBuilding;
				if (building == null)
				{
					global::Kampai.Game.BuildingDefinition buildingDefinition = definitionService.Get(item.BuildingId) as global::Kampai.Game.BuildingDefinition;
					bridgeBuilding = buildingDefinition.Build() as global::Kampai.Game.BridgeBuilding;
					bridgeBuilding.BridgeId = item.ID;
					bridgeBuilding.Location = new global::Kampai.Game.Location(item.location.x, item.location.y);
					global::UnityEngine.Vector3 pos = new global::UnityEngine.Vector3(item.location.x, 0f, item.location.y);
					global::Kampai.Game.Location type = new global::Kampai.Game.Location(pos);
					playerService.Add(bridgeBuilding);
					createInventroyBuildingSignal.Dispatch(bridgeBuilding, type);
					changeState.Dispatch(bridgeBuilding.ID, global::Kampai.Game.BuildingState.Idle);
				}
				else if (bridgeBuilding.BridgeId != 0 && questService.IsBridgeQuestComplete(bridgeBuilding.BridgeId))
				{
					repairSignal.Dispatch(bridgeBuilding);
				}
				if (bridgeBuilding != null)
				{
					bridgeBuilding.UnlockLevel = GetBridgeUnlockLevel(item);
				}
			}
			gridSignal.Dispatch();
		}

		private int GetBridgeUnlockLevel(global::Kampai.Game.BridgeDefinition bridgeDef)
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.QuestDefinition> all = definitionService.GetAll<global::Kampai.Game.QuestDefinition>();
			foreach (global::Kampai.Game.QuestDefinition item in all)
			{
				if (item.SurfaceType != global::Kampai.Game.QuestSurfaceType.Character)
				{
					continue;
				}
				for (int i = 0; i < item.QuestSteps.Count; i++)
				{
					if (item.QuestSteps[i].ItemDefinitionID == bridgeDef.ID && item.QuestSteps[i].Type == global::Kampai.Game.QuestStepType.BridgeRepair)
					{
						return item.UnlockLevel;
					}
				}
			}
			return 0;
		}
	}
}
