namespace Kampai.Game
{
	public interface IConfigurationsService
	{
		global::Kampai.Game.ConfigurationDefinition GetConfigurations();

		void GetConfigurationCallback(global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse response);

		string GetConfigURL();

		void setInitonCallback(bool init);

		bool isKillSwitchOn(global::Kampai.Game.KillSwitch killswitchType);

		string GetConfigVariant();

		string GetDefinitionVariants();

		void OverrideKillswitch(global::Kampai.Game.KillSwitch killswitchType, bool killswitchValue);

		void ClearKillswitchOverride(global::Kampai.Game.KillSwitch killswitchType);

		void ClearAllKillswitchOverrides();
	}
}
