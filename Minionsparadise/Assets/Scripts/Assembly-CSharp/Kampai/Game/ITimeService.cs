namespace Kampai.Game
{
	public interface ITimeService
	{
		int CurrentTime();

		int Uptime();

		int AppTime();

		bool WithinRange(int a, int b);

		float RealtimeSinceStartup();

		bool WithinRange(global::Kampai.Util.IUTCRangeable rangeable, bool eternal = false);
	}
}
