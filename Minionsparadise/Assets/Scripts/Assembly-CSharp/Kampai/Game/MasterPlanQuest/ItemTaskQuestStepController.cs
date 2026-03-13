namespace Kampai.Game.MasterPlanQuest
{
	public abstract class ItemTaskQuestStepController : global::Kampai.Game.MasterPlanQuestStepController
	{
		public override int AmountNeeded
		{
			get
			{
				return (int)task.remainingQuantity;
			}
		}

		public override bool NeedActiveDeliverButton
		{
			get
			{
				global::Kampai.Game.QuestStepState stepState = StepState;
				return stepState == global::Kampai.Game.QuestStepState.Inprogress || stepState == global::Kampai.Game.QuestStepState.Ready;
			}
		}

		protected ItemTaskQuestStepController(global::Kampai.Game.Quest quest, int stepIndex, global::Kampai.Game.IDefinitionService definitionService, global::Kampai.Game.IPlayerService playerService, global::strange.extensions.context.api.ICrossContextCapable gameContext, global::Kampai.Util.IKampaiLogger logger)
			: base(quest, stepIndex, definitionService, playerService, gameContext, logger)
		{
		}

		public override void GetStepDescIcon(global::Kampai.Game.IDefinitionService defService, out global::UnityEngine.Sprite mainSprite, out global::UnityEngine.Sprite maskSprite)
		{
			int buildingDefintionIDFromItemDefintionID = defService.GetBuildingDefintionIDFromItemDefintionID(taskDefinition.requiredItemId);
			global::Kampai.Game.BuildingDefinition buildingDefinition = definitionService.Get<global::Kampai.Game.BuildingDefinition>(buildingDefintionIDFromItemDefintionID);
			if (buildingDefinition == null)
			{
				mainSprite = UIUtils.LoadSpriteFromPath(string.Empty);
				maskSprite = UIUtils.LoadSpriteFromPath(string.Empty, "btn_Main01_mask");
			}
			else
			{
				mainSprite = UIUtils.LoadSpriteFromPath(buildingDefinition.Image);
				maskSprite = UIUtils.LoadSpriteFromPath(buildingDefinition.Mask, "btn_Main01_mask");
			}
		}

		protected override object[] DescriptionArgs(global::Kampai.Main.ILocalizationService localizationService)
		{
			return new object[1] { ItemName(localizationService) };
		}

		protected virtual string ItemName(global::Kampai.Main.ILocalizationService localizationService)
		{
			global::Kampai.Game.ItemDefinition definition;
			definitionService.TryGet<global::Kampai.Game.ItemDefinition>(taskQuestDef.ItemDefinitionID, out definition);
			return localizationService.GetString(definition.LocalizedKey + "*", task.Definition.requiredQuantity);
		}
	}
}
