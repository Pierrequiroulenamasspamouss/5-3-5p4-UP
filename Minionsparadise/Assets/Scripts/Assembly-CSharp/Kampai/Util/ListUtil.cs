namespace Kampai.Util
{
	public static class ListUtil
	{
		public static void Shuffle<T>(global::Kampai.Common.IRandomService randomService, global::System.Collections.Generic.IList<T> list)
		{
			if (list != null && randomService != null)
			{
				int num = list.Count;
				while (num > 1)
				{
					num--;
					int index = randomService.NextInt(num + 1);
					T value = list[index];
					list[index] = list[num];
					list[num] = value;
				}
			}
		}

		public static void Shuffle<T>(global::Kampai.Common.IRandomService randomService, T[] array)
		{
			if (array != null && randomService != null)
			{
				int num = array.Length;
				while (num > 1)
				{
					num--;
					int num2 = randomService.NextInt(num + 1);
					T val = array[num2];
					array[num2] = array[num];
					array[num] = val;
				}
			}
		}

		public static void RandomSublist<T>(global::Kampai.Common.IRandomService randomService, global::System.Collections.Generic.ICollection<T> source, global::System.Collections.Generic.ICollection<T> result, int picks)
		{
			int count = source.Count;
			if (randomService != null && source != null && picks >= 1 && picks <= count)
			{
				T[] array = new T[count];
				source.CopyTo(array, 0);
				Shuffle(randomService, array);
				for (int i = 0; i < picks; i++)
				{
					result.Add(array[i]);
				}
			}
		}

		public static bool Contains<T>(T[] array, T target)
		{
			foreach (T val in array)
			{
				if (target.Equals(val))
				{
					return true;
				}
			}
			return false;
		}

		public static string ListToString(global::System.Collections.IEnumerable list)
		{
			string text = "{";
			int num = 0;
			foreach (object item in list)
			{
				if (num > 0)
				{
					text += ", ";
				}
				text += item.ToString();
				num++;
			}
			return text + "}";
		}

		public static bool StringIsInList(string test, global::System.Collections.Generic.IEnumerable<string> list)
		{
			if (list != null && !string.IsNullOrEmpty(test))
			{
				test = test.ToLower();
				foreach (string item in list)
				{
					if (!string.IsNullOrEmpty(item) && test.Equals(item.ToLower()))
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
