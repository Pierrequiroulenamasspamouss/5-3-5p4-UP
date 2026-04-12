namespace GooglePlayGames.Native.PInvoke
{
	internal class PlayerSelectUIResponse : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder, global::System.Collections.IEnumerable, global::System.Collections.Generic.IEnumerable<string>
	{
		internal PlayerSelectUIResponse(global::System.IntPtr selfPointer)
			: base(selfPointer)
		{
		}

		global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		internal global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus Status()
		{
			return global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetStatus(SelfPtr());
		}

		private string PlayerIdAtIndex(global::System.UIntPtr index)
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr size) => global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetPlayerIds_GetElement(SelfPtr(), index, out_string, size));
		}

		public global::System.Collections.Generic.IEnumerator<string> GetEnumerator()
		{
			global::System.UIntPtr length = global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetPlayerIds_Length(SelfPtr());
			global::System.Func<global::System.UIntPtr, string> getElement = (global::System.UIntPtr i) => PlayerIdAtIndex(i);
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.ToEnumerator<string>(length, getElement);
		}

		internal uint MinimumAutomatchingPlayers()
		{
			return global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetMinimumAutomatchingPlayers(SelfPtr());
		}

		internal uint MaximumAutomatchingPlayers()
		{
			return global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetMaximumAutomatchingPlayers(SelfPtr());
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_PlayerSelectUIResponse_Dispose(selfPointer);
		}

		internal static global::GooglePlayGames.Native.PInvoke.PlayerSelectUIResponse FromPointer(global::System.IntPtr pointer)
		{
			if (global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.IsNull(pointer))
			{
				return null;
			}
			return new global::GooglePlayGames.Native.PInvoke.PlayerSelectUIResponse(pointer);
		}
	}
}
