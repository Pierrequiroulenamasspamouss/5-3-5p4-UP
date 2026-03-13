namespace Kampai.Util
{
	public interface IFastPooledCommandBase : global::strange.extensions.pool.api.IPoolable
	{
		global::Kampai.Util.FastCommandPool commandPool { get; set; }
	}
}
