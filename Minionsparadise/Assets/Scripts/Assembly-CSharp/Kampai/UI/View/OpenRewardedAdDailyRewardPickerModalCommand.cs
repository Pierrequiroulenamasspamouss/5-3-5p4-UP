namespace Kampai.UI.View
{
	public class OpenRewardedAdDailyRewardPickerModalCommand : global::strange.extensions.command.impl.Command
	{
		private const int TIER_COUNT = 3;

		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("OpenRewardedAdDailyRewardPickerModalCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.OnTheGlassDailyRewardDefinition onTheGlassDailyRewardDefinition { get; set; }

		[Inject]
		public global::Kampai.Game.AdPlacementInstance adPlacementInstance { get; set; }

		[Inject]
		public global::Kampai.Common.IRandomService randomService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseAllOtherMenuSignal closeAllOtherMenuSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal sfxSignal { get; set; }

		public override void Execute()
		{
			closeAllOtherMenuSignal.Dispatch(null);
			global::Kampai.Util.QuantityItem[] array = PickRewardVariants(onTheGlassDailyRewardDefinition);
			if (array != null)
			{
				LoadRewardPickerUI(array);
				sfxSignal.Dispatch("Play_menu_popUp_01");
			}
		}

		public global::Kampai.Util.QuantityItem[] PickRewardVariants(global::Kampai.Game.OnTheGlassDailyRewardDefinition definition)
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.IngredientsItemDefinition> list = GenerateCategory("Craftable");
			if (list.Count <= 0)
			{
				logger.Error("No items are unlocked yet, so there should be no daily reward avaiable.");
				return null;
			}
			global::Kampai.Util.QuantityItem[] array = new global::Kampai.Util.QuantityItem[3];
			global::Kampai.Game.OnTheGlassDailyRewardTier onTheGlassDailyRewardTier = PickTier(definition);
			array[0] = GetRewardOfTier(onTheGlassDailyRewardTier, list);
			global::Kampai.Game.OnTheGlassDailyRewardTier[] array2 = new global::Kampai.Game.OnTheGlassDailyRewardTier[3]
			{
				definition.RewardTiers.Tier1,
				definition.RewardTiers.Tier2,
				definition.RewardTiers.Tier3
			};
			int num = 1;
			global::Kampai.Game.OnTheGlassDailyRewardTier[] array3 = array2;
			foreach (global::Kampai.Game.OnTheGlassDailyRewardTier onTheGlassDailyRewardTier2 in array3)
			{
				if (!object.ReferenceEquals(onTheGlassDailyRewardTier2, onTheGlassDailyRewardTier))
				{
					array[num] = GetRewardOfTier(onTheGlassDailyRewardTier2, list);
					num++;
				}
			}
			if (randomService.NextBoolean())
			{
				global::Kampai.Util.QuantityItem quantityItem = array[1];
				array[1] = array[2];
				array[2] = quantityItem;
			}
			return array;
		}

		private global::Kampai.Game.OnTheGlassDailyRewardTier PickTier(global::Kampai.Game.OnTheGlassDailyRewardDefinition definition)
		{
			global::Kampai.Game.RewardTiers rewardTiers = definition.RewardTiers;
			int num = rewardTiers.Tier1.Weight + rewardTiers.Tier2.Weight + rewardTiers.Tier3.Weight;
			int num2 = randomService.NextInt(0, num);
			global::Kampai.Game.OnTheGlassDailyRewardTier result = rewardTiers.Tier1;
			if (num2 < rewardTiers.Tier1.Weight)
			{
				result = rewardTiers.Tier1;
			}
			else if (num2 < rewardTiers.Tier1.Weight + rewardTiers.Tier2.Weight)
			{
				result = rewardTiers.Tier2;
			}
			else if (num2 < num)
			{
				result = rewardTiers.Tier3;
			}
			return result;
		}

		private global::Kampai.Util.QuantityItem GetRewardOfTier(global::Kampai.Game.OnTheGlassDailyRewardTier tier, global::System.Collections.Generic.IList<global::Kampai.Game.IngredientsItemDefinition> unlockedCraftables)
		{
			global::Kampai.Game.Transaction.WeightedDefinition weightedDefinition = new global::Kampai.Game.Transaction.WeightedDefinition();
			global::System.Collections.Generic.List<global::Kampai.Game.Transaction.WeightedQuantityItem> list = (global::System.Collections.Generic.List<global::Kampai.Game.Transaction.WeightedQuantityItem>)(weightedDefinition.Entities = new global::System.Collections.Generic.List<global::Kampai.Game.Transaction.WeightedQuantityItem>());
			foreach (global::Kampai.Game.Transaction.WeightedQuantityItem entity in tier.PredefinedRewards.Entities)
			{
				list.Add(entity);
			}
			int craftableRewardMinTier = tier.CraftableRewardMinTier;
			global::System.Collections.Generic.List<global::Kampai.Game.IngredientsItemDefinition> list3 = new global::System.Collections.Generic.List<global::Kampai.Game.IngredientsItemDefinition>();
			while (craftableRewardMinTier >= 0 && list3.Count == 0)
			{
				list3.AddRange(FilterByMinTier(unlockedCraftables, craftableRewardMinTier));
			}
			foreach (global::Kampai.Game.IngredientsItemDefinition item2 in list3)
			{
				uint quantity = (uint)randomService.NextInt(1, tier.CraftableRewardMaxQuantity + 1);
				global::Kampai.Game.Transaction.WeightedQuantityItem weightedQuantityItem = new global::Kampai.Game.Transaction.WeightedQuantityItem();
				weightedQuantityItem.ID = item2.ID;
				weightedQuantityItem.Quantity = quantity;
				weightedQuantityItem.Weight = (uint)tier.CraftableRewardWeight;
				global::Kampai.Game.Transaction.WeightedQuantityItem item = weightedQuantityItem;
				list.Add(item);
			}
			if (weightedDefinition.Entities.Count > 0)
			{
				global::Kampai.Game.Transaction.WeightedInstance weightedInstance = new global::Kampai.Game.Transaction.WeightedInstance(weightedDefinition);
				return weightedInstance.NextPick(randomService);
			}
			return null;
		}

		private global::System.Collections.Generic.IList<global::Kampai.Game.IngredientsItemDefinition> GenerateCategory(string category)
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.IngredientsItemDefinition> list = new global::System.Collections.Generic.List<global::Kampai.Game.IngredientsItemDefinition>();
			global::System.Collections.Generic.IList<global::Kampai.Game.IngredientsItemDefinition> unlockedDefsByType = playerService.GetUnlockedDefsByType<global::Kampai.Game.IngredientsItemDefinition>();
			for (int i = 0; i < unlockedDefsByType.Count; i++)
			{
				if (category.Equals(unlockedDefsByType[i].TaxonomySpecific))
				{
					list.Add(unlockedDefsByType[i]);
				}
			}
			return list;
		}

		private global::System.Collections.Generic.IList<global::Kampai.Game.IngredientsItemDefinition> FilterByMinTier(global::System.Collections.Generic.IList<global::Kampai.Game.IngredientsItemDefinition> items, int minTier)
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.IngredientsItemDefinition> list = new global::System.Collections.Generic.List<global::Kampai.Game.IngredientsItemDefinition>();
			for (int i = 0; i < items.Count; i++)
			{
				if (items[i].Tier >= minTier)
				{
					list.Add(items[i]);
				}
			}
			return items;
		}

		private void LoadRewardPickerUI(global::Kampai.Util.QuantityItem[] rewards)
		{
			string text = "popup_RewardedAdPickDailyReward";
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Queue, text);
			iGUICommand.skrimScreen = "RewardedAdPickReward";
			iGUICommand.disableSkrimButton = true;
			iGUICommand.darkSkrim = true;
			global::Kampai.UI.View.GUIArguments args = iGUICommand.Args;
			args.Add(text);
			args.Add(typeof(global::Kampai.Util.QuantityItem[]), rewards);
			args.Add(typeof(global::Kampai.Game.AdPlacementInstance), adPlacementInstance);
			guiService.Execute(iGUICommand);
		}
	}
}
