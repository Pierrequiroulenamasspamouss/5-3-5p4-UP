namespace Kampai.Game
{
	public class MinionTaskQuestStepController : global::Kampai.Game.QuestStepController
	{
		public int RequiredMinionLevel
		{
			get
			{
				return base.questStepDefinition.UpgradeLevel;
			}
		}

		public MinionTaskQuestStepController(global::Kampai.Game.Quest quest, int stepIndex, global::Kampai.Game.IQuestScriptService questScriptService, global::Kampai.Game.IPlayerService playerService, global::strange.extensions.context.api.ICrossContextCapable gameContext, global::Kampai.Util.IKampaiLogger logger)
			: base(quest, stepIndex, questScriptService, playerService, gameContext, logger)
		{
		}

		public override void UpdateTask(global::Kampai.Game.QuestTaskTransition questTaskTransition, global::Kampai.Game.Building building, int buildingDefId, int itemDefId)
		{
			global::Kampai.Game.QuestStep questStep = base.questStep;
			if ((questStep.TrackedID != 0 && questStep.TrackedID != buildingDefId) || itemDefId < RequiredMinionLevel)
			{
				return;
			}
			if (base.questStep.state == global::Kampai.Game.QuestStepState.Notstarted)
			{
				GoToNextState();
			}
			if (questTaskTransition == global::Kampai.Game.QuestTaskTransition.Complete && base.questStep.state != global::Kampai.Game.QuestStepState.RunningStartScript)
			{
				questStep.AmountCompleted++;
				if (questStep.AmountCompleted >= base.questStepDefinition.ItemAmount)
				{
					GoToNextState(true);
				}
			}
		}

		public override string GetStepAction(global::Kampai.Main.ILocalizationService localService)
		{
			return localService.GetString("PlayerTrainingTask");
		}

		public override string GetStepDescription(global::Kampai.Main.ILocalizationService localService, global::Kampai.Game.IDefinitionService defService)
		{
			return GetDescription(localService, RequiredMinionLevel, base.questStepDefinition);
		}

		public static string GetDescription(global::Kampai.Main.ILocalizationService localService, int level, global::Kampai.Game.QuestStepDefinition questStepDefinition)
		{
			int itemAmount = questStepDefinition.ItemAmount;
			if (itemAmount > 1)
			{
				return localService.GetString("minionUpgradeTaskWithLevelMultiple", questStepDefinition.ItemAmount, level + 1);
			}
			return localService.GetString("minionUpgradeTaskWithLevel", level + 1);
		}

		public override void GetStepDescIcon(global::Kampai.Game.IDefinitionService defService, out global::UnityEngine.Sprite mainSprite, out global::UnityEngine.Sprite maskSprite)
		{
			global::Kampai.Game.BuildingDefinition buildingDefinition = defService.Get<global::Kampai.Game.BuildingDefinition>((base.questStep.TrackedID != 0) ? base.questStep.TrackedID : 3002);
			mainSprite = UIUtils.LoadSpriteFromPath(buildingDefinition.Image);
			maskSprite = UIUtils.LoadSpriteFromPath(buildingDefinition.Mask);
		}

		public override void SetupTracking()
		{
			base.questStep.TrackedID = base.questStepDefinition.ItemDefinitionID;
		}
	}
}
