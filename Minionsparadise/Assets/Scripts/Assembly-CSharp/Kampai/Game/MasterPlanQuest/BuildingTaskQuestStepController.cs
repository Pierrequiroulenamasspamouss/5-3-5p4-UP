namespace Kampai.Game.MasterPlanQuest
{
	public abstract class BuildingTaskQuestStepController : global::Kampai.Game.MasterPlanQuest.ItemTaskQuestStepController
	{
		public override bool NeedActiveDeliverButton
		{
			get
			{
				return StepState == global::Kampai.Game.QuestStepState.Ready || StepState == global::Kampai.Game.QuestStepState.WaitComplete;
			}
		}

		public override bool NeedGoToButton
		{
			get
			{
				return !NeedActiveDeliverButton;
			}
		}

		protected abstract string BuildingLocName { get; }

		protected BuildingTaskQuestStepController(global::Kampai.Game.Quest quest, int stepIndex, global::Kampai.Game.IDefinitionService definitionService, global::Kampai.Game.IPlayerService playerService, global::strange.extensions.context.api.ICrossContextCapable gameContext, global::Kampai.Util.IKampaiLogger logger)
			: base(quest, stepIndex, definitionService, playerService, gameContext, logger)
		{
		}

		public override void GetStepDescIcon(global::Kampai.Game.IDefinitionService defService, out global::UnityEngine.Sprite mainSprite, out global::UnityEngine.Sprite maskSprite)
		{
			global::Kampai.Game.BuildingDefinition definition;
			bool flag = definitionService.TryGet<global::Kampai.Game.BuildingDefinition>(taskDefinition.requiredItemId, out definition);
			bool flag2 = taskDefinition.Type == global::Kampai.Game.MasterPlanComponentTaskType.EarnSandDollars;
			global::Kampai.Game.DisplayableDefinition displayableDefinition = ((!flag) ? definitionService.Get<global::Kampai.Game.DisplayableDefinition>((!flag2) ? 2 : 0) : definition);
			if (displayableDefinition == null)
			{
				mainSprite = UIUtils.LoadSpriteFromPath(string.Empty);
				maskSprite = UIUtils.LoadSpriteFromPath(string.Empty, "btn_Main01_mask");
			}
			else
			{
				mainSprite = UIUtils.LoadSpriteFromPath(displayableDefinition.Image);
				maskSprite = UIUtils.LoadSpriteFromPath(displayableDefinition.Mask, "btn_Main01_mask");
			}
		}

		protected override object[] DescriptionArgs(global::Kampai.Main.ILocalizationService localizationService)
		{
			return new object[2]
			{
				taskQuestDef.ItemAmount,
				ItemName(localizationService)
			};
		}

		protected override string ItemName(global::Kampai.Main.ILocalizationService localizationService)
		{
			global::Kampai.Game.BuildingDefinition definition;
			bool flag = definitionService.TryGet<global::Kampai.Game.BuildingDefinition>(taskQuestDef.ItemDefinitionID, out definition);
			return localizationService.GetString((!flag) ? BuildingLocName : definition.LocalizedKey);
		}
	}
}
