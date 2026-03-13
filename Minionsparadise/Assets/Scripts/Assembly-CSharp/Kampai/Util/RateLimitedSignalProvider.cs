namespace Kampai.Util
{
	public interface RateLimitedSignalProvider
	{
		float MinimumGap { get; }

		float CurrentTime { get; }
	}
}
