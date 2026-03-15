namespace Kampai.Game
{
	public class MignetteQuestStepController : global::Kampai.Game.QuestStepController
	{
		public MignetteQuestStepController(global::Kampai.Game.Quest quest, int stepIndex, global::Kampai.Game.IQuestScriptService questScriptService, global::Kampai.Game.IPlayerService playerService, global::strange.extensions.context.api.ICrossContextCapable gameContext, global::Kampai.Util.IKampaiLogger logger)
			: base(quest, stepIndex, questScriptService, playerService, gameContext, logger)
		{
		}

		public override void UpdateTask(global::Kampai.Game.QuestTaskTransition questTaskTransition, global::Kampai.Game.Building building, int buildingDefId, int itemDefId)
		{
			global::Kampai.Game.QuestStepState state = base.questStep.state;
			if (base.questStepDefinition.ItemDefinitionID != building.Definition.ID)
			{
				return;
			}
			if (questTaskTransition == global::Kampai.Game.QuestTaskTransition.Start && state == global::Kampai.Game.QuestStepState.Notstarted)
			{
				GoToNextState();
			}
			else if (questTaskTransition == global::Kampai.Game.QuestTaskTransition.Complete && state == global::Kampai.Game.QuestStepState.Inprogress)
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
			int itemDefinitionID = base.questStepDefinition.ItemDefinitionID;
			global::Kampai.Game.MignetteBuilding firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MignetteBuilding>(itemDefinitionID);
			if (firstInstanceByDefinitionId == null)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.PS_MISSING_MIGNETTE, "Mignette instance not found!");
			}
			else
			{
				base.questStep.TrackedID = firstInstanceByDefinitionId.ID;
			}
		}

		public override string GetStepAction(global::Kampai.Main.ILocalizationService localService)
		{
			return localService.GetString("mignetteAction");
		}

		public override string GetStepDescription(global::Kampai.Main.ILocalizationService localService, global::Kampai.Game.IDefinitionService defService)
		{
			global::Kampai.Game.BuildingDefinition buildingDefinition = defService.Get<global::Kampai.Game.BuildingDefinition>(base.questStepDefinition.ItemDefinitionID);
			string text = localService.GetString(buildingDefinition.LocalizedKey);
			return localService.GetString("mignetteTaskDescWrap", text, localService.GetString("mignetteTaskDesc*", base.questStepDefinition.ItemAmount));
		}

		public override void GetStepDescIcon(global::Kampai.Game.IDefinitionService defService, out global::UnityEngine.Sprite mainSprite, out global::UnityEngine.Sprite maskSprite)
		{
			global::Kampai.Game.BuildingDefinition buildingDefinition = defService.Get<global::Kampai.Game.BuildingDefinition>(base.questStepDefinition.ItemDefinitionID);
			mainSprite = UIUtils.LoadSpriteFromPath(buildingDefinition.Image);
			maskSprite = UIUtils.LoadSpriteFromPath(buildingDefinition.Mask);
		}
	}
}
