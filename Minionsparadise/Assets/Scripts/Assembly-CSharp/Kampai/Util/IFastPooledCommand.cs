namespace Kampai.Util
{
	public interface IFastPooledCommand : global::strange.extensions.pool.api.IPoolable, global::Kampai.Util.IFastPooledCommandBase
	{
		void Execute();
	}
	public interface IFastPooledCommand<T> : global::strange.extensions.pool.api.IPoolable, global::Kampai.Util.IFastPooledCommandBase
	{
		void Execute(T arg1);
	}
	public interface IFastPooledCommand<T1, T2> : global::strange.extensions.pool.api.IPoolable, global::Kampai.Util.IFastPooledCommandBase
	{
		void Execute(T1 arg1, T2 arg2);
	}
	public interface IFastPooledCommand<T1, T2, T3> : global::strange.extensions.pool.api.IPoolable, global::Kampai.Util.IFastPooledCommandBase
	{
		void Execute(T1 arg1, T2 arg2, T3 arg3);
	}
	public interface IFastPooledCommand<T1, T2, T3, T4> : global::strange.extensions.pool.api.IPoolable, global::Kampai.Util.IFastPooledCommandBase
	{
		void Execute(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
	}
}
