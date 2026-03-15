namespace Kampai.Game.MasterPlanQuest
{
	public class CompleteOrdersQuestStepController : global::Kampai.Game.MasterPlanQuestStepController
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

		protected override string DescriptionLocKey
		{
			get
			{
				return "MasterPlanTaskCompleteOrders";
			}
		}

		public CompleteOrdersQuestStepController(global::Kampai.Game.Quest quest, int stepIndex, global::Kampai.Game.IDefinitionService definitionService, global::Kampai.Game.IPlayerService playerService, global::strange.extensions.context.api.ICrossContextCapable gameContext, global::Kampai.Util.IKampaiLogger logger)
			: base(quest, stepIndex, definitionService, playerService, gameContext, logger)
		{
		}

		public override void GetStepDescIcon(global::Kampai.Game.IDefinitionService defService, out global::UnityEngine.Sprite mainSprite, out global::UnityEngine.Sprite maskSprite)
		{
			mainSprite = UIUtils.LoadSpriteFromPath("img_orderboard_item_fill");
			maskSprite = UIUtils.LoadSpriteFromPath("img_orderboard_item_mask", "btn_Main01_mask");
		}

		protected override object[] DescriptionArgs(global::Kampai.Main.ILocalizationService localizationService)
		{
			return new object[1] { taskQuestDef.ItemAmount };
		}
	}
}
