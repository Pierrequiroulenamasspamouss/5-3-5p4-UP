namespace Kampai.Game.MasterPlanQuest
{
	public class MiniGameScoreQuestStepController : global::Kampai.Game.MasterPlanQuest.BuildingTaskQuestStepController
	{
		protected override string BuildingLocName
		{
			get
			{
				return "MiniGames";
			}
		}

		protected override string DescriptionLocKey
		{
			get
			{
				return "MasterPlanTaskMiniGameScore";
			}
		}

		public MiniGameScoreQuestStepController(global::Kampai.Game.Quest quest, int stepIndex, global::Kampai.Game.IDefinitionService definitionService, global::Kampai.Game.IPlayerService playerService, global::strange.extensions.context.api.ICrossContextCapable gameContext, global::Kampai.Util.IKampaiLogger logger)
			: base(quest, stepIndex, definitionService, playerService, gameContext, logger)
		{
		}

		public override void SetupTracking()
		{
			int itemDefinitionID = base.questStepDefinition.ItemDefinitionID;
			if (itemDefinitionID != 0)
			{
				global::Kampai.Game.MignetteBuilding firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MignetteBuilding>(itemDefinitionID);
				if (firstInstanceByDefinitionId == null)
				{
					logger.Fatal(global::Kampai.Util.FatalCode.PS_MISSING_MIGNETTE, "Mignette instance not found!");
				}
				else
				{
					base.questStep.TrackedID = firstInstanceByDefinitionId.ID;
				}
			}
		}
	}
}
