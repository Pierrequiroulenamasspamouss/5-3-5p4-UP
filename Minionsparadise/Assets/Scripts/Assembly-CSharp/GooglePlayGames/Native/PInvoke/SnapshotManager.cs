namespace GooglePlayGames.Native.PInvoke
{
	internal class SnapshotManager
	{
		internal class OpenResponse : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
		{
			internal OpenResponse(global::System.IntPtr selfPointer)
				: base(selfPointer)
			{
			}

			internal bool RequestSucceeded()
			{
				return ResponseStatus() > (global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.SnapshotOpenStatus)0;
			}

			internal global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.SnapshotOpenStatus ResponseStatus()
			{
				return global::GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_OpenResponse_GetStatus(SelfPtr());
			}

			internal string ConflictId()
			{
				if (ResponseStatus() != global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.SnapshotOpenStatus.VALID_WITH_CONFLICT)
				{
					throw new global::System.InvalidOperationException("OpenResponse did not have a conflict");
				}
				return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_OpenResponse_GetConflictId(SelfPtr(), out_string, out_size));
			}

			internal global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadata Data()
			{
				if (ResponseStatus() != global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.SnapshotOpenStatus.VALID)
				{
					throw new global::System.InvalidOperationException("OpenResponse had a conflict");
				}
				return new global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadata(global::GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_OpenResponse_GetData(SelfPtr()));
			}

			internal global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadata ConflictOriginal()
			{
				if (ResponseStatus() != global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.SnapshotOpenStatus.VALID_WITH_CONFLICT)
				{
					throw new global::System.InvalidOperationException("OpenResponse did not have a conflict");
				}
				return new global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadata(global::GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_OpenResponse_GetConflictOriginal(SelfPtr()));
			}

			internal global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadata ConflictUnmerged()
			{
				if (ResponseStatus() != global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.SnapshotOpenStatus.VALID_WITH_CONFLICT)
				{
					throw new global::System.InvalidOperationException("OpenResponse did not have a conflict");
				}
				return new global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadata(global::GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_OpenResponse_GetConflictUnmerged(SelfPtr()));
			}

			protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
			{
				global::GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_OpenResponse_Dispose(selfPointer);
			}

			internal static global::GooglePlayGames.Native.PInvoke.SnapshotManager.OpenResponse FromPointer(global::System.IntPtr pointer)
			{
				if (pointer.Equals(global::System.IntPtr.Zero))
				{
					return null;
				}
				return new global::GooglePlayGames.Native.PInvoke.SnapshotManager.OpenResponse(pointer);
			}
		}

		internal class FetchAllResponse : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
		{
			internal FetchAllResponse(global::System.IntPtr selfPointer)
				: base(selfPointer)
			{
			}

			internal global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus ResponseStatus()
			{
				return global::GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_FetchAllResponse_GetStatus(SelfPtr());
			}

			internal bool RequestSucceeded()
			{
				return ResponseStatus() > (global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus)0;
			}

			internal global::System.Collections.Generic.IEnumerable<global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadata> Data()
			{
				return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.ToEnumerable(global::GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_FetchAllResponse_GetData_Length(SelfPtr()), (global::System.UIntPtr index) => new global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadata(global::GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_FetchAllResponse_GetData_GetElement(SelfPtr(), index)));
			}

			protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
			{
				global::GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_FetchAllResponse_Dispose(selfPointer);
			}

			internal static global::GooglePlayGames.Native.PInvoke.SnapshotManager.FetchAllResponse FromPointer(global::System.IntPtr pointer)
			{
				if (pointer.Equals(global::System.IntPtr.Zero))
				{
					return null;
				}
				return new global::GooglePlayGames.Native.PInvoke.SnapshotManager.FetchAllResponse(pointer);
			}
		}

		internal class CommitResponse : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
		{
			internal CommitResponse(global::System.IntPtr selfPointer)
				: base(selfPointer)
			{
			}

			internal global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus ResponseStatus()
			{
				return global::GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_CommitResponse_GetStatus(SelfPtr());
			}

			internal bool RequestSucceeded()
			{
				return ResponseStatus() > (global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus)0;
			}

			internal global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadata Data()
			{
				if (!RequestSucceeded())
				{
					throw new global::System.InvalidOperationException("Request did not succeed");
				}
				return new global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadata(global::GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_CommitResponse_GetData(SelfPtr()));
			}

			protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
			{
				global::GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_CommitResponse_Dispose(selfPointer);
			}

			internal static global::GooglePlayGames.Native.PInvoke.SnapshotManager.CommitResponse FromPointer(global::System.IntPtr pointer)
			{
				if (pointer.Equals(global::System.IntPtr.Zero))
				{
					return null;
				}
				return new global::GooglePlayGames.Native.PInvoke.SnapshotManager.CommitResponse(pointer);
			}
		}

		internal class ReadResponse : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
		{
			internal ReadResponse(global::System.IntPtr selfPointer)
				: base(selfPointer)
			{
			}

			internal global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus ResponseStatus()
			{
				return global::GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_CommitResponse_GetStatus(SelfPtr());
			}

			internal bool RequestSucceeded()
			{
				return ResponseStatus() > (global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus)0;
			}

			internal byte[] Data()
			{
				if (!RequestSucceeded())
				{
					throw new global::System.InvalidOperationException("Request did not succeed");
				}
				return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToArray((byte[] out_bytes, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_ReadResponse_GetData(SelfPtr(), out_bytes, out_size));
			}

			protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
			{
				global::GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_ReadResponse_Dispose(selfPointer);
			}

			internal static global::GooglePlayGames.Native.PInvoke.SnapshotManager.ReadResponse FromPointer(global::System.IntPtr pointer)
			{
				if (pointer.Equals(global::System.IntPtr.Zero))
				{
					return null;
				}
				return new global::GooglePlayGames.Native.PInvoke.SnapshotManager.ReadResponse(pointer);
			}
		}

		internal class SnapshotSelectUIResponse : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
		{
			internal SnapshotSelectUIResponse(global::System.IntPtr selfPointer)
				: base(selfPointer)
			{
			}

			internal global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus RequestStatus()
			{
				return global::GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_SnapshotSelectUIResponse_GetStatus(SelfPtr());
			}

			internal bool RequestSucceeded()
			{
				return RequestStatus() > (global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus)0;
			}

			internal global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadata Data()
			{
				if (!RequestSucceeded())
				{
					throw new global::System.InvalidOperationException("Request did not succeed");
				}
				return new global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadata(global::GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_SnapshotSelectUIResponse_GetData(SelfPtr()));
			}

			protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
			{
				global::GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_SnapshotSelectUIResponse_Dispose(selfPointer);
			}

			internal static global::GooglePlayGames.Native.PInvoke.SnapshotManager.SnapshotSelectUIResponse FromPointer(global::System.IntPtr pointer)
			{
				if (pointer.Equals(global::System.IntPtr.Zero))
				{
					return null;
				}
				return new global::GooglePlayGames.Native.PInvoke.SnapshotManager.SnapshotSelectUIResponse(pointer);
			}
		}

		private readonly global::GooglePlayGames.Native.PInvoke.GameServices mServices;

		internal SnapshotManager(global::GooglePlayGames.Native.PInvoke.GameServices services)
		{
			mServices = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(services);
		}

		internal void FetchAll(global::GooglePlayGames.Native.Cwrapper.Types.DataSource source, global::System.Action<global::GooglePlayGames.Native.PInvoke.SnapshotManager.FetchAllResponse> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_FetchAll(mServices.AsHandle(), source, InternalFetchAllCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback, global::GooglePlayGames.Native.PInvoke.SnapshotManager.FetchAllResponse.FromPointer));
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.SnapshotManager.FetchAllCallback))]
		internal static void InternalFetchAllCallback(global::System.IntPtr response, global::System.IntPtr data)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("SnapshotManager#FetchAllCallback", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Temporary, response, data);
		}

		internal void SnapshotSelectUI(bool allowCreate, bool allowDelete, uint maxSnapshots, string uiTitle, global::System.Action<global::GooglePlayGames.Native.PInvoke.SnapshotManager.SnapshotSelectUIResponse> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_ShowSelectUIOperation(mServices.AsHandle(), allowCreate, allowDelete, maxSnapshots, uiTitle, InternalSnapshotSelectUICallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback, global::GooglePlayGames.Native.PInvoke.SnapshotManager.SnapshotSelectUIResponse.FromPointer));
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotSelectUICallback))]
		internal static void InternalSnapshotSelectUICallback(global::System.IntPtr response, global::System.IntPtr data)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("SnapshotManager#SnapshotSelectUICallback", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Temporary, response, data);
		}

		internal void Open(string fileName, global::GooglePlayGames.Native.Cwrapper.Types.DataSource source, global::GooglePlayGames.Native.Cwrapper.Types.SnapshotConflictPolicy conflictPolicy, global::System.Action<global::GooglePlayGames.Native.PInvoke.SnapshotManager.OpenResponse> callback)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(fileName);
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(callback);
			global::GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_Open(mServices.AsHandle(), source, fileName, conflictPolicy, InternalOpenCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback, global::GooglePlayGames.Native.PInvoke.SnapshotManager.OpenResponse.FromPointer));
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.SnapshotManager.OpenCallback))]
		internal static void InternalOpenCallback(global::System.IntPtr response, global::System.IntPtr data)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("SnapshotManager#OpenCallback", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Temporary, response, data);
		}

		internal void Commit(global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadata metadata, global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadataChange metadataChange, byte[] updatedData, global::System.Action<global::GooglePlayGames.Native.PInvoke.SnapshotManager.CommitResponse> callback)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(metadata);
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(metadataChange);
			global::GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_Commit(mServices.AsHandle(), metadata.AsPointer(), metadataChange.AsPointer(), updatedData, new global::System.UIntPtr((ulong)updatedData.Length), InternalCommitCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback, global::GooglePlayGames.Native.PInvoke.SnapshotManager.CommitResponse.FromPointer));
		}

		internal void Resolve(global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadata metadata, global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadataChange metadataChange, string conflictId, global::System.Action<global::GooglePlayGames.Native.PInvoke.SnapshotManager.CommitResponse> callback)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(metadata);
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(metadataChange);
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(conflictId);
			global::GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_ResolveConflict(mServices.AsHandle(), metadata.AsPointer(), metadataChange.AsPointer(), conflictId, InternalCommitCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback, global::GooglePlayGames.Native.PInvoke.SnapshotManager.CommitResponse.FromPointer));
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.SnapshotManager.CommitCallback))]
		internal static void InternalCommitCallback(global::System.IntPtr response, global::System.IntPtr data)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("SnapshotManager#CommitCallback", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Temporary, response, data);
		}

		internal void Delete(global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadata metadata)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(metadata);
			global::GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_Delete(mServices.AsHandle(), metadata.AsPointer());
		}

		internal void Read(global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadata metadata, global::System.Action<global::GooglePlayGames.Native.PInvoke.SnapshotManager.ReadResponse> callback)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(metadata);
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(callback);
			global::GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_Read(mServices.AsHandle(), metadata.AsPointer(), InternalReadCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback, global::GooglePlayGames.Native.PInvoke.SnapshotManager.ReadResponse.FromPointer));
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.SnapshotManager.ReadCallback))]
		internal static void InternalReadCallback(global::System.IntPtr response, global::System.IntPtr data)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("SnapshotManager#ReadCallback", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Temporary, response, data);
		}
	}
}
