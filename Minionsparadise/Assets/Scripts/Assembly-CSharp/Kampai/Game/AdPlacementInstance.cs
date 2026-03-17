namespace Kampai.Game
{
	public class AdPlacementInstance : global::Kampai.Game.Instance<global::Kampai.Game.AdPlacementDefinition>, global::System.IEquatable<global::Kampai.Game.AdPlacementInstance>
	{
		public const int AdRewardLimitPeriodDurationSeconds = 86400;

		public int PlacementInstanceId;

		public int LastRewardPeriodStartTimestamp;

		public int RewardPerPeriodCount;

		public int LastWatchAdAcceptTimestamp;

		public int LastWatchAdDeclineTimestamp;

		[global::Newtonsoft.Json.JsonIgnore]
		public bool ActivityStateOnLastCheck;

		public AdPlacementInstance(global::Kampai.Game.AdPlacementDefinition definition)
			: base(definition)
		{
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "PLACEMENTINSTANCEID":
				reader.Read();
				PlacementInstanceId = global::System.Convert.ToInt32(reader.Value);
				break;
			case "LASTREWARDPERIODSTARTTIMESTAMP":
				reader.Read();
				LastRewardPeriodStartTimestamp = global::System.Convert.ToInt32(reader.Value);
				break;
			case "REWARDPERPERIODCOUNT":
				reader.Read();
				RewardPerPeriodCount = global::System.Convert.ToInt32(reader.Value);
				break;
			case "LASTWATCHADACCEPTTIMESTAMP":
				reader.Read();
				LastWatchAdAcceptTimestamp = global::System.Convert.ToInt32(reader.Value);
				break;
			case "LASTWATCHADDECLINETIMESTAMP":
				reader.Read();
				LastWatchAdDeclineTimestamp = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as global::Kampai.Game.AdPlacementInstance);
		}

		public bool Equals(global::Kampai.Game.AdPlacementInstance p)
		{
			if (p == null)
			{
				return false;
			}
			return ID == p.ID && PlacementInstanceId == p.PlacementInstanceId;
		}

		public override int GetHashCode()
		{
			return ID ^ PlacementInstanceId;
		}

		public bool IsActive(int currentTimeUTC)
		{
			return !IsCooldownActive(currentTimeUTC);
		}

		public bool IsCooldownActive(int currentTimeUTC)
		{
			bool flag = currentTimeUTC - LastWatchAdAcceptTimestamp < base.Definition.CooldownSeconds;
			bool flag2 = currentTimeUTC - LastWatchAdDeclineTimestamp < base.Definition.CooldownWatchDeclineSeconds;
			return flag || flag2 || IsRewardLimitPerPeriodExceeded(currentTimeUTC);
		}

		public int GetCooldownEndTimestamp(int currentTimeUTC)
		{
			int val = LastWatchAdAcceptTimestamp + base.Definition.CooldownSeconds;
			int val2 = LastWatchAdDeclineTimestamp + base.Definition.CooldownWatchDeclineSeconds;
			int num = global::System.Math.Max(val, val2);
			if (IsRewardLimitPerPeriodExceeded(currentTimeUTC))
			{
				int val3 = LastRewardPeriodStartTimestamp + 86400;
				num = global::System.Math.Max(num, val3);
			}
			return num;
		}

		public void ResetCooldown()
		{
			LastRewardPeriodStartTimestamp = 0;
			RewardPerPeriodCount = 0;
			LastWatchAdAcceptTimestamp = 0;
			LastWatchAdDeclineTimestamp = 0;
		}

		public void ActivateWatchAdAcceptCooldown(int currentTimeUTC)
		{
			LastWatchAdAcceptTimestamp = currentTimeUTC;
		}

		public void ActivateWatchAdDeclineCooldown(int currentTimeUTC)
		{
			LastWatchAdDeclineTimestamp = currentTimeUTC;
		}

		public bool IsRewardLimitPerPeriodExceeded(int currentTimeUTC)
		{
			UpdateRewardLimitCounter(currentTimeUTC);
			return RewardPerPeriodCount >= base.Definition.MaxRewardsPerDay;
		}

		public void RegisterReward(int currentTimeUTC)
		{
			if (RewardPerPeriodCount == 0)
			{
				LastRewardPeriodStartTimestamp = currentTimeUTC;
			}
			RewardPerPeriodCount++;
		}

		public void UpdateRewardLimitCounter(int currentTimeUTC)
		{
			if (currentTimeUTC - LastRewardPeriodStartTimestamp > 86400)
			{
				RewardPerPeriodCount = 0;
			}
		}

		public override void Serialize(global::Newtonsoft.Json.JsonWriter writer)
		{
			writer.WriteStartObject();
			SerializeProperties(writer);
			writer.WriteEndObject();
		}

		protected override void SerializeProperties(global::Newtonsoft.Json.JsonWriter writer)
		{
			base.SerializeProperties(writer);
			writer.WritePropertyName("PlacementInstanceId");
			writer.WriteValue(PlacementInstanceId);
			writer.WritePropertyName("LastRewardPeriodStartTimestamp");
			writer.WriteValue(LastRewardPeriodStartTimestamp);
			writer.WritePropertyName("RewardPerPeriodCount");
			writer.WriteValue(RewardPerPeriodCount);
			writer.WritePropertyName("LastWatchAdAcceptTimestamp");
			writer.WriteValue(LastWatchAdAcceptTimestamp);
			writer.WritePropertyName("LastWatchAdDeclineTimestamp");
			writer.WriteValue(LastWatchAdDeclineTimestamp);
		}
	}
}
