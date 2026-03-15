namespace Newtonsoft.Json.Utilities
{
	internal class ListWrapper<T> : global::Newtonsoft.Json.Utilities.CollectionWrapper<T>, global::System.Collections.Generic.IList<T>, global::System.Collections.Generic.ICollection<T>, global::System.Collections.Generic.IEnumerable<T>, global::Newtonsoft.Json.Utilities.IWrappedList, global::System.Collections.IList, global::System.Collections.ICollection, global::System.Collections.IEnumerable
	{
		private readonly global::System.Collections.Generic.IList<T> _genericList;

		public T this[int index]
		{
			get
			{
				if (_genericList != null)
				{
					return _genericList[index];
				}
				return (T)((global::System.Collections.IList)this)[index];
			}
			set
			{
				if (_genericList != null)
				{
					_genericList[index] = value;
				}
				else
				{
					((global::System.Collections.IList)this)[index] = value;
				}
			}
		}

		public override int Count
		{
			get
			{
				if (_genericList != null)
				{
					return _genericList.Count;
				}
				return base.Count;
			}
		}

		public override bool IsReadOnly
		{
			get
			{
				if (_genericList != null)
				{
					return _genericList.IsReadOnly;
				}
				return base.IsReadOnly;
			}
		}

		public object UnderlyingList
		{
			get
			{
				if (_genericList != null)
				{
					return _genericList;
				}
				return base.UnderlyingCollection;
			}
		}

		public ListWrapper(global::System.Collections.IList list)
			: base(list)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(list, "list");
			if (list is global::System.Collections.Generic.IList<T>)
			{
				_genericList = (global::System.Collections.Generic.IList<T>)list;
			}
		}

		public ListWrapper(global::System.Collections.Generic.IList<T> list)
			: base((global::System.Collections.Generic.ICollection<T>)list)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(list, "list");
			_genericList = list;
		}

		public int IndexOf(T item)
		{
			if (_genericList != null)
			{
				return _genericList.IndexOf(item);
			}
			return ((global::System.Collections.IList)this).IndexOf((object)item);
		}

		public void Insert(int index, T item)
		{
			if (_genericList != null)
			{
				_genericList.Insert(index, item);
			}
			else
			{
				((global::System.Collections.IList)this).Insert(index, (object)item);
			}
		}

		public void RemoveAt(int index)
		{
			if (_genericList != null)
			{
				_genericList.RemoveAt(index);
			}
			else
			{
				((global::System.Collections.IList)this).RemoveAt(index);
			}
		}

		public override void Add(T item)
		{
			if (_genericList != null)
			{
				_genericList.Add(item);
			}
			else
			{
				base.Add(item);
			}
		}

		public override void Clear()
		{
			if (_genericList != null)
			{
				_genericList.Clear();
			}
			else
			{
				base.Clear();
			}
		}

		public override bool Contains(T item)
		{
			if (_genericList != null)
			{
				return _genericList.Contains(item);
			}
			return base.Contains(item);
		}

		public override void CopyTo(T[] array, int arrayIndex)
		{
			if (_genericList != null)
			{
				_genericList.CopyTo(array, arrayIndex);
			}
			else
			{
				base.CopyTo(array, arrayIndex);
			}
		}

		public override bool Remove(T item)
		{
			if (_genericList != null)
			{
				return _genericList.Remove(item);
			}
			bool flag = base.Contains(item);
			if (flag)
			{
				base.Remove(item);
			}
			return flag;
		}

		public override global::System.Collections.Generic.IEnumerator<T> GetEnumerator()
		{
			if (_genericList != null)
			{
				return _genericList.GetEnumerator();
			}
			return base.GetEnumerator();
		}
	}
}
