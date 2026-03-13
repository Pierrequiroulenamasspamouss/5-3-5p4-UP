namespace Kampai.Game
{
	public interface IUserSessionService
	{
		global::Kampai.Game.UserSession UserSession { get; set; }

		void LoginRequestCallback(global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse response);

		void RegisterRequestCallback(global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse response);

		void UserUpdateRequestCallback(string synergyID, global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse response);

		void OpenURL(string url);

		void setLoginCallback(global::strange.extensions.signal.impl.Signal a);
	}
}
