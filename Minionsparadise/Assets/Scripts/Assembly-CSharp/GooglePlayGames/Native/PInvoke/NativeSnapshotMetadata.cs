namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeSnapshotMetadata : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder, global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata
	{
		public bool IsOpen
		{
			get
			{
				return global::GooglePlayGames.Native.Cwrapper.SnapshotMetadata.SnapshotMetadata_IsOpen(SelfPtr());
			}
		}

		public string Filename
		{
			get
			{
				return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.SnapshotMetadata.SnapshotMetadata_FileName(SelfPtr(), out_string, out_size));
			}
		}

		public string Description
		{
			get
			{
				return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.SnapshotMetadata.SnapshotMetadata_Description(SelfPtr(), out_string, out_size));
			}
		}

		public string CoverImageURL
		{
			get
			{
				return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.SnapshotMetadata.SnapshotMetadata_CoverImageURL(SelfPtr(), out_string, out_size));
			}
		}

		public global::System.TimeSpan TotalTimePlayed
		{
			get
			{
				long num = global::GooglePlayGames.Native.Cwrapper.SnapshotMetadata.SnapshotMetadata_PlayedTime(SelfPtr());
				if (num < 0)
				{
					return global::System.TimeSpan.FromMilliseconds(0.0);
				}
				return global::System.TimeSpan.FromMilliseconds(num);
			}
		}

		public global::System.DateTime LastModifiedTimestamp
		{
			get
			{
				return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.FromMillisSinceUnixEpoch(global::GooglePlayGames.Native.Cwrapper.SnapshotMetadata.SnapshotMetadata_LastModifiedTime(SelfPtr()));
			}
		}

		internal NativeSnapshotMetadata(global::System.IntPtr selfPointer)
			: base(selfPointer)
		{
		}

		public override string ToString()
		{
			if (IsDisposed())
			{
				return "[NativeSnapshotMetadata: DELETED]";
			}
			return string.Format("[NativeSnapshotMetadata: IsOpen={0}, Filename={1}, Description={2}, CoverImageUrl={3}, TotalTimePlayed={4}, LastModifiedTimestamp={5}]", IsOpen, Filename, Description, CoverImageURL, TotalTimePlayed, LastModifiedTimestamp);
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.SnapshotMetadata.SnapshotMetadata_Dispose(SelfPtr());
		}
	}
}
