namespace Kampai.Util
{
	public class ScatterList<T>
	{
		private bool dirty = true;

		private int size;

		private int index;

		private T[] items;

		private global::System.Collections.Generic.List<T> insertions;

		public int MaxSize { get; private set; }

		public ScatterList(int maxSize)
		{
			if (maxSize <= 0)
			{
				throw new global::System.ArgumentOutOfRangeException();
			}
			MaxSize = maxSize;
		}

		public void Add(T item)
		{
			dirty = true;
			if (insertions == null)
			{
				insertions = new global::System.Collections.Generic.List<T>();
			}
			insertions.Add(item);
		}

		public T Pick(global::Kampai.Common.IRandomService randomService)
		{
			if (dirty)
			{
				Dirty(randomService);
			}
			if (size > 0)
			{
				if (index >= size)
				{
					global::Kampai.Util.ListUtil.Shuffle(randomService, items);
					index = 0;
				}
				return items[index++];
			}
			return default(T);
		}

		public void ShuffleContents(global::Kampai.Common.IRandomService randomService)
		{
			global::Kampai.Util.ListUtil.Shuffle(randomService, items);
			index = 0;
		}

		private void Dirty(global::Kampai.Common.IRandomService randomService)
		{
			if (insertions != null && insertions.Count > 0)
			{
				global::Kampai.Util.ListUtil.Shuffle(randomService, insertions);
				size = global::System.Math.Min(MaxSize, insertions.Count);
				index = 0;
				items = new T[size];
				for (int i = 0; i < size; i++)
				{
					items[i] = insertions[i];
				}
				insertions = null;
				dirty = false;
			}
		}

		public void Clear()
		{
			if (insertions != null)
			{
				insertions.Clear();
			}
		}
	}
}
