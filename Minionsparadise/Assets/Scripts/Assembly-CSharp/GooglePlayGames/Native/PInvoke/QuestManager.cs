namespace GooglePlayGames.Native.PInvoke
{
	internal class QuestManager
	{
		internal class FetchResponse : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
		{
			internal FetchResponse(global::System.IntPtr selfPointer)
				: base(selfPointer)
			{
			}

			internal global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus ResponseStatus()
			{
				return global::GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_FetchResponse_GetStatus(SelfPtr());
			}

			internal global::GooglePlayGames.Native.PInvoke.NativeQuest Data()
			{
				if (!RequestSucceeded())
				{
					return null;
				}
				return new global::GooglePlayGames.Native.PInvoke.NativeQuest(global::GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_FetchResponse_GetData(SelfPtr()));
			}

			internal bool RequestSucceeded()
			{
				return ResponseStatus() > (global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus)0;
			}

			protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
			{
				global::GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_FetchResponse_Dispose(selfPointer);
			}

			internal static global::GooglePlayGames.Native.PInvoke.QuestManager.FetchResponse FromPointer(global::System.IntPtr pointer)
			{
				if (pointer.Equals(global::System.IntPtr.Zero))
				{
					return null;
				}
				return new global::GooglePlayGames.Native.PInvoke.QuestManager.FetchResponse(pointer);
			}
		}

		internal class FetchListResponse : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
		{
			internal FetchListResponse(global::System.IntPtr selfPointer)
				: base(selfPointer)
			{
			}

			internal global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus ResponseStatus()
			{
				return global::GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_FetchListResponse_GetStatus(SelfPtr());
			}

			internal bool RequestSucceeded()
			{
				return ResponseStatus() > (global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus)0;
			}

			internal global::System.Collections.Generic.IEnumerable<global::GooglePlayGames.Native.PInvoke.NativeQuest> Data()
			{
				return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.ToEnumerable(global::GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_FetchListResponse_GetData_Length(SelfPtr()), (global::System.UIntPtr index) => new global::GooglePlayGames.Native.PInvoke.NativeQuest(global::GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_FetchListResponse_GetData_GetElement(SelfPtr(), index)));
			}

			protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
			{
				global::GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_FetchListResponse_Dispose(selfPointer);
			}

			internal static global::GooglePlayGames.Native.PInvoke.QuestManager.FetchListResponse FromPointer(global::System.IntPtr pointer)
			{
				if (pointer.Equals(global::System.IntPtr.Zero))
				{
					return null;
				}
				return new global::GooglePlayGames.Native.PInvoke.QuestManager.FetchListResponse(pointer);
			}
		}

		internal class ClaimMilestoneResponse : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
		{
			internal ClaimMilestoneResponse(global::System.IntPtr selfPointer)
				: base(selfPointer)
			{
			}

			internal global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.QuestClaimMilestoneStatus ResponseStatus()
			{
				return global::GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_ClaimMilestoneResponse_GetStatus(SelfPtr());
			}

			internal global::GooglePlayGames.Native.PInvoke.NativeQuest Quest()
			{
				if (!RequestSucceeded())
				{
					return null;
				}
				global::GooglePlayGames.Native.PInvoke.NativeQuest nativeQuest = new global::GooglePlayGames.Native.PInvoke.NativeQuest(global::GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_ClaimMilestoneResponse_GetQuest(SelfPtr()));
				if (nativeQuest.Valid())
				{
					return nativeQuest;
				}
				nativeQuest.Dispose();
				return null;
			}

			internal global::GooglePlayGames.Native.PInvoke.NativeQuestMilestone ClaimedMilestone()
			{
				if (!RequestSucceeded())
				{
					return null;
				}
				global::GooglePlayGames.Native.PInvoke.NativeQuestMilestone nativeQuestMilestone = new global::GooglePlayGames.Native.PInvoke.NativeQuestMilestone(global::GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_ClaimMilestoneResponse_GetClaimedMilestone(SelfPtr()));
				if (nativeQuestMilestone.Valid())
				{
					return nativeQuestMilestone;
				}
				nativeQuestMilestone.Dispose();
				return null;
			}

			internal bool RequestSucceeded()
			{
				return ResponseStatus() > (global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.QuestClaimMilestoneStatus)0;
			}

			protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
			{
				global::GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_ClaimMilestoneResponse_Dispose(selfPointer);
			}

			internal static global::GooglePlayGames.Native.PInvoke.QuestManager.ClaimMilestoneResponse FromPointer(global::System.IntPtr pointer)
			{
				if (pointer.Equals(global::System.IntPtr.Zero))
				{
					return null;
				}
				return new global::GooglePlayGames.Native.PInvoke.QuestManager.ClaimMilestoneResponse(pointer);
			}
		}

		internal class AcceptResponse : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
		{
			internal AcceptResponse(global::System.IntPtr selfPointer)
				: base(selfPointer)
			{
			}

			internal global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.QuestAcceptStatus ResponseStatus()
			{
				return global::GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_AcceptResponse_GetStatus(SelfPtr());
			}

			internal global::GooglePlayGames.Native.PInvoke.NativeQuest AcceptedQuest()
			{
				if (!RequestSucceeded())
				{
					return null;
				}
				return new global::GooglePlayGames.Native.PInvoke.NativeQuest(global::GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_AcceptResponse_GetAcceptedQuest(SelfPtr()));
			}

			internal bool RequestSucceeded()
			{
				return ResponseStatus() > (global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.QuestAcceptStatus)0;
			}

			protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
			{
				global::GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_AcceptResponse_Dispose(selfPointer);
			}

			internal static global::GooglePlayGames.Native.PInvoke.QuestManager.AcceptResponse FromPointer(global::System.IntPtr pointer)
			{
				if (pointer.Equals(global::System.IntPtr.Zero))
				{
					return null;
				}
				return new global::GooglePlayGames.Native.PInvoke.QuestManager.AcceptResponse(pointer);
			}
		}

		internal class QuestUIResponse : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
		{
			internal QuestUIResponse(global::System.IntPtr selfPointer)
				: base(selfPointer)
			{
			}

			internal global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus RequestStatus()
			{
				return global::GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_QuestUIResponse_GetStatus(SelfPtr());
			}

			internal bool RequestSucceeded()
			{
				return RequestStatus() > (global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus)0;
			}

			internal global::GooglePlayGames.Native.PInvoke.NativeQuest AcceptedQuest()
			{
				if (!RequestSucceeded())
				{
					return null;
				}
				global::GooglePlayGames.Native.PInvoke.NativeQuest nativeQuest = new global::GooglePlayGames.Native.PInvoke.NativeQuest(global::GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_QuestUIResponse_GetAcceptedQuest(SelfPtr()));
				if (nativeQuest.Valid())
				{
					return nativeQuest;
				}
				nativeQuest.Dispose();
				return null;
			}

			internal global::GooglePlayGames.Native.PInvoke.NativeQuestMilestone MilestoneToClaim()
			{
				if (!RequestSucceeded())
				{
					return null;
				}
				global::GooglePlayGames.Native.PInvoke.NativeQuestMilestone nativeQuestMilestone = new global::GooglePlayGames.Native.PInvoke.NativeQuestMilestone(global::GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_QuestUIResponse_GetMilestoneToClaim(SelfPtr()));
				if (nativeQuestMilestone.Valid())
				{
					return nativeQuestMilestone;
				}
				nativeQuestMilestone.Dispose();
				return null;
			}

			protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
			{
				global::GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_QuestUIResponse_Dispose(selfPointer);
			}

			internal static global::GooglePlayGames.Native.PInvoke.QuestManager.QuestUIResponse FromPointer(global::System.IntPtr pointer)
			{
				if (pointer.Equals(global::System.IntPtr.Zero))
				{
					return null;
				}
				return new global::GooglePlayGames.Native.PInvoke.QuestManager.QuestUIResponse(pointer);
			}
		}

		private readonly global::GooglePlayGames.Native.PInvoke.GameServices mServices;

		internal QuestManager(global::GooglePlayGames.Native.PInvoke.GameServices services)
		{
			mServices = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(services);
		}

		internal void Fetch(global::GooglePlayGames.Native.Cwrapper.Types.DataSource source, string questId, global::System.Action<global::GooglePlayGames.Native.PInvoke.QuestManager.FetchResponse> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_Fetch(mServices.AsHandle(), source, questId, InternalFetchCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback, global::GooglePlayGames.Native.PInvoke.QuestManager.FetchResponse.FromPointer));
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.QuestManager.FetchCallback))]
		internal static void InternalFetchCallback(global::System.IntPtr response, global::System.IntPtr data)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("QuestManager#FetchCallback", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Temporary, response, data);
		}

		internal void FetchList(global::GooglePlayGames.Native.Cwrapper.Types.DataSource source, int fetchFlags, global::System.Action<global::GooglePlayGames.Native.PInvoke.QuestManager.FetchListResponse> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_FetchList(mServices.AsHandle(), source, fetchFlags, InternalFetchListCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback, global::GooglePlayGames.Native.PInvoke.QuestManager.FetchListResponse.FromPointer));
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.QuestManager.FetchListCallback))]
		internal static void InternalFetchListCallback(global::System.IntPtr response, global::System.IntPtr data)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("QuestManager#FetchListCallback", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Temporary, response, data);
		}

		internal void ShowAllQuestUI(global::System.Action<global::GooglePlayGames.Native.PInvoke.QuestManager.QuestUIResponse> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_ShowAllUI(mServices.AsHandle(), InternalQuestUICallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback, global::GooglePlayGames.Native.PInvoke.QuestManager.QuestUIResponse.FromPointer));
		}

		internal void ShowQuestUI(global::GooglePlayGames.Native.PInvoke.NativeQuest quest, global::System.Action<global::GooglePlayGames.Native.PInvoke.QuestManager.QuestUIResponse> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_ShowUI(mServices.AsHandle(), quest.AsPointer(), InternalQuestUICallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback, global::GooglePlayGames.Native.PInvoke.QuestManager.QuestUIResponse.FromPointer));
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.QuestManager.QuestUICallback))]
		internal static void InternalQuestUICallback(global::System.IntPtr response, global::System.IntPtr data)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("QuestManager#QuestUICallback", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Temporary, response, data);
		}

		internal void Accept(global::GooglePlayGames.Native.PInvoke.NativeQuest quest, global::System.Action<global::GooglePlayGames.Native.PInvoke.QuestManager.AcceptResponse> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_Accept(mServices.AsHandle(), quest.AsPointer(), InternalAcceptCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback, global::GooglePlayGames.Native.PInvoke.QuestManager.AcceptResponse.FromPointer));
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.QuestManager.AcceptCallback))]
		internal static void InternalAcceptCallback(global::System.IntPtr response, global::System.IntPtr data)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("QuestManager#AcceptCallback", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Temporary, response, data);
		}

		internal void ClaimMilestone(global::GooglePlayGames.Native.PInvoke.NativeQuestMilestone milestone, global::System.Action<global::GooglePlayGames.Native.PInvoke.QuestManager.ClaimMilestoneResponse> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_ClaimMilestone(mServices.AsHandle(), milestone.AsPointer(), InternalClaimMilestoneCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback, global::GooglePlayGames.Native.PInvoke.QuestManager.ClaimMilestoneResponse.FromPointer));
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.QuestManager.ClaimMilestoneCallback))]
		internal static void InternalClaimMilestoneCallback(global::System.IntPtr response, global::System.IntPtr data)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("QuestManager#ClaimMilestoneCallback", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Temporary, response, data);
		}
	}
}
