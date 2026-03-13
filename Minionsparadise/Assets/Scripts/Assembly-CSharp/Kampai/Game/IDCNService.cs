namespace Kampai.Game
{
	public interface IDCNService
	{
		void Perform(global::System.Func<global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest> request, bool isTokenRequest = false);

		void SetToken(global::Kampai.Game.DCNToken token);

		string GetToken();

		bool SetFeaturedContent(int featuredContentId, string htmlUrl);

		int GetFeaturedContentId();

		void OpenFeaturedContent(bool open);

		bool HasFeaturedContent();

		string GetLaunchURL();
	}
}
