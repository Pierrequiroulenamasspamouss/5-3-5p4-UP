namespace Kampai.Util
{
	public interface IClientVersion
	{
		string GetClientVersion();

		string GetClientPlatform();

		string GetClientDeviceType();

		void RemoveOverrideVersion();
	}
}
