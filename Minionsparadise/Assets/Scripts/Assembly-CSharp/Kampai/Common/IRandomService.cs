namespace Kampai.Common
{
	public interface IRandomService
	{
		float NextFloat();

		float NextFloat(float min, float max);

		float NextFloat(float max);

		bool NextBoolean();

		int NextInt(int exclusiveMax);

		int NextInt(int exclusiveMax, long seed, int step = 1);

		int NextInt(int inclusiveMin, int exclusiveMax);

		long GetSeed();
	}
}
