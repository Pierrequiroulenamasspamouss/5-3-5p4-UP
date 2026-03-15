namespace Kampai.Game
{
	public class BridgeRepairQuestStepController : global::Kampai.Game.QuestStepController
	{
		private global::Kampai.Game.IDefinitionService definitionService;

		private global::Kampai.Game.Environment environment;

		public BridgeRepairQuestStepController(global::Kampai.Game.Quest quest, int stepIndex, global::Kampai.Game.IQuestScriptService questScriptService, global::Kampai.Game.IPlayerService playerService, global::Kampai.Game.IDefinitionService definitionService, global::strange.extensions.context.api.ICrossContextCapable gameContext, global::Kampai.Util.IKampaiLogger logger, global::Kampai.Game.Environment environment)
			: base(quest, stepIndex, questScriptService, playerService, gameContext, logger)
		{
			this.environment = environment;
			this.definitionService = definitionService;
		}

		public override void UpdateTask(global::Kampai.Game.QuestTaskTransition questTaskTransition, global::Kampai.Game.Building building, int buildingDefId, int itemDefId)
		{
			if (base.questStep.TrackedID == building.ID)
			{
				if (questTaskTransition == global::Kampai.Game.QuestTaskTransition.Start && base.questStep.state == global::Kampai.Game.QuestStepState.Notstarted)
				{
					GoToNextState();
				}
				if (questTaskTransition == global::Kampai.Game.QuestTaskTransition.Complete)
				{
					GoToTaskState(global::Kampai.Game.QuestStepState.Complete);
				}
			}
		}

		public override void SetupTracking()
		{
			global::Kampai.Game.Definition definition = definitionService.Get(base.questStepDefinition.ItemDefinitionID);
			if (definition == null)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.DS_NO_BRIDGE_DEF, "Bridge definition not found");
				return;
			}
			global::Kampai.Game.BridgeDefinition bridgeDefinition = definition as global::Kampai.Game.BridgeDefinition;
			if (bridgeDefinition != null)
			{
				global::Kampai.Game.Building building = environment.GetBuilding(bridgeDefinition.location.x, bridgeDefinition.location.y);
				if (building == null)
				{
					logger.Fatal(global::Kampai.Util.FatalCode.QS_BUILDING_MISSING, "Building not found in environment");
				}
				base.questStep.TrackedID = building.ID;
			}
		}

		public override string GetStepAction(global::Kampai.Main.ILocalizationService localService)
		{
			return localService.GetString("repairAction");
		}

		public override string GetStepDescription(global::Kampai.Main.ILocalizationService localService, global::Kampai.Game.IDefinitionService defService)
		{
			return localService.GetString("repairBridgeDesc");
		}

		public override void GetStepDescIcon(global::Kampai.Game.IDefinitionService defService, out global::UnityEngine.Sprite mainSprite, out global::UnityEngine.Sprite maskSprite)
		{
			global::Kampai.Game.ItemDefinition itemDefinition = defService.Get<global::Kampai.Game.ItemDefinition>(base.questStepDefinition.ItemDefinitionID);
			mainSprite = UIUtils.LoadSpriteFromPath(itemDefinition.Image);
			maskSprite = UIUtils.LoadSpriteFromPath(itemDefinition.Mask);
		}
	}
}
