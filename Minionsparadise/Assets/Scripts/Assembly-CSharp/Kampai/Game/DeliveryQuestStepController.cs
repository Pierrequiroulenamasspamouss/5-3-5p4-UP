namespace Kampai.Game
{
	public class DeliveryQuestStepController : global::Kampai.Game.QuestStepController
	{
		private global::Kampai.Game.IDefinitionService definitionService;

		public override string DeliverButtonLocKey
		{
			get
			{
				return "Deliver";
			}
		}

		public override bool NeedActiveDeliverButton
		{
			get
			{
				global::Kampai.Game.QuestStepState stepState = StepState;
				return stepState == global::Kampai.Game.QuestStepState.Inprogress || stepState == global::Kampai.Game.QuestStepState.Notstarted || stepState == global::Kampai.Game.QuestStepState.Ready;
			}
		}

		public override int AmountNeeded
		{
			get
			{
				uint quantityByDefinitionId = playerService.GetQuantityByDefinitionId(base.questStepDefinition.ItemDefinitionID);
				base.questStep.AmountCompleted = (int)quantityByDefinitionId;
				return base.questStepDefinition.ItemAmount - (int)quantityByDefinitionId;
			}
		}

		public DeliveryQuestStepController(global::Kampai.Game.Quest quest, int stepIndex, global::Kampai.Game.IQuestScriptService questScriptService, global::Kampai.Game.IPlayerService playerService, global::Kampai.Game.IDefinitionService definitionService, global::strange.extensions.context.api.ICrossContextCapable gameContext, global::Kampai.Util.IKampaiLogger logger)
			: base(quest, stepIndex, questScriptService, playerService, gameContext, logger)
		{
			this.definitionService = definitionService;
		}

		public override void UpdateTask(global::Kampai.Game.QuestTaskTransition questTaskTransition, global::Kampai.Game.Building building, int buildingDefId, int itemDefId)
		{
			base.questStep.AmountCompleted = (int)playerService.GetQuantityByDefinitionId(base.questStepDefinition.ItemDefinitionID);
			if (base.questStep.state == global::Kampai.Game.QuestStepState.Notstarted)
			{
				GoToNextState();
			}
			else if (base.questStep.AmountCompleted < base.questStepDefinition.ItemAmount)
			{
				GoToTaskState(global::Kampai.Game.QuestStepState.Inprogress);
			}
			else if (base.questStep.state != global::Kampai.Game.QuestStepState.Ready)
			{
				GoToTaskState(global::Kampai.Game.QuestStepState.Ready);
			}
		}

		public override void SetupTracking()
		{
			base.questStep.TrackedID = definitionService.GetBuildingDefintionIDFromItemDefintionID(base.questStepDefinition.ItemDefinitionID);
			if (base.questStep.TrackedID == 0)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.QS_NO_SUCH_TRACKED_STEP_ID, "Item definition id not found for Delivery Type quests");
			}
		}

		public override string GetStepAction(global::Kampai.Main.ILocalizationService localService)
		{
			return localService.GetString("deliveryAction");
		}

		public override string GetStepDescription(global::Kampai.Main.ILocalizationService localService, global::Kampai.Game.IDefinitionService defService)
		{
			global::Kampai.Game.ItemDefinition itemDefinition = defService.Get<global::Kampai.Game.ItemDefinition>(base.questStepDefinition.ItemDefinitionID);
			string text = localService.GetString(itemDefinition.LocalizedKey + "*", base.questStepDefinition.ItemAmount);
			return localService.GetString("deliverTaskDesc", text);
		}

		public override void GetStepDescIcon(global::Kampai.Game.IDefinitionService defService, out global::UnityEngine.Sprite mainSprite, out global::UnityEngine.Sprite maskSprite)
		{
			int buildingDefintionIDFromItemDefintionID = defService.GetBuildingDefintionIDFromItemDefintionID(base.questStepDefinition.ItemDefinitionID);
			global::Kampai.Game.BuildingDefinition buildingDefinition = definitionService.Get<global::Kampai.Game.BuildingDefinition>(buildingDefintionIDFromItemDefintionID);
			mainSprite = UIUtils.LoadSpriteFromPath(buildingDefinition.Image);
			maskSprite = UIUtils.LoadSpriteFromPath(buildingDefinition.Mask);
		}
	}
}
