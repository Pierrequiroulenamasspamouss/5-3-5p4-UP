namespace Kampai.Game
{
	internal class PlayerSerializerV11 : global::Kampai.Game.PlayerSerializerV10
	{
		public override int Version
		{
			get
			{
				return 11;
			}
		}

		public override global::Kampai.Game.Player Deserialize(string json, global::Kampai.Game.IDefinitionService definitionService, ILocalPersistanceService localPersistanceService, global::Kampai.Game.IPartyService partyService, global::Kampai.Util.IKampaiLogger logger)
		{
			global::Kampai.Game.Player player = base.Deserialize(json, definitionService, localPersistanceService, partyService, logger);
			if (player.Version < 11)
			{
				CleanUpStuartPrestige(player);
				CleanUpMasterPlanInfo(player, definitionService);
				FixPartyFTUEPopup(player, definitionService);
				player.Version = 11;
			}
			return player;
		}

		private void FixPartyFTUEPopup(global::Kampai.Game.Player player, global::Kampai.Game.IDefinitionService definitionService)
		{
			global::Kampai.Game.Quest firstInstanceByDefinitionId = player.GetFirstInstanceByDefinitionId<global::Kampai.Game.Quest>(31050);
			int[] array = new int[2] { 101133, 101222 };
			int[] array2 = new int[7] { 101130, 101201, 101211, 101131, 101220, 101221, 101132 };
			if (firstInstanceByDefinitionId == null)
			{
				return;
			}
			bool flag = false;
			int[] array3 = array;
			foreach (int num in array3)
			{
				global::Kampai.Game.Quest firstInstanceByDefinitionId2 = player.GetFirstInstanceByDefinitionId<global::Kampai.Game.Quest>(num);
				if (firstInstanceByDefinitionId2 == null)
				{
					global::Kampai.Game.QuestDefinition def = definitionService.Get<global::Kampai.Game.QuestDefinition>(num);
					firstInstanceByDefinitionId2 = new global::Kampai.Game.Quest(def);
					firstInstanceByDefinitionId2.state = global::Kampai.Game.QuestState.Complete;
					player.AssignNextInstanceId(firstInstanceByDefinitionId2);
					player.Add(firstInstanceByDefinitionId2);
					flag = true;
				}
			}
			if (!flag)
			{
				return;
			}
			global::System.Collections.Generic.List<global::Kampai.Game.Quest> instancesByType = player.GetInstancesByType<global::Kampai.Game.Quest>();
			foreach (global::Kampai.Game.Quest item in instancesByType)
			{
				int[] array4 = array2;
				foreach (int num2 in array4)
				{
					if (item.Definition.ID == num2)
					{
						player.Remove(item);
					}
				}
			}
		}

		private void CleanUpMasterPlanInfo(global::Kampai.Game.Player player, global::Kampai.Game.IDefinitionService definitionService)
		{
			global::Kampai.Game.MasterPlan firstInstanceByDefinitionId = player.GetFirstInstanceByDefinitionId<global::Kampai.Game.MasterPlan>(65000);
			if (firstInstanceByDefinitionId == null)
			{
				return;
			}
			if (player.GetQuantityByDefinitionId(710) == 0)
			{
				player.SetQuantityByStaticItem(global::Kampai.Game.StaticItem.MASTER_PLAN_COMPLETION_COUNT, (uint)firstInstanceByDefinitionId.completionCount);
				firstInstanceByDefinitionId.completionCount = 0;
			}
			global::Kampai.Game.VillainLair firstInstanceByDefinitionId2 = player.GetFirstInstanceByDefinitionId<global::Kampai.Game.VillainLair>(3137);
			if (firstInstanceByDefinitionId2.hasVisited)
			{
				firstInstanceByDefinitionId.introHasBeenDisplayed = true;
			}
			global::Kampai.Game.VillainLairDefinition villainLairDefinition = definitionService.Get<global::Kampai.Game.VillainLairDefinition>(global::Kampai.Game.StaticItem.VILLAIN_LAIR_DEFINITION_ID);
			global::System.Collections.Generic.List<global::Kampai.Game.PlatformDefinition> platforms = villainLairDefinition.Platforms;
			global::System.Collections.Generic.List<global::Kampai.Game.MasterPlanComponent> instancesByType = player.GetInstancesByType<global::Kampai.Game.MasterPlanComponent>();
			for (int i = 0; i < instancesByType.Count; i++)
			{
				global::Kampai.Game.MasterPlanComponent masterPlanComponent = instancesByType[i];
				switch (masterPlanComponent.Definition.ID)
				{
				case 66000:
					masterPlanComponent.buildingDefID = 3140;
					masterPlanComponent.buildingLocation = platforms[0].placementLocation;
					break;
				case 66001:
					masterPlanComponent.buildingDefID = 3141;
					masterPlanComponent.buildingLocation = platforms[1].placementLocation;
					break;
				case 66002:
					masterPlanComponent.buildingDefID = 3142;
					masterPlanComponent.buildingLocation = platforms[2].placementLocation;
					break;
				case 66003:
					masterPlanComponent.buildingDefID = 3143;
					masterPlanComponent.buildingLocation = platforms[3].placementLocation;
					break;
				case 66004:
					masterPlanComponent.buildingDefID = 3144;
					masterPlanComponent.buildingLocation = platforms[4].placementLocation;
					break;
				}
				global::Kampai.Game.MasterPlanComponentBuilding firstInstanceByDefinitionId3 = player.GetFirstInstanceByDefinitionId<global::Kampai.Game.MasterPlanComponentBuilding>(masterPlanComponent.buildingDefID);
				if (firstInstanceByDefinitionId3 != null)
				{
					firstInstanceByDefinitionId3.Location = masterPlanComponent.buildingLocation;
				}
				if (masterPlanComponent.reward == null)
				{
					masterPlanComponent.reward = GenerateComponentReward();
				}
			}
			global::Kampai.Game.MasterPlanComponentBuilding firstInstanceByDefinitionId4 = player.GetFirstInstanceByDefinitionId<global::Kampai.Game.MasterPlanComponentBuilding>(firstInstanceByDefinitionId.Definition.BuildingDefID);
			if (firstInstanceByDefinitionId4 != null)
			{
				firstInstanceByDefinitionId4.Location = platforms[5].placementLocation;
			}
		}

		private void CleanUpStuartPrestige(global::Kampai.Game.Player player)
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.Prestige> byDefinitionId = player.GetByDefinitionId<global::Kampai.Game.Prestige>(40003);
			if (byDefinitionId != null && byDefinitionId.Count > 0)
			{
				global::Kampai.Game.Prestige prestige = byDefinitionId[0];
				int currentPrestigeLevel = prestige.CurrentPrestigeLevel;
				if (currentPrestigeLevel >= 3 && (prestige.state == global::Kampai.Game.PrestigeState.Questing || prestige.state == global::Kampai.Game.PrestigeState.Taskable) && currentPrestigeLevel++ < prestige.Definition.PrestigeLevelSettings.Count)
				{
					prestige.CurrentPrestigeLevel++;
				}
			}
		}

		private global::Kampai.Game.MasterPlanComponentReward GenerateComponentReward()
		{
			global::Kampai.Game.MasterPlanComponentReward masterPlanComponentReward = new global::Kampai.Game.MasterPlanComponentReward();
			masterPlanComponentReward.Definition = new global::Kampai.Game.MasterPlanComponentRewardDefinition();
			masterPlanComponentReward.Definition.grindReward = 7500u;
			masterPlanComponentReward.Definition.premiumReward = 5u;
			masterPlanComponentReward.Definition.rewardItemId = 1;
			masterPlanComponentReward.Definition.rewardQuantity = 5u;
			return masterPlanComponentReward;
		}
	}
}
