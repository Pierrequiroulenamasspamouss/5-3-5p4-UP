namespace Kampai.Util
{
	public class KDNode<T> where T : global::Kampai.Util.SpatiallySortable
	{
		private sealed class CompareByXAxis : global::System.Collections.Generic.IComparer<T>
		{
			public int Compare(T a, T b)
			{
				return a.Position.x.CompareTo(b.Position.x);
			}
		}

		private sealed class CompareByZAxis : global::System.Collections.Generic.IComparer<T>
		{
			public int Compare(T a, T b)
			{
				return a.Position.z.CompareTo(b.Position.z);
			}
		}

		internal T data;

		internal global::Kampai.Util.KDNode<T> leftChild;

		internal global::Kampai.Util.KDNode<T> rightChild;

		private static global::Kampai.Util.KDNode<T>.CompareByXAxis compareByXAxis = new global::Kampai.Util.KDNode<T>.CompareByXAxis();

		private static global::Kampai.Util.KDNode<T>.CompareByZAxis compareByZAxis = new global::Kampai.Util.KDNode<T>.CompareByZAxis();

		public KDNode()
		{
			data = default(T);
			leftChild = null;
			rightChild = null;
		}

		internal void Build(global::Kampai.Util.KDTreeNodesPool<T> pool, global::System.Collections.Generic.List<T> objectList, int first, int count, int depth)
		{
			data = default(T);
			leftChild = (rightChild = null);
			if (objectList == null)
			{
				return;
			}
			switch (count)
			{
			case 0:
				return;
			case 1:
				data = objectList[first];
				return;
			}
			if (depth % 2 == 0)
			{
				objectList.Sort(first, count, compareByXAxis);
			}
			else
			{
				objectList.Sort(first, count, compareByZAxis);
			}
			int num = count / 2;
			data = objectList[first + num];
			if (num > 0)
			{
				leftChild = pool.GetFreeNode();
				leftChild.Build(pool, objectList, first, num, depth + 1);
			}
			int num2 = count - (num + 1);
			if (num2 > 0)
			{
				rightChild = pool.GetFreeNode();
				rightChild.Build(pool, objectList, first + num + 1, num2, depth + 1);
			}
		}

		public override string ToString()
		{
			return string.Format("[{0}, {1}, {2}]", (data == null) ? "null" : data.Position.ToString(), (leftChild == null) ? "null" : leftChild.ToString(), (rightChild == null) ? "null" : rightChild.ToString());
		}
	}
}
