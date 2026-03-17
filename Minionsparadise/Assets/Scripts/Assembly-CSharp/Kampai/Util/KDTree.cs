namespace Kampai.Util
{
	public class KDTree<T> : global::Kampai.Util.KDNode<T> where T : global::Kampai.Util.SpatiallySortable
	{
		private global::System.Collections.Generic.List<T> objectList;

		private global::System.Collections.Generic.List<T> foundObjects;

		public int Count { get; private set; }

		public KDTree(global::System.Collections.Generic.List<T> objectList)
		{
			Count = objectList.Count;
			foundObjects = new global::System.Collections.Generic.List<T>(Count);
			this.objectList = objectList;
		}

		public void Rebuild(global::System.Collections.Generic.List<T> objectList)
		{
			Count = objectList.Count;
			this.objectList = objectList;
		}

		public global::System.Collections.Generic.List<T> WithinRange(global::UnityEngine.Vector3 center, float size)
		{
			foundObjects.Clear();
			global::Kampai.Util.AABox2D aABox2D = new global::Kampai.Util.AABox2D(center.x, center.z, size, size);
			for (int i = 0; i < objectList.Count; i++)
			{
				T item = objectList[i];
				if (aABox2D.Contains(new global::UnityEngine.Vector2(item.Position.x, item.Position.z)))
				{
					foundObjects.Add(item);
				}
			}
			return foundObjects;
		}

		private void WithinRangeRecursive(global::Kampai.Util.AABox2D box, global::Kampai.Util.KDNode<T> node, int depth, global::Kampai.Util.AABox2D space, ref global::System.Collections.Generic.List<T> found)
		{
			if (node != null && box.Overlaps(space))
			{
				global::UnityEngine.Vector3 position = node.data.Position;
				if (box.Contains(new global::UnityEngine.Vector2(position.x, position.z)))
				{
					found.Add(node.data);
				}
				int num = depth % 2;
				float v = position.x;
				if (num == 1)
				{
					v = position.z;
				}
				global::Kampai.Util.AABox2D left;
				global::Kampai.Util.AABox2D right;
				space.Split(v, num, out left, out right);
				WithinRangeRecursive(box, node.leftChild, depth + 1, left, ref found);
				WithinRangeRecursive(box, node.rightChild, depth + 1, right, ref found);
			}
		}
	}
}
