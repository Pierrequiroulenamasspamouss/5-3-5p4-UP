namespace Kampai.Game
{
	public class AdPlacementDefinitionConverter : global::Newtonsoft.Json.Converters.CustomCreationConverter<global::Kampai.Game.AdPlacementDefinition>
	{
		private global::Kampai.Game.RewardedAdType rewardType;

		public override object ReadJson(global::Newtonsoft.Json.JsonReader reader, global::System.Type objectType, object existingValue, global::Newtonsoft.Json.JsonSerializer serializer)
		{
			global::Newtonsoft.Json.Linq.JObject jObject = global::Newtonsoft.Json.Linq.JObject.Load(reader);
			if (jObject.Property("type") != null)
			{
				string value = jObject.Property("type").Value.ToString();
				rewardType = (global::Kampai.Game.RewardedAdType)(int)global::System.Enum.Parse(typeof(global::Kampai.Game.RewardedAdType), value);
			}
			reader = jObject.CreateReader();
			return base.ReadJson(reader, objectType, existingValue, serializer);
		}

		public override global::Kampai.Game.AdPlacementDefinition Create(global::System.Type objectType)
		{
			switch (rewardType)
			{
			case global::Kampai.Game.RewardedAdType.OnTheGlassDailyReward:
				return new global::Kampai.Game.OnTheGlassDailyRewardDefinition();
			case global::Kampai.Game.RewardedAdType.CraftingRushReward:
				return new global::Kampai.Game.CraftingRushRewardDefinition();
			case global::Kampai.Game.RewardedAdType.MarketplaceRefreshRushReward:
				return new global::Kampai.Game.MarketplaceRefreshRushRewardDefinition();
			case global::Kampai.Game.RewardedAdType.MissingResourcesReward:
				return new global::Kampai.Game.MissingResourcesRewardDefinition();
			case global::Kampai.Game.RewardedAdType.Offerwall:
				return new global::Kampai.Game.OfferwallPlacementDefinition();
			case global::Kampai.Game.RewardedAdType.OrderboardFillOrderReward:
				return new global::Kampai.Game.OrderboardFillOrderRewardDefinition();
			case global::Kampai.Game.RewardedAdType.Quest2xReward:
				return new global::Kampai.Game.Quest2xRewardDefinition();
			default:
				return null;
			}
		}
	}
}
