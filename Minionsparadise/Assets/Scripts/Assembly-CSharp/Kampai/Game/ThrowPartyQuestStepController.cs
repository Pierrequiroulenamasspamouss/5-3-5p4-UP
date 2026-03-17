namespace Kampai.Game
{
	public class ThrowPartyQuestStepController : global::Kampai.Game.QuestStepController
	{
		public override bool NeedActiveProgressBar
		{
			get
			{
				if (playerService.GetHighestFtueCompleted() < 999999)
				{
					return false;
				}
				return true;
			}
		}

		public ThrowPartyQuestStepController(global::Kampai.Game.Quest quest, int stepIndex, global::Kampai.Game.IQuestScriptService questScriptService, global::Kampai.Game.IPlayerService playerService, global::strange.extensions.context.api.ICrossContextCapable gameContext, global::Kampai.Util.IKampaiLogger logger)
			: base(quest, stepIndex, questScriptService, playerService, gameContext, logger)
		{
		}

		public override void UpdateTask(global::Kampai.Game.QuestTaskTransition questTaskTransition, global::Kampai.Game.Building building, int buildingDefId, int itemDefId)
		{
			if (base.questStep.state == global::Kampai.Game.QuestStepState.Notstarted)
			{
				GoToNextState();
			}
			if ((playerService.GetMinionPartyInstance().IsPartyHappening || questTaskTransition == global::Kampai.Game.QuestTaskTransition.Complete) && base.questStep.state == global::Kampai.Game.QuestStepState.Inprogress)
			{
				base.questStep.AmountCompleted++;
				if (base.questStep.AmountCompleted >= base.questStepDefinition.ItemAmount)
				{
					GoToTaskState(global::Kampai.Game.QuestStepState.Complete);
				}
			}
		}

		public override void SetupTracking()
		{
		}

		public override string GetStepAction(global::Kampai.Main.ILocalizationService localService)
		{
			return localService.GetString("throwAction");
		}

		public override string GetStepDescription(global::Kampai.Main.ILocalizationService localService, global::Kampai.Game.IDefinitionService defService)
		{
			return localService.GetString("throwParty");
		}

		public override void GetStepDescIcon(global::Kampai.Game.IDefinitionService defService, out global::UnityEngine.Sprite mainSprite, out global::UnityEngine.Sprite maskSprite)
		{
			mainSprite = UIUtils.LoadSpriteFromPath("img_throwparty_fill");
			maskSprite = UIUtils.LoadSpriteFromPath("img_throwparty_mask");
		}
	}
}
