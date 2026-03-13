namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeEvent : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder, global::GooglePlayGames.BasicApi.Events.IEvent
	{
		public string Id
		{
			get
			{
				return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.Event.Event_Id(SelfPtr(), out_string, out_size));
			}
		}

		public string Name
		{
			get
			{
				return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.Event.Event_Name(SelfPtr(), out_string, out_size));
			}
		}

		public string Description
		{
			get
			{
				return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.Event.Event_Description(SelfPtr(), out_string, out_size));
			}
		}

		public string ImageUrl
		{
			get
			{
				return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.Event.Event_ImageUrl(SelfPtr(), out_string, out_size));
			}
		}

		public ulong CurrentCount
		{
			get
			{
				return global::GooglePlayGames.Native.Cwrapper.Event.Event_Count(SelfPtr());
			}
		}

		public global::GooglePlayGames.BasicApi.Events.EventVisibility Visibility
		{
			get
			{
				global::GooglePlayGames.Native.Cwrapper.Types.EventVisibility eventVisibility = global::GooglePlayGames.Native.Cwrapper.Event.Event_Visibility(SelfPtr());
				switch (eventVisibility)
				{
				case global::GooglePlayGames.Native.Cwrapper.Types.EventVisibility.HIDDEN:
					return global::GooglePlayGames.BasicApi.Events.EventVisibility.Hidden;
				case global::GooglePlayGames.Native.Cwrapper.Types.EventVisibility.REVEALED:
					return global::GooglePlayGames.BasicApi.Events.EventVisibility.Revealed;
				default:
					throw new global::System.InvalidOperationException("Unknown visibility: " + eventVisibility);
				}
			}
		}

		internal NativeEvent(global::System.IntPtr selfPointer)
			: base(selfPointer)
		{
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.Event.Event_Dispose(selfPointer);
		}

		public override string ToString()
		{
			if (IsDisposed())
			{
				return "[NativeEvent: DELETED]";
			}
			return string.Format("[NativeEvent: Id={0}, Name={1}, Description={2}, ImageUrl={3}, CurrentCount={4}, Visibility={5}]", Id, Name, Description, ImageUrl, CurrentCount, Visibility);
		}
	}
}
