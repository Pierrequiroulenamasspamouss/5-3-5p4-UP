namespace Kampai.Game
{
	public class MysteryBoxOnboardingQuestStepController : global::Kampai.Game.QuestStepController
	{
		private int benefitBuildingId;

		private global::Kampai.Game.IDefinitionService definitionService;

		public MysteryBoxOnboardingQuestStepController(global::Kampai.Game.Quest quest, int stepIndex, global::Kampai.Game.IQuestScriptService questScriptService, global::Kampai.Game.IPlayerService playerService, global::strange.extensions.context.api.ICrossContextCapable gameContext, global::Kampai.Util.IKampaiLogger logger)
			: base(quest, stepIndex, questScriptService, playerService, gameContext, logger)
		{
			definitionService = gameContext.injectionBinder.GetInstance<global::Kampai.Game.IDefinitionService>();
		}

		public override void UpdateTask(global::Kampai.Game.QuestTaskTransition questTaskTransition, global::Kampai.Game.Building building, int buildingDefId, int itemDefId)
		{
			if (base.questStep.state == global::Kampai.Game.QuestStepState.Notstarted)
			{
				GoToNextState();
			}
			global::Kampai.Game.QuestStepDefinition questStepDefinition = base.questStepDefinition;
			global::Kampai.Game.QuestStep questStep = base.questStep;
			global::Kampai.Game.ResourceBuilding resourceBuilding = building as global::Kampai.Game.ResourceBuilding;
			if (resourceBuilding == null || buildingDefId != benefitBuildingId)
			{
				return;
			}
			if (questTaskTransition == global::Kampai.Game.QuestTaskTransition.Complete)
			{
				questStep.AmountCompleted++;
			}
			if (questStep.AmountCompleted >= questStepDefinition.ItemAmount)
			{
				if (base.questStep.state == global::Kampai.Game.QuestStepState.Notstarted)
				{
					GoToTaskState(global::Kampai.Game.QuestStepState.WaitComplete);
				}
				else
				{
					GoToTaskState(global::Kampai.Game.QuestStepState.Complete);
				}
			}
		}

		public override void SetupTracking()
		{
			if (benefitBuildingId == 0)
			{
				benefitBuildingId = definitionService.Get<global::Kampai.Game.MinionBenefitLevelBandDefintion>(global::Kampai.Game.StaticItem.MINION_BENEFITS_DEF_ID).FirstBuildingId;
			}
		}

		public override string GetStepAction(global::Kampai.Main.ILocalizationService localService)
		{
			return localService.GetString("mysteryBoxAction");
		}

		public override string GetStepDescription(global::Kampai.Main.ILocalizationService localService, global::Kampai.Game.IDefinitionService defService)
		{
			return localService.GetString("mysteryBoxDesc");
		}

		public override void GetStepDescIcon(global::Kampai.Game.IDefinitionService defService, out global::UnityEngine.Sprite mainSprite, out global::UnityEngine.Sprite maskSprite)
		{
			global::Kampai.Game.ItemDefinition itemDefinition = definitionService.Get<global::Kampai.Game.ItemDefinition>(global::Kampai.Game.StaticItem.RANDOM_RANDOMDROP_ITEM);
			mainSprite = UIUtils.LoadSpriteFromPath(itemDefinition.Image);
			maskSprite = UIUtils.LoadSpriteFromPath(itemDefinition.Mask);
		}
	}
}
