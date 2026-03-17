namespace Kampai.Util
{
	public static class BinarySerializationUtil
	{
		public static class BinarySerializationFactory
		{
			public enum TypeCode
			{
				Kampai_Game_Definition = 1000,
				Kampai_Game_DefinitionGroup = 1001,
				Kampai_Game_DropLevelBandDefinition = 1002,
				Kampai_Game_HelpTipDefinition = 1003,
				Kampai_Game_LegalDocumentDefinition = 1004,
				Kampai_Game_LevelFunTable = 1005,
				Kampai_Game_PartyUpDefinition = 1006,
				Kampai_Game_Transaction_TransactionDefinition = 1007,
				Kampai_Util_QuantityItem = 1008,
				Kampai_Game_LevelUpDefinition = 1009,
				Kampai_Game_LevelXPTable = 1010,
				Kampai_Game_NotificationDefinition = 1011,
				Kampai_Game_NotificationSystemDefinition = 1012,
				Kampai_Game_PrestigeDefinition = 1013,
				Kampai_Game_TaxonomyDefinition = 1014,
				Kampai_Game_DisplayableDefinition = 1015,
				Kampai_Game_AchievementDefinition = 1016,
				Kampai_Game_PendingRewardDefinition = 1017,
				Kampai_Game_RewardedAdvertisementDefinition = 1018,
				Kampai_Game_AdPlacementDefinition = 1019,
				Kampai_Game_Trigger_TriggerConditionDefinition = 1020,
				Kampai_Game_OnTheGlassDailyRewardDefinition = 1021,
				Kampai_Game_Transaction_WeightedDefinition = 1022,
				Kampai_Game_Transaction_WeightedQuantityItem = 1023,
				Kampai_Game_CraftingRushRewardDefinition = 1024,
				Kampai_Game_MarketplaceRefreshRushRewardDefinition = 1025,
				Kampai_Game_MissingResourcesRewardDefinition = 1026,
				Kampai_Game_OrderboardFillOrderRewardDefinition = 1027,
				Kampai_Game_Quest2xRewardDefinition = 1028,
				Kampai_Game_OfferwallPlacementDefinition = 1029,
				Kampai_Game_AnimatingBuildingDefinition = 1030,
				Kampai_Game_BuildingAnimationDefinition = 1031,
				Kampai_Game_AnimationDefinition = 1032,
				Kampai_Game_RepairableBuildingDefinition = 1033,
				Kampai_Game_BuildingDefinition = 1034,
				Kampai_Game_BlackMarketBoardDefinition = 1035,
				Kampai_Game_BlackMarketBoardUnlockedOrderSlotDefinition = 1036,
				Kampai_Game_BlackMarketBoardSlotDefinition = 1037,
				Kampai_Game_BlackMarketBoardMultiplierDefinition = 1038,
				Kampai_Game_BridgeBuildingDefinition = 1039,
				Kampai_Game_CabanaBuildingDefinition = 1040,
				Kampai_Game_CompositeBuildingDefinition = 1041,
				Kampai_Game_CompositeBuildingPieceDefinition = 1042,
				Kampai_Game_ConnectableBuildingDefinition = 1043,
				Kampai_Game_DecorationBuildingDefinition = 1044,
				Kampai_Game_CraftingBuildingDefinition = 1045,
				Kampai_Game_RecipeDefinition = 1046,
				Kampai_Game_DCNBuildingDefinition = 1047,
				Kampai_Game_DebrisBuildingDefinition = 1048,
				Kampai_Game_TaskableBuildingDefinition = 1049,
				Kampai_Game_FountainBuildingDefinition = 1050,
				Kampai_Game_LandExpansionBuildingDefinition = 1051,
				Kampai_Game_LeisureBuildingDefintiion = 1052,
				Kampai_Game_MIBBuildingDefinition = 1053,
				Kampai_Game_MasterPlanComponentBuildingDefinition = 1054,
				Kampai_Game_MasterPlanLeftOverBuildingDefinition = 1055,
				Kampai_Game_MignetteBuildingDefinition = 1056,
				Kampai_Game_MinionPartyBuildingDefinition = 1057,
				Kampai_Game_MinionUpgradeBuildingDefinition = 1058,
				Kampai_Game_PurchasedLandExpansionDefinition = 1059,
				Kampai_Game_ResourceBuildingDefinition = 1060,
				Kampai_Game_SpecialBuildingDefinition = 1061,
				Kampai_Game_StageBuildingDefinition = 1062,
				Kampai_Game_StorageBuildingDefinition = 1063,
				Kampai_Game_TaskableMinionPartyBuildingDefinition = 1064,
				Kampai_Game_TikiBarBuildingDefinition = 1065,
				Kampai_Game_VillainLairDefinition = 1066,
				Kampai_Game_VillainLairEntranceBuildingDefinition = 1067,
				Kampai_Game_VillainLairResourcePlotDefinition = 1068,
				Kampai_Game_WelcomeHutBuildingDefinition = 1069,
				Kampai_Game_CameraDefinition = 1070,
				Kampai_Game_CustomCameraPositionDefinition = 1071,
				Kampai_Game_BigThreeCharacterDefinition = 1072,
				Kampai_Game_NamedCharacterDefinition = 1073,
				Kampai_Game_NamedCharacterAnimationDefinition = 1074,
				Kampai_Game_BobCharacterDefinition = 1075,
				Kampai_Game_MinionAnimationDefinition = 1076,
				Kampai_Game_FrolicCharacterDefinition = 1077,
				Kampai_Game_LocationIncidentalAnimationDefinition = 1078,
				Kampai_Game_KevinCharacterDefinition = 1079,
				Kampai_Game_PhilCharacterDefinition = 1080,
				Kampai_Game_SpecialEventCharacterDefinition = 1081,
				Kampai_Game_StuartCharacterDefinition = 1082,
				Kampai_Game_TSMCharacterDefinition = 1083,
				Kampai_Game_UIAnimationDefinition = 1084,
				Kampai_Game_RewardCollectionDefinition = 1085,
				Kampai_Game_EnvironmentDefinition = 1086,
				Kampai_Game_FlyOverDefinition = 1087,
				Kampai_Game_BridgeDefinition = 1088,
				Kampai_Game_ItemDefinition = 1089,
				Kampai_Game_CostumeItemDefinition = 1090,
				Kampai_Game_DropItemDefinition = 1091,
				Kampai_Game_DynamicIngredientsDefinition = 1092,
				Kampai_Game_IngredientsItemDefinition = 1093,
				Kampai_Game_PartyFavorAnimationItemDefinition = 1094,
				Kampai_Game_SpecialEventItemDefinition = 1095,
				Kampai_Game_AspirationalBuildingDefinition = 1096,
				Kampai_Game_CommonLandExpansionDefinition = 1097,
				Kampai_Game_DebrisDefinition = 1098,
				Kampai_Game_LandExpansionConfig = 1099,
				Kampai_Game_LandExpansionDefinition = 1100,
				Kampai_Game_LocalizedTextDefinition = 1101,
				Kampai_Game_MarketplaceDefinition = 1102,
				Kampai_Game_MarketplaceItemDefinition = 1103,
				Kampai_Game_MarketplaceSaleSlotDefinition = 1104,
				Kampai_Game_MarketplaceRefreshTimerDefinition = 1105,
				Kampai_Game_DynamicMasterPlanDefinition = 1106,
				Kampai_Game_MasterPlanComponentDefinition = 1107,
				Kampai_Game_MasterPlanDefinition = 1108,
				Kampai_Game_MasterPlanOnboardDefinition = 1109,
				Kampai_Game_GachaAnimationDefinition = 1110,
				Kampai_Game_GachaWeightedDefinition = 1111,
				Kampai_Game_MinionDefinition = 1112,
				Kampai_Game_PartyFavorAnimationDefinition = 1113,
				Kampai_Game_BuffDefinition = 1114,
				Kampai_Game_GuestOfHonorDefinition = 1115,
				Kampai_Game_MinionPartyDefinition = 1116,
				Kampai_Game_PartyMeterDefinition = 1117,
				Kampai_Game_PartyMeterTierDefinition = 1118,
				Kampai_Game_MinionPartyLevelBandDefinition = 1119,
				Kampai_Game_PartyPointsPerLevelDefinition = 1120,
				Kampai_Game_MinionBenefitLevelBandDefintion = 1121,
				Kampai_Game_PopulationBenefitDefinition = 1122,
				Kampai_Game_PlayerTrainingCardDefinition = 1123,
				Kampai_Game_PlayerTrainingCategoryDefinition = 1124,
				Kampai_Game_PlayerTrainingDefinition = 1125,
				Kampai_Game_FootprintDefinition = 1126,
				Kampai_Game_NoOpPlotDefinition = 1127,
				Kampai_Game_PlotDefinition = 1128,
				Kampai_Game_DynamicQuestDefinition = 1129,
				Kampai_Game_QuestDefinition = 1130,
				Kampai_Game_LimitedQuestDefinition = 1131,
				Kampai_Game_QuestChainDefinition = 1132,
				Kampai_Game_QuestResourceDefinition = 1133,
				Kampai_Game_TimedQuestDefinition = 1134,
				Kampai_Game_RushTimeBandDefinition = 1135,
				Kampai_Game_SalePackDefinition = 1136,
				Kampai_Game_PackDefinition = 1137,
				Kampai_Game_PremiumCurrencyItemDefinition = 1138,
				Kampai_Game_CurrencyItemDefinition = 1139,
				Kampai_Game_SocialSettingsDefinition = 1140,
				Kampai_Game_StickerDefinition = 1141,
				Kampai_Game_CurrencyStoreCategoryDefinition = 1142,
				Kampai_Game_CurrencyStoreDefinition = 1143,
				Kampai_Game_CurrencyStorePackDefinition = 1144,
				Kampai_Game_StoreItemDefinition = 1145,
				Kampai_Game_TaskLevelBandDefinition = 1146,
				Kampai_Game_TimedSocialEventDefinition = 1147,
				Kampai_Game_UnlockDefinition = 1148,
				Kampai_Game_Trigger_TSMTriggerDefinition = 1149,
				Kampai_Game_Trigger_TriggerDefinition = 1150,
				Kampai_Game_Trigger_UpsellTriggerDefinition = 1151,
				Kampai_Game_Trigger_AvailableLandTriggerConditionDefinition = 1152,
				Kampai_Game_Trigger_ChurnTriggerConditionDefinition = 1153,
				Kampai_Game_Trigger_ConsecutiveDaysConditionDefinition = 1154,
				Kampai_Game_Trigger_CountryTriggerConditionDefinition = 1155,
				Kampai_Game_Trigger_DaysSinceInstallTriggerConditionDefinition = 1156,
				Kampai_Game_Trigger_HelpButtonTriggerConditionDefinition = 1157,
				Kampai_Game_Trigger_HoursPlayedTriggerConditionDefinition = 1158,
				Kampai_Game_Trigger_LandExpansionTriggerConditionDefinition = 1159,
				Kampai_Game_Trigger_MignetteScoreTriggerConditionDefinition = 1160,
				Kampai_Game_Trigger_OrderBoardTriggerConditionDefinition = 1161,
				Kampai_Game_Trigger_PlatformTriggerConditionDefinition = 1162,
				Kampai_Game_Trigger_PrestigeLevelTriggerConditionDefinition = 1163,
				Kampai_Game_Trigger_PrestigeTriggerConditionDefinitionBase = 1164,
				Kampai_Game_Trigger_PrestigeStateTriggerConditionDefinition = 1165,
				Kampai_Game_Trigger_PrestigeTriggerConditionDefinition = 1166,
				Kampai_Game_Trigger_PurchaseTriggerConditionDefinition = 1167,
				Kampai_Game_Trigger_QuantityItemTriggerConditionDefinition = 1168,
				Kampai_Game_Trigger_QuestTriggerConditionDefinition = 1169,
				Kampai_Game_Trigger_SaleItemTriggerConditionDefinition = 1170,
				Kampai_Game_Trigger_SaleSlotTriggerConditionDefinition = 1171,
				Kampai_Game_Trigger_SegmentTriggerConditionDefinition = 1172,
				Kampai_Game_Trigger_SessionCountTriggerConditionDefinition = 1173,
				Kampai_Game_Trigger_SocialOrderTriggerConditionDefinition = 1174,
				Kampai_Game_Trigger_SocialTimeTriggerConditionDefinition = 1175,
				Kampai_Game_Trigger_StorageTriggerConditionDefinition = 1176,
				Kampai_Game_Trigger_CaptainTeaseTriggerRewardDefinition = 1177,
				Kampai_Game_Trigger_TriggerRewardDefinition = 1178,
				Kampai_Game_Trigger_MignetteScoreTriggerRewardDefinition = 1179,
				Kampai_Game_Trigger_PartyPointsTriggerRewardDefinition = 1180,
				Kampai_Game_Trigger_QuantityItemTriggerRewardDefinition = 1181,
				Kampai_Game_Trigger_SaleItemTriggerRewardDefinition = 1182,
				Kampai_Game_Trigger_SaleSlotTriggerRewardDefinition = 1183,
				Kampai_Game_Trigger_SocialOrderTriggerRewardDefinition = 1184,
				Kampai_Game_Trigger_TSMMessageTriggerRewardDefinition = 1185,
				Kampai_Game_Trigger_UpsellTriggerRewardDefinition = 1186,
				Kampai_Game_VillainDefinition = 1187,
				Kampai_Game_WayFinderDefinition = 1188,
				Kampai_Game_PetsXPromoDefinition = 1189,
				Kampai_Game_Definitions = 1190,
				Kampai_Splash_LoadinTipBucketDefinition = 1191,
				Kampai_Splash_LoadInTipDefinition = 1192,
				Kampai_Main_HindsightCampaignDefinition = 1193
			}

			public static global::Kampai.Util.IBinarySerializable CreateInstance(int typeCode)
			{
				switch (typeCode)
				{
				case 1001:
					return new global::Kampai.Game.DefinitionGroup();
				case 1002:
					return new global::Kampai.Game.DropLevelBandDefinition();
				case 1003:
					return new global::Kampai.Game.HelpTipDefinition();
				case 1004:
					return new global::Kampai.Game.LegalDocumentDefinition();
				case 1005:
					return new global::Kampai.Game.LevelFunTable();
				case 1006:
					return new global::Kampai.Game.PartyUpDefinition();
				case 1007:
					return new global::Kampai.Game.Transaction.TransactionDefinition();
				case 1008:
					return new global::Kampai.Util.QuantityItem();
				case 1009:
					return new global::Kampai.Game.LevelUpDefinition();
				case 1010:
					return new global::Kampai.Game.LevelXPTable();
				case 1011:
					return new global::Kampai.Game.NotificationDefinition();
				case 1012:
					return new global::Kampai.Game.NotificationSystemDefinition();
				case 1013:
					return new global::Kampai.Game.PrestigeDefinition();
				case 1014:
					return new global::Kampai.Game.TaxonomyDefinition();
				case 1015:
					return new global::Kampai.Game.DisplayableDefinition();
				case 1016:
					return new global::Kampai.Game.AchievementDefinition();
				case 1017:
					return new global::Kampai.Game.PendingRewardDefinition();
				case 1018:
					return new global::Kampai.Game.RewardedAdvertisementDefinition();
				case 1019:
					return new global::Kampai.Game.AdPlacementDefinition();
				case 1021:
					return new global::Kampai.Game.OnTheGlassDailyRewardDefinition();
				case 1022:
					return new global::Kampai.Game.Transaction.WeightedDefinition();
				case 1023:
					return new global::Kampai.Game.Transaction.WeightedQuantityItem();
				case 1024:
					return new global::Kampai.Game.CraftingRushRewardDefinition();
				case 1025:
					return new global::Kampai.Game.MarketplaceRefreshRushRewardDefinition();
				case 1026:
					return new global::Kampai.Game.MissingResourcesRewardDefinition();
				case 1027:
					return new global::Kampai.Game.OrderboardFillOrderRewardDefinition();
				case 1028:
					return new global::Kampai.Game.Quest2xRewardDefinition();
				case 1029:
					return new global::Kampai.Game.OfferwallPlacementDefinition();
				case 1031:
					return new global::Kampai.Game.BuildingAnimationDefinition();
				case 1032:
					return new global::Kampai.Game.AnimationDefinition();
				case 1035:
					return new global::Kampai.Game.BlackMarketBoardDefinition();
				case 1036:
					return new global::Kampai.Game.BlackMarketBoardUnlockedOrderSlotDefinition();
				case 1037:
					return new global::Kampai.Game.BlackMarketBoardSlotDefinition();
				case 1038:
					return new global::Kampai.Game.BlackMarketBoardMultiplierDefinition();
				case 1039:
					return new global::Kampai.Game.BridgeBuildingDefinition();
				case 1040:
					return new global::Kampai.Game.CabanaBuildingDefinition();
				case 1041:
					return new global::Kampai.Game.CompositeBuildingDefinition();
				case 1042:
					return new global::Kampai.Game.CompositeBuildingPieceDefinition();
				case 1043:
					return new global::Kampai.Game.ConnectableBuildingDefinition();
				case 1044:
					return new global::Kampai.Game.DecorationBuildingDefinition();
				case 1045:
					return new global::Kampai.Game.CraftingBuildingDefinition();
				case 1046:
					return new global::Kampai.Game.RecipeDefinition();
				case 1047:
					return new global::Kampai.Game.DCNBuildingDefinition();
				case 1048:
					return new global::Kampai.Game.DebrisBuildingDefinition();
				case 1050:
					return new global::Kampai.Game.FountainBuildingDefinition();
				case 1051:
					return new global::Kampai.Game.LandExpansionBuildingDefinition();
				case 1052:
					return new global::Kampai.Game.LeisureBuildingDefintiion();
				case 1053:
					return new global::Kampai.Game.MIBBuildingDefinition();
				case 1054:
					return new global::Kampai.Game.MasterPlanComponentBuildingDefinition();
				case 1055:
					return new global::Kampai.Game.MasterPlanLeftOverBuildingDefinition();
				case 1056:
					return new global::Kampai.Game.MignetteBuildingDefinition();
				case 1058:
					return new global::Kampai.Game.MinionUpgradeBuildingDefinition();
				case 1059:
					return new global::Kampai.Game.PurchasedLandExpansionDefinition();
				case 1060:
					return new global::Kampai.Game.ResourceBuildingDefinition();
				case 1061:
					return new global::Kampai.Game.SpecialBuildingDefinition();
				case 1062:
					return new global::Kampai.Game.StageBuildingDefinition();
				case 1063:
					return new global::Kampai.Game.StorageBuildingDefinition();
				case 1065:
					return new global::Kampai.Game.TikiBarBuildingDefinition();
				case 1066:
					return new global::Kampai.Game.VillainLairDefinition();
				case 1067:
					return new global::Kampai.Game.VillainLairEntranceBuildingDefinition();
				case 1068:
					return new global::Kampai.Game.VillainLairResourcePlotDefinition();
				case 1069:
					return new global::Kampai.Game.WelcomeHutBuildingDefinition();
				case 1070:
					return new global::Kampai.Game.CameraDefinition();
				case 1071:
					return new global::Kampai.Game.CustomCameraPositionDefinition();
				case 1072:
					return new global::Kampai.Game.BigThreeCharacterDefinition();
				case 1074:
					return new global::Kampai.Game.NamedCharacterAnimationDefinition();
				case 1075:
					return new global::Kampai.Game.BobCharacterDefinition();
				case 1076:
					return new global::Kampai.Game.MinionAnimationDefinition();
				case 1078:
					return new global::Kampai.Game.LocationIncidentalAnimationDefinition();
				case 1079:
					return new global::Kampai.Game.KevinCharacterDefinition();
				case 1080:
					return new global::Kampai.Game.PhilCharacterDefinition();
				case 1081:
					return new global::Kampai.Game.SpecialEventCharacterDefinition();
				case 1082:
					return new global::Kampai.Game.StuartCharacterDefinition();
				case 1083:
					return new global::Kampai.Game.TSMCharacterDefinition();
				case 1084:
					return new global::Kampai.Game.UIAnimationDefinition();
				case 1085:
					return new global::Kampai.Game.RewardCollectionDefinition();
				case 1086:
					return new global::Kampai.Game.EnvironmentDefinition();
				case 1087:
					return new global::Kampai.Game.FlyOverDefinition();
				case 1088:
					return new global::Kampai.Game.BridgeDefinition();
				case 1089:
					return new global::Kampai.Game.ItemDefinition();
				case 1090:
					return new global::Kampai.Game.CostumeItemDefinition();
				case 1091:
					return new global::Kampai.Game.DropItemDefinition();
				case 1092:
					return new global::Kampai.Game.DynamicIngredientsDefinition();
				case 1093:
					return new global::Kampai.Game.IngredientsItemDefinition();
				case 1094:
					return new global::Kampai.Game.PartyFavorAnimationItemDefinition();
				case 1095:
					return new global::Kampai.Game.SpecialEventItemDefinition();
				case 1096:
					return new global::Kampai.Game.AspirationalBuildingDefinition();
				case 1097:
					return new global::Kampai.Game.CommonLandExpansionDefinition();
				case 1098:
					return new global::Kampai.Game.DebrisDefinition();
				case 1099:
					return new global::Kampai.Game.LandExpansionConfig();
				case 1100:
					return new global::Kampai.Game.LandExpansionDefinition();
				case 1101:
					return new global::Kampai.Game.LocalizedTextDefinition();
				case 1102:
					return new global::Kampai.Game.MarketplaceDefinition();
				case 1103:
					return new global::Kampai.Game.MarketplaceItemDefinition();
				case 1104:
					return new global::Kampai.Game.MarketplaceSaleSlotDefinition();
				case 1105:
					return new global::Kampai.Game.MarketplaceRefreshTimerDefinition();
				case 1106:
					return new global::Kampai.Game.DynamicMasterPlanDefinition();
				case 1107:
					return new global::Kampai.Game.MasterPlanComponentDefinition();
				case 1108:
					return new global::Kampai.Game.MasterPlanDefinition();
				case 1109:
					return new global::Kampai.Game.MasterPlanOnboardDefinition();
				case 1110:
					return new global::Kampai.Game.GachaAnimationDefinition();
				case 1111:
					return new global::Kampai.Game.GachaWeightedDefinition();
				case 1112:
					return new global::Kampai.Game.MinionDefinition();
				case 1113:
					return new global::Kampai.Game.PartyFavorAnimationDefinition();
				case 1114:
					return new global::Kampai.Game.BuffDefinition();
				case 1115:
					return new global::Kampai.Game.GuestOfHonorDefinition();
				case 1116:
					return new global::Kampai.Game.MinionPartyDefinition();
				case 1117:
					return new global::Kampai.Game.PartyMeterDefinition();
				case 1118:
					return new global::Kampai.Game.PartyMeterTierDefinition();
				case 1119:
					return new global::Kampai.Game.MinionPartyLevelBandDefinition();
				case 1120:
					return new global::Kampai.Game.PartyPointsPerLevelDefinition();
				case 1121:
					return new global::Kampai.Game.MinionBenefitLevelBandDefintion();
				case 1122:
					return new global::Kampai.Game.PopulationBenefitDefinition();
				case 1123:
					return new global::Kampai.Game.PlayerTrainingCardDefinition();
				case 1124:
					return new global::Kampai.Game.PlayerTrainingCategoryDefinition();
				case 1125:
					return new global::Kampai.Game.PlayerTrainingDefinition();
				case 1126:
					return new global::Kampai.Game.FootprintDefinition();
				case 1127:
					return new global::Kampai.Game.NoOpPlotDefinition();
				case 1129:
					return new global::Kampai.Game.DynamicQuestDefinition();
				case 1130:
					return new global::Kampai.Game.QuestDefinition();
				case 1131:
					return new global::Kampai.Game.LimitedQuestDefinition();
				case 1132:
					return new global::Kampai.Game.QuestChainDefinition();
				case 1133:
					return new global::Kampai.Game.QuestResourceDefinition();
				case 1134:
					return new global::Kampai.Game.TimedQuestDefinition();
				case 1135:
					return new global::Kampai.Game.RushTimeBandDefinition();
				case 1136:
					return new global::Kampai.Game.SalePackDefinition();
				case 1137:
					return new global::Kampai.Game.PackDefinition();
				case 1138:
					return new global::Kampai.Game.PremiumCurrencyItemDefinition();
				case 1139:
					return new global::Kampai.Game.CurrencyItemDefinition();
				case 1140:
					return new global::Kampai.Game.SocialSettingsDefinition();
				case 1141:
					return new global::Kampai.Game.StickerDefinition();
				case 1142:
					return new global::Kampai.Game.CurrencyStoreCategoryDefinition();
				case 1143:
					return new global::Kampai.Game.CurrencyStoreDefinition();
				case 1144:
					return new global::Kampai.Game.CurrencyStorePackDefinition();
				case 1145:
					return new global::Kampai.Game.StoreItemDefinition();
				case 1146:
					return new global::Kampai.Game.TaskLevelBandDefinition();
				case 1147:
					return new global::Kampai.Game.TimedSocialEventDefinition();
				case 1148:
					return new global::Kampai.Game.UnlockDefinition();
				case 1149:
					return new global::Kampai.Game.Trigger.TSMTriggerDefinition();
				case 1151:
					return new global::Kampai.Game.Trigger.UpsellTriggerDefinition();
				case 1152:
					return new global::Kampai.Game.Trigger.AvailableLandTriggerConditionDefinition();
				case 1153:
					return new global::Kampai.Game.Trigger.ChurnTriggerConditionDefinition();
				case 1154:
					return new global::Kampai.Game.Trigger.ConsecutiveDaysConditionDefinition();
				case 1155:
					return new global::Kampai.Game.Trigger.CountryTriggerConditionDefinition();
				case 1156:
					return new global::Kampai.Game.Trigger.DaysSinceInstallTriggerConditionDefinition();
				case 1157:
					return new global::Kampai.Game.Trigger.HelpButtonTriggerConditionDefinition();
				case 1158:
					return new global::Kampai.Game.Trigger.HoursPlayedTriggerConditionDefinition();
				case 1159:
					return new global::Kampai.Game.Trigger.LandExpansionTriggerConditionDefinition();
				case 1160:
					return new global::Kampai.Game.Trigger.MignetteScoreTriggerConditionDefinition();
				case 1161:
					return new global::Kampai.Game.Trigger.OrderBoardTriggerConditionDefinition();
				case 1162:
					return new global::Kampai.Game.Trigger.PlatformTriggerConditionDefinition();
				case 1163:
					return new global::Kampai.Game.Trigger.PrestigeLevelTriggerConditionDefinition();
				case 1165:
					return new global::Kampai.Game.Trigger.PrestigeStateTriggerConditionDefinition();
				case 1166:
					return new global::Kampai.Game.Trigger.PrestigeTriggerConditionDefinition();
				case 1167:
					return new global::Kampai.Game.Trigger.PurchaseTriggerConditionDefinition();
				case 1168:
					return new global::Kampai.Game.Trigger.QuantityItemTriggerConditionDefinition();
				case 1169:
					return new global::Kampai.Game.Trigger.QuestTriggerConditionDefinition();
				case 1170:
					return new global::Kampai.Game.Trigger.SaleItemTriggerConditionDefinition();
				case 1171:
					return new global::Kampai.Game.Trigger.SaleSlotTriggerConditionDefinition();
				case 1172:
					return new global::Kampai.Game.Trigger.SegmentTriggerConditionDefinition();
				case 1173:
					return new global::Kampai.Game.Trigger.SessionCountTriggerConditionDefinition();
				case 1174:
					return new global::Kampai.Game.Trigger.SocialOrderTriggerConditionDefinition();
				case 1175:
					return new global::Kampai.Game.Trigger.SocialTimeTriggerConditionDefinition();
				case 1176:
					return new global::Kampai.Game.Trigger.StorageTriggerConditionDefinition();
				case 1177:
					return new global::Kampai.Game.Trigger.CaptainTeaseTriggerRewardDefinition();
				case 1179:
					return new global::Kampai.Game.Trigger.MignetteScoreTriggerRewardDefinition();
				case 1180:
					return new global::Kampai.Game.Trigger.PartyPointsTriggerRewardDefinition();
				case 1181:
					return new global::Kampai.Game.Trigger.QuantityItemTriggerRewardDefinition();
				case 1182:
					return new global::Kampai.Game.Trigger.SaleItemTriggerRewardDefinition();
				case 1183:
					return new global::Kampai.Game.Trigger.SaleSlotTriggerRewardDefinition();
				case 1184:
					return new global::Kampai.Game.Trigger.SocialOrderTriggerRewardDefinition();
				case 1185:
					return new global::Kampai.Game.Trigger.TSMMessageTriggerRewardDefinition();
				case 1186:
					return new global::Kampai.Game.Trigger.UpsellTriggerRewardDefinition();
				case 1187:
					return new global::Kampai.Game.VillainDefinition();
				case 1188:
					return new global::Kampai.Game.WayFinderDefinition();
				case 1189:
					return new global::Kampai.Game.PetsXPromoDefinition();
				case 1190:
					return new global::Kampai.Game.Definitions();
				case 1191:
					return new global::Kampai.Splash.LoadinTipBucketDefinition();
				case 1192:
					return new global::Kampai.Splash.LoadInTipDefinition();
				case 1193:
					return new global::Kampai.Main.HindsightCampaignDefinition();
				default:
					throw new global::Kampai.Util.BinarySerializationException(string.Format("BinarySerializationFactory.CreateInstance: unsupported type code {0}", typeCode));
				}
			}
		}

		private const bool NULL_MARKER = true;

		private const bool NOT_NULL_MARKER = false;

		public static void WriteLegalDocumentURL(global::System.IO.BinaryWriter writer, global::Kampai.Game.LegalDocumentURL instance)
		{
			WriteString(writer, instance.language);
			WriteString(writer, instance.url);
		}

		public static global::Kampai.Game.LegalDocumentURL ReadLegalDocumentURL(global::System.IO.BinaryReader reader)
		{
			return new global::Kampai.Game.LegalDocumentURL
			{
				language = ReadString(reader),
				url = ReadString(reader)
			};
		}

		public static void WriteNotificationReminder(global::System.IO.BinaryWriter writer, global::Kampai.Game.NotificationReminder instance)
		{
			writer.Write(instance.level);
			WriteString(writer, instance.messageLocalizedKey);
		}

		public static global::Kampai.Game.NotificationReminder ReadNotificationReminder(global::System.IO.BinaryReader reader)
		{
			return new global::Kampai.Game.NotificationReminder
			{
				level = reader.ReadInt32(),
				messageLocalizedKey = ReadString(reader)
			};
		}

		public static void WriteCharacterPrestigeLevelDefinition(global::System.IO.BinaryWriter writer, global::Kampai.Game.CharacterPrestigeLevelDefinition instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				writer.Write(instance.UnlockLevel);
				writer.Write(instance.UnlockQuestID);
				writer.Write(instance.PointsNeeded);
				writer.Write(instance.AttachedQuestID);
				WriteString(writer, instance.WelcomePanelMessageLocalizedKey);
				WriteString(writer, instance.FarewellPanelMessageLocalizedKey);
			}
		}

		public static global::Kampai.Game.CharacterPrestigeLevelDefinition ReadCharacterPrestigeLevelDefinition(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.CharacterPrestigeLevelDefinition characterPrestigeLevelDefinition = new global::Kampai.Game.CharacterPrestigeLevelDefinition();
			characterPrestigeLevelDefinition.UnlockLevel = reader.ReadUInt32();
			characterPrestigeLevelDefinition.UnlockQuestID = reader.ReadInt32();
			characterPrestigeLevelDefinition.PointsNeeded = reader.ReadUInt32();
			characterPrestigeLevelDefinition.AttachedQuestID = reader.ReadInt32();
			characterPrestigeLevelDefinition.WelcomePanelMessageLocalizedKey = ReadString(reader);
			characterPrestigeLevelDefinition.FarewellPanelMessageLocalizedKey = ReadString(reader);
			return characterPrestigeLevelDefinition;
		}

		public static void WriteAchievementID(global::System.IO.BinaryWriter writer, global::Kampai.Game.AchievementID instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				WriteString(writer, instance.GameCenterID);
				WriteString(writer, instance.GooglePlayID);
			}
		}

		public static global::Kampai.Game.AchievementID ReadAchievementID(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.AchievementID achievementID = new global::Kampai.Game.AchievementID();
			achievementID.GameCenterID = ReadString(reader);
			achievementID.GooglePlayID = ReadString(reader);
			return achievementID;
		}

		public static void WriteRewardTiers(global::System.IO.BinaryWriter writer, global::Kampai.Game.RewardTiers instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				WriteOnTheGlassDailyRewardTier(writer, instance.Tier1);
				WriteOnTheGlassDailyRewardTier(writer, instance.Tier2);
				WriteOnTheGlassDailyRewardTier(writer, instance.Tier3);
			}
		}

		public static global::Kampai.Game.RewardTiers ReadRewardTiers(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.RewardTiers rewardTiers = new global::Kampai.Game.RewardTiers();
			rewardTiers.Tier1 = ReadOnTheGlassDailyRewardTier(reader);
			rewardTiers.Tier2 = ReadOnTheGlassDailyRewardTier(reader);
			rewardTiers.Tier3 = ReadOnTheGlassDailyRewardTier(reader);
			return rewardTiers;
		}

		public static void WriteOnTheGlassDailyRewardTier(global::System.IO.BinaryWriter writer, global::Kampai.Game.OnTheGlassDailyRewardTier instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				writer.Write(instance.Weight);
				WriteObject(writer, instance.PredefinedRewards);
				writer.Write(instance.CraftableRewardMinTier);
				writer.Write(instance.CraftableRewardMaxQuantity);
				writer.Write(instance.CraftableRewardWeight);
			}
		}

		public static global::Kampai.Game.OnTheGlassDailyRewardTier ReadOnTheGlassDailyRewardTier(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.OnTheGlassDailyRewardTier onTheGlassDailyRewardTier = new global::Kampai.Game.OnTheGlassDailyRewardTier();
			onTheGlassDailyRewardTier.Weight = reader.ReadInt32();
			onTheGlassDailyRewardTier.PredefinedRewards = ReadObject<global::Kampai.Game.Transaction.WeightedDefinition>(reader);
			onTheGlassDailyRewardTier.CraftableRewardMinTier = reader.ReadInt32();
			onTheGlassDailyRewardTier.CraftableRewardMaxQuantity = reader.ReadInt32();
			onTheGlassDailyRewardTier.CraftableRewardWeight = reader.ReadInt32();
			return onTheGlassDailyRewardTier;
		}

		public static void WriteScreenPosition(global::System.IO.BinaryWriter writer, global::Kampai.Game.ScreenPosition instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				writer.Write(instance.x);
				writer.Write(instance.z);
				writer.Write(instance.zoom);
			}
		}

		public static global::Kampai.Game.ScreenPosition ReadScreenPosition(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.ScreenPosition screenPosition = new global::Kampai.Game.ScreenPosition();
			screenPosition.x = reader.ReadSingle();
			screenPosition.z = reader.ReadSingle();
			screenPosition.zoom = reader.ReadSingle();
			return screenPosition;
		}

		public static void WriteVector3(global::System.IO.BinaryWriter writer, global::UnityEngine.Vector3 instance)
		{
			writer.Write(instance.x);
			writer.Write(instance.y);
			writer.Write(instance.z);
		}

		public static global::UnityEngine.Vector3 ReadVector3(global::System.IO.BinaryReader reader)
		{
			return new global::UnityEngine.Vector3
			{
				x = reader.ReadSingle(),
				y = reader.ReadSingle(),
				z = reader.ReadSingle()
			};
		}

		public static void WriteConnectablePiecePrefabDefinition(global::System.IO.BinaryWriter writer, global::Kampai.Game.ConnectablePiecePrefabDefinition instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				WriteString(writer, instance.straight);
				WriteString(writer, instance.cross);
				WriteString(writer, instance.post);
				WriteString(writer, instance.tshape);
				WriteString(writer, instance.endcap);
				WriteString(writer, instance.corner);
			}
		}

		public static global::Kampai.Game.ConnectablePiecePrefabDefinition ReadConnectablePiecePrefabDefinition(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.ConnectablePiecePrefabDefinition connectablePiecePrefabDefinition = new global::Kampai.Game.ConnectablePiecePrefabDefinition();
			connectablePiecePrefabDefinition.straight = ReadString(reader);
			connectablePiecePrefabDefinition.cross = ReadString(reader);
			connectablePiecePrefabDefinition.post = ReadString(reader);
			connectablePiecePrefabDefinition.tshape = ReadString(reader);
			connectablePiecePrefabDefinition.endcap = ReadString(reader);
			connectablePiecePrefabDefinition.corner = ReadString(reader);
			return connectablePiecePrefabDefinition;
		}

		public static void WriteSlotUnlock(global::System.IO.BinaryWriter writer, global::Kampai.Game.SlotUnlock instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				WriteListInt32(writer, instance.SlotUnlockLevels);
				WriteListInt32(writer, instance.SlotUnlockCosts);
			}
		}

		public static global::Kampai.Game.SlotUnlock ReadSlotUnlock(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.SlotUnlock slotUnlock = new global::Kampai.Game.SlotUnlock();
			slotUnlock.SlotUnlockLevels = ReadListInt32(reader, slotUnlock.SlotUnlockLevels);
			slotUnlock.SlotUnlockCosts = ReadListInt32(reader, slotUnlock.SlotUnlockCosts);
			return slotUnlock;
		}

		public static void WriteUserSegment(global::System.IO.BinaryWriter writer, global::Kampai.Game.UserSegment instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				writer.Write(instance.LevelGreaterThanOrEqualTo);
				writer.Write(instance.FirstXReturnRewardsWeightedDefinitionId);
				writer.Write(instance.SecondXReturnRewardsWeightedDefinitionId);
				writer.Write(instance.AfterXReturnRewards);
			}
		}

		public static global::Kampai.Game.UserSegment ReadUserSegment(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.UserSegment userSegment = new global::Kampai.Game.UserSegment();
			userSegment.LevelGreaterThanOrEqualTo = reader.ReadInt32();
			userSegment.FirstXReturnRewardsWeightedDefinitionId = reader.ReadInt32();
			userSegment.SecondXReturnRewardsWeightedDefinitionId = reader.ReadInt32();
			userSegment.AfterXReturnRewards = reader.ReadInt32();
			return userSegment;
		}

		public static void WriteLocation(global::System.IO.BinaryWriter writer, global::Kampai.Game.Location instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				writer.Write(instance.x);
				writer.Write(instance.y);
			}
		}

		public static global::Kampai.Game.Location ReadLocation(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.Location location = new global::Kampai.Game.Location();
			location.x = reader.ReadInt32();
			location.y = reader.ReadInt32();
			return location;
		}

		public static void WriteMignetteRuleDefinition(global::System.IO.BinaryWriter writer, MignetteRuleDefinition instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				WriteString(writer, instance.CauseImage);
				WriteString(writer, instance.CauseImageMask);
				WriteString(writer, instance.EffectImage);
				WriteString(writer, instance.EffectImageMask);
				writer.Write(instance.EffectAmount);
			}
		}

		public static MignetteRuleDefinition ReadMignetteRuleDefinition(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			MignetteRuleDefinition mignetteRuleDefinition = new MignetteRuleDefinition();
			mignetteRuleDefinition.CauseImage = ReadString(reader);
			mignetteRuleDefinition.CauseImageMask = ReadString(reader);
			mignetteRuleDefinition.EffectImage = ReadString(reader);
			mignetteRuleDefinition.EffectImageMask = ReadString(reader);
			mignetteRuleDefinition.EffectAmount = reader.ReadInt32();
			return mignetteRuleDefinition;
		}

		public static void WriteMignetteChildObjectDefinition(global::System.IO.BinaryWriter writer, global::Kampai.Game.MignetteChildObjectDefinition instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				WriteString(writer, instance.Prefab);
				WriteVector3(writer, instance.Position);
				writer.Write(instance.IsLocal);
				writer.Write(instance.Rotation);
			}
		}

		public static global::Kampai.Game.MignetteChildObjectDefinition ReadMignetteChildObjectDefinition(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.MignetteChildObjectDefinition mignetteChildObjectDefinition = new global::Kampai.Game.MignetteChildObjectDefinition();
			mignetteChildObjectDefinition.Prefab = ReadString(reader);
			mignetteChildObjectDefinition.Position = ReadVector3(reader);
			mignetteChildObjectDefinition.IsLocal = reader.ReadBoolean();
			mignetteChildObjectDefinition.Rotation = reader.ReadSingle();
			return mignetteChildObjectDefinition;
		}

		public static void WriteMinionPartyPrefabDefinition(global::System.IO.BinaryWriter writer, global::Kampai.Game.MinionPartyPrefabDefinition instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				WriteString(writer, instance.EventType);
				WriteString(writer, instance.Prefab);
			}
		}

		public static global::Kampai.Game.MinionPartyPrefabDefinition ReadMinionPartyPrefabDefinition(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.MinionPartyPrefabDefinition minionPartyPrefabDefinition = new global::Kampai.Game.MinionPartyPrefabDefinition();
			minionPartyPrefabDefinition.EventType = ReadString(reader);
			minionPartyPrefabDefinition.Prefab = ReadString(reader);
			return minionPartyPrefabDefinition;
		}

		public static void WriteArea(global::System.IO.BinaryWriter writer, global::Kampai.Game.Area instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				WriteLocation(writer, instance.a);
				WriteLocation(writer, instance.b);
			}
		}

		public static global::Kampai.Game.Area ReadArea(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.Area area = new global::Kampai.Game.Area();
			area.a = ReadLocation(reader);
			area.b = ReadLocation(reader);
			return area;
		}

		public static void WriteStorageUpgradeDefinition(global::System.IO.BinaryWriter writer, global::Kampai.Game.StorageUpgradeDefinition instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				writer.Write(instance.Level);
				writer.Write(instance.StorageCapacity);
				writer.Write(instance.TransactionId);
			}
		}

		public static global::Kampai.Game.StorageUpgradeDefinition ReadStorageUpgradeDefinition(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.StorageUpgradeDefinition storageUpgradeDefinition = new global::Kampai.Game.StorageUpgradeDefinition();
			storageUpgradeDefinition.Level = reader.ReadInt32();
			storageUpgradeDefinition.StorageCapacity = reader.ReadUInt32();
			storageUpgradeDefinition.TransactionId = reader.ReadInt32();
			return storageUpgradeDefinition;
		}

		public static void WritePlatformDefinition(global::System.IO.BinaryWriter writer, global::Kampai.Game.PlatformDefinition instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				WriteString(writer, instance.buildingRemovalAnimController);
				writer.Write(instance.customCameraPosID);
				WriteString(writer, instance.description);
				WriteVector3(writer, instance.offset);
				WriteLocation(writer, instance.placementLocation);
			}
		}

		public static global::Kampai.Game.PlatformDefinition ReadPlatformDefinition(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.PlatformDefinition platformDefinition = new global::Kampai.Game.PlatformDefinition();
			platformDefinition.buildingRemovalAnimController = ReadString(reader);
			platformDefinition.customCameraPosID = reader.ReadInt32();
			platformDefinition.description = ReadString(reader);
			platformDefinition.offset = ReadVector3(reader);
			platformDefinition.placementLocation = ReadLocation(reader);
			return platformDefinition;
		}

		public static void WriteResourcePlotDefinition(global::System.IO.BinaryWriter writer, global::Kampai.Game.ResourcePlotDefinition instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				WriteString(writer, instance.descriptionKey);
				writer.Write(instance.isAutomaticallyUnlocked);
				WriteLocation(writer, instance.location);
				writer.Write(instance.unlockTransactionID);
				writer.Write(instance.rotation);
			}
		}

		public static global::Kampai.Game.ResourcePlotDefinition ReadResourcePlotDefinition(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.ResourcePlotDefinition resourcePlotDefinition = new global::Kampai.Game.ResourcePlotDefinition();
			resourcePlotDefinition.descriptionKey = ReadString(reader);
			resourcePlotDefinition.isAutomaticallyUnlocked = reader.ReadBoolean();
			resourcePlotDefinition.location = ReadLocation(reader);
			resourcePlotDefinition.unlockTransactionID = reader.ReadInt32();
			resourcePlotDefinition.rotation = reader.ReadInt32();
			return resourcePlotDefinition;
		}

		public static void WriteCharacterUIAnimationDefinition(global::System.IO.BinaryWriter writer, global::Kampai.Game.CharacterUIAnimationDefinition instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				WriteString(writer, instance.StateMachine);
				writer.Write(instance.IdleWeightedAnimationID);
				writer.Write(instance.IdleCount);
				writer.Write(instance.HappyWeightedAnimationID);
				writer.Write(instance.HappyCount);
				writer.Write(instance.SelectedWeightedAnimationID);
				writer.Write(instance.SelectedCount);
				writer.Write(instance.UseLegacy);
			}
		}

		public static global::Kampai.Game.CharacterUIAnimationDefinition ReadCharacterUIAnimationDefinition(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.CharacterUIAnimationDefinition characterUIAnimationDefinition = new global::Kampai.Game.CharacterUIAnimationDefinition();
			characterUIAnimationDefinition.StateMachine = ReadString(reader);
			characterUIAnimationDefinition.IdleWeightedAnimationID = reader.ReadInt32();
			characterUIAnimationDefinition.IdleCount = reader.ReadInt32();
			characterUIAnimationDefinition.HappyWeightedAnimationID = reader.ReadInt32();
			characterUIAnimationDefinition.HappyCount = reader.ReadInt32();
			characterUIAnimationDefinition.SelectedWeightedAnimationID = reader.ReadInt32();
			characterUIAnimationDefinition.SelectedCount = reader.ReadInt32();
			characterUIAnimationDefinition.UseLegacy = reader.ReadBoolean();
			return characterUIAnimationDefinition;
		}

		public static void WriteFloatLocation(global::System.IO.BinaryWriter writer, global::Kampai.Game.FloatLocation instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				writer.Write(instance.x);
				writer.Write(instance.y);
			}
		}

		public static global::Kampai.Game.FloatLocation ReadFloatLocation(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.FloatLocation floatLocation = new global::Kampai.Game.FloatLocation();
			floatLocation.x = reader.ReadSingle();
			floatLocation.y = reader.ReadSingle();
			return floatLocation;
		}

		public static void WriteAngle(global::System.IO.BinaryWriter writer, global::Kampai.Game.Angle instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				writer.Write(instance.Degrees);
			}
		}

		public static global::Kampai.Game.Angle ReadAngle(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.Angle angle = new global::Kampai.Game.Angle();
			angle.Degrees = reader.ReadSingle();
			return angle;
		}

		public static void WriteCollectionReward(global::System.IO.BinaryWriter writer, global::Kampai.Game.CollectionReward instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				writer.Write(instance.RequiredPoints);
				writer.Write(instance.TransactionID);
			}
		}

		public static global::Kampai.Game.CollectionReward ReadCollectionReward(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.CollectionReward collectionReward = new global::Kampai.Game.CollectionReward();
			collectionReward.RequiredPoints = reader.ReadInt32();
			collectionReward.TransactionID = reader.ReadInt32();
			return collectionReward;
		}

		public static void WriteFlyOverNode(global::System.IO.BinaryWriter writer, global::Kampai.Game.FlyOverNode instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				writer.Write(instance.x);
				writer.Write(instance.y);
				writer.Write(instance.z);
			}
		}

		public static global::Kampai.Game.FlyOverNode ReadFlyOverNode(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.FlyOverNode flyOverNode = new global::Kampai.Game.FlyOverNode();
			flyOverNode.x = reader.ReadSingle();
			flyOverNode.y = reader.ReadSingle();
			flyOverNode.z = reader.ReadSingle();
			return flyOverNode;
		}

		public static void WriteBridgeScreenPosition(global::System.IO.BinaryWriter writer, global::Kampai.Game.BridgeScreenPosition instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				writer.Write(instance.x);
				writer.Write(instance.y);
				writer.Write(instance.z);
				writer.Write(instance.zoom);
			}
		}

		public static global::Kampai.Game.BridgeScreenPosition ReadBridgeScreenPosition(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.BridgeScreenPosition bridgeScreenPosition = new global::Kampai.Game.BridgeScreenPosition();
			bridgeScreenPosition.x = reader.ReadSingle();
			bridgeScreenPosition.y = reader.ReadSingle();
			bridgeScreenPosition.z = reader.ReadSingle();
			bridgeScreenPosition.zoom = reader.ReadSingle();
			return bridgeScreenPosition;
		}

		public static void WriteKampaiColor(global::System.IO.BinaryWriter writer, global::Kampai.Util.KampaiColor instance)
		{
			writer.Write(instance.r);
			writer.Write(instance.g);
			writer.Write(instance.b);
			writer.Write(instance.a);
		}

		public static global::Kampai.Util.KampaiColor ReadKampaiColor(global::System.IO.BinaryReader reader)
		{
			return new global::Kampai.Util.KampaiColor
			{
				r = reader.ReadSingle(),
				g = reader.ReadSingle(),
				b = reader.ReadSingle(),
				a = reader.ReadSingle()
			};
		}

		public static void WriteReward(global::System.IO.BinaryWriter writer, global::Kampai.Game.Reward instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				writer.Write(instance.requiredQuantity);
				writer.Write(instance.premiumReward);
			}
		}

		public static global::Kampai.Game.Reward ReadReward(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.Reward reward = new global::Kampai.Game.Reward();
			reward.requiredQuantity = reader.ReadUInt32();
			reward.premiumReward = reader.ReadUInt32();
			return reward;
		}

		public static void WriteMiniGameScoreReward(global::System.IO.BinaryWriter writer, global::Kampai.Game.MiniGameScoreReward instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				writer.Write(instance.MiniGameId);
				WriteList<global::Kampai.Game.Reward>(writer, WriteReward, instance.rewardTable);
			}
		}

		public static global::Kampai.Game.MiniGameScoreReward ReadMiniGameScoreReward(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.MiniGameScoreReward miniGameScoreReward = new global::Kampai.Game.MiniGameScoreReward();
			miniGameScoreReward.MiniGameId = reader.ReadInt32();
			miniGameScoreReward.rewardTable = ReadList<global::Kampai.Game.Reward>(reader, ReadReward, miniGameScoreReward.rewardTable);
			return miniGameScoreReward;
		}

		public static void WriteMiniGameScoreRange(global::System.IO.BinaryWriter writer, global::Kampai.Game.MiniGameScoreRange instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				writer.Write(instance.MiniGameId);
				writer.Write(instance.ScoreRangeMax);
				writer.Write(instance.ScoreRangeMin);
			}
		}

		public static global::Kampai.Game.MiniGameScoreRange ReadMiniGameScoreRange(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.MiniGameScoreRange miniGameScoreRange = new global::Kampai.Game.MiniGameScoreRange();
			miniGameScoreRange.MiniGameId = reader.ReadInt32();
			miniGameScoreRange.ScoreRangeMax = reader.ReadInt32();
			miniGameScoreRange.ScoreRangeMin = reader.ReadInt32();
			return miniGameScoreRange;
		}

		public static void WriteMasterPlanComponentRewardDefinition(global::System.IO.BinaryWriter writer, global::Kampai.Game.MasterPlanComponentRewardDefinition instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				writer.Write(instance.rewardItemId);
				writer.Write(instance.rewardQuantity);
				writer.Write(instance.grindReward);
				writer.Write(instance.premiumReward);
			}
		}

		public static global::Kampai.Game.MasterPlanComponentRewardDefinition ReadMasterPlanComponentRewardDefinition(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.MasterPlanComponentRewardDefinition masterPlanComponentRewardDefinition = new global::Kampai.Game.MasterPlanComponentRewardDefinition();
			masterPlanComponentRewardDefinition.rewardItemId = reader.ReadInt32();
			masterPlanComponentRewardDefinition.rewardQuantity = reader.ReadUInt32();
			masterPlanComponentRewardDefinition.grindReward = reader.ReadUInt32();
			masterPlanComponentRewardDefinition.premiumReward = reader.ReadUInt32();
			return masterPlanComponentRewardDefinition;
		}

		public static void WriteMasterPlanComponentTaskDefinition(global::System.IO.BinaryWriter writer, global::Kampai.Game.MasterPlanComponentTaskDefinition instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				writer.Write(instance.requiredItemId);
				writer.Write(instance.requiredQuantity);
				writer.Write(instance.ShowWayfinder);
				WriteEnum(writer, instance.Type);
			}
		}

		public static global::Kampai.Game.MasterPlanComponentTaskDefinition ReadMasterPlanComponentTaskDefinition(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.MasterPlanComponentTaskDefinition masterPlanComponentTaskDefinition = new global::Kampai.Game.MasterPlanComponentTaskDefinition();
			masterPlanComponentTaskDefinition.requiredItemId = reader.ReadInt32();
			masterPlanComponentTaskDefinition.requiredQuantity = reader.ReadUInt32();
			masterPlanComponentTaskDefinition.ShowWayfinder = reader.ReadBoolean();
			masterPlanComponentTaskDefinition.Type = ReadEnum<global::Kampai.Game.MasterPlanComponentTaskType>(reader);
			return masterPlanComponentTaskDefinition;
		}

		public static void WriteGhostFunctionDefinition(global::System.IO.BinaryWriter writer, global::Kampai.Game.GhostFunctionDefinition instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				WriteEnum(writer, instance.startType);
				WriteEnum(writer, instance.closeType);
				writer.Write(instance.componentBuildingDefID);
			}
		}

		public static global::Kampai.Game.GhostFunctionDefinition ReadGhostFunctionDefinition(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.GhostFunctionDefinition ghostFunctionDefinition = new global::Kampai.Game.GhostFunctionDefinition();
			ghostFunctionDefinition.startType = ReadEnum<global::Kampai.UI.GhostComponentFunctionType>(reader);
			ghostFunctionDefinition.closeType = ReadEnum<global::Kampai.UI.GhostFunctionCloseType>(reader);
			ghostFunctionDefinition.componentBuildingDefID = reader.ReadInt32();
			return ghostFunctionDefinition;
		}

		public static void WriteKnuckleheadednessInfo(global::System.IO.BinaryWriter writer, global::Kampai.Game.KnuckleheadednessInfo instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				writer.Write(instance.KnuckleheaddednessMin);
				writer.Write(instance.KnuckleheaddednessMax);
				writer.Write(instance.KnuckleheaddednessScale);
			}
		}

		public static global::Kampai.Game.KnuckleheadednessInfo ReadKnuckleheadednessInfo(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.KnuckleheadednessInfo knuckleheadednessInfo = new global::Kampai.Game.KnuckleheadednessInfo();
			knuckleheadednessInfo.KnuckleheaddednessMin = reader.ReadSingle();
			knuckleheadednessInfo.KnuckleheaddednessMax = reader.ReadSingle();
			knuckleheadednessInfo.KnuckleheaddednessScale = reader.ReadSingle();
			return knuckleheadednessInfo;
		}

		public static void WriteAnimationAlternate(global::System.IO.BinaryWriter writer, global::Kampai.Game.AnimationAlternate instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				writer.Write(instance.GroupID);
				writer.Write(instance.PercentChance);
			}
		}

		public static global::Kampai.Game.AnimationAlternate ReadAnimationAlternate(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.AnimationAlternate animationAlternate = new global::Kampai.Game.AnimationAlternate();
			animationAlternate.GroupID = reader.ReadInt32();
			animationAlternate.PercentChance = reader.ReadSingle();
			return animationAlternate;
		}

		public static void WriteCameraControlSettings(global::System.IO.BinaryWriter writer, global::Kampai.Game.CameraControlSettings instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				writer.Write(instance.customCameraPosTiki);
				writer.Write(instance.customCameraPosStage);
				writer.Write(instance.customCameraPosTownHall);
				writer.Write(instance.customCameraPosPartyDefault);
			}
		}

		public static global::Kampai.Game.CameraControlSettings ReadCameraControlSettings(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.CameraControlSettings cameraControlSettings = new global::Kampai.Game.CameraControlSettings();
			cameraControlSettings.customCameraPosTiki = reader.ReadInt32();
			cameraControlSettings.customCameraPosStage = reader.ReadInt32();
			cameraControlSettings.customCameraPosTownHall = reader.ReadInt32();
			cameraControlSettings.customCameraPosPartyDefault = reader.ReadInt32();
			return cameraControlSettings;
		}

		public static void WriteVFXAssetDefinition(global::System.IO.BinaryWriter writer, global::Kampai.Game.VFXAssetDefinition instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				WriteLocation(writer, instance.location);
				WriteString(writer, instance.Prefab);
			}
		}

		public static global::Kampai.Game.VFXAssetDefinition ReadVFXAssetDefinition(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.VFXAssetDefinition vFXAssetDefinition = new global::Kampai.Game.VFXAssetDefinition();
			vFXAssetDefinition.location = ReadLocation(reader);
			vFXAssetDefinition.Prefab = ReadString(reader);
			return vFXAssetDefinition;
		}

		public static void WriteMinionBenefit(global::System.IO.BinaryWriter writer, global::Kampai.Game.MinionBenefit instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				WriteString(writer, instance.localizedKey);
				writer.Write(instance.itemIconId);
				WriteEnum(writer, instance.type);
			}
		}

		public static global::Kampai.Game.MinionBenefit ReadMinionBenefit(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.MinionBenefit minionBenefit = new global::Kampai.Game.MinionBenefit();
			minionBenefit.localizedKey = ReadString(reader);
			minionBenefit.itemIconId = reader.ReadInt32();
			minionBenefit.type = ReadEnum<global::Kampai.UI.View.Benefit>(reader);
			return minionBenefit;
		}

		public static void WriteMinionBenefitLevel(global::System.IO.BinaryWriter writer, global::Kampai.Game.MinionBenefitLevel instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				writer.Write(instance.doubleDropPercentage);
				writer.Write(instance.doubleDropLevel);
				writer.Write(instance.premiumDropPercentage);
				writer.Write(instance.premiumDropLevel);
				writer.Write(instance.rareDropPercentage);
				writer.Write(instance.rareDropLevel);
				writer.Write(instance.tokensToLevel);
				writer.Write(instance.costumeId);
				WriteString(writer, instance.image);
				WriteString(writer, instance.mask);
			}
		}

		public static global::Kampai.Game.MinionBenefitLevel ReadMinionBenefitLevel(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.MinionBenefitLevel minionBenefitLevel = new global::Kampai.Game.MinionBenefitLevel();
			minionBenefitLevel.doubleDropPercentage = reader.ReadSingle();
			minionBenefitLevel.doubleDropLevel = reader.ReadInt32();
			minionBenefitLevel.premiumDropPercentage = reader.ReadSingle();
			minionBenefitLevel.premiumDropLevel = reader.ReadInt32();
			minionBenefitLevel.rareDropPercentage = reader.ReadSingle();
			minionBenefitLevel.rareDropLevel = reader.ReadInt32();
			minionBenefitLevel.tokensToLevel = reader.ReadInt32();
			minionBenefitLevel.costumeId = reader.ReadInt32();
			minionBenefitLevel.image = ReadString(reader);
			minionBenefitLevel.mask = ReadString(reader);
			return minionBenefitLevel;
		}

		public static void WriteImageMaskCombo(global::System.IO.BinaryWriter writer, global::Kampai.Game.ImageMaskCombo instance)
		{
			WriteString(writer, instance.image);
			WriteString(writer, instance.mask);
		}

		public static global::Kampai.Game.ImageMaskCombo ReadImageMaskCombo(global::System.IO.BinaryReader reader)
		{
			return new global::Kampai.Game.ImageMaskCombo
			{
				image = ReadString(reader),
				mask = ReadString(reader)
			};
		}

		public static void WriteTransactionInstance(global::System.IO.BinaryWriter writer, global::Kampai.Game.Transaction.TransactionInstance instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				writer.Write(instance.ID);
				WriteList(writer, instance.Inputs);
				WriteList(writer, instance.Outputs);
			}
		}

		public static global::Kampai.Game.Transaction.TransactionInstance ReadTransactionInstance(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.Transaction.TransactionInstance transactionInstance = new global::Kampai.Game.Transaction.TransactionInstance();
			transactionInstance.ID = reader.ReadInt32();
			transactionInstance.Inputs = ReadList(reader, transactionInstance.Inputs);
			transactionInstance.Outputs = ReadList(reader, transactionInstance.Outputs);
			return transactionInstance;
		}

		public static void WriteQuestStepDefinition(global::System.IO.BinaryWriter writer, global::Kampai.Game.QuestStepDefinition instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				WriteEnum(writer, instance.Type);
				writer.Write(instance.ItemAmount);
				writer.Write(instance.ItemDefinitionID);
				writer.Write(instance.CostumeDefinitionID);
				writer.Write(instance.ShowWayfinder);
				writer.Write(instance.QuestStepCompletePlayerTrainingCategoryItemId);
				writer.Write(instance.UpgradeLevel);
			}
		}

		public static global::Kampai.Game.QuestStepDefinition ReadQuestStepDefinition(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.QuestStepDefinition questStepDefinition = new global::Kampai.Game.QuestStepDefinition();
			questStepDefinition.Type = ReadEnum<global::Kampai.Game.QuestStepType>(reader);
			questStepDefinition.ItemAmount = reader.ReadInt32();
			questStepDefinition.ItemDefinitionID = reader.ReadInt32();
			questStepDefinition.CostumeDefinitionID = reader.ReadInt32();
			questStepDefinition.ShowWayfinder = reader.ReadBoolean();
			questStepDefinition.QuestStepCompletePlayerTrainingCategoryItemId = reader.ReadInt32();
			questStepDefinition.UpgradeLevel = reader.ReadInt32();
			return questStepDefinition;
		}

		public static void WriteQuestChainStepDefinition(global::System.IO.BinaryWriter writer, global::Kampai.Game.QuestChainStepDefinition instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				WriteString(writer, instance.Intro);
				WriteString(writer, instance.Voice);
				WriteString(writer, instance.Outro);
				writer.Write(instance.XP);
				writer.Write(instance.Grind);
				writer.Write(instance.Premium);
				WriteList<global::Kampai.Game.QuestChainTask>(writer, WriteQuestChainTask, instance.Tasks);
			}
		}

		public static global::Kampai.Game.QuestChainStepDefinition ReadQuestChainStepDefinition(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.QuestChainStepDefinition questChainStepDefinition = new global::Kampai.Game.QuestChainStepDefinition();
			questChainStepDefinition.Intro = ReadString(reader);
			questChainStepDefinition.Voice = ReadString(reader);
			questChainStepDefinition.Outro = ReadString(reader);
			questChainStepDefinition.XP = reader.ReadInt32();
			questChainStepDefinition.Grind = reader.ReadInt32();
			questChainStepDefinition.Premium = reader.ReadInt32();
			questChainStepDefinition.Tasks = ReadList<global::Kampai.Game.QuestChainTask>(reader, ReadQuestChainTask, questChainStepDefinition.Tasks);
			return questChainStepDefinition;
		}

		public static void WriteQuestChainTask(global::System.IO.BinaryWriter writer, global::Kampai.Game.QuestChainTask instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				WriteEnum(writer, instance.Type);
				writer.Write(instance.Item);
				writer.Write(instance.Count);
			}
		}

		public static global::Kampai.Game.QuestChainTask ReadQuestChainTask(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.QuestChainTask questChainTask = new global::Kampai.Game.QuestChainTask();
			questChainTask.Type = ReadEnum<global::Kampai.Game.QuestChainTaskType>(reader);
			questChainTask.Item = reader.ReadInt32();
			questChainTask.Count = reader.ReadInt32();
			return questChainTask;
		}

		public static void WritePlatformStoreSkuDefinition(global::System.IO.BinaryWriter writer, global::Kampai.Game.PlatformStoreSkuDefinition instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				WriteString(writer, instance.appleAppstore);
				WriteString(writer, instance.googlePlay);
				WriteString(writer, instance.defaultStore);
			}
		}

		public static global::Kampai.Game.PlatformStoreSkuDefinition ReadPlatformStoreSkuDefinition(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.PlatformStoreSkuDefinition platformStoreSkuDefinition = new global::Kampai.Game.PlatformStoreSkuDefinition();
			platformStoreSkuDefinition.appleAppstore = ReadString(reader);
			platformStoreSkuDefinition.googlePlay = ReadString(reader);
			platformStoreSkuDefinition.defaultStore = ReadString(reader);
			return platformStoreSkuDefinition;
		}

		public static void WriteVector3Serialize(global::System.IO.BinaryWriter writer, global::Kampai.Util.Vector3Serialize instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				writer.Write(instance.x);
				writer.Write(instance.y);
				writer.Write(instance.z);
			}
		}

		public static global::Kampai.Util.Vector3Serialize ReadVector3Serialize(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Util.Vector3Serialize vector3Serialize = new global::Kampai.Util.Vector3Serialize();
			vector3Serialize.x = reader.ReadInt32();
			vector3Serialize.y = reader.ReadInt32();
			vector3Serialize.z = reader.ReadInt32();
			return vector3Serialize;
		}

		public static void WriteSocialEventOrderDefinition(global::System.IO.BinaryWriter writer, global::Kampai.Game.SocialEventOrderDefinition instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				writer.Write(instance.OrderID);
				writer.Write(instance.Transaction);
			}
		}

		public static global::Kampai.Game.SocialEventOrderDefinition ReadSocialEventOrderDefinition(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.SocialEventOrderDefinition socialEventOrderDefinition = new global::Kampai.Game.SocialEventOrderDefinition();
			socialEventOrderDefinition.OrderID = reader.ReadInt32();
			socialEventOrderDefinition.Transaction = reader.ReadInt32();
			return socialEventOrderDefinition;
		}

		public static void WriteTriggerRewardLayout(global::System.IO.BinaryWriter writer, global::Kampai.Game.Trigger.TriggerRewardLayout instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				writer.Write(instance.index);
				WriteListInt32(writer, instance.itemIds);
				WriteEnum(writer, instance.layout);
			}
		}

		public static global::Kampai.Game.Trigger.TriggerRewardLayout ReadTriggerRewardLayout(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.Trigger.TriggerRewardLayout triggerRewardLayout = new global::Kampai.Game.Trigger.TriggerRewardLayout();
			triggerRewardLayout.index = reader.ReadInt32();
			triggerRewardLayout.itemIds = ReadListInt32(reader, triggerRewardLayout.itemIds);
			triggerRewardLayout.layout = ReadEnum<global::Kampai.Game.Trigger.TriggerRewardLayout.Layout>(reader);
			return triggerRewardLayout;
		}

		public static void WriteGachaConfig(global::System.IO.BinaryWriter writer, global::Kampai.Game.GachaConfig instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				WriteList(writer, instance.GatchaAnimationDefinitions);
				WriteList(writer, instance.DistributionTables);
			}
		}

		public static global::Kampai.Game.GachaConfig ReadGachaConfig(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.GachaConfig gachaConfig = new global::Kampai.Game.GachaConfig();
			gachaConfig.GatchaAnimationDefinitions = ReadList(reader, gachaConfig.GatchaAnimationDefinitions);
			gachaConfig.DistributionTables = ReadList(reader, gachaConfig.DistributionTables);
			return gachaConfig;
		}

		public static void WriteTaskDefinition(global::System.IO.BinaryWriter writer, global::Kampai.Game.TaskDefinition instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				WriteList(writer, instance.levelBands);
			}
		}

		public static global::Kampai.Game.TaskDefinition ReadTaskDefinition(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.TaskDefinition taskDefinition = new global::Kampai.Game.TaskDefinition();
			taskDefinition.levelBands = ReadList(reader, taskDefinition.levelBands);
			return taskDefinition;
		}

		public static void WritePlayerVersion(global::System.IO.BinaryWriter writer, global::Kampai.Game.PlayerVersion instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				writer.Write(instance.Version);
			}
		}

		public static global::Kampai.Game.PlayerVersion ReadPlayerVersion(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Game.PlayerVersion playerVersion = new global::Kampai.Game.PlayerVersion();
			playerVersion.Version = reader.ReadInt32();
			return playerVersion;
		}

		public static void WriteBucketAssignment(global::System.IO.BinaryWriter writer, global::Kampai.Splash.BucketAssignment instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				writer.Write(instance.BucketId);
				writer.Write(instance.Time);
			}
		}

		public static global::Kampai.Splash.BucketAssignment ReadBucketAssignment(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			global::Kampai.Splash.BucketAssignment bucketAssignment = new global::Kampai.Splash.BucketAssignment();
			bucketAssignment.BucketId = reader.ReadInt32();
			bucketAssignment.Time = reader.ReadSingle();
			return bucketAssignment;
		}

		private static bool WriteNullMarker(global::System.IO.BinaryWriter writer, object instance)
		{
			if (instance == null)
			{
				writer.Write(true);
				return true;
			}
			writer.Write(false);
			return false;
		}

		private static bool ReadNullMarker(global::System.IO.BinaryReader reader)
		{
			if (reader.ReadBoolean())
			{
				return true;
			}
			return false;
		}

		public static void WriteObject(global::System.IO.BinaryWriter writer, global::Kampai.Util.IBinarySerializable instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				writer.Write(instance.TypeCode);
				instance.Write(writer);
			}
		}

		public static T ReadObject<T>(global::System.IO.BinaryReader reader, T existing_instance = null) where T : class, global::Kampai.Util.IBinarySerializable
		{
			if (ReadNullMarker(reader))
			{
				return (T)null;
			}
			int typeCode = reader.ReadInt32();
			T result = existing_instance ?? ((T)global::Kampai.Util.BinarySerializationUtil.BinarySerializationFactory.CreateInstance(typeCode));
			result.Read(reader);
			return result;
		}

		public static T ReadObjectFactory<T>(global::System.IO.BinaryReader reader, global::System.Func<global::System.IO.BinaryReader, T> readFactory, T existing_instance = null) where T : class, global::Kampai.Util.IBinarySerializable
		{
			if (ReadNullMarker(reader))
			{
				return (T)null;
			}
			T result = readFactory(reader);
			if (existing_instance != null)
			{
				result = existing_instance;
			}
			result.Read(reader);
			return result;
		}

		public static void WriteList<T>(global::System.IO.BinaryWriter writer, global::System.Action<global::System.IO.BinaryWriter, T> elementWriter, global::System.Collections.Generic.IList<T> list)
		{
			if (!WriteNullMarker(writer, list))
			{
				int count = list.Count;
				writer.Write(count);
				for (int i = 0; i < list.Count; i++)
				{
					T arg = list[i];
					elementWriter(writer, arg);
				}
			}
		}

		public static global::System.Collections.Generic.List<T> ReadList<T>(global::System.IO.BinaryReader reader, global::System.Func<global::System.IO.BinaryReader, T> elementReader, global::System.Collections.Generic.IEnumerable<T> existingValue = null)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			int num = reader.ReadInt32();
			global::System.Collections.Generic.List<T> list = ((existingValue == null) ? new global::System.Collections.Generic.List<T>(num) : new global::System.Collections.Generic.List<T>(existingValue));
			for (int i = 0; i < num; i++)
			{
				T item = elementReader(reader);
				list.Add(item);
			}
			return list;
		}

		public static void WriteListInt32(global::System.IO.BinaryWriter writer, global::System.Collections.Generic.IList<int> list)
		{
			if (!WriteNullMarker(writer, list))
			{
				int count = list.Count;
				writer.Write(count);
				for (int i = 0; i < list.Count; i++)
				{
					int value = list[i];
					writer.Write(value);
				}
			}
		}

		public static global::System.Collections.Generic.List<int> ReadListInt32(global::System.IO.BinaryReader reader, global::System.Collections.Generic.IEnumerable<int> existingValue = null)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			int num = reader.ReadInt32();
			global::System.Collections.Generic.List<int> list = ((existingValue == null) ? new global::System.Collections.Generic.List<int>(num) : new global::System.Collections.Generic.List<int>(existingValue));
			for (int i = 0; i < num; i++)
			{
				int item = reader.ReadInt32();
				list.Add(item);
			}
			return list;
		}

		public static void WriteListSingle(global::System.IO.BinaryWriter writer, global::System.Collections.Generic.IList<float> list)
		{
			if (!WriteNullMarker(writer, list))
			{
				int count = list.Count;
				writer.Write(count);
				for (int i = 0; i < list.Count; i++)
				{
					float value = list[i];
					writer.Write(value);
				}
			}
		}

		public static global::System.Collections.Generic.List<float> ReadListSingle(global::System.IO.BinaryReader reader, global::System.Collections.Generic.IEnumerable<float> existingValue = null)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			int num = reader.ReadInt32();
			global::System.Collections.Generic.List<float> list = ((existingValue == null) ? new global::System.Collections.Generic.List<float>(num) : new global::System.Collections.Generic.List<float>(existingValue));
			for (int i = 0; i < num; i++)
			{
				float item = reader.ReadSingle();
				list.Add(item);
			}
			return list;
		}

		public static void WriteList<T>(global::System.IO.BinaryWriter writer, global::System.Collections.Generic.IList<T> list) where T : class, global::Kampai.Util.IBinarySerializable
		{
			if (!WriteNullMarker(writer, list))
			{
				int count = list.Count;
				writer.Write(count);
				for (int i = 0; i < list.Count; i++)
				{
					T instance = list[i];
					WriteObject(writer, instance);
				}
			}
		}

		public static global::System.Collections.Generic.List<T> ReadList<T>(global::System.IO.BinaryReader reader, global::System.Collections.Generic.IEnumerable<T> existingValue = null) where T : class, global::Kampai.Util.IBinarySerializable
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			int num = reader.ReadInt32();
			global::System.Collections.Generic.List<T> list = ((existingValue == null) ? new global::System.Collections.Generic.List<T>(num) : new global::System.Collections.Generic.List<T>(existingValue));
			for (int i = 0; i < num; i++)
			{
				T item = ReadObject(reader, (T)null);
				list.Add(item);
			}
			return list;
		}

		public static void WriteDictionary<K, V>(global::System.IO.BinaryWriter writer, global::System.Action<global::System.IO.BinaryWriter, K> keyWriter, global::System.Action<global::System.IO.BinaryWriter, V> valueWriter, global::System.Collections.Generic.Dictionary<K, V> dictionary)
		{
			if (WriteNullMarker(writer, dictionary))
			{
				return;
			}
			int count = dictionary.Count;
			writer.Write(count);
			foreach (global::System.Collections.Generic.KeyValuePair<K, V> item in dictionary)
			{
				keyWriter(writer, item.Key);
				valueWriter(writer, item.Value);
			}
		}

		public static global::System.Collections.Generic.Dictionary<K, V> ReadDictionary<K, V>(global::System.IO.BinaryReader reader, global::System.Func<global::System.IO.BinaryReader, K> keyReader, global::System.Func<global::System.IO.BinaryReader, V> valueReader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			int num = reader.ReadInt32();
			global::System.Collections.Generic.Dictionary<K, V> dictionary = new global::System.Collections.Generic.Dictionary<K, V>(num);
			for (int i = 0; i < num; i++)
			{
				K key = keyReader(reader);
				V value = valueReader(reader);
				dictionary.Add(key, value);
			}
			return dictionary;
		}

		public static void WriteString(global::System.IO.BinaryWriter writer, string instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				writer.Write(instance);
			}
		}

		public static string ReadString(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			return reader.ReadString();
		}

		public static void WriteObject(global::System.IO.BinaryWriter writer, object instance)
		{
			if (!WriteNullMarker(writer, instance))
			{
				global::System.TypeCode typeCode = global::System.Type.GetTypeCode(instance.GetType());
				writer.Write((int)typeCode);
				switch (typeCode)
				{
				case global::System.TypeCode.Boolean:
					writer.Write((bool)instance);
					break;
				case global::System.TypeCode.Int32:
					writer.Write((int)instance);
					break;
				case global::System.TypeCode.Int64:
					writer.Write((long)instance);
					break;
				case global::System.TypeCode.String:
					writer.Write((string)instance);
					break;
				default:
					throw new global::System.ArgumentException(string.Format("WriteObject: type {0} is not supported, instance {1}", instance.GetType(), instance));
				}
			}
		}

		public static object ReadObject(global::System.IO.BinaryReader reader)
		{
			if (ReadNullMarker(reader))
			{
				return null;
			}
			int num = reader.ReadInt32();
			switch (num)
			{
			case 3:
				return reader.ReadBoolean();
			case 9:
				return reader.ReadInt32();
			case 11:
				return reader.ReadInt64();
			case 18:
				return reader.ReadString();
			default:
				throw new global::System.ArgumentException(string.Format("ReadObject: type code {0} is not supported", num));
			}
		}

		public static void WriteDictionary(global::System.IO.BinaryWriter writer, global::System.Collections.Generic.Dictionary<string, object> dictionary)
		{
			WriteDictionary<string, object>(writer, WriteString, WriteObject, dictionary);
		}

		public static global::System.Collections.Generic.Dictionary<string, object> ReadDictionary(global::System.IO.BinaryReader reader)
		{
			return ReadDictionary<string, object>(reader, ReadString, ReadObject);
		}

		public static void WriteListString(global::System.IO.BinaryWriter writer, global::System.Collections.Generic.IList<string> list)
		{
			WriteList<string>(writer, WriteString, list);
		}

		public static global::System.Collections.Generic.List<string> ReadListString(global::System.IO.BinaryReader reader, global::System.Collections.Generic.IList<string> existingValue)
		{
			return ReadList<string>(reader, ReadString, existingValue);
		}

		public static void WriteEnum<T>(global::System.IO.BinaryWriter writer, T value) where T : struct, global::System.IConvertible
		{
			int value2 = value.ToInt32(global::System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
			writer.Write(value2);
		}

		public static T ReadEnum<T>(global::System.IO.BinaryReader reader) where T : struct, global::System.IConvertible
		{
			int num = reader.ReadInt32();
			return (T)(object)num;
		}
	}
}
