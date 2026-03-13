namespace Kampai.Util
{
	public interface IFastCommand
	{
		void Execute();
	}
	public interface IFastCommand<T>
	{
		void Execute(T arg1);
	}
	public interface IFastCommand<T1, T2>
	{
		void Execute(T1 arg1, T2 arg2);
	}
	public interface IFastCommand<T1, T2, T3>
	{
		void Execute(T1 arg1, T2 arg2, T3 arg3);
	}
	public interface IFastCommand<T1, T2, T3, T4>
	{
		void Execute(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
	}
}
