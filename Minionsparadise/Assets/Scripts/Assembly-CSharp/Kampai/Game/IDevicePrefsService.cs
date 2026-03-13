namespace Kampai.Game
{
	public interface IDevicePrefsService
	{
		global::Kampai.Game.DevicePrefs GetDevicePrefs();

		void Deserialize(string serialized);

		string Serialize();
	}
}
