namespace Kampai.Wrappers
{
	public class SafeGCHandle : global::System.Runtime.InteropServices.SafeHandle
	{
		public override bool IsInvalid
		{
			get
			{
				return handle == global::System.IntPtr.Zero;
			}
		}

		public object Target
		{
			get
			{
				return global::System.Runtime.InteropServices.GCHandle.FromIntPtr(handle).Target;
			}
		}

		public SafeGCHandle()
			: base(global::System.IntPtr.Zero, true)
		{
			handle = global::System.IntPtr.Zero;
		}

		public SafeGCHandle(object o)
			: base(global::System.IntPtr.Zero, true)
		{
			global::System.Runtime.InteropServices.GCHandle value = global::System.Runtime.InteropServices.GCHandle.Alloc(o);
			handle = global::System.Runtime.InteropServices.GCHandle.ToIntPtr(value);
		}

		protected override bool ReleaseHandle()
		{
			global::System.Runtime.InteropServices.GCHandle gCHandle = global::System.Runtime.InteropServices.GCHandle.FromIntPtr(handle);
			handle = global::System.IntPtr.Zero;
			gCHandle.Free();
			return true;
		}
	}
}
