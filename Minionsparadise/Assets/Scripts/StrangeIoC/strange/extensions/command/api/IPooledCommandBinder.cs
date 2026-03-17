namespace strange.extensions.command.api
{
	public interface IPooledCommandBinder
	{
		bool usePooling { get; set; }

		global::strange.extensions.pool.impl.Pool<T> GetPool<T>();
	}
}
