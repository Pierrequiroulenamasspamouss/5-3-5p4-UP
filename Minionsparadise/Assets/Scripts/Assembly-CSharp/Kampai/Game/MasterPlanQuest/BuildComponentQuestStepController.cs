namespace Kampai.Game.MasterPlanQuest
{
	public class BuildComponentQuestStepController : global::Kampai.Game.QuestStepController
	{
		protected global::Kampai.Game.MasterPlanQuestType.Component questComponent;

		public override bool NeedActiveDeliverButton
		{
			get
			{
				return questComponent.component != null && questComponent.component.State == global::Kampai.Game.MasterPlanComponentState.TasksCollected && gameContext.injectionBinder.GetInstance<global::Kampai.Game.VillainLairModel>().currentActiveLair != null;
			}
		}

		public override bool NeedActiveProgressBar
		{
			get
			{
				return false;
			}
		}

		public override bool NeedGoToButton
		{
			get
			{
				return true;
			}
		}

		public BuildComponentQuestStepController(global::Kampai.Game.Quest quest, int stepIndex, global::Kampai.Game.IPlayerService playerService, global::strange.extensions.context.api.ICrossContextCapable gameContext, global::Kampai.Util.IKampaiLogger logger)
			: base(quest, stepIndex, null, playerService, gameContext, logger)
		{
			questComponent = quest as global::Kampai.Game.MasterPlanQuestType.Component;
		}

		public override string GetStepAction(global::Kampai.Main.ILocalizationService localService)
		{
			return localService.GetString("buildAction");
		}

		public override void GetStepDescIcon(global::Kampai.Game.IDefinitionService defService, out global::UnityEngine.Sprite mainSprite, out global::UnityEngine.Sprite maskSprite)
		{
			global::Kampai.Game.MasterPlanComponentBuildingDefinition masterPlanComponentBuildingDefinition = defService.Get<global::Kampai.Game.MasterPlanComponentBuildingDefinition>(questComponent.buildDefId);
			mainSprite = UIUtils.LoadSpriteFromPath(masterPlanComponentBuildingDefinition.Image, "btn_Main01_fill");
			maskSprite = UIUtils.LoadSpriteFromPath(masterPlanComponentBuildingDefinition.Mask, "btn_Main01_mask");
		}

		public override string GetStepDescription(global::Kampai.Main.ILocalizationService localService, global::Kampai.Game.IDefinitionService defService)
		{
			global::Kampai.Game.MasterPlanComponentBuildingDefinition masterPlanComponentBuildingDefinition = defService.Get<global::Kampai.Game.MasterPlanComponentBuildingDefinition>(questComponent.buildDefId);
			return localService.GetString(masterPlanComponentBuildingDefinition.LocalizedKey);
		}

		public override void UpdateTask(global::Kampai.Game.QuestTaskTransition questTaskTransition, global::Kampai.Game.Building building, int buildingDefId, int itemDefId)
		{
		}
	}
}
