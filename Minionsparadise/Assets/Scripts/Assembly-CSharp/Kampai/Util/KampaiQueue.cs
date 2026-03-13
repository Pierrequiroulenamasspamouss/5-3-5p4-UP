namespace Kampai.Util
{
	public class KampaiQueue<T> : global::System.Collections.IEnumerable, global::System.Collections.Generic.IEnumerable<T>
	{
		protected global::System.Collections.Generic.LinkedList<T> queue;

		public int Count
		{
			get
			{
				return queue.Count;
			}
		}

		public KampaiQueue()
		{
			queue = new global::System.Collections.Generic.LinkedList<T>();
		}

		public KampaiQueue(global::System.Collections.Generic.IEnumerable<T> enumerable)
		{
			queue = new global::System.Collections.Generic.LinkedList<T>();
			foreach (T item in enumerable)
			{
				Enqueue(item);
			}
		}

		global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public global::System.Collections.Generic.IEnumerator<T> GetEnumerator()
		{
			return queue.GetEnumerator();
		}

		public T Peek()
		{
			return queue.First.Value;
		}

		public T Dequeue()
		{
			T value = queue.First.Value;
			queue.RemoveFirst();
			return value;
		}

		public void AddFirst(T item)
		{
			queue.AddFirst(item);
		}

		public void Enqueue(T item)
		{
			queue.AddLast(item);
		}

		public bool Remove(T item)
		{
			return queue.Remove(item);
		}

		public void Clear()
		{
			queue.Clear();
		}
	}
}
