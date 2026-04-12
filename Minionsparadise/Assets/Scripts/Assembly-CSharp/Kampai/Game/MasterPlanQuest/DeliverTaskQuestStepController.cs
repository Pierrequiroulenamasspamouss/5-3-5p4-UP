namespace Kampai.Game.MasterPlanQuest
{
	public class DeliverTaskQuestStepController : global::Kampai.Game.MasterPlanQuest.ItemTaskQuestStepController
	{
		protected override string DescriptionLocKey
		{
			get
			{
				return "MasterPlanTaskDeliver";
			}
		}

		public DeliverTaskQuestStepController(global::Kampai.Game.Quest quest, int stepIndex, global::Kampai.Game.IDefinitionService definitionService, global::Kampai.Game.IPlayerService playerService, global::strange.extensions.context.api.ICrossContextCapable gameContext, global::Kampai.Util.IKampaiLogger logger)
			: base(quest, stepIndex, definitionService, playerService, gameContext, logger)
		{
		}
	}
}
