namespace Kampai.Game
{
	public class ConstuctionQuestStepController : global::Kampai.Game.QuestStepController
	{
		public ConstuctionQuestStepController(global::Kampai.Game.Quest quest, int stepIndex, global::Kampai.Game.IQuestScriptService questScriptService, global::Kampai.Game.IPlayerService playerService, global::strange.extensions.context.api.ICrossContextCapable gameContext, global::Kampai.Util.IKampaiLogger logger)
			: base(quest, stepIndex, questScriptService, playerService, gameContext, logger)
		{
		}

		public override void UpdateTask(global::Kampai.Game.QuestTaskTransition questTaskTransition, global::Kampai.Game.Building building, int buildingDefId, int itemDefId)
		{
			int num = 0;
			int num2 = 0;
			bool flag = false;
			global::System.Collections.Generic.ICollection<global::Kampai.Game.Building> byDefinitionId = playerService.GetByDefinitionId<global::Kampai.Game.Building>(base.questStepDefinition.ItemDefinitionID);
			global::Kampai.Game.QuestStep questStep = base.questStep;
			foreach (global::Kampai.Game.Building item in byDefinitionId)
			{
				global::Kampai.Game.BuildingState state = item.State;
				global::Kampai.Game.QuestStepState state2 = questStep.state;
				int iD = item.Definition.ID;
				if (base.questStepDefinition.ItemDefinitionID == iD)
				{
					global::Kampai.Game.Building unreadyBuilding = GetUnreadyBuilding(iD);
					questStep.TrackedID = ((unreadyBuilding != null) ? unreadyBuilding.ID : 0);
					if (state2 == global::Kampai.Game.QuestStepState.Notstarted)
					{
						GoToNextState();
						flag = true;
					}
					switch (state)
					{
					case global::Kampai.Game.BuildingState.Complete:
						num++;
						break;
					case global::Kampai.Game.BuildingState.Idle:
					case global::Kampai.Game.BuildingState.Working:
					case global::Kampai.Game.BuildingState.Harvestable:
					case global::Kampai.Game.BuildingState.Cooldown:
					case global::Kampai.Game.BuildingState.HarvestableAndWorking:
						num2++;
						break;
					}
				}
			}
			questStep.AmountReady = num;
			questStep.AmountCompleted = num2;
			if (questStep.state != global::Kampai.Game.QuestStepState.Ready && questStep.AmountReady + questStep.AmountCompleted >= base.questStepDefinition.ItemAmount)
			{
				GoToTaskState(global::Kampai.Game.QuestStepState.Ready);
			}
			if (questStep.AmountCompleted >= base.questStepDefinition.ItemAmount)
			{
				if (flag)
				{
					GoToTaskState(global::Kampai.Game.QuestStepState.WaitComplete);
				}
				else
				{
					GoToNextState(true);
				}
			}
		}

		private global::Kampai.Game.Building GetUnreadyBuilding(int buildingDefId)
		{
			global::System.Collections.Generic.ICollection<global::Kampai.Game.Building> byDefinitionId = playerService.GetByDefinitionId<global::Kampai.Game.Building>(buildingDefId);
			foreach (global::Kampai.Game.Building item in byDefinitionId)
			{
				global::Kampai.Game.BuildingState state = item.State;
				if (state == global::Kampai.Game.BuildingState.Inactive || state == global::Kampai.Game.BuildingState.Construction || state == global::Kampai.Game.BuildingState.Complete)
				{
					return item;
				}
			}
			return null;
		}

		public override void SetupTracking()
		{
			int itemDefinitionID = base.questStepDefinition.ItemDefinitionID;
			global::Kampai.Game.Building unreadyBuilding = GetUnreadyBuilding(itemDefinitionID);
			base.questStep.TrackedID = ((unreadyBuilding != null) ? unreadyBuilding.ID : 0);
		}

		public override string GetStepAction(global::Kampai.Main.ILocalizationService localService)
		{
			return localService.GetString("constructAction");
		}

		public override string GetStepDescription(global::Kampai.Main.ILocalizationService localService, global::Kampai.Game.IDefinitionService defService)
		{
			global::Kampai.Game.BuildingDefinition buildingDefinition = defService.Get<global::Kampai.Game.BuildingDefinition>(base.questStepDefinition.ItemDefinitionID);
			string text = localService.GetString(buildingDefinition.LocalizedKey + "*", base.questStepDefinition.ItemAmount);
			return localService.GetString("constructTaskDesc", text);
		}

		public override void GetStepDescIcon(global::Kampai.Game.IDefinitionService defService, out global::UnityEngine.Sprite mainSprite, out global::UnityEngine.Sprite maskSprite)
		{
			global::Kampai.Game.BuildingDefinition buildingDefinition = defService.Get<global::Kampai.Game.BuildingDefinition>(base.questStepDefinition.ItemDefinitionID);
			mainSprite = UIUtils.LoadSpriteFromPath(buildingDefinition.Image);
			maskSprite = UIUtils.LoadSpriteFromPath(buildingDefinition.Mask);
		}
	}
}
