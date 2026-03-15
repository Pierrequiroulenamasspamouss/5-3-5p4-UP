namespace Kampai.Game
{
	public interface ISocialService
	{
		string userID { get; }

		string userName { get; }

		bool isLoggedIn { get; }

		string accessToken { get; }

		global::System.DateTime tokenExpiry { get; }

		global::Kampai.Game.SocialServices type { get; }

		string LoginSource { get; set; }

		bool isKillSwitchEnabled { get; }

		string locKey { get; }

		void Init(global::strange.extensions.signal.impl.Signal<global::Kampai.Game.ISocialService> successSignal, global::strange.extensions.signal.impl.Signal<global::Kampai.Game.ISocialService> failureSignal);

		void Login(global::strange.extensions.signal.impl.Signal<global::Kampai.Game.ISocialService> successSignal, global::strange.extensions.signal.impl.Signal<global::Kampai.Game.ISocialService> failureSignal, global::System.Action callback);

		void Logout();

		void SendLoginTelemetry(string loginLocation);

		void updateKillSwitchFlag();

		void incrementAchievement(string achievementID, float percentComplete);

		void ShowAchievements();
	}
}
