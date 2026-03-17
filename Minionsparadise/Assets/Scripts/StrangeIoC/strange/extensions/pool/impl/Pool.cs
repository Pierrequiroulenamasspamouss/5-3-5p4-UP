namespace strange.extensions.pool.impl
{
	public class Pool : global::strange.extensions.pool.api.IPool, global::strange.framework.api.IManagedList, global::strange.extensions.pool.api.IPoolable
	{
		protected global::System.Collections.Stack instancesAvailable = new global::System.Collections.Stack();

		protected global::System.Collections.Generic.HashSet<object> instancesInUse = new global::System.Collections.Generic.HashSet<object>();

		protected int _instanceCount;

		[Inject]
		public global::strange.framework.api.IInstanceProvider instanceProvider { get; set; }

		public virtual object value
		{
			get
			{
				return GetInstance();
			}
		}

		public virtual bool uniqueValues { get; set; }

		public virtual global::System.Enum constraint { get; set; }

		public global::System.Type poolType { get; set; }

		public int instanceCount
		{
			get
			{
				return _instanceCount;
			}
		}

		public virtual int available
		{
			get
			{
				return instancesAvailable.Count;
			}
		}

		public virtual int size { get; set; }

		public virtual global::strange.extensions.pool.api.PoolOverflowBehavior overflowBehavior { get; set; }

		public virtual global::strange.extensions.pool.api.PoolInflationType inflationType { get; set; }

		public bool retain { get; set; }

		public Pool()
		{
			size = 0;
			constraint = global::strange.framework.api.BindingConstraintType.POOL;
			uniqueValues = true;
			overflowBehavior = global::strange.extensions.pool.api.PoolOverflowBehavior.EXCEPTION;
			inflationType = global::strange.extensions.pool.api.PoolInflationType.DOUBLE;
		}

		public virtual global::strange.framework.api.IManagedList Add(object value)
		{
			failIf(value.GetType() != poolType, "Pool Type mismatch. Pools must consist of a common concrete type.\n\t\tPool type: " + poolType.ToString() + "\n\t\tMismatch type: " + value.GetType().ToString(), global::strange.extensions.pool.api.PoolExceptionType.TYPE_MISMATCH);
			_instanceCount++;
			instancesAvailable.Push(value);
			return this;
		}

		public virtual global::strange.framework.api.IManagedList Add(object[] list)
		{
			foreach (object obj in list)
			{
				Add(obj);
			}
			return this;
		}

		public virtual global::strange.framework.api.IManagedList Remove(object value)
		{
			_instanceCount--;
			removeInstance(value);
			return this;
		}

		public virtual global::strange.framework.api.IManagedList Remove(object[] list)
		{
			foreach (object obj in list)
			{
				Remove(obj);
			}
			return this;
		}

		public virtual object GetInstance()
		{
			if (instancesAvailable.Count > 0)
			{
				object obj = instancesAvailable.Pop();
				instancesInUse.Add(obj);
				return obj;
			}
			int num = 0;
			if (size <= 0)
			{
				num = ((instanceCount == 0 || inflationType == global::strange.extensions.pool.api.PoolInflationType.INCREMENT) ? 1 : instanceCount);
			}
			else
			{
				if (instanceCount != 0)
				{
					failIf(overflowBehavior == global::strange.extensions.pool.api.PoolOverflowBehavior.EXCEPTION, "A pool has overflowed its limit.\n\t\tPool type: " + poolType, global::strange.extensions.pool.api.PoolExceptionType.OVERFLOW);
					if (overflowBehavior == global::strange.extensions.pool.api.PoolOverflowBehavior.WARNING)
					{
						global::System.Console.WriteLine("WARNING: A pool has overflowed its limit.\n\t\tPool type: " + poolType, global::strange.extensions.pool.api.PoolExceptionType.OVERFLOW);
					}
					return null;
				}
				num = size;
			}
			if (num > 0)
			{
				failIf(instanceProvider == null, string.Concat("A Pool of type: ", poolType, " has no instance provider."), global::strange.extensions.pool.api.PoolExceptionType.NO_INSTANCE_PROVIDER);
				for (int i = 0; i < num; i++)
				{
					object instance = instanceProvider.GetInstance(poolType);
					Add(instance);
				}
				return GetInstance();
			}
			return null;
		}

		public virtual void ReturnInstance(object value)
		{
			if (instancesInUse.Contains(value))
			{
				if (value is global::strange.extensions.pool.api.IPoolable)
				{
					(value as global::strange.extensions.pool.api.IPoolable).Restore();
				}
				instancesInUse.Remove(value);
				instancesAvailable.Push(value);
			}
		}

		public virtual void Clean()
		{
			instancesAvailable.Clear();
			instancesInUse = new global::System.Collections.Generic.HashSet<object>();
			_instanceCount = 0;
		}

		public void Restore()
		{
			Clean();
			size = 0;
		}

		public void Retain()
		{
			retain = true;
		}

		public void Release()
		{
			retain = false;
		}

		protected virtual void removeInstance(object value)
		{
			failIf(value.GetType() != poolType, "Attempt to remove a instance from a pool that is of the wrong Type:\n\t\tPool type: " + poolType.ToString() + "\n\t\tInstance type: " + value.GetType().ToString(), global::strange.extensions.pool.api.PoolExceptionType.TYPE_MISMATCH);
			if (instancesInUse.Contains(value))
			{
				instancesInUse.Remove(value);
			}
			else
			{
				instancesAvailable.Pop();
			}
		}

		protected void failIf(bool condition, string message, global::strange.extensions.pool.api.PoolExceptionType type)
		{
			if (condition)
			{
				throw new global::strange.extensions.pool.impl.PoolException(message, type);
			}
		}
	}
	public class Pool<T> : global::strange.extensions.pool.impl.Pool, global::strange.extensions.pool.api.IPool<T>, global::strange.extensions.pool.api.IPool, global::strange.framework.api.IManagedList
	{
		public Pool()
		{
			base.poolType = typeof(T);
		}

		public new T GetInstance()
		{
			return (T)base.GetInstance();
		}
	}
}
