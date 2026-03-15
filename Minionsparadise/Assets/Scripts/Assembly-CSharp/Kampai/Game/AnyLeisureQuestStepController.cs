namespace Kampai.Game
{
	public class AnyLeisureQuestStepController : global::Kampai.Game.QuestStepController
	{
		public AnyLeisureQuestStepController(global::Kampai.Game.Quest quest, int stepIndex, global::Kampai.Game.IQuestScriptService questScriptService, global::Kampai.Game.IPlayerService playerService, global::strange.extensions.context.api.ICrossContextCapable gameContext, global::Kampai.Util.IKampaiLogger logger)
			: base(quest, stepIndex, questScriptService, playerService, gameContext, logger)
		{
		}

		public override void UpdateTask(global::Kampai.Game.QuestTaskTransition questTaskTransition, global::Kampai.Game.Building building, int buildingDefId, int itemDefId)
		{
			if (base.questStep.state == global::Kampai.Game.QuestStepState.Notstarted)
			{
				GoToNextState();
			}
			if (base.questStep.state == global::Kampai.Game.QuestStepState.Inprogress)
			{
				base.questStep.AmountCompleted++;
				if (base.questStep.AmountCompleted >= base.questStepDefinition.ItemAmount)
				{
					GoToNextState(true);
				}
			}
		}

		public override void SetupTracking()
		{
		}

		public override string GetStepAction(global::Kampai.Main.ILocalizationService localService)
		{
			if (StepType == global::Kampai.Game.QuestStepType.PlayAnyLeisure)
			{
				return localService.GetString("leisureAction");
			}
			return localService.GetString("harvestAction");
		}

		public override string GetStepDescription(global::Kampai.Main.ILocalizationService localService, global::Kampai.Game.IDefinitionService defService)
		{
			return localService.GetString("anyLeisureBuildingDesc");
		}

		public override void GetStepDescIcon(global::Kampai.Game.IDefinitionService defService, out global::UnityEngine.Sprite mainSprite, out global::UnityEngine.Sprite maskSprite)
		{
			mainSprite = UIUtils.LoadSpriteFromPath("img_fill_32_Yellow");
			maskSprite = UIUtils.LoadSpriteFromPath(string.Format("icn_build_mask_cat_{0}", "leisure"));
		}
	}
}
