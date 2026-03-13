namespace Kampai.Game
{
	public interface IPushNotificationService
	{
		void Start(string userAlias, int birthdateYear, int birthdateMonth);
	}
}
