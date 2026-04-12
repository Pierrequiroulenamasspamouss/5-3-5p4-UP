namespace Kampai.Util
{
	public abstract class FastPooledCommand : global::Kampai.Util.FastPooledCommandBase
	{
		public abstract void Execute();
	}
	public abstract class FastPooledCommand<T> : global::Kampai.Util.FastPooledCommandBase, global::strange.extensions.pool.api.IPoolable, global::Kampai.Util.IFastPooledCommand<T>, global::Kampai.Util.IFastPooledCommandBase
	{
		public abstract void Execute(T arg1);
	}
	public abstract class FastPooledCommand<T1, T2> : global::Kampai.Util.FastPooledCommandBase, global::strange.extensions.pool.api.IPoolable, global::Kampai.Util.IFastPooledCommand<T1, T2>, global::Kampai.Util.IFastPooledCommandBase
	{
		public abstract void Execute(T1 arg1, T2 arg2);
	}
	public abstract class FastPooledCommand<T1, T2, T3> : global::Kampai.Util.FastPooledCommandBase, global::strange.extensions.pool.api.IPoolable, global::Kampai.Util.IFastPooledCommand<T1, T2, T3>, global::Kampai.Util.IFastPooledCommandBase
	{
		public abstract void Execute(T1 arg1, T2 arg2, T3 arg3);
	}
	public abstract class FastPooledCommand<T1, T2, T3, T4> : global::Kampai.Util.FastPooledCommandBase, global::strange.extensions.pool.api.IPoolable, global::Kampai.Util.IFastPooledCommand<T1, T2, T3, T4>, global::Kampai.Util.IFastPooledCommandBase
	{
		public abstract void Execute(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
	}
}
