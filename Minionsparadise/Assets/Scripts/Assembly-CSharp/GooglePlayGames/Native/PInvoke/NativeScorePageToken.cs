namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeScorePageToken : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
	{
		internal NativeScorePageToken(global::System.IntPtr selfPtr)
			: base(selfPtr)
		{
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.ScorePage.ScorePage_ScorePageToken_Dispose(selfPointer);
		}
	}
}
