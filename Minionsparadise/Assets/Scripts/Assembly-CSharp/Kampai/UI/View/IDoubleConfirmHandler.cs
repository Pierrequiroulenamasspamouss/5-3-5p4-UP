namespace Kampai.UI.View
{
	public interface IDoubleConfirmHandler
	{
		void ShowConfirmMessage();

		bool isDoubleConfirmed();

		void ResetTapState();

		void updateTapCount();
	}
}
