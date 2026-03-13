namespace Kampai.UI
{
	public interface IPopupController
	{
		bool isOpened { get; }

		void Open();

		void Close(bool instant = false);
	}
}
