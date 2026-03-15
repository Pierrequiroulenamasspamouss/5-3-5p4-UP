namespace strange.extensions.pool.api
{
	public interface IPoolable
	{
		bool retain { get; }

		void Restore();

		void Retain();

		void Release();
	}
}
