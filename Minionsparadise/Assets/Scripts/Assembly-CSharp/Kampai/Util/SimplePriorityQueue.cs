namespace Kampai.Util
{
	public class SimplePriorityQueue<T>
	{
		private global::System.Collections.Generic.SortedDictionary<int, global::System.Collections.Generic.Queue<T>> heap;

		private int count;

		public int Count
		{
			get
			{
				return count;
			}
		}

		public SimplePriorityQueue()
		{
			heap = new global::System.Collections.Generic.SortedDictionary<int, global::System.Collections.Generic.Queue<T>>();
			count = 0;
		}

		public T Dequeue()
		{
			if (count <= 0)
			{
				return default(T);
			}
			T result = default(T);
			foreach (global::System.Collections.Generic.Queue<T> value in heap.Values)
			{
				if (value.Count > 0)
				{
					count--;
					result = value.Dequeue();
					break;
				}
			}
			return result;
		}

		public void Enqueue(T item, int priority)
		{
			if (!heap.ContainsKey(priority))
			{
				heap.Add(priority, new global::System.Collections.Generic.Queue<T>());
			}
			heap[priority].Enqueue(item);
			count++;
		}

		public T Peek()
		{
			if (count <= 0)
			{
				throw new global::System.InvalidOperationException();
			}
			T result = default(T);
			foreach (global::System.Collections.Generic.Queue<T> value in heap.Values)
			{
				if (value.Count > 0)
				{
					result = value.Peek();
					break;
				}
			}
			return result;
		}

		public void Clear()
		{
			count = 0;
			heap.Clear();
		}
	}
}
