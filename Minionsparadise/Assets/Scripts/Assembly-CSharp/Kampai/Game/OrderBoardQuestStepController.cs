namespace Kampai.Game
{
	public class OrderBoardQuestStepController : global::Kampai.Game.QuestStepController
	{
		public OrderBoardQuestStepController(global::Kampai.Game.Quest quest, int stepIndex, global::Kampai.Game.IQuestScriptService questScriptService, global::Kampai.Game.IPlayerService playerService, global::strange.extensions.context.api.ICrossContextCapable gameContext, global::Kampai.Util.IKampaiLogger logger)
			: base(quest, stepIndex, questScriptService, playerService, gameContext, logger)
		{
		}

		public override void UpdateTask(global::Kampai.Game.QuestTaskTransition questTaskTransition, global::Kampai.Game.Building building, int buildingDefId, int itemDefId)
		{
			global::Kampai.Game.QuestStep questStep = base.questStep;
			if (questStep.state == global::Kampai.Game.QuestStepState.Notstarted)
			{
				GoToNextState();
			}
			if (questTaskTransition == global::Kampai.Game.QuestTaskTransition.Harvestable)
			{
				if (questStep.state == global::Kampai.Game.QuestStepState.Inprogress)
				{
					questStep.AmountReady++;
					if (questStep.AmountReady + questStep.AmountCompleted >= base.questStepDefinition.ItemAmount)
					{
						GoToNextState();
					}
				}
			}
			else
			{
				if (questStep.state != global::Kampai.Game.QuestStepState.Inprogress && questStep.state != global::Kampai.Game.QuestStepState.Ready)
				{
					return;
				}
				questStep.AmountCompleted++;
				questStep.AmountReady--;
				if (questStep.AmountCompleted >= base.questStepDefinition.ItemAmount)
				{
					GoToNextState(true);
					if (base.isProceduralQuest)
					{
						gameContext.injectionBinder.GetInstance<global::Kampai.UI.View.UpdateProceduralQuestPanelSignal>().Dispatch(base.QuestInstanceID);
					}
				}
			}
		}

		public override void SetupTracking()
		{
		}

		public override string GetStepAction(global::Kampai.Main.ILocalizationService localService)
		{
			return localService.GetString("orderBoardAction");
		}

		public override string GetStepDescription(global::Kampai.Main.ILocalizationService localService, global::Kampai.Game.IDefinitionService defService)
		{
			return localService.GetString("orderboardTaskDesc", base.questStepDefinition.ItemAmount);
		}

		public override void GetStepDescIcon(global::Kampai.Game.IDefinitionService defService, out global::UnityEngine.Sprite mainSprite, out global::UnityEngine.Sprite maskSprite)
		{
			global::Kampai.Game.BuildingDefinition buildingDefinition = defService.Get<global::Kampai.Game.BuildingDefinition>(3022);
			mainSprite = UIUtils.LoadSpriteFromPath(buildingDefinition.Image);
			maskSprite = UIUtils.LoadSpriteFromPath(buildingDefinition.Mask);
		}
	}
}
