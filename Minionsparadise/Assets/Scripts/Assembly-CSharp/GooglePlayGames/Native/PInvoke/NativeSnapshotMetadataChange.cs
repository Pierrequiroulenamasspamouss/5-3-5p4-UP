namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeSnapshotMetadataChange : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
	{
		internal class Builder : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
		{
			internal Builder()
				: base(global::GooglePlayGames.Native.Cwrapper.SnapshotMetadataChangeBuilder.SnapshotMetadataChange_Builder_Construct())
			{
			}

			protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
			{
				global::GooglePlayGames.Native.Cwrapper.SnapshotMetadataChangeBuilder.SnapshotMetadataChange_Builder_Dispose(selfPointer);
			}

			internal global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadataChange.Builder SetDescription(string description)
			{
				global::GooglePlayGames.Native.Cwrapper.SnapshotMetadataChangeBuilder.SnapshotMetadataChange_Builder_SetDescription(SelfPtr(), description);
				return this;
			}

			internal global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadataChange.Builder SetPlayedTime(ulong playedTime)
			{
				global::GooglePlayGames.Native.Cwrapper.SnapshotMetadataChangeBuilder.SnapshotMetadataChange_Builder_SetPlayedTime(SelfPtr(), playedTime);
				return this;
			}

			internal global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadataChange.Builder SetCoverImageFromPngData(byte[] pngData)
			{
				global::GooglePlayGames.OurUtils.Misc.CheckNotNull(pngData);
				global::GooglePlayGames.Native.Cwrapper.SnapshotMetadataChangeBuilder.SnapshotMetadataChange_Builder_SetCoverImageFromPngData(SelfPtr(), pngData, new global::System.UIntPtr((ulong)pngData.LongLength));
				return this;
			}

			internal global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadataChange Build()
			{
				return FromPointer(global::GooglePlayGames.Native.Cwrapper.SnapshotMetadataChangeBuilder.SnapshotMetadataChange_Builder_Create(SelfPtr()));
			}
		}

		internal NativeSnapshotMetadataChange(global::System.IntPtr selfPointer)
			: base(selfPointer)
		{
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.SnapshotMetadataChange.SnapshotMetadataChange_Dispose(selfPointer);
		}

		internal static global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadataChange FromPointer(global::System.IntPtr pointer)
		{
			if (pointer.Equals(global::System.IntPtr.Zero))
			{
				return null;
			}
			return new global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadataChange(pointer);
		}
	}
}
