namespace Kampai.Main
{
	public interface ISupersonicService
	{
		void Init();

		bool IsRewardedVideoAvailable();

		void ShowRewardedVideo(string placement = null);

		void ShowOfferwall();
	}
}
