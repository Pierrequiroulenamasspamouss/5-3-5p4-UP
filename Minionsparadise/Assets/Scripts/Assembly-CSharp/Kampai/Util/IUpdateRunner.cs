namespace Kampai.Util
{
	public interface IUpdateRunner
	{
		void Subscribe(global::System.Action action);

		void Unsubscribe(global::System.Action action);
	}
}
