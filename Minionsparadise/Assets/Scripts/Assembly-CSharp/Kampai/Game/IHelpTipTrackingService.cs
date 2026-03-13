namespace Kampai.Game
{
	public interface IHelpTipTrackingService
	{
		void TrackHelpTipShown(int tipDefinitionId);

		bool GetHelpTipWasShown(int tipDefinitionId);

		int GetHelpTipShowCount(int tipDefinitionId);

		int GetSecondsSinceHelpTipShown(int tipDefinitionId);
	}
}
