namespace Kampai.Common
{
	public interface IVideoService
	{
		void playVideo(string urlOrFilename, bool showControls, bool closeOnTouch);

		void playIntro(bool showControls, bool closeOnTouch, global::System.Action videoPlayingCallback = null, string videoUriTemplate = null);

		bool IsIntroCached(string videoUriTemplate = null);
	}
}
