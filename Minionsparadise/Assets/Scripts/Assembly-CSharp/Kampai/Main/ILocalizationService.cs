namespace Kampai.Main
{
	public interface ILocalizationService
	{
		global::System.Globalization.CultureInfo CultureInfo { get; }

		void Initialize(string langCode);

		bool IsInitialized();

		void Update();

		string GetLanguage();

		string GetCountry();

		bool IsLanguageSupported();

		string GetString(string key, params object[] args);

		string GetStringUpper(string key, params object[] args);

		string GetStringLower(string key, params object[] args);

		string StringToUpper(string str);

		string StringToLower(string str);

		string GetLanguageKey();

		bool Contains(string key);

		void RetrieveCultureInfo(global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse response);

		void SetCultureInfo(string cultureInfoStr);
	}
}
