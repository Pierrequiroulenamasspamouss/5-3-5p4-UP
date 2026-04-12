namespace GooglePlayGames.Native.PInvoke
{
	internal class TurnBasedManager
	{
		internal class MatchInboxUIResponse : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
		{
			internal MatchInboxUIResponse(global::System.IntPtr selfPointer)
				: base(selfPointer)
			{
			}

			internal global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus UiStatus()
			{
				return global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_MatchInboxUIResponse_GetStatus(SelfPtr());
			}

			internal global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch Match()
			{
				if (UiStatus() != global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.VALID)
				{
					return null;
				}
				return new global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch(global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_MatchInboxUIResponse_GetMatch(SelfPtr()));
			}

			protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
			{
				global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_MatchInboxUIResponse_Dispose(selfPointer);
			}

			internal static global::GooglePlayGames.Native.PInvoke.TurnBasedManager.MatchInboxUIResponse FromPointer(global::System.IntPtr pointer)
			{
				if (pointer.Equals(global::System.IntPtr.Zero))
				{
					return null;
				}
				return new global::GooglePlayGames.Native.PInvoke.TurnBasedManager.MatchInboxUIResponse(pointer);
			}
		}

		internal class TurnBasedMatchResponse : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
		{
			internal TurnBasedMatchResponse(global::System.IntPtr selfPointer)
				: base(selfPointer)
			{
			}

			internal global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus ResponseStatus()
			{
				return global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchResponse_GetStatus(SelfPtr());
			}

			internal bool RequestSucceeded()
			{
				return ResponseStatus() > (global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus)0;
			}

			internal global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch Match()
			{
				if (!RequestSucceeded())
				{
					return null;
				}
				return new global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch(global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchResponse_GetMatch(SelfPtr()));
			}

			protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
			{
				global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchResponse_Dispose(selfPointer);
			}

			internal static global::GooglePlayGames.Native.PInvoke.TurnBasedManager.TurnBasedMatchResponse FromPointer(global::System.IntPtr pointer)
			{
				if (pointer.Equals(global::System.IntPtr.Zero))
				{
					return null;
				}
				return new global::GooglePlayGames.Native.PInvoke.TurnBasedManager.TurnBasedMatchResponse(pointer);
			}
		}

		internal class TurnBasedMatchesResponse : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
		{
			internal TurnBasedMatchesResponse(global::System.IntPtr selfPointer)
				: base(selfPointer)
			{
			}

			protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
			{
				global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_Dispose(SelfPtr());
			}

			internal global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus Status()
			{
				return global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetStatus(SelfPtr());
			}

			internal global::System.Collections.Generic.IEnumerable<global::GooglePlayGames.Native.PInvoke.MultiplayerInvitation> Invitations()
			{
				return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.ToEnumerable(global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetInvitations_Length(SelfPtr()), (global::System.UIntPtr index) => new global::GooglePlayGames.Native.PInvoke.MultiplayerInvitation(global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetInvitations_GetElement(SelfPtr(), index)));
			}

			internal int InvitationCount()
			{
				return (int)global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetInvitations_Length(SelfPtr()).ToUInt32();
			}

			internal global::System.Collections.Generic.IEnumerable<global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch> MyTurnMatches()
			{
				return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.ToEnumerable(global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetMyTurnMatches_Length(SelfPtr()), (global::System.UIntPtr index) => new global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch(global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetMyTurnMatches_GetElement(SelfPtr(), index)));
			}

			internal int MyTurnMatchesCount()
			{
				return (int)global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetMyTurnMatches_Length(SelfPtr()).ToUInt32();
			}

			internal global::System.Collections.Generic.IEnumerable<global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch> TheirTurnMatches()
			{
				return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.ToEnumerable(global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetTheirTurnMatches_Length(SelfPtr()), (global::System.UIntPtr index) => new global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch(global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetTheirTurnMatches_GetElement(SelfPtr(), index)));
			}

			internal int TheirTurnMatchesCount()
			{
				return (int)global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetTheirTurnMatches_Length(SelfPtr()).ToUInt32();
			}

			internal global::System.Collections.Generic.IEnumerable<global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch> CompletedMatches()
			{
				return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.ToEnumerable(global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetCompletedMatches_Length(SelfPtr()), (global::System.UIntPtr index) => new global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch(global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetCompletedMatches_GetElement(SelfPtr(), index)));
			}

			internal int CompletedMatchesCount()
			{
				return (int)global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetCompletedMatches_Length(SelfPtr()).ToUInt32();
			}

			internal static global::GooglePlayGames.Native.PInvoke.TurnBasedManager.TurnBasedMatchesResponse FromPointer(global::System.IntPtr pointer)
			{
				if (global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.IsNull(pointer))
				{
					return null;
				}
				return new global::GooglePlayGames.Native.PInvoke.TurnBasedManager.TurnBasedMatchesResponse(pointer);
			}
		}

		internal delegate void TurnBasedMatchCallback(global::GooglePlayGames.Native.PInvoke.TurnBasedManager.TurnBasedMatchResponse response);

		private readonly global::GooglePlayGames.Native.PInvoke.GameServices mGameServices;

		internal TurnBasedManager(global::GooglePlayGames.Native.PInvoke.GameServices services)
		{
			mGameServices = services;
		}

		internal void GetMatch(string matchId, global::System.Action<global::GooglePlayGames.Native.PInvoke.TurnBasedManager.TurnBasedMatchResponse> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_FetchMatch(mGameServices.AsHandle(), matchId, InternalTurnBasedMatchCallback, ToCallbackPointer(callback));
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMatchCallback))]
		internal static void InternalTurnBasedMatchCallback(global::System.IntPtr response, global::System.IntPtr data)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("TurnBasedManager#InternalTurnBasedMatchCallback", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Temporary, response, data);
		}

		internal void CreateMatch(global::GooglePlayGames.Native.PInvoke.TurnBasedMatchConfig config, global::System.Action<global::GooglePlayGames.Native.PInvoke.TurnBasedManager.TurnBasedMatchResponse> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_CreateTurnBasedMatch(mGameServices.AsHandle(), config.AsPointer(), InternalTurnBasedMatchCallback, ToCallbackPointer(callback));
		}

		internal void ShowPlayerSelectUI(uint minimumPlayers, uint maxiumPlayers, bool allowAutomatching, global::System.Action<global::GooglePlayGames.Native.PInvoke.PlayerSelectUIResponse> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_ShowPlayerSelectUI(mGameServices.AsHandle(), minimumPlayers, maxiumPlayers, allowAutomatching, InternalPlayerSelectUIcallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback, global::GooglePlayGames.Native.PInvoke.PlayerSelectUIResponse.FromPointer));
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.PlayerSelectUICallback))]
		internal static void InternalPlayerSelectUIcallback(global::System.IntPtr response, global::System.IntPtr data)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("TurnBasedManager#PlayerSelectUICallback", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Temporary, response, data);
		}

		internal void GetAllTurnbasedMatches(global::System.Action<global::GooglePlayGames.Native.PInvoke.TurnBasedManager.TurnBasedMatchesResponse> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_FetchMatches(mGameServices.AsHandle(), InternalTurnBasedMatchesCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback, global::GooglePlayGames.Native.PInvoke.TurnBasedManager.TurnBasedMatchesResponse.FromPointer));
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMatchesCallback))]
		internal static void InternalTurnBasedMatchesCallback(global::System.IntPtr response, global::System.IntPtr data)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("TurnBasedManager#TurnBasedMatchesCallback", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Temporary, response, data);
		}

		internal void AcceptInvitation(global::GooglePlayGames.Native.PInvoke.MultiplayerInvitation invitation, global::System.Action<global::GooglePlayGames.Native.PInvoke.TurnBasedManager.TurnBasedMatchResponse> callback)
		{
			global::GooglePlayGames.OurUtils.Logger.d("Accepting invitation: " + invitation.AsPointer().ToInt64());
			global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_AcceptInvitation(mGameServices.AsHandle(), invitation.AsPointer(), InternalTurnBasedMatchCallback, ToCallbackPointer(callback));
		}

		internal void DeclineInvitation(global::GooglePlayGames.Native.PInvoke.MultiplayerInvitation invitation)
		{
			global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_DeclineInvitation(mGameServices.AsHandle(), invitation.AsPointer());
		}

		internal void TakeTurn(global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch match, byte[] data, global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant nextParticipant, global::System.Action<global::GooglePlayGames.Native.PInvoke.TurnBasedManager.TurnBasedMatchResponse> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TakeMyTurn(mGameServices.AsHandle(), match.AsPointer(), data, new global::System.UIntPtr((uint)data.Length), match.Results().AsPointer(), nextParticipant.AsPointer(), InternalTurnBasedMatchCallback, ToCallbackPointer(callback));
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.MatchInboxUICallback))]
		internal static void InternalMatchInboxUICallback(global::System.IntPtr response, global::System.IntPtr data)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("TurnBasedManager#MatchInboxUICallback", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Temporary, response, data);
		}

		internal void ShowInboxUI(global::System.Action<global::GooglePlayGames.Native.PInvoke.TurnBasedManager.MatchInboxUIResponse> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_ShowMatchInboxUI(mGameServices.AsHandle(), InternalMatchInboxUICallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback, global::GooglePlayGames.Native.PInvoke.TurnBasedManager.MatchInboxUIResponse.FromPointer));
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.MultiplayerStatusCallback))]
		internal static void InternalMultiplayerStatusCallback(global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus status, global::System.IntPtr data)
		{
			global::GooglePlayGames.OurUtils.Logger.d("InternalMultiplayerStatusCallback: " + status);
			global::System.Action<global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus> action = global::GooglePlayGames.Native.PInvoke.Callbacks.IntPtrToTempCallback<global::System.Action<global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus>>(data);
			try
			{
				action(status);
			}
			catch (global::System.Exception ex)
			{
				global::GooglePlayGames.OurUtils.Logger.e("Error encountered executing InternalMultiplayerStatusCallback. Smothering to avoid passing exception into Native: " + ex);
			}
		}

		internal void LeaveDuringMyTurn(global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch match, global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant nextParticipant, global::System.Action<global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_LeaveMatchDuringMyTurn(mGameServices.AsHandle(), match.AsPointer(), nextParticipant.AsPointer(), InternalMultiplayerStatusCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback));
		}

		internal void FinishMatchDuringMyTurn(global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch match, byte[] data, global::GooglePlayGames.Native.PInvoke.ParticipantResults results, global::System.Action<global::GooglePlayGames.Native.PInvoke.TurnBasedManager.TurnBasedMatchResponse> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_FinishMatchDuringMyTurn(mGameServices.AsHandle(), match.AsPointer(), data, new global::System.UIntPtr((uint)data.Length), results.AsPointer(), InternalTurnBasedMatchCallback, ToCallbackPointer(callback));
		}

		internal void ConfirmPendingCompletion(global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch match, global::System.Action<global::GooglePlayGames.Native.PInvoke.TurnBasedManager.TurnBasedMatchResponse> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_ConfirmPendingCompletion(mGameServices.AsHandle(), match.AsPointer(), InternalTurnBasedMatchCallback, ToCallbackPointer(callback));
		}

		internal void LeaveMatchDuringTheirTurn(global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch match, global::System.Action<global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_LeaveMatchDuringTheirTurn(mGameServices.AsHandle(), match.AsPointer(), InternalMultiplayerStatusCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback));
		}

		internal void CancelMatch(global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch match, global::System.Action<global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_CancelMatch(mGameServices.AsHandle(), match.AsPointer(), InternalMultiplayerStatusCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback));
		}

		internal void Rematch(global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch match, global::System.Action<global::GooglePlayGames.Native.PInvoke.TurnBasedManager.TurnBasedMatchResponse> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_Rematch(mGameServices.AsHandle(), match.AsPointer(), InternalTurnBasedMatchCallback, ToCallbackPointer(callback));
		}

		private static global::System.IntPtr ToCallbackPointer(global::System.Action<global::GooglePlayGames.Native.PInvoke.TurnBasedManager.TurnBasedMatchResponse> callback)
		{
			return global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback, global::GooglePlayGames.Native.PInvoke.TurnBasedManager.TurnBasedMatchResponse.FromPointer);
		}
	}
}
