namespace Kampai.Game.MasterPlanQuest
{
	public class EarnLeisurePartyPointsQuestStepController : global::Kampai.Game.MasterPlanQuest.BuildingTaskQuestStepController
	{
		protected override string BuildingLocName
		{
			get
			{
				return "Distractivities";
			}
		}

		protected override string DescriptionLocKey
		{
			get
			{
				return "MasterPlanTaskEarnLeisurePartyPoints";
			}
		}

		public EarnLeisurePartyPointsQuestStepController(global::Kampai.Game.Quest quest, int stepIndex, global::Kampai.Game.IDefinitionService definitionService, global::Kampai.Game.IPlayerService playerService, global::strange.extensions.context.api.ICrossContextCapable gameContext, global::Kampai.Util.IKampaiLogger logger)
			: base(quest, stepIndex, definitionService, playerService, gameContext, logger)
		{
		}
	}
}
