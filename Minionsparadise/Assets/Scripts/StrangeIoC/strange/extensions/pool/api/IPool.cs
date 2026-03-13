namespace strange.extensions.pool.api
{
	public interface IPool : global::strange.framework.api.IManagedList
	{
		global::strange.framework.api.IInstanceProvider instanceProvider { get; set; }

		global::System.Type poolType { get; set; }

		int available { get; }

		int size { get; set; }

		int instanceCount { get; }

		global::strange.extensions.pool.api.PoolOverflowBehavior overflowBehavior { get; set; }

		global::strange.extensions.pool.api.PoolInflationType inflationType { get; set; }

		object GetInstance();

		void ReturnInstance(object value);

		void Clean();
	}
	public interface IPool<T> : global::strange.extensions.pool.api.IPool, global::strange.framework.api.IManagedList
	{
		new T GetInstance();
	}
}
