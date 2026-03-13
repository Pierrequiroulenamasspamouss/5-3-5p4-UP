namespace Kampai.Util
{
	public static class FeatureAccessUtil
	{
		public static bool isAccessible(global::Kampai.Game.AccessControlledFeature feature, global::Kampai.Game.ConfigurationDefinition config, string userId, global::Kampai.Util.IKampaiLogger logger = null)
		{
			bool flag = false;
			global::Kampai.Game.FeatureAccess value;
			if (config.featureAccess != null && config.featureAccess.TryGetValue(feature.ToString(), out value))
			{
				if (value.userIdWhitelist != null)
				{
					flag = value.userIdWhitelist.Contains(userId);
					if (logger != null && flag)
					{
						logger.Log(global::Kampai.Util.KampaiLogLevel.Info, "FeatureAccessUtil: User {0} had been whitelisted for {1}", userId, feature);
					}
				}
				if (!flag && value.accessPercentage > 0)
				{
					int num = global::System.Math.Min(userId.Length, 2);
					string s = userId.Substring(userId.Length - num, num);
					int result = 100;
					if (!int.TryParse(s, out result))
					{
						logger.Log(global::Kampai.Util.KampaiLogLevel.Error, "FeatureAccessUtil: Unable to parse last portion of userId {0}", userId);
					}
					flag = result < value.accessPercentage;
					if (logger != null && flag)
					{
						logger.Log(global::Kampai.Util.KampaiLogLevel.Info, "FeatureAccessUtil: User {0} allowed through throttle for {1}", userId, feature);
					}
				}
			}
			return flag;
		}
	}
}
