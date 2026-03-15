namespace Kampai.Common
{
	public class RandomService : global::Kampai.Common.IRandomService
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("RandomService") as global::Kampai.Util.IKampaiLogger;

		private global::Kampai.Util.JavaRandom random;

		public RandomService(long seed)
		{
			random = new global::Kampai.Util.JavaRandom(seed);
		}

		public float NextFloat()
		{
			return random.nextFloat();
		}

		public float NextFloat(float min, float max)
		{
			if (max >= min)
			{
				return (max - min) * NextFloat() + min;
			}
			return NextFloat(max, min);
		}

		public float NextFloat(float max)
		{
			return NextFloat(0f, max);
		}

		public bool NextBoolean()
		{
			return random.nextBoolean();
		}

		public int NextInt(int exclusiveMax)
		{
			if (exclusiveMax >= 0)
			{
				return random.nextInt(exclusiveMax);
			}
			logger.Log(global::Kampai.Util.KampaiLogLevel.Error, "Illegal argument: {0}", exclusiveMax);
			return 0;
		}

		public int NextInt(int inclusiveMin, int exclusiveMax)
		{
			if (inclusiveMin >= 0 && exclusiveMax >= inclusiveMin)
			{
				return inclusiveMin + NextInt(exclusiveMax - inclusiveMin);
			}
			logger.Log(global::Kampai.Util.KampaiLogLevel.Error, "Illegal arguments: {0}, {1}", inclusiveMin, exclusiveMax);
			return inclusiveMin;
		}

		public long GetSeed()
		{
			return random.getSeed();
		}

		public int NextInt(int exclusiveMax, long seed, int step = 1)
		{
			if (exclusiveMax >= 0)
			{
				global::Kampai.Util.JavaRandom javaRandom = new global::Kampai.Util.JavaRandom(seed);
				javaRandom.advanceSeed(step - 1);
				return javaRandom.nextInt(exclusiveMax);
			}
			logger.Log(global::Kampai.Util.KampaiLogLevel.Error, "Illegal argument: {0}", exclusiveMax);
			return 0;
		}
	}
}
