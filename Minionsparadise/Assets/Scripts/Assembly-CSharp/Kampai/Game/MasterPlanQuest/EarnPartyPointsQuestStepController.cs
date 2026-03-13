namespace Kampai.Game.MasterPlanQuest
{
	public class EarnPartyPointsQuestStepController : global::Kampai.Game.MasterPlanQuest.BuildingTaskQuestStepController
	{
		protected override string BuildingLocName
		{
			get
			{
				return "Anything";
			}
		}

		protected override string DescriptionLocKey
		{
			get
			{
				return "MasterPlanTaskEarnPartyPoints";
			}
		}

		public EarnPartyPointsQuestStepController(global::Kampai.Game.Quest quest, int stepIndex, global::Kampai.Game.IDefinitionService definitionService, global::Kampai.Game.IPlayerService playerService, global::strange.extensions.context.api.ICrossContextCapable gameContext, global::Kampai.Util.IKampaiLogger logger)
			: base(quest, stepIndex, definitionService, playerService, gameContext, logger)
		{
		}
	}
}
