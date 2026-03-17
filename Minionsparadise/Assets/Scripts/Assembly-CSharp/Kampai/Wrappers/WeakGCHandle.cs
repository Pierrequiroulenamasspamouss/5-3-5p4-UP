namespace Kampai.Wrappers
{
	public class WeakGCHandle : global::Kampai.Wrappers.SafeGCHandle
	{
		public WeakGCHandle(global::System.IntPtr ptr)
		{
			handle = ptr;
		}

		protected override bool ReleaseHandle()
		{
			handle = global::System.IntPtr.Zero;
			return true;
		}
	}
}
