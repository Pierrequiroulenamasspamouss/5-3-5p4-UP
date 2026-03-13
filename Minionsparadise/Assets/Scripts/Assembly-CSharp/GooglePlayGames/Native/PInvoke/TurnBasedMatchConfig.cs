namespace GooglePlayGames.Native.PInvoke
{
	internal class TurnBasedMatchConfig : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
	{
		internal TurnBasedMatchConfig(global::System.IntPtr selfPointer)
			: base(selfPointer)
		{
		}

		private string PlayerIdAtIndex(global::System.UIntPtr index)
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr size) => global::GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfig.TurnBasedMatchConfig_PlayerIdsToInvite_GetElement(SelfPtr(), index, out_string, size));
		}

		internal global::System.Collections.Generic.IEnumerator<string> PlayerIdsToInvite()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.ToEnumerator<string>(global::GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfig.TurnBasedMatchConfig_PlayerIdsToInvite_Length(SelfPtr()), (global::System.UIntPtr i) => PlayerIdAtIndex(i));
		}

		internal uint Variant()
		{
			return global::GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfig.TurnBasedMatchConfig_Variant(SelfPtr());
		}

		internal long ExclusiveBitMask()
		{
			return global::GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfig.TurnBasedMatchConfig_ExclusiveBitMask(SelfPtr());
		}

		internal uint MinimumAutomatchingPlayers()
		{
			return global::GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfig.TurnBasedMatchConfig_MinimumAutomatchingPlayers(SelfPtr());
		}

		internal uint MaximumAutomatchingPlayers()
		{
			return global::GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfig.TurnBasedMatchConfig_MaximumAutomatchingPlayers(SelfPtr());
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfig.TurnBasedMatchConfig_Dispose(selfPointer);
		}
	}
}
