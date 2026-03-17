namespace GooglePlayGames.Native.PInvoke
{
	internal abstract class BaseReferenceHolder : global::System.IDisposable
	{
		private static global::System.Collections.Generic.Dictionary<global::System.Runtime.InteropServices.HandleRef, global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder> _refs = new global::System.Collections.Generic.Dictionary<global::System.Runtime.InteropServices.HandleRef, global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder>();

		private global::System.Runtime.InteropServices.HandleRef mSelfPointer;

		public BaseReferenceHolder(global::System.IntPtr pointer)
		{
			mSelfPointer = global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.CheckNonNull(new global::System.Runtime.InteropServices.HandleRef(this, pointer));
		}

		protected bool IsDisposed()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.IsNull(mSelfPointer);
		}

		protected global::System.Runtime.InteropServices.HandleRef SelfPtr()
		{
			if (IsDisposed())
			{
				throw new global::System.InvalidOperationException("Attempted to use object after it was cleaned up");
			}
			return mSelfPointer;
		}

		protected abstract void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer);

		~BaseReferenceHolder()
		{
			Dispose(true);
		}

		public void Dispose()
		{
			Dispose(false);
			global::System.GC.SuppressFinalize(this);
		}

		internal global::System.IntPtr AsPointer()
		{
			return SelfPtr().Handle;
		}

		private void Dispose(bool fromFinalizer)
		{
			if ((fromFinalizer || !_refs.ContainsKey(mSelfPointer)) && !global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.IsNull(mSelfPointer))
			{
				CallDispose(mSelfPointer);
				mSelfPointer = new global::System.Runtime.InteropServices.HandleRef(this, global::System.IntPtr.Zero);
			}
		}

		internal void ReferToMe()
		{
			_refs[SelfPtr()] = this;
		}

		internal void ForgetMe()
		{
			if (_refs.ContainsKey(SelfPtr()))
			{
				_refs.Remove(SelfPtr());
				Dispose(false);
			}
		}
	}
}
