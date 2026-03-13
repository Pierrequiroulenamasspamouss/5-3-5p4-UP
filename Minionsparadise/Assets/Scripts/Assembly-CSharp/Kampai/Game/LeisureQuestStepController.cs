namespace Kampai.Game
{
	public class LeisureQuestStepController : global::Kampai.Game.QuestStepController
	{
		public LeisureQuestStepController(global::Kampai.Game.Quest quest, int stepIndex, global::Kampai.Game.IQuestScriptService questScriptService, global::Kampai.Game.IPlayerService playerService, global::strange.extensions.context.api.ICrossContextCapable gameContext, global::Kampai.Util.IKampaiLogger logger)
			: base(quest, stepIndex, questScriptService, playerService, gameContext, logger)
		{
		}

		public override void UpdateTask(global::Kampai.Game.QuestTaskTransition questTaskTransition, global::Kampai.Game.Building building, int buildingDefId, int itemDefId)
		{
			if (questTaskTransition == global::Kampai.Game.QuestTaskTransition.Complete && base.questStep.TrackedID == buildingDefId)
			{
				base.questStep.AmountReady++;
				base.questStep.AmountCompleted++;
				if (base.questStep.AmountCompleted >= base.questStepDefinition.ItemAmount)
				{
					GoToTaskState(global::Kampai.Game.QuestStepState.Complete);
				}
			}
		}

		public override void SetupTracking()
		{
			base.questStep.TrackedID = base.questStepDefinition.ItemDefinitionID;
			if (base.questStep.TrackedID == 0)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.QS_NO_SUCH_TRACKED_LEISURE_ID, "Item definition id not found for {0} Type quests", base.questStepDefinition.Type);
			}
		}

		public override string GetStepAction(global::Kampai.Main.ILocalizationService localService)
		{
			return localService.GetString("leisureAction");
		}

		public override string GetStepDescription(global::Kampai.Main.ILocalizationService localService, global::Kampai.Game.IDefinitionService defService)
		{
			global::Kampai.Game.BuildingDefinition buildingDefinition = defService.Get<global::Kampai.Game.BuildingDefinition>(base.questStepDefinition.ItemDefinitionID);
			string text = localService.GetString(buildingDefinition.LocalizedKey + "*", base.questStepDefinition.ItemAmount);
			return localService.GetString("leisureTaskDesc", text);
		}

		public override void GetStepDescIcon(global::Kampai.Game.IDefinitionService defService, out global::UnityEngine.Sprite mainSprite, out global::UnityEngine.Sprite maskSprite)
		{
			global::Kampai.Game.BuildingDefinition buildingDefinition = defService.Get<global::Kampai.Game.BuildingDefinition>(base.questStepDefinition.ItemDefinitionID);
			mainSprite = UIUtils.LoadSpriteFromPath(buildingDefinition.Image);
			maskSprite = UIUtils.LoadSpriteFromPath(buildingDefinition.Mask);
		}
	}
}
