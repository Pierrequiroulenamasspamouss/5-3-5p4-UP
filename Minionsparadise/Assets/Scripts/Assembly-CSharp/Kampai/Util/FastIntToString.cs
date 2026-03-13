namespace Kampai.Util
{
	public sealed class FastIntToString
	{
		private char[] array = new char[64];

		private static int ToArrayInternal(uint value, char[] buffer, int bufferIndex)
		{
			if (value == 0)
			{
				buffer[bufferIndex] = '0';
				return 1;
			}
			int num = 1;
			for (uint num2 = value / 10; num2 != 0; num2 /= 10)
			{
				num++;
			}
			for (int num3 = num - 1; num3 >= 0; num3--)
			{
				buffer[bufferIndex + num3] = (char)(48 + value % 10);
				value /= 10;
			}
			return num;
		}

		private static int ToArrayInternal(ulong value, char[] buffer, int bufferIndex)
		{
			if (value == 0L)
			{
				buffer[bufferIndex] = '0';
				return 1;
			}
			int num = 1;
			for (ulong num2 = value / 10; num2 != 0; num2 /= 10)
			{
				num++;
			}
			for (int num3 = num - 1; num3 >= 0; num3--)
			{
				buffer[bufferIndex + num3] = (char)(48 + value % 10);
				value /= 10;
			}
			return num;
		}

		public void ToCharArray(uint value, out char[] buffer, out int len)
		{
			buffer = array;
			len = ToArrayInternal(value, array, 0);
		}

		public void ToCharArray(int value, out char[] buffer, out int len)
		{
			buffer = array;
			if (value >= 0)
			{
				len = ToArrayInternal((uint)value, array, 0);
				return;
			}
			array[0] = '-';
			len = 1 + ToArrayInternal((uint)(-value), array, 1);
		}

		public void ToCharArray(ulong value, out char[] buffer, out int len)
		{
			buffer = array;
			len = ToArrayInternal(value, array, 0);
		}

		public void ToCharArray(long value, out char[] buffer, out int len)
		{
			buffer = array;
			if (value >= 0)
			{
				len = ToArrayInternal((ulong)value, array, 0);
				return;
			}
			array[0] = '-';
			len = 1 + ToArrayInternal((ulong)(-value), array, 1);
		}

		public string ToString(uint value)
		{
			char[] buffer;
			int len;
			ToCharArray(value, out buffer, out len);
			return new string(buffer, 0, len);
		}

		public string ToString(int value)
		{
			char[] buffer;
			int len;
			ToCharArray(value, out buffer, out len);
			return new string(buffer, 0, len);
		}

		public string ToString(ulong value)
		{
			char[] buffer;
			int len;
			ToCharArray(value, out buffer, out len);
			return new string(buffer, 0, len);
		}

		public string ToString(long value)
		{
			char[] buffer;
			int len;
			ToCharArray(value, out buffer, out len);
			return new string(buffer, 0, len);
		}
	}
}
