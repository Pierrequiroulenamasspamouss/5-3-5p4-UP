namespace GooglePlayGames.Native.PInvoke
{
	internal class AchievementManager
	{
		internal class FetchResponse : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
		{
			internal FetchResponse(global::System.IntPtr selfPointer)
				: base(selfPointer)
			{
			}

			internal global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus Status()
			{
				return global::GooglePlayGames.Native.Cwrapper.AchievementManager.AchievementManager_FetchResponse_GetStatus(SelfPtr());
			}

			internal global::GooglePlayGames.Native.PInvoke.NativeAchievement Achievement()
			{
				global::System.IntPtr selfPointer = global::GooglePlayGames.Native.Cwrapper.AchievementManager.AchievementManager_FetchResponse_GetData(SelfPtr());
				return new global::GooglePlayGames.Native.PInvoke.NativeAchievement(selfPointer);
			}

			protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
			{
				global::GooglePlayGames.Native.Cwrapper.AchievementManager.AchievementManager_FetchResponse_Dispose(selfPointer);
			}

			internal static global::GooglePlayGames.Native.PInvoke.AchievementManager.FetchResponse FromPointer(global::System.IntPtr pointer)
			{
				if (pointer.Equals(global::System.IntPtr.Zero))
				{
					return null;
				}
				return new global::GooglePlayGames.Native.PInvoke.AchievementManager.FetchResponse(pointer);
			}
		}

		internal class FetchAllResponse : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder, global::System.Collections.IEnumerable, global::System.Collections.Generic.IEnumerable<global::GooglePlayGames.Native.PInvoke.NativeAchievement>
		{
			internal FetchAllResponse(global::System.IntPtr selfPointer)
				: base(selfPointer)
			{
			}

			global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			internal global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus Status()
			{
				return global::GooglePlayGames.Native.Cwrapper.AchievementManager.AchievementManager_FetchAllResponse_GetStatus(SelfPtr());
			}

			private global::System.UIntPtr Length()
			{
				return global::GooglePlayGames.Native.Cwrapper.AchievementManager.AchievementManager_FetchAllResponse_GetData_Length(SelfPtr());
			}

			private global::GooglePlayGames.Native.PInvoke.NativeAchievement GetElement(global::System.UIntPtr index)
			{
				if (index.ToUInt64() >= Length().ToUInt64())
				{
					throw new global::System.ArgumentOutOfRangeException();
				}
				return new global::GooglePlayGames.Native.PInvoke.NativeAchievement(global::GooglePlayGames.Native.Cwrapper.AchievementManager.AchievementManager_FetchAllResponse_GetData_GetElement(SelfPtr(), index));
			}

			public global::System.Collections.Generic.IEnumerator<global::GooglePlayGames.Native.PInvoke.NativeAchievement> GetEnumerator()
			{
				return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.ToEnumerator<global::GooglePlayGames.Native.PInvoke.NativeAchievement>(global::GooglePlayGames.Native.Cwrapper.AchievementManager.AchievementManager_FetchAllResponse_GetData_Length(SelfPtr()), (global::System.UIntPtr index) => GetElement(index));
			}

			protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
			{
				global::GooglePlayGames.Native.Cwrapper.AchievementManager.AchievementManager_FetchAllResponse_Dispose(selfPointer);
			}

			internal static global::GooglePlayGames.Native.PInvoke.AchievementManager.FetchAllResponse FromPointer(global::System.IntPtr pointer)
			{
				if (pointer.Equals(global::System.IntPtr.Zero))
				{
					return null;
				}
				return new global::GooglePlayGames.Native.PInvoke.AchievementManager.FetchAllResponse(pointer);
			}
		}

		private readonly global::GooglePlayGames.Native.PInvoke.GameServices mServices;

		internal AchievementManager(global::GooglePlayGames.Native.PInvoke.GameServices services)
		{
			mServices = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(services);
		}

		internal void ShowAllUI(global::System.Action<global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus> callback)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(callback);
			global::GooglePlayGames.Native.Cwrapper.AchievementManager.AchievementManager_ShowAllUI(mServices.AsHandle(), global::GooglePlayGames.Native.PInvoke.Callbacks.InternalShowUICallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback));
		}

		internal void FetchAll(global::System.Action<global::GooglePlayGames.Native.PInvoke.AchievementManager.FetchAllResponse> callback)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(callback);
			global::GooglePlayGames.Native.Cwrapper.AchievementManager.AchievementManager_FetchAll(mServices.AsHandle(), global::GooglePlayGames.Native.Cwrapper.Types.DataSource.CACHE_OR_NETWORK, InternalFetchAllCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback, global::GooglePlayGames.Native.PInvoke.AchievementManager.FetchAllResponse.FromPointer));
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.AchievementManager.FetchAllCallback))]
		private static void InternalFetchAllCallback(global::System.IntPtr response, global::System.IntPtr data)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("AchievementManager#InternalFetchAllCallback", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Temporary, response, data);
		}

		internal void Fetch(string achId, global::System.Action<global::GooglePlayGames.Native.PInvoke.AchievementManager.FetchResponse> callback)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(achId);
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(callback);
			global::GooglePlayGames.Native.Cwrapper.AchievementManager.AchievementManager_Fetch(mServices.AsHandle(), global::GooglePlayGames.Native.Cwrapper.Types.DataSource.CACHE_OR_NETWORK, achId, InternalFetchCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback, global::GooglePlayGames.Native.PInvoke.AchievementManager.FetchResponse.FromPointer));
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.AchievementManager.FetchCallback))]
		private static void InternalFetchCallback(global::System.IntPtr response, global::System.IntPtr data)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("AchievementManager#InternalFetchCallback", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Temporary, response, data);
		}

		internal void Increment(string achievementId, uint numSteps)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(achievementId);
			global::GooglePlayGames.Native.Cwrapper.AchievementManager.AchievementManager_Increment(mServices.AsHandle(), achievementId, numSteps);
		}

		internal void SetStepsAtLeast(string achivementId, uint numSteps)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(achivementId);
			global::GooglePlayGames.Native.Cwrapper.AchievementManager.AchievementManager_SetStepsAtLeast(mServices.AsHandle(), achivementId, numSteps);
		}

		internal void Reveal(string achievementId)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(achievementId);
			global::GooglePlayGames.Native.Cwrapper.AchievementManager.AchievementManager_Reveal(mServices.AsHandle(), achievementId);
		}

		internal void Unlock(string achievementId)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(achievementId);
			global::GooglePlayGames.Native.Cwrapper.AchievementManager.AchievementManager_Unlock(mServices.AsHandle(), achievementId);
		}
	}
}
