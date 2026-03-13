namespace Kampai.Util
{
	public static class LegalDocuments
	{
		public enum LegalType
		{
			EULA = 0,
			TOS = 1,
			PRIVACY = 2
		}

		public static void TermsOfServiceClicked(global::Kampai.Main.ILocalizationService loc, global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal, global::Kampai.Util.IKampaiLogger logger, global::Kampai.Game.IDefinitionService defService)
		{
			soundFXSignal.Dispatch("Play_button_click_01");
			global::UnityEngine.Application.OpenURL(GetLocalizedURL(loc, global::Kampai.Util.LegalDocuments.LegalType.TOS, logger, defService));
		}

		public static void PrivacyPolicyClicked(global::Kampai.Main.ILocalizationService loc, global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal, global::Kampai.Util.IKampaiLogger logger, global::Kampai.Game.IDefinitionService defService)
		{
			soundFXSignal.Dispatch("Play_button_click_01");
			global::UnityEngine.Application.OpenURL(GetLocalizedURL(loc, global::Kampai.Util.LegalDocuments.LegalType.PRIVACY, logger, defService));
		}

		public static void EulaClicked(global::Kampai.Main.ILocalizationService loc, global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal, global::Kampai.Util.IKampaiLogger logger, global::Kampai.Game.IDefinitionService defService)
		{
			soundFXSignal.Dispatch("Play_button_click_01");
			global::UnityEngine.Application.OpenURL(GetLocalizedURL(loc, global::Kampai.Util.LegalDocuments.LegalType.EULA, logger, defService));
		}

		public static string GetLocalizedURL(global::Kampai.Main.ILocalizationService loc, global::Kampai.Util.LegalDocuments.LegalType legalType, global::Kampai.Util.IKampaiLogger logger, global::Kampai.Game.IDefinitionService defService)
		{
			string text = loc.GetLanguage();
			if (text == "zh")
			{
				text = ((!(loc.GetLanguageKey() == "ZH-CN")) ? "tc" : "sc");
			}
			else if (text == "pt" && loc.GetLanguageKey() == "PT-BR")
			{
				text = "br";
			}
			string text2 = "mobileeula";
			string text3 = "PC";
			switch (legalType)
			{
			case global::Kampai.Util.LegalDocuments.LegalType.EULA:
			{
				text2 = "mobileeula";
				text3 = ((global::UnityEngine.Application.platform != global::UnityEngine.RuntimePlatform.IPhonePlayer) ? "GM" : "OTHER");
				string legalURL = defService.GetLegalURL(global::Kampai.Util.LegalDocuments.LegalType.EULA, text);
				return string.Format(legalURL, text2, text3);
			}
			case global::Kampai.Util.LegalDocuments.LegalType.TOS:
				return defService.GetLegalURL(global::Kampai.Util.LegalDocuments.LegalType.TOS, text);
			case global::Kampai.Util.LegalDocuments.LegalType.PRIVACY:
				return defService.GetLegalURL(global::Kampai.Util.LegalDocuments.LegalType.PRIVACY, text);
			default:
				logger.Error("Supported LegalType must be specified");
				return string.Empty;
			}
		}
	}
}
