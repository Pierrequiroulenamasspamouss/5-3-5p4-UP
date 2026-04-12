namespace Kampai.Util
{
	internal sealed class KDTreeNodesPool<T> where T : global::Kampai.Util.SpatiallySortable
	{
		private global::System.Collections.Generic.List<global::Kampai.Util.KDNode<T>> pool;

		private int currentIndex;

		public KDTreeNodesPool(int initialCapacity)
		{
			pool = new global::System.Collections.Generic.List<global::Kampai.Util.KDNode<T>>(initialCapacity);
			currentIndex = -1;
		}

		internal void Reset()
		{
			global::System.Collections.Generic.List<global::Kampai.Util.KDNode<T>>.Enumerator enumerator = pool.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					global::Kampai.Util.KDNode<T> current = enumerator.Current;
					current.data = default(T);
					current.leftChild = null;
					current.rightChild = null;
				}
			}
			finally
			{
				enumerator.Dispose();
			}
			currentIndex = -1;
		}

		internal global::Kampai.Util.KDNode<T> GetFreeNode()
		{
			currentIndex++;
			if (currentIndex < pool.Count)
			{
				return pool[currentIndex];
			}
			global::Kampai.Util.KDNode<T> kDNode = new global::Kampai.Util.KDNode<T>();
			pool.Add(kDNode);
			return kDNode;
		}
	}
}
