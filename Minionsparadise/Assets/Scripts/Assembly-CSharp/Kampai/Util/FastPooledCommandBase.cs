namespace Kampai.Util
{
	public class FastPooledCommandBase : global::strange.extensions.pool.api.IPoolable, global::Kampai.Util.IFastPooledCommandBase
	{
		public global::Kampai.Util.FastCommandPool commandPool { get; set; }

		public bool retain { get; private set; }

		public void Restore()
		{
		}

		public void Retain()
		{
			retain = true;
		}

		public void Release()
		{
			retain = false;
			if (commandPool != null)
			{
				commandPool.ReturnToPool(this);
			}
		}
	}
}
