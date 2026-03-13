namespace Kampai.Game
{
	public class UpgradeMinionQuestStepController : global::Kampai.Game.QuestStepController
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
				if (UpgradeLevel == 0)
				{
					return base.questStep.AmountCompleted;
				}
				int num = CurrentAmtUpgraded();
				return (base.questStepDefinition.ItemAmount >= num) ? num : base.questStepDefinition.ItemAmount;
			}
		}

		public override int AmountNeeded
		{
			get
			{
				return base.questStepDefinition.ItemAmount - AmountCompleted;
			}
		}

		public UpgradeMinionQuestStepController(global::Kampai.Game.Quest quest, int stepIndex, global::Kampai.Game.IQuestScriptService questScriptService, global::Kampai.Game.IPlayerService playerService, global::strange.extensions.context.api.ICrossContextCapable gameContext, global::Kampai.Util.IKampaiLogger logger)
			: base(quest, stepIndex, questScriptService, playerService, gameContext, logger)
		{
		}

		private int CurrentAmtUpgraded()
		{
			return playerService.GetMinionCountAtOrAboveLevel(UpgradeLevel);
		}

		public override void UpdateTask(global::Kampai.Game.QuestTaskTransition questTaskTransition, global::Kampai.Game.Building building, int buildingDefId, int itemDefId)
		{
			if (questTaskTransition == global::Kampai.Game.QuestTaskTransition.Complete)
			{
				base.questStep.AmountCompleted++;
			}
			if (AmountNeeded < 1)
			{
				if (base.questStep.state == global::Kampai.Game.QuestStepState.Notstarted)
				{
					GoToTaskState(global::Kampai.Game.QuestStepState.WaitComplete);
				}
				else if (questTaskTransition == global::Kampai.Game.QuestTaskTransition.Complete && base.questStep.state != global::Kampai.Game.QuestStepState.RunningStartScript)
				{
					GoToNextState(true);
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
			return localService.GetString("LevelUpAction");
		}

		public override string GetStepDescription(global::Kampai.Main.ILocalizationService localService, global::Kampai.Game.IDefinitionService defService)
		{
			return localService.GetString("Minions*", base.questStepDefinition.ItemAmount);
		}

		public override void GetStepDescIcon(global::Kampai.Game.IDefinitionService defService, out global::UnityEngine.Sprite mainSprite, out global::UnityEngine.Sprite maskSprite)
		{
			global::Kampai.Game.ItemDefinition itemDefinition = defService.Get<global::Kampai.Game.ItemDefinition>(base.questStepDefinition.ItemDefinitionID);
			mainSprite = UIUtils.LoadSpriteFromPath(itemDefinition.Image);
			maskSprite = UIUtils.LoadSpriteFromPath(itemDefinition.Mask);
		}
	}
}
