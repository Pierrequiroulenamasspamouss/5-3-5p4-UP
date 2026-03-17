namespace Kampai.Game
{
	public class MinionUpgradeToLevelQuestStepController : global::Kampai.Game.QuestStepController
	{
		public int UpgradeLevel
		{
			get
			{
				return base.questStepDefinition.UpgradeLevel;
			}
		}

		public int AmountCompleted
		{
			get
			{
				int num = CurrentAmtUpgraded();
				base.questStep.AmountCompleted = ((base.questStepDefinition.ItemAmount >= num) ? num : base.questStepDefinition.ItemAmount);
				return base.questStep.AmountCompleted;
			}
		}

		public override int AmountNeeded
		{
			get
			{
				return base.questStepDefinition.ItemAmount - AmountCompleted;
			}
		}

		public MinionUpgradeToLevelQuestStepController(global::Kampai.Game.Quest quest, int stepIndex, global::Kampai.Game.IQuestScriptService questScriptService, global::Kampai.Game.IPlayerService playerService, global::strange.extensions.context.api.ICrossContextCapable gameContext, global::Kampai.Util.IKampaiLogger logger)
			: base(quest, stepIndex, questScriptService, playerService, gameContext, logger)
		{
		}

		private int CurrentAmtUpgraded()
		{
			return playerService.GetMinionCountAtOrAboveLevel(UpgradeLevel);
		}

		public override void UpdateTask(global::Kampai.Game.QuestTaskTransition questTaskTransition, global::Kampai.Game.Building building, int buildingDefId, int itemDefId)
		{
			if (AmountNeeded < 1)
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
			else if (base.questStep.state == global::Kampai.Game.QuestStepState.Notstarted)
			{
				GoToNextState();
			}
		}

		public override void SetupTracking()
		{
			base.questStep.TrackedID = 375;
		}

		public override string GetStepAction(global::Kampai.Main.ILocalizationService localService)
		{
			return localService.GetString("haveAction");
		}

		public override string GetStepDescription(global::Kampai.Main.ILocalizationService localService, global::Kampai.Game.IDefinitionService defService)
		{
			return global::Kampai.Game.MinionTaskQuestStepController.GetDescription(localService, UpgradeLevel, base.questStepDefinition);
		}

		public override void GetStepDescIcon(global::Kampai.Game.IDefinitionService defService, out global::UnityEngine.Sprite mainSprite, out global::UnityEngine.Sprite maskSprite)
		{
			mainSprite = UIUtils.LoadSpriteFromPath("icn_populationGoals_fill");
			maskSprite = UIUtils.LoadSpriteFromPath("icn_populationGoals_mask");
		}
	}
}
