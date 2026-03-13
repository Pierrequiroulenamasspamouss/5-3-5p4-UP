namespace Elevation.Collections.Generic
{
	public class LockFreeQueue<T> where T : class
	{
		private sealed class Node<U>
		{
			public global::Elevation.Collections.Generic.LockFreeQueue<T>.Node<U> Next;

			public U Item;
		}

		private global::Elevation.Collections.Generic.LockFreeQueue<T>.Node<T> _head;

		private global::Elevation.Collections.Generic.LockFreeQueue<T>.Node<T> _tail;

		public LockFreeQueue()
		{
			_head = new global::Elevation.Collections.Generic.LockFreeQueue<T>.Node<T>();
			_tail = _head;
		}

		public void Enqueue(T item)
		{
			global::Elevation.Collections.Generic.LockFreeQueue<T>.Node<T> node = null;
			global::Elevation.Collections.Generic.LockFreeQueue<T>.Node<T> node2 = new global::Elevation.Collections.Generic.LockFreeQueue<T>.Node<T>();
			node2.Item = item;
			bool flag = false;
			while (!flag)
			{
				node = _tail;
				global::Elevation.Collections.Generic.LockFreeQueue<T>.Node<T> next = node.Next;
				if (_tail == node)
				{
					if (next == null)
					{
						flag = CompareExchange(ref _tail.Next, node2, null);
					}
					else
					{
						CompareExchange(ref _tail, next, node);
					}
				}
			}
			CompareExchange(ref _tail, node2, node);
		}

		public bool Dequeue(out T item)
		{
			item = (T)null;
			global::Elevation.Collections.Generic.LockFreeQueue<T>.Node<T> node = null;
			bool flag = false;
			while (!flag)
			{
				node = _head;
				global::Elevation.Collections.Generic.LockFreeQueue<T>.Node<T> next = node.Next;
				global::Elevation.Collections.Generic.LockFreeQueue<T>.Node<T> tail = _tail;
				if (node != _head)
				{
					continue;
				}
				if (node == tail)
				{
					if (next == null)
					{
						return false;
					}
					CompareExchange(ref _tail, next, tail);
				}
				else
				{
					item = next.Item;
					flag = CompareExchange(ref _head, next, node);
				}
			}
			return true;
		}

		public T Dequeue()
		{
			T item;
			Dequeue(out item);
			return item;
		}

		private static bool CompareExchange(ref global::Elevation.Collections.Generic.LockFreeQueue<T>.Node<T> location, global::Elevation.Collections.Generic.LockFreeQueue<T>.Node<T> newValue, global::Elevation.Collections.Generic.LockFreeQueue<T>.Node<T> comparand)
		{
			return comparand == global::System.Threading.Interlocked.CompareExchange(ref location, newValue, comparand);
		}
	}
}
