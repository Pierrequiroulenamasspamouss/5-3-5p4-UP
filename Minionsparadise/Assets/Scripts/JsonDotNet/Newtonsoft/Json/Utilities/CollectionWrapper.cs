namespace Newtonsoft.Json.Utilities
{
	internal class CollectionWrapper<T> : global::System.Collections.Generic.ICollection<T>, global::System.Collections.Generic.IEnumerable<T>, global::Newtonsoft.Json.Utilities.IWrappedCollection, global::System.Collections.IList, global::System.Collections.ICollection, global::System.Collections.IEnumerable
	{
		private readonly global::System.Collections.IList _list;

		private readonly global::System.Collections.Generic.ICollection<T> _genericCollection;

		private object _syncRoot;

		public virtual int Count
		{
			get
			{
				if (_genericCollection != null)
				{
					return _genericCollection.Count;
				}
				return _list.Count;
			}
		}

		public virtual bool IsReadOnly
		{
			get
			{
				if (_genericCollection != null)
				{
					return _genericCollection.IsReadOnly;
				}
				return _list.IsReadOnly;
			}
		}

		bool global::System.Collections.IList.IsFixedSize
		{
			get
			{
				if (_genericCollection != null)
				{
					return _genericCollection.IsReadOnly;
				}
				return _list.IsFixedSize;
			}
		}

		object global::System.Collections.IList.this[int index]
		{
			get
			{
				if (_genericCollection != null)
				{
					throw new global::System.Exception("Wrapped ICollection<T> does not support indexer.");
				}
				return _list[index];
			}
			set
			{
				if (_genericCollection != null)
				{
					throw new global::System.Exception("Wrapped ICollection<T> does not support indexer.");
				}
				VerifyValueType(value);
				_list[index] = (T)value;
			}
		}

		bool global::System.Collections.ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		object global::System.Collections.ICollection.SyncRoot
		{
			get
			{
				if (_syncRoot == null)
				{
					global::System.Threading.Interlocked.CompareExchange(ref _syncRoot, new object(), null);
				}
				return _syncRoot;
			}
		}

		public object UnderlyingCollection
		{
			get
			{
				if (_genericCollection != null)
				{
					return _genericCollection;
				}
				return _list;
			}
		}

		public CollectionWrapper(global::System.Collections.IList list)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(list, "list");
			if (list is global::System.Collections.Generic.ICollection<T>)
			{
				_genericCollection = (global::System.Collections.Generic.ICollection<T>)list;
			}
			else
			{
				_list = list;
			}
		}

		public CollectionWrapper(global::System.Collections.Generic.ICollection<T> list)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(list, "list");
			_genericCollection = list;
		}

		public virtual void Add(T item)
		{
			if (_genericCollection != null)
			{
				_genericCollection.Add(item);
			}
			else
			{
				_list.Add(item);
			}
		}

		public virtual void Clear()
		{
			if (_genericCollection != null)
			{
				_genericCollection.Clear();
			}
			else
			{
				_list.Clear();
			}
		}

		public virtual bool Contains(T item)
		{
			if (_genericCollection != null)
			{
				return _genericCollection.Contains(item);
			}
			return _list.Contains(item);
		}

		public virtual void CopyTo(T[] array, int arrayIndex)
		{
			if (_genericCollection != null)
			{
				_genericCollection.CopyTo(array, arrayIndex);
			}
			else
			{
				_list.CopyTo(array, arrayIndex);
			}
		}

		public virtual bool Remove(T item)
		{
			if (_genericCollection != null)
			{
				return _genericCollection.Remove(item);
			}
			bool flag = _list.Contains(item);
			if (flag)
			{
				_list.Remove(item);
			}
			return flag;
		}

		public virtual global::System.Collections.Generic.IEnumerator<T> GetEnumerator()
		{
			if (_genericCollection != null)
			{
				return _genericCollection.GetEnumerator();
			}
			return global::System.Linq.Enumerable.Cast<T>(_list).GetEnumerator();
		}

		global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
		{
			return (_genericCollection != null) ? ((global::System.Collections.IEnumerable)_genericCollection).GetEnumerator() : _list.GetEnumerator();
		}

		int global::System.Collections.IList.Add(object value)
		{
			VerifyValueType(value);
			Add((T)value);
			return Count - 1;
		}

		bool global::System.Collections.IList.Contains(object value)
		{
			if (IsCompatibleObject(value))
			{
				return Contains((T)value);
			}
			return false;
		}

		int global::System.Collections.IList.IndexOf(object value)
		{
			if (_genericCollection != null)
			{
				throw new global::System.Exception("Wrapped ICollection<T> does not support IndexOf.");
			}
			if (IsCompatibleObject(value))
			{
				return _list.IndexOf((T)value);
			}
			return -1;
		}

		void global::System.Collections.IList.RemoveAt(int index)
		{
			if (_genericCollection != null)
			{
				throw new global::System.Exception("Wrapped ICollection<T> does not support RemoveAt.");
			}
			_list.RemoveAt(index);
		}

		void global::System.Collections.IList.Insert(int index, object value)
		{
			if (_genericCollection != null)
			{
				throw new global::System.Exception("Wrapped ICollection<T> does not support Insert.");
			}
			VerifyValueType(value);
			_list.Insert(index, (T)value);
		}

		void global::System.Collections.IList.Remove(object value)
		{
			if (IsCompatibleObject(value))
			{
				Remove((T)value);
			}
		}

		void global::System.Collections.ICollection.CopyTo(global::System.Array array, int arrayIndex)
		{
			CopyTo((T[])array, arrayIndex);
		}

		private static void VerifyValueType(object value)
		{
			if (!IsCompatibleObject(value))
			{
				throw new global::System.ArgumentException("The value '{0}' is not of type '{1}' and cannot be used in this generic collection.".FormatWith(global::System.Globalization.CultureInfo.InvariantCulture, value, typeof(T)), "value");
			}
		}

		private static bool IsCompatibleObject(object value)
		{
			if (!(value is T) && (value != null || (typeof(T).IsValueType && !global::Newtonsoft.Json.Utilities.ReflectionUtils.IsNullableType(typeof(T)))))
			{
				return false;
			}
			return true;
		}
	}
}
