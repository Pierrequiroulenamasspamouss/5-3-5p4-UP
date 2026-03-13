namespace GooglePlayGames.Native
{
	public class NativeRealtimeMultiplayerClient : global::GooglePlayGames.BasicApi.Multiplayer.IRealTimeMultiplayerClient
	{
		private class NoopListener : global::GooglePlayGames.BasicApi.Multiplayer.RealTimeMultiplayerListener
		{
			public void OnRoomSetupProgress(float percent)
			{
			}

			public void OnRoomConnected(bool success)
			{
			}

			public void OnLeftRoom()
			{
			}

			public void OnParticipantLeft(global::GooglePlayGames.BasicApi.Multiplayer.Participant participant)
			{
			}

			public void OnPeersConnected(string[] participantIds)
			{
			}

			public void OnPeersDisconnected(string[] participantIds)
			{
			}

			public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
			{
			}
		}

		private class RoomSession
		{
			private readonly object mLifecycleLock = new object();

			private readonly global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.OnGameThreadForwardingListener mListener;

			private readonly global::GooglePlayGames.Native.PInvoke.RealtimeManager mManager;

			private volatile string mCurrentPlayerId;

			private volatile global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.State mState;

			private volatile bool mStillPreRoomCreation;

			private global::GooglePlayGames.BasicApi.Multiplayer.Invitation mInvitation;

			private volatile bool mShowingUI;

			private uint mMinPlayersToStart;

			internal bool ShowingUI
			{
				get
				{
					return mShowingUI;
				}
				set
				{
					mShowingUI = value;
				}
			}

			internal uint MinPlayersToStart
			{
				get
				{
					return mMinPlayersToStart;
				}
				set
				{
					mMinPlayersToStart = value;
				}
			}

			internal RoomSession(global::GooglePlayGames.Native.PInvoke.RealtimeManager manager, global::GooglePlayGames.BasicApi.Multiplayer.RealTimeMultiplayerListener listener)
			{
				mManager = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(manager);
				mListener = new global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.OnGameThreadForwardingListener(listener);
				EnterState(new global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.BeforeRoomCreateStartedState(this), false);
				mStillPreRoomCreation = true;
			}

			internal global::GooglePlayGames.Native.PInvoke.RealtimeManager Manager()
			{
				return mManager;
			}

			internal bool IsActive()
			{
				return mState.IsActive();
			}

			internal string SelfPlayerId()
			{
				return mCurrentPlayerId;
			}

			public void SetInvitation(global::GooglePlayGames.BasicApi.Multiplayer.Invitation invitation)
			{
				mInvitation = invitation;
			}

			public global::GooglePlayGames.BasicApi.Multiplayer.Invitation GetInvitation()
			{
				return mInvitation;
			}

			internal global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.OnGameThreadForwardingListener OnGameThreadListener()
			{
				return mListener;
			}

			internal void EnterState(global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.State handler)
			{
				EnterState(handler, true);
			}

			internal void EnterState(global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.State handler, bool fireStateEnteredEvent)
			{
				lock (mLifecycleLock)
				{
					mState = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(handler);
					if (fireStateEnteredEvent)
					{
						global::GooglePlayGames.OurUtils.Logger.d("Entering state: " + handler.GetType().Name);
						mState.OnStateEntered();
					}
				}
			}

			internal void LeaveRoom()
			{
				if (!ShowingUI)
				{
					lock (mLifecycleLock)
					{
						mState.LeaveRoom();
						return;
					}
				}
				global::GooglePlayGames.OurUtils.Logger.d("Not leaving room since showing UI");
			}

			internal void ShowWaitingRoomUI()
			{
				mState.ShowWaitingRoomUI(MinPlayersToStart);
			}

			internal void StartRoomCreation(string currentPlayerId, global::System.Action createRoom)
			{
				lock (mLifecycleLock)
				{
					if (!mStillPreRoomCreation)
					{
						global::GooglePlayGames.OurUtils.Logger.e("Room creation started more than once, this shouldn't happen!");
						return;
					}
					if (!mState.IsActive())
					{
						global::GooglePlayGames.OurUtils.Logger.w("Received an attempt to create a room after the session was already torn down!");
						return;
					}
					mCurrentPlayerId = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(currentPlayerId);
					mStillPreRoomCreation = false;
					EnterState(new global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.RoomCreationPendingState(this));
					createRoom();
				}
			}

			internal void OnRoomStatusChanged(global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom room)
			{
				lock (mLifecycleLock)
				{
					mState.OnRoomStatusChanged(room);
				}
			}

			internal void OnConnectedSetChanged(global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom room)
			{
				lock (mLifecycleLock)
				{
					mState.OnConnectedSetChanged(room);
				}
			}

			internal void OnParticipantStatusChanged(global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom room, global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant)
			{
				lock (mLifecycleLock)
				{
					mState.OnParticipantStatusChanged(room, participant);
				}
			}

			internal void HandleRoomResponse(global::GooglePlayGames.Native.PInvoke.RealtimeManager.RealTimeRoomResponse response)
			{
				lock (mLifecycleLock)
				{
					mState.HandleRoomResponse(response);
				}
			}

			internal void OnDataReceived(global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom room, global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant sender, byte[] data, bool isReliable)
			{
				mState.OnDataReceived(room, sender, data, isReliable);
			}

			internal void SendMessageToAll(bool reliable, byte[] data)
			{
				SendMessageToAll(reliable, data, 0, data.Length);
			}

			internal void SendMessageToAll(bool reliable, byte[] data, int offset, int length)
			{
				mState.SendToAll(data, offset, length, reliable);
			}

			internal void SendMessage(bool reliable, string participantId, byte[] data)
			{
				SendMessage(reliable, participantId, data, 0, data.Length);
			}

			internal void SendMessage(bool reliable, string participantId, byte[] data, int offset, int length)
			{
				mState.SendToSpecificRecipient(participantId, data, offset, length, reliable);
			}

			internal global::System.Collections.Generic.List<global::GooglePlayGames.BasicApi.Multiplayer.Participant> GetConnectedParticipants()
			{
				return mState.GetConnectedParticipants();
			}

			internal virtual global::GooglePlayGames.BasicApi.Multiplayer.Participant GetSelf()
			{
				return mState.GetSelf();
			}

			internal virtual global::GooglePlayGames.BasicApi.Multiplayer.Participant GetParticipant(string participantId)
			{
				return mState.GetParticipant(participantId);
			}

			internal virtual bool IsRoomConnected()
			{
				return mState.IsRoomConnected();
			}
		}

		private class OnGameThreadForwardingListener
		{
			private readonly global::GooglePlayGames.BasicApi.Multiplayer.RealTimeMultiplayerListener mListener;

			internal OnGameThreadForwardingListener(global::GooglePlayGames.BasicApi.Multiplayer.RealTimeMultiplayerListener listener)
			{
				mListener = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(listener);
			}

			public void RoomSetupProgress(float percent)
			{
				global::GooglePlayGames.OurUtils.PlayGamesHelperObject.RunOnGameThread(delegate
				{
					mListener.OnRoomSetupProgress(percent);
				});
			}

			public void RoomConnected(bool success)
			{
				global::GooglePlayGames.OurUtils.PlayGamesHelperObject.RunOnGameThread(delegate
				{
					mListener.OnRoomConnected(success);
				});
			}

			public void LeftRoom()
			{
				global::GooglePlayGames.OurUtils.PlayGamesHelperObject.RunOnGameThread(delegate
				{
					mListener.OnLeftRoom();
				});
			}

			public void PeersConnected(string[] participantIds)
			{
				global::GooglePlayGames.OurUtils.PlayGamesHelperObject.RunOnGameThread(delegate
				{
					mListener.OnPeersConnected(participantIds);
				});
			}

			public void PeersDisconnected(string[] participantIds)
			{
				global::GooglePlayGames.OurUtils.PlayGamesHelperObject.RunOnGameThread(delegate
				{
					mListener.OnPeersDisconnected(participantIds);
				});
			}

			public void RealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
			{
				global::GooglePlayGames.OurUtils.PlayGamesHelperObject.RunOnGameThread(delegate
				{
					mListener.OnRealTimeMessageReceived(isReliable, senderId, data);
				});
			}

			public void ParticipantLeft(global::GooglePlayGames.BasicApi.Multiplayer.Participant participant)
			{
				global::GooglePlayGames.OurUtils.PlayGamesHelperObject.RunOnGameThread(delegate
				{
					mListener.OnParticipantLeft(participant);
				});
			}
		}

		internal abstract class State
		{
			internal virtual void HandleRoomResponse(global::GooglePlayGames.Native.PInvoke.RealtimeManager.RealTimeRoomResponse response)
			{
				global::GooglePlayGames.OurUtils.Logger.d(GetType().Name + ".HandleRoomResponse: Defaulting to no-op.");
			}

			internal virtual bool IsActive()
			{
				global::GooglePlayGames.OurUtils.Logger.d(GetType().Name + ".IsNonPreemptable: Is preemptable by default.");
				return true;
			}

			internal virtual void LeaveRoom()
			{
				global::GooglePlayGames.OurUtils.Logger.d(GetType().Name + ".LeaveRoom: Defaulting to no-op.");
			}

			internal virtual void ShowWaitingRoomUI(uint minimumParticipantsBeforeStarting)
			{
				global::GooglePlayGames.OurUtils.Logger.d(GetType().Name + ".ShowWaitingRoomUI: Defaulting to no-op.");
			}

			internal virtual void OnStateEntered()
			{
				global::GooglePlayGames.OurUtils.Logger.d(GetType().Name + ".OnStateEntered: Defaulting to no-op.");
			}

			internal virtual void OnRoomStatusChanged(global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom room)
			{
				global::GooglePlayGames.OurUtils.Logger.d(GetType().Name + ".OnRoomStatusChanged: Defaulting to no-op.");
			}

			internal virtual void OnConnectedSetChanged(global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom room)
			{
				global::GooglePlayGames.OurUtils.Logger.d(GetType().Name + ".OnConnectedSetChanged: Defaulting to no-op.");
			}

			internal virtual void OnParticipantStatusChanged(global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom room, global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant)
			{
				global::GooglePlayGames.OurUtils.Logger.d(GetType().Name + ".OnParticipantStatusChanged: Defaulting to no-op.");
			}

			internal virtual void OnDataReceived(global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom room, global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant sender, byte[] data, bool isReliable)
			{
				global::GooglePlayGames.OurUtils.Logger.d(GetType().Name + ".OnDataReceived: Defaulting to no-op.");
			}

			internal virtual void SendToSpecificRecipient(string recipientId, byte[] data, int offset, int length, bool isReliable)
			{
				global::GooglePlayGames.OurUtils.Logger.d(GetType().Name + ".SendToSpecificRecipient: Defaulting to no-op.");
			}

			internal virtual void SendToAll(byte[] data, int offset, int length, bool isReliable)
			{
				global::GooglePlayGames.OurUtils.Logger.d(GetType().Name + ".SendToApp: Defaulting to no-op.");
			}

			internal virtual global::System.Collections.Generic.List<global::GooglePlayGames.BasicApi.Multiplayer.Participant> GetConnectedParticipants()
			{
				global::GooglePlayGames.OurUtils.Logger.d(GetType().Name + ".GetConnectedParticipants: Returning empty connected participants");
				return new global::System.Collections.Generic.List<global::GooglePlayGames.BasicApi.Multiplayer.Participant>();
			}

			internal virtual global::GooglePlayGames.BasicApi.Multiplayer.Participant GetSelf()
			{
				global::GooglePlayGames.OurUtils.Logger.d(GetType().Name + ".GetSelf: Returning null self.");
				return null;
			}

			internal virtual global::GooglePlayGames.BasicApi.Multiplayer.Participant GetParticipant(string participantId)
			{
				global::GooglePlayGames.OurUtils.Logger.d(GetType().Name + ".GetSelf: Returning null participant.");
				return null;
			}

			internal virtual bool IsRoomConnected()
			{
				global::GooglePlayGames.OurUtils.Logger.d(GetType().Name + ".IsRoomConnected: Returning room not connected.");
				return false;
			}
		}

		private abstract class MessagingEnabledState : global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.State
		{
			protected readonly global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.RoomSession mSession;

			protected global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom mRoom;

			protected global::System.Collections.Generic.Dictionary<string, global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant> mNativeParticipants;

			protected global::System.Collections.Generic.Dictionary<string, global::GooglePlayGames.BasicApi.Multiplayer.Participant> mParticipants;

			internal MessagingEnabledState(global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.RoomSession session, global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom room)
			{
				mSession = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(session);
				UpdateCurrentRoom(room);
			}

			internal void UpdateCurrentRoom(global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom room)
			{
				if (mRoom != null)
				{
					mRoom.Dispose();
				}
				mRoom = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(room);
				mNativeParticipants = global::System.Linq.Enumerable.ToDictionary(mRoom.Participants(), (global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant p) => p.Id());
				mParticipants = global::System.Linq.Enumerable.ToDictionary(global::System.Linq.Enumerable.Select(mNativeParticipants.Values, (global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant p) => p.AsParticipant()), (global::GooglePlayGames.BasicApi.Multiplayer.Participant p) => p.ParticipantId);
			}

			internal sealed override void OnRoomStatusChanged(global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom room)
			{
				HandleRoomStatusChanged(room);
				UpdateCurrentRoom(room);
			}

			internal virtual void HandleRoomStatusChanged(global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom room)
			{
			}

			internal sealed override void OnConnectedSetChanged(global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom room)
			{
				HandleConnectedSetChanged(room);
				UpdateCurrentRoom(room);
			}

			internal virtual void HandleConnectedSetChanged(global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom room)
			{
			}

			internal sealed override void OnParticipantStatusChanged(global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom room, global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant)
			{
				HandleParticipantStatusChanged(room, participant);
				UpdateCurrentRoom(room);
			}

			internal virtual void HandleParticipantStatusChanged(global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom room, global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant)
			{
			}

			internal sealed override global::System.Collections.Generic.List<global::GooglePlayGames.BasicApi.Multiplayer.Participant> GetConnectedParticipants()
			{
				global::System.Collections.Generic.List<global::GooglePlayGames.BasicApi.Multiplayer.Participant> list = global::System.Linq.Enumerable.ToList(global::System.Linq.Enumerable.Where(mParticipants.Values, (global::GooglePlayGames.BasicApi.Multiplayer.Participant p) => p.IsConnectedToRoom));
				list.Sort();
				return list;
			}

			internal override void SendToSpecificRecipient(string recipientId, byte[] data, int offset, int length, bool isReliable)
			{
				if (!mNativeParticipants.ContainsKey(recipientId))
				{
					global::GooglePlayGames.OurUtils.Logger.e("Attempted to send message to unknown participant " + recipientId);
					return;
				}
				if (isReliable)
				{
					mSession.Manager().SendReliableMessage(mRoom, mNativeParticipants[recipientId], global::GooglePlayGames.OurUtils.Misc.GetSubsetBytes(data, offset, length), null);
					return;
				}
				mSession.Manager().SendUnreliableMessageToSpecificParticipants(mRoom, new global::System.Collections.Generic.List<global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant> { mNativeParticipants[recipientId] }, global::GooglePlayGames.OurUtils.Misc.GetSubsetBytes(data, offset, length));
			}

			internal override void SendToAll(byte[] data, int offset, int length, bool isReliable)
			{
				byte[] subsetBytes = global::GooglePlayGames.OurUtils.Misc.GetSubsetBytes(data, offset, length);
				if (isReliable)
				{
					foreach (string key in mNativeParticipants.Keys)
					{
						SendToSpecificRecipient(key, subsetBytes, 0, subsetBytes.Length, true);
					}
					return;
				}
				mSession.Manager().SendUnreliableMessageToAll(mRoom, subsetBytes);
			}

			internal override void OnDataReceived(global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom room, global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant sender, byte[] data, bool isReliable)
			{
				mSession.OnGameThreadListener().RealTimeMessageReceived(isReliable, sender.Id(), data);
			}
		}

		private class BeforeRoomCreateStartedState : global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.State
		{
			private readonly global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.RoomSession mContainingSession;

			internal BeforeRoomCreateStartedState(global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.RoomSession session)
			{
				mContainingSession = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(session);
			}

			internal override void LeaveRoom()
			{
				global::GooglePlayGames.OurUtils.Logger.d("Session was torn down before room was created.");
				mContainingSession.OnGameThreadListener().RoomConnected(false);
				mContainingSession.EnterState(new global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.ShutdownState(mContainingSession));
			}
		}

		private class RoomCreationPendingState : global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.State
		{
			private readonly global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.RoomSession mContainingSession;

			internal RoomCreationPendingState(global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.RoomSession session)
			{
				mContainingSession = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(session);
			}

			internal override void HandleRoomResponse(global::GooglePlayGames.Native.PInvoke.RealtimeManager.RealTimeRoomResponse response)
			{
				if (!response.RequestSucceeded())
				{
					mContainingSession.EnterState(new global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.ShutdownState(mContainingSession));
					mContainingSession.OnGameThreadListener().RoomConnected(false);
				}
				else
				{
					mContainingSession.EnterState(new global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.ConnectingState(response.Room(), mContainingSession));
				}
			}

			internal override bool IsActive()
			{
				return true;
			}

			internal override void LeaveRoom()
			{
				global::GooglePlayGames.OurUtils.Logger.d("Received request to leave room during room creation, aborting creation.");
				mContainingSession.EnterState(new global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.AbortingRoomCreationState(mContainingSession));
			}
		}

		private class ConnectingState : global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.MessagingEnabledState
		{
			private const float InitialPercentComplete = 20f;

			private static readonly global::System.Collections.Generic.HashSet<global::GooglePlayGames.Native.Cwrapper.Types.ParticipantStatus> FailedStatuses = new global::System.Collections.Generic.HashSet<global::GooglePlayGames.Native.Cwrapper.Types.ParticipantStatus>
			{
				global::GooglePlayGames.Native.Cwrapper.Types.ParticipantStatus.DECLINED,
				global::GooglePlayGames.Native.Cwrapper.Types.ParticipantStatus.LEFT
			};

			private global::System.Collections.Generic.HashSet<string> mConnectedParticipants = new global::System.Collections.Generic.HashSet<string>();

			private float mPercentComplete = 20f;

			private float mPercentPerParticipant;

			internal ConnectingState(global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom room, global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.RoomSession session)
				: base(session, room)
			{
				mPercentPerParticipant = 80f / (float)session.MinPlayersToStart;
			}

			internal override void OnStateEntered()
			{
				mSession.OnGameThreadListener().RoomSetupProgress(mPercentComplete);
			}

			internal override void HandleConnectedSetChanged(global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom room)
			{
				global::System.Collections.Generic.HashSet<string> hashSet = new global::System.Collections.Generic.HashSet<string>();
				if ((room.Status() == global::GooglePlayGames.Native.Cwrapper.Types.RealTimeRoomStatus.AUTO_MATCHING || room.Status() == global::GooglePlayGames.Native.Cwrapper.Types.RealTimeRoomStatus.CONNECTING) && mSession.MinPlayersToStart <= room.ParticipantCount())
				{
					mSession.MinPlayersToStart += room.ParticipantCount();
					mPercentPerParticipant = 80f / (float)mSession.MinPlayersToStart;
				}
				foreach (global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant item in room.Participants())
				{
					using (item)
					{
						if (item.IsConnectedToRoom())
						{
							hashSet.Add(item.Id());
						}
					}
				}
				if (mConnectedParticipants.Equals(hashSet))
				{
					global::GooglePlayGames.OurUtils.Logger.w("Received connected set callback with unchanged connected set!");
					return;
				}
				global::System.Collections.Generic.IEnumerable<string> source = global::System.Linq.Enumerable.Except(mConnectedParticipants, hashSet);
				if (room.Status() == global::GooglePlayGames.Native.Cwrapper.Types.RealTimeRoomStatus.DELETED)
				{
					global::GooglePlayGames.OurUtils.Logger.e("Participants disconnected during room setup, failing. Participants were: " + string.Join(",", global::System.Linq.Enumerable.ToArray(source)));
					mSession.OnGameThreadListener().RoomConnected(false);
					mSession.EnterState(new global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.ShutdownState(mSession));
					return;
				}
				global::System.Collections.Generic.IEnumerable<string> source2 = global::System.Linq.Enumerable.Except(hashSet, mConnectedParticipants);
				global::GooglePlayGames.OurUtils.Logger.d("New participants connected: " + string.Join(",", global::System.Linq.Enumerable.ToArray(source2)));
				if (room.Status() == global::GooglePlayGames.Native.Cwrapper.Types.RealTimeRoomStatus.ACTIVE)
				{
					global::GooglePlayGames.OurUtils.Logger.d("Fully connected! Transitioning to active state.");
					mSession.EnterState(new global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.ActiveState(room, mSession));
					mSession.OnGameThreadListener().RoomConnected(true);
				}
				else
				{
					mPercentComplete += mPercentPerParticipant * (float)global::System.Linq.Enumerable.Count(source2);
					mConnectedParticipants = hashSet;
					mSession.OnGameThreadListener().RoomSetupProgress(mPercentComplete);
				}
			}

			internal override void HandleParticipantStatusChanged(global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom room, global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant)
			{
				if (FailedStatuses.Contains(participant.Status()))
				{
					mSession.OnGameThreadListener().ParticipantLeft(participant.AsParticipant());
					if (room.Status() != global::GooglePlayGames.Native.Cwrapper.Types.RealTimeRoomStatus.CONNECTING && room.Status() != global::GooglePlayGames.Native.Cwrapper.Types.RealTimeRoomStatus.AUTO_MATCHING)
					{
						LeaveRoom();
					}
				}
			}

			internal override void LeaveRoom()
			{
				mSession.EnterState(new global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.LeavingRoom(mSession, mRoom, delegate
				{
					mSession.OnGameThreadListener().RoomConnected(false);
				}));
			}

			internal override void ShowWaitingRoomUI(uint minimumParticipantsBeforeStarting)
			{
				mSession.ShowingUI = true;
				mSession.Manager().ShowWaitingRoomUI(mRoom, minimumParticipantsBeforeStarting, delegate(global::GooglePlayGames.Native.PInvoke.RealtimeManager.WaitingRoomUIResponse response)
				{
					mSession.ShowingUI = false;
					global::GooglePlayGames.OurUtils.Logger.d("ShowWaitingRoomUI Response: " + response.ResponseStatus());
					if (response.ResponseStatus() == global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.VALID)
					{
						global::GooglePlayGames.OurUtils.Logger.d("Connecting state ShowWaitingRoomUI: room pcount:" + response.Room().ParticipantCount() + " status: " + response.Room().Status());
						if (response.Room().Status() == global::GooglePlayGames.Native.Cwrapper.Types.RealTimeRoomStatus.ACTIVE)
						{
							mSession.EnterState(new global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.ActiveState(response.Room(), mSession));
						}
					}
					else if (response.ResponseStatus() == global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.ERROR_LEFT_ROOM)
					{
						LeaveRoom();
					}
					else
					{
						mSession.OnGameThreadListener().RoomSetupProgress(mPercentComplete);
					}
				});
			}
		}

		private class ActiveState : global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.MessagingEnabledState
		{
			internal ActiveState(global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom room, global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.RoomSession session)
				: base(session, room)
			{
			}

			internal override void OnStateEntered()
			{
				if (GetSelf() == null)
				{
					global::GooglePlayGames.OurUtils.Logger.e("Room reached active state with unknown participant for the player");
					LeaveRoom();
				}
			}

			internal override bool IsRoomConnected()
			{
				return true;
			}

			internal override global::GooglePlayGames.BasicApi.Multiplayer.Participant GetParticipant(string participantId)
			{
				if (!mParticipants.ContainsKey(participantId))
				{
					global::GooglePlayGames.OurUtils.Logger.e("Attempted to retrieve unknown participant " + participantId);
					return null;
				}
				return mParticipants[participantId];
			}

			internal override global::GooglePlayGames.BasicApi.Multiplayer.Participant GetSelf()
			{
				foreach (global::GooglePlayGames.BasicApi.Multiplayer.Participant value in mParticipants.Values)
				{
					if (value.Player != null && value.Player.id.Equals(mSession.SelfPlayerId()))
					{
						return value;
					}
				}
				return null;
			}

			internal override void HandleConnectedSetChanged(global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom room)
			{
				global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>();
				global::System.Collections.Generic.List<string> list2 = new global::System.Collections.Generic.List<string>();
				global::System.Collections.Generic.Dictionary<string, global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant> dictionary = global::System.Linq.Enumerable.ToDictionary(room.Participants(), (global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant p) => p.Id());
				foreach (string key in mNativeParticipants.Keys)
				{
					global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant multiplayerParticipant = dictionary[key];
					global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant multiplayerParticipant2 = mNativeParticipants[key];
					if (!multiplayerParticipant.IsConnectedToRoom())
					{
						list2.Add(key);
					}
					if (!multiplayerParticipant2.IsConnectedToRoom() && multiplayerParticipant.IsConnectedToRoom())
					{
						list.Add(key);
					}
				}
				foreach (global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant value in mNativeParticipants.Values)
				{
					value.Dispose();
				}
				mNativeParticipants = dictionary;
				mParticipants = global::System.Linq.Enumerable.ToDictionary(global::System.Linq.Enumerable.Select(mNativeParticipants.Values, (global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant p) => p.AsParticipant()), (global::GooglePlayGames.BasicApi.Multiplayer.Participant p) => p.ParticipantId);
				global::GooglePlayGames.OurUtils.Logger.d("Updated participant statuses: " + string.Join(",", global::System.Linq.Enumerable.ToArray(global::System.Linq.Enumerable.Select(mParticipants.Values, (global::GooglePlayGames.BasicApi.Multiplayer.Participant p) => p.ToString()))));
				if (list2.Contains(GetSelf().ParticipantId))
				{
					global::GooglePlayGames.OurUtils.Logger.w("Player was disconnected from the multiplayer session.");
				}
				string selfId = GetSelf().ParticipantId;
				list = global::System.Linq.Enumerable.ToList(global::System.Linq.Enumerable.Where(list, (string peerId) => !peerId.Equals(selfId)));
				list2 = global::System.Linq.Enumerable.ToList(global::System.Linq.Enumerable.Where(list2, (string peerId) => !peerId.Equals(selfId)));
				if (list.Count > 0)
				{
					list.Sort();
					mSession.OnGameThreadListener().PeersConnected(global::System.Linq.Enumerable.ToArray(global::System.Linq.Enumerable.Where(list, (string peer) => !peer.Equals(selfId))));
				}
				if (list2.Count > 0)
				{
					list2.Sort();
					mSession.OnGameThreadListener().PeersDisconnected(global::System.Linq.Enumerable.ToArray(global::System.Linq.Enumerable.Where(list2, (string peer) => !peer.Equals(selfId))));
				}
			}

			internal override void LeaveRoom()
			{
				mSession.EnterState(new global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.LeavingRoom(mSession, mRoom, delegate
				{
					mSession.OnGameThreadListener().LeftRoom();
				}));
			}
		}

		private class ShutdownState : global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.State
		{
			private readonly global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.RoomSession mSession;

			internal ShutdownState(global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.RoomSession session)
			{
				mSession = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(session);
			}

			internal override bool IsActive()
			{
				return false;
			}

			internal override void LeaveRoom()
			{
				mSession.OnGameThreadListener().LeftRoom();
			}
		}

		private class LeavingRoom : global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.State
		{
			private readonly global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.RoomSession mSession;

			private readonly global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom mRoomToLeave;

			private readonly global::System.Action mLeavingCompleteCallback;

			internal LeavingRoom(global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.RoomSession session, global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom room, global::System.Action leavingCompleteCallback)
			{
				mSession = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(session);
				mRoomToLeave = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(room);
				mLeavingCompleteCallback = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(leavingCompleteCallback);
			}

			internal override bool IsActive()
			{
				return false;
			}

			internal override void OnStateEntered()
			{
				mSession.Manager().LeaveRoom(mRoomToLeave, delegate
				{
					mLeavingCompleteCallback();
					mSession.EnterState(new global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.ShutdownState(mSession));
				});
			}
		}

		private class AbortingRoomCreationState : global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.State
		{
			private readonly global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.RoomSession mSession;

			internal AbortingRoomCreationState(global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.RoomSession session)
			{
				mSession = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(session);
			}

			internal override bool IsActive()
			{
				return false;
			}

			internal override void HandleRoomResponse(global::GooglePlayGames.Native.PInvoke.RealtimeManager.RealTimeRoomResponse response)
			{
				if (!response.RequestSucceeded())
				{
					mSession.EnterState(new global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.ShutdownState(mSession));
					mSession.OnGameThreadListener().RoomConnected(false);
				}
				else
				{
					mSession.EnterState(new global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.LeavingRoom(mSession, response.Room(), delegate
					{
						mSession.OnGameThreadListener().RoomConnected(false);
					}));
				}
			}
		}

		private readonly object mSessionLock = new object();

		private readonly global::GooglePlayGames.Native.NativeClient mNativeClient;

		private readonly global::GooglePlayGames.Native.PInvoke.RealtimeManager mRealtimeManager;

		private volatile global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.RoomSession mCurrentSession;

		internal NativeRealtimeMultiplayerClient(global::GooglePlayGames.Native.NativeClient nativeClient, global::GooglePlayGames.Native.PInvoke.RealtimeManager manager)
		{
			mNativeClient = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(nativeClient);
			mRealtimeManager = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(manager);
			mCurrentSession = GetTerminatedSession();
			global::GooglePlayGames.OurUtils.PlayGamesHelperObject.AddPauseCallback(HandleAppPausing);
		}

		private global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.RoomSession GetTerminatedSession()
		{
			global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.RoomSession roomSession = new global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.RoomSession(mRealtimeManager, new global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.NoopListener());
			roomSession.EnterState(new global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.ShutdownState(roomSession), false);
			return roomSession;
		}

		public void CreateQuickGame(uint minOpponents, uint maxOpponents, uint variant, global::GooglePlayGames.BasicApi.Multiplayer.RealTimeMultiplayerListener listener)
		{
			CreateQuickGame(minOpponents, maxOpponents, variant, 0uL, listener);
		}

		public void CreateQuickGame(uint minOpponents, uint maxOpponents, uint variant, ulong exclusiveBitMask, global::GooglePlayGames.BasicApi.Multiplayer.RealTimeMultiplayerListener listener)
		{
			lock (mSessionLock)
			{
				global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.RoomSession newSession = new global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.RoomSession(mRealtimeManager, listener);
				if (mCurrentSession.IsActive())
				{
					global::GooglePlayGames.OurUtils.Logger.e("Received attempt to create a new room without cleaning up the old one.");
					newSession.LeaveRoom();
					return;
				}
				mCurrentSession = newSession;
				global::GooglePlayGames.OurUtils.Logger.d("QuickGame: Setting MinPlayersToStart = " + minOpponents);
				mCurrentSession.MinPlayersToStart = minOpponents;
				using (global::GooglePlayGames.Native.PInvoke.RealtimeRoomConfigBuilder realtimeRoomConfigBuilder = global::GooglePlayGames.Native.PInvoke.RealtimeRoomConfigBuilder.Create())
				{
					global::GooglePlayGames.Native.PInvoke.RealtimeRoomConfig config = realtimeRoomConfigBuilder.SetMinimumAutomatchingPlayers(minOpponents).SetMaximumAutomatchingPlayers(maxOpponents).SetVariant(variant)
						.SetExclusiveBitMask(exclusiveBitMask)
						.Build();
					using (config)
					{
						global::GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper helper = HelperForSession(newSession);
						try
						{
							newSession.StartRoomCreation(mNativeClient.GetUserId(), delegate
							{
								mRealtimeManager.CreateRoom(config, helper, newSession.HandleRoomResponse);
							});
						}
						finally
						{
							if (helper != null)
							{
								((global::System.IDisposable)helper).Dispose();
							}
						}
					}
				}
			}
		}

		private static global::GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper HelperForSession(global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.RoomSession session)
		{
			return global::GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper.Create().SetOnDataReceivedCallback(delegate(global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom room, global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant, byte[] data, bool isReliable)
			{
				session.OnDataReceived(room, participant, data, isReliable);
			}).SetOnParticipantStatusChangedCallback(delegate(global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom room, global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant)
			{
				session.OnParticipantStatusChanged(room, participant);
			})
				.SetOnRoomConnectedSetChangedCallback(delegate(global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom room)
				{
					session.OnConnectedSetChanged(room);
				})
				.SetOnRoomStatusChangedCallback(delegate(global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom room)
				{
					session.OnRoomStatusChanged(room);
				});
		}

		private void HandleAppPausing(bool paused)
		{
			if (paused)
			{
				global::GooglePlayGames.OurUtils.Logger.d("Application is pausing, which disconnects the RTMP  client.  Leaving room.");
				LeaveRoom();
			}
		}

		public void CreateWithInvitationScreen(uint minOpponents, uint maxOppponents, uint variant, global::GooglePlayGames.BasicApi.Multiplayer.RealTimeMultiplayerListener listener)
		{
			lock (mSessionLock)
			{
				global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.RoomSession newRoom = new global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.RoomSession(mRealtimeManager, listener);
				if (mCurrentSession.IsActive())
				{
					global::GooglePlayGames.OurUtils.Logger.e("Received attempt to create a new room without cleaning up the old one.");
					newRoom.LeaveRoom();
					return;
				}
				mCurrentSession = newRoom;
				mCurrentSession.ShowingUI = true;
				mRealtimeManager.ShowPlayerSelectUI(minOpponents, maxOppponents, true, delegate(global::GooglePlayGames.Native.PInvoke.PlayerSelectUIResponse response)
				{
					mCurrentSession.ShowingUI = false;
					if (response.Status() != global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.VALID)
					{
						global::GooglePlayGames.OurUtils.Logger.d("User did not complete invitation screen.");
						newRoom.LeaveRoom();
						return;
					}
					mCurrentSession.MinPlayersToStart = (uint)((int)response.MinimumAutomatchingPlayers() + global::System.Linq.Enumerable.Count(response) + 1);
					using (global::GooglePlayGames.Native.PInvoke.RealtimeRoomConfigBuilder realtimeRoomConfigBuilder = global::GooglePlayGames.Native.PInvoke.RealtimeRoomConfigBuilder.Create())
					{
						realtimeRoomConfigBuilder.SetVariant(variant);
						realtimeRoomConfigBuilder.PopulateFromUIResponse(response);
						global::GooglePlayGames.Native.PInvoke.RealtimeRoomConfig config = realtimeRoomConfigBuilder.Build();
						try
						{
							global::GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper helper = HelperForSession(newRoom);
							try
							{
								newRoom.StartRoomCreation(mNativeClient.GetUserId(), delegate
								{
									mRealtimeManager.CreateRoom(config, helper, newRoom.HandleRoomResponse);
								});
							}
							finally
							{
								if (helper != null)
								{
									((global::System.IDisposable)helper).Dispose();
								}
							}
						}
						finally
						{
							if (config != null)
							{
								((global::System.IDisposable)config).Dispose();
							}
						}
					}
				});
			}
		}

		public void ShowWaitingRoomUI()
		{
			lock (mSessionLock)
			{
				mCurrentSession.ShowWaitingRoomUI();
			}
		}

		public void GetAllInvitations(global::System.Action<global::GooglePlayGames.BasicApi.Multiplayer.Invitation[]> callback)
		{
			mRealtimeManager.FetchInvitations(delegate(global::GooglePlayGames.Native.PInvoke.RealtimeManager.FetchInvitationsResponse response)
			{
				if (!response.RequestSucceeded())
				{
					global::GooglePlayGames.OurUtils.Logger.e("Couldn't load invitations.");
					callback(new global::GooglePlayGames.BasicApi.Multiplayer.Invitation[0]);
				}
				else
				{
					global::System.Collections.Generic.List<global::GooglePlayGames.BasicApi.Multiplayer.Invitation> list = new global::System.Collections.Generic.List<global::GooglePlayGames.BasicApi.Multiplayer.Invitation>();
					foreach (global::GooglePlayGames.Native.PInvoke.MultiplayerInvitation item in response.Invitations())
					{
						using (item)
						{
							list.Add(item.AsInvitation());
						}
					}
					callback(list.ToArray());
				}
			});
		}

		public void AcceptFromInbox(global::GooglePlayGames.BasicApi.Multiplayer.RealTimeMultiplayerListener listener)
		{
			lock (mSessionLock)
			{
				global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.RoomSession newRoom = new global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.RoomSession(mRealtimeManager, listener);
				if (mCurrentSession.IsActive())
				{
					global::GooglePlayGames.OurUtils.Logger.e("Received attempt to accept invitation without cleaning up active session.");
					newRoom.LeaveRoom();
					return;
				}
				mCurrentSession = newRoom;
				mCurrentSession.ShowingUI = true;
				mRealtimeManager.ShowRoomInboxUI(delegate(global::GooglePlayGames.Native.PInvoke.RealtimeManager.RoomInboxUIResponse response)
				{
					mCurrentSession.ShowingUI = false;
					if (response.ResponseStatus() != global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.VALID)
					{
						global::GooglePlayGames.OurUtils.Logger.d("User did not complete invitation screen.");
						newRoom.LeaveRoom();
						return;
					}
					global::GooglePlayGames.Native.PInvoke.MultiplayerInvitation invitation = response.Invitation();
					global::GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper helper = HelperForSession(newRoom);
					try
					{
						global::GooglePlayGames.OurUtils.Logger.d("About to accept invitation " + invitation.Id());
						newRoom.StartRoomCreation(mNativeClient.GetUserId(), delegate
						{
							mRealtimeManager.AcceptInvitation(invitation, helper, delegate(global::GooglePlayGames.Native.PInvoke.RealtimeManager.RealTimeRoomResponse acceptResponse)
							{
								using (invitation)
								{
									newRoom.HandleRoomResponse(acceptResponse);
									newRoom.SetInvitation(invitation.AsInvitation());
								}
							});
						});
					}
					finally
					{
						if (helper != null)
						{
							((global::System.IDisposable)helper).Dispose();
						}
					}
				});
			}
		}

		public void AcceptInvitation(string invitationId, global::GooglePlayGames.BasicApi.Multiplayer.RealTimeMultiplayerListener listener)
		{
			lock (mSessionLock)
			{
				global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.RoomSession newRoom = new global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient.RoomSession(mRealtimeManager, listener);
				if (mCurrentSession.IsActive())
				{
					global::GooglePlayGames.OurUtils.Logger.e("Received attempt to accept invitation without cleaning up active session.");
					newRoom.LeaveRoom();
					return;
				}
				mCurrentSession = newRoom;
				mRealtimeManager.FetchInvitations(delegate(global::GooglePlayGames.Native.PInvoke.RealtimeManager.FetchInvitationsResponse response)
				{
					if (!response.RequestSucceeded())
					{
						global::GooglePlayGames.OurUtils.Logger.e("Couldn't load invitations.");
						newRoom.LeaveRoom();
					}
					else
					{
						foreach (global::GooglePlayGames.Native.PInvoke.MultiplayerInvitation invitation in response.Invitations())
						{
							using (invitation)
							{
								if (invitation.Id().Equals(invitationId))
								{
									mCurrentSession.MinPlayersToStart = invitation.AutomatchingSlots() + invitation.ParticipantCount();
									global::GooglePlayGames.OurUtils.Logger.d("Setting MinPlayersToStart with invitation to : " + mCurrentSession.MinPlayersToStart);
									global::GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper helper = HelperForSession(newRoom);
									try
									{
										newRoom.StartRoomCreation(mNativeClient.GetUserId(), delegate
										{
											mRealtimeManager.AcceptInvitation(invitation, helper, newRoom.HandleRoomResponse);
										});
										return;
									}
									finally
									{
										if (helper != null)
										{
											((global::System.IDisposable)helper).Dispose();
										}
									}
								}
							}
						}
						global::GooglePlayGames.OurUtils.Logger.e("Room creation failed since we could not find invitation with ID " + invitationId);
						newRoom.LeaveRoom();
					}
				});
			}
		}

		public global::GooglePlayGames.BasicApi.Multiplayer.Invitation GetInvitation()
		{
			return mCurrentSession.GetInvitation();
		}

		public void LeaveRoom()
		{
			mCurrentSession.LeaveRoom();
		}

		public void SendMessageToAll(bool reliable, byte[] data)
		{
			mCurrentSession.SendMessageToAll(reliable, data);
		}

		public void SendMessageToAll(bool reliable, byte[] data, int offset, int length)
		{
			mCurrentSession.SendMessageToAll(reliable, data, offset, length);
		}

		public void SendMessage(bool reliable, string participantId, byte[] data)
		{
			mCurrentSession.SendMessage(reliable, participantId, data);
		}

		public void SendMessage(bool reliable, string participantId, byte[] data, int offset, int length)
		{
			mCurrentSession.SendMessage(reliable, participantId, data, offset, length);
		}

		public global::System.Collections.Generic.List<global::GooglePlayGames.BasicApi.Multiplayer.Participant> GetConnectedParticipants()
		{
			return mCurrentSession.GetConnectedParticipants();
		}

		public global::GooglePlayGames.BasicApi.Multiplayer.Participant GetSelf()
		{
			return mCurrentSession.GetSelf();
		}

		public global::GooglePlayGames.BasicApi.Multiplayer.Participant GetParticipant(string participantId)
		{
			return mCurrentSession.GetParticipant(participantId);
		}

		public bool IsRoomConnected()
		{
			return mCurrentSession.IsRoomConnected();
		}

		public void DeclineInvitation(string invitationId)
		{
			mRealtimeManager.FetchInvitations(delegate(global::GooglePlayGames.Native.PInvoke.RealtimeManager.FetchInvitationsResponse response)
			{
				if (!response.RequestSucceeded())
				{
					global::GooglePlayGames.OurUtils.Logger.e("Couldn't load invitations.");
					return;
				}
				foreach (global::GooglePlayGames.Native.PInvoke.MultiplayerInvitation item in response.Invitations())
				{
					using (item)
					{
						if (item.Id().Equals(invitationId))
						{
							mRealtimeManager.DeclineInvitation(item);
						}
					}
				}
			});
		}

		private static T WithDefault<T>(T presented, T defaultValue) where T : class
		{
			return (presented == null) ? defaultValue : presented;
		}
	}
}
