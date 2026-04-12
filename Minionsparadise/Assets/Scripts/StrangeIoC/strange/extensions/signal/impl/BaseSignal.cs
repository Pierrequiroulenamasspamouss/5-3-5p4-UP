namespace strange.extensions.signal.impl
{
	public class BaseSignal : global::strange.extensions.signal.api.IBaseSignal
	{
		private global::System.Action<global::strange.extensions.signal.api.IBaseSignal, object[]> BaseListener = delegate
		{
		};

		private int BaseListenerSubsrcibersCount;

		protected static global::System.Collections.Generic.List<global::System.Type> emptyTypesList = new global::System.Collections.Generic.List<global::System.Type>();

		public virtual bool NonEmpty
		{
			get
			{
				if (this.OnceBaseListener == null)
				{
					return BaseListenerSubsrcibersCount > 0;
				}
				return true;
			}
		}

		private event global::System.Action<global::strange.extensions.signal.api.IBaseSignal, object[]> OnceBaseListener;

		public virtual void Dispatch(object[] args)
		{
			BaseListener(this, args);
			if (this.OnceBaseListener != null)
			{
				global::System.Action<global::strange.extensions.signal.api.IBaseSignal, object[]> onceBaseListener = this.OnceBaseListener;
				this.OnceBaseListener = null;
				onceBaseListener(this, args);
			}
		}

		public virtual global::System.Collections.Generic.List<global::System.Type> GetTypes()
		{
			return emptyTypesList;
		}

		public virtual void AddListener(global::System.Action<global::strange.extensions.signal.api.IBaseSignal, object[]> callback)
		{
			if (!global::System.Linq.Enumerable.Contains(BaseListener.GetInvocationList(), callback))
			{
				BaseListener = (global::System.Action<global::strange.extensions.signal.api.IBaseSignal, object[]>)global::System.Delegate.Combine(BaseListener, callback);
				BaseListenerSubsrcibersCount++;
			}
		}

		public virtual void AddOnce(global::System.Action<global::strange.extensions.signal.api.IBaseSignal, object[]> callback)
		{
			if (this.OnceBaseListener == null)
			{
				this.OnceBaseListener = delegate
				{
				};
			}
			else if (global::System.Linq.Enumerable.Contains(this.OnceBaseListener.GetInvocationList(), callback))
			{
				return;
			}
			OnceBaseListener += callback;
		}

		public virtual void RemoveListener(global::System.Action<global::strange.extensions.signal.api.IBaseSignal, object[]> callback)
		{
			global::System.Action<global::strange.extensions.signal.api.IBaseSignal, object[]> baseListener = BaseListener;
			BaseListener = (global::System.Action<global::strange.extensions.signal.api.IBaseSignal, object[]>)global::System.Delegate.Remove(BaseListener, callback);
			if (object.ReferenceEquals(BaseListener, baseListener))
			{
				BaseListenerSubsrcibersCount--;
			}
		}
	}
}
