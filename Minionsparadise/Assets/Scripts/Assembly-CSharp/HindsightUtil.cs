public static class HindsightUtil
{
	public static string GetUri(global::Kampai.Main.HindsightCampaignDefinition definition, string languageKey)
	{
		if (definition.URI.ContainsKey(languageKey))
		{
			return definition.URI[languageKey].ToString();
		}
		if (definition.URI.ContainsKey("default"))
		{
			return definition.URI["default"].ToString();
		}
		return string.Empty;
	}

	public static string GetContentUri(global::Kampai.Main.HindsightCampaignDefinition definition, string languageKey)
	{
		if (definition.Content.ContainsKey(languageKey))
		{
			return definition.Content[languageKey].ToString();
		}
		if (definition.Content.ContainsKey("default"))
		{
			return definition.Content["default"].ToString();
		}
		return string.Empty;
	}

	public static string GetContentCachePath(global::Kampai.Main.HindsightCampaignDefinition definition, string languageKey)
	{
		string contentUri = GetContentUri(definition, languageKey);
		if (string.IsNullOrEmpty(contentUri))
		{
			return string.Empty;
		}
		string extension = global::System.IO.Path.GetExtension(contentUri);
		if (string.IsNullOrEmpty(extension) || !extension.Contains("."))
		{
			return string.Empty;
		}
		return string.Format("{0}{1}_{2}{3}", global::Kampai.Util.GameConstants.IMAGE_PATH, definition.ID, languageKey, extension);
	}

	public static global::Kampai.Main.HindsightCampaign.Scope GetScope(global::Kampai.Main.HindsightCampaignDefinition definition)
	{
		return (global::Kampai.Main.HindsightCampaign.Scope)(int)global::System.Enum.Parse(typeof(global::Kampai.Main.HindsightCampaign.Scope), definition.Scope);
	}

	public static bool ValidPlatform(global::Kampai.Main.HindsightCampaignDefinition definition)
	{
		global::Kampai.Main.HindsightCampaign.Platform platform = (global::Kampai.Main.HindsightCampaign.Platform)(int)global::System.Enum.Parse(typeof(global::Kampai.Main.HindsightCampaign.Platform), definition.Platform);
		if (platform == global::Kampai.Main.HindsightCampaign.Platform.all)
		{
			return true;
		}
		return platform == global::Kampai.Main.HindsightCampaign.Platform.android;
	}
}
