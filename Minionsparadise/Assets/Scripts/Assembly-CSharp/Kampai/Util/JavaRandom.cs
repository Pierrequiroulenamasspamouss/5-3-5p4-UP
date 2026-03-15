namespace Kampai.Util
{
	public class JavaRandom
	{
		private const long multiplier = 25214903917L;

		private object locker = new object();

		private bool haveNextNextGaussian;

		private long baseSeed;

		private long seed;

		private double nextNextGaussian;

		public JavaRandom(long seed)
		{
			setSeed(seed);
		}

		protected int next(int bits)
		{
			lock (locker)
			{
				advanceSeed();
				return (int)shiftRightUnsignet(seed, 48 - bits);
			}
		}

		public void advanceSeed(int iterations = 1)
		{
			while (iterations-- > 0)
			{
				seed = (seed * 25214903917L + 11) & 0xFFFFFFFFFFFFL;
			}
		}

		protected long shiftRightUnsignet(long value, int shift)
		{
			return (long)((ulong)value >> shift);
		}

		public bool nextBoolean()
		{
			return next(1) != 0;
		}

		public void nextBytes(byte[] buf)
		{
			if (buf == null)
			{
				return;
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			while (num2 < buf.Length)
			{
				if (num3 == 0)
				{
					num = nextInt();
					num3 = 3;
				}
				else
				{
					num3--;
				}
				buf[num2++] = (byte)num;
				num >>= 8;
			}
		}

		public double nextDouble()
		{
			return (double)(((long)next(26) << 27) + next(27)) / 9007199254740992.0;
		}

		public float nextFloat()
		{
			return (float)next(24) / 16777216f;
		}

		public double nextGaussian()
		{
			lock (locker)
			{
				if (haveNextNextGaussian)
				{
					haveNextNextGaussian = false;
					return nextNextGaussian;
				}
				double num;
				double num2;
				double num3;
				do
				{
					num = 2.0 * nextDouble() - 1.0;
					num2 = 2.0 * nextDouble() - 1.0;
					num3 = num * num + num2 * num2;
				}
				while (num3 >= 1.0);
				double num4 = global::System.Math.Sqrt(-2.0 * global::System.Math.Log(num3) / num3);
				nextNextGaussian = num2 * num4;
				haveNextNextGaussian = true;
				return num * num4;
			}
		}

		public int nextInt()
		{
			return next(32);
		}

		public int nextInt(int n)
		{
			if (n > 0)
			{
				if ((n & -n) == n)
				{
					return (int)((long)n * (long)next(31) >> 31);
				}
				int num;
				int num2;
				do
				{
					num = next(31);
					num2 = num % n;
				}
				while (num - num2 + (n - 1) < 0);
				return num2;
			}
			return 0;
		}

		public long nextLong()
		{
			return ((long)next(32) << 32) + next(32);
		}

		public long getSeed()
		{
			return baseSeed;
		}

		public void setSeed(long seed)
		{
			lock (locker)
			{
				baseSeed = seed;
				this.seed = (seed ^ 0x5DEECE66DL) & 0xFFFFFFFFFFFFL;
				haveNextNextGaussian = false;
			}
		}
	}
}
