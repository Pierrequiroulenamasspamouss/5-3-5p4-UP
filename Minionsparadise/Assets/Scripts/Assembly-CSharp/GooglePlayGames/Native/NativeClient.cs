namespace GooglePlayGames.Native
{
	public class NativeClient : global::GooglePlayGames.BasicApi.IPlayGamesClient
	{
		private enum AuthState
		{
			Unauthenticated = 0,
			Authenticated = 1,
			SilentPending = 2
		}

		private readonly global::GooglePlayGames.IClientImpl clientImpl;

		private readonly object GameServicesLock = new object();

		private readonly object AuthStateLock = new object();

		private readonly global::GooglePlayGames.BasicApi.PlayGamesClientConfiguration mConfiguration;

		private global::GooglePlayGames.Native.PInvoke.GameServices mServices;

		private volatile global::GooglePlayGames.Native.NativeTurnBasedMultiplayerClient mTurnBasedClient;

		private volatile global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient mRealTimeClient;

		private volatile global::GooglePlayGames.BasicApi.SavedGame.ISavedGameClient mSavedGameClient;

		private volatile global::GooglePlayGames.BasicApi.Events.IEventsClient mEventsClient;

		private volatile global::GooglePlayGames.BasicApi.Quests.IQuestsClient mQuestsClient;

		private volatile global::GooglePlayGames.TokenClient mTokenClient;

		private volatile global::System.Action<global::GooglePlayGames.BasicApi.Multiplayer.Invitation, bool> mInvitationDelegate;

		private volatile global::System.Collections.Generic.Dictionary<string, global::GooglePlayGames.BasicApi.Achievement> mAchievements;

		private volatile global::GooglePlayGames.BasicApi.Multiplayer.Player mUser;

		private volatile global::System.Collections.Generic.List<global::GooglePlayGames.BasicApi.Multiplayer.Player> mFriends;

		private volatile global::System.Action<bool> mPendingAuthCallbacks;

		private volatile global::System.Action<bool> mSilentAuthCallbacks;

		private volatile global::GooglePlayGames.Native.NativeClient.AuthState mAuthState;

		private volatile uint mAuthGeneration;

		private volatile bool mSilentAuthFailed;

		private volatile bool friendsLoading;

		private string rationale;

		private int webclientWarningFreq = 100000;

		private int noWebClientIdWarningCount;

		internal NativeClient(global::GooglePlayGames.BasicApi.PlayGamesClientConfiguration configuration, global::GooglePlayGames.IClientImpl clientImpl)
		{
			global::GooglePlayGames.OurUtils.PlayGamesHelperObject.CreateObject();
			mConfiguration = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(configuration);
			this.clientImpl = clientImpl;
			rationale = configuration.PermissionRationale;
			if (string.IsNullOrEmpty(rationale))
			{
				rationale = "Select email address to send to this game or hit cancel to not share.";
			}
		}

		private global::GooglePlayGames.Native.PInvoke.GameServices GameServices()
		{
			lock (GameServicesLock)
			{
				return mServices;
			}
		}

		public void Authenticate(global::System.Action<bool> callback, bool silent)
		{
			lock (AuthStateLock)
			{
				if (mAuthState == global::GooglePlayGames.Native.NativeClient.AuthState.Authenticated)
				{
					InvokeCallbackOnGameThread(callback, true);
					return;
				}
				if (mSilentAuthFailed && silent)
				{
					InvokeCallbackOnGameThread(callback, false);
					return;
				}
				if (callback != null)
				{
					if (silent)
					{
						mSilentAuthCallbacks = (global::System.Action<bool>)global::System.Delegate.Combine(mSilentAuthCallbacks, callback);
					}
					else
					{
						mPendingAuthCallbacks = (global::System.Action<bool>)global::System.Delegate.Combine(mPendingAuthCallbacks, callback);
					}
				}
			}
			InitializeGameServices();
			friendsLoading = false;
			if (!silent)
			{
				GameServices().StartAuthorizationUI();
			}
		}

		private static global::System.Action<T> AsOnGameThreadCallback<T>(global::System.Action<T> callback)
		{
			if (callback == null)
			{
				return delegate
				{
				};
			}
			return delegate(T result)
			{
				InvokeCallbackOnGameThread(callback, result);
			};
		}

		private static void InvokeCallbackOnGameThread<T>(global::System.Action<T> callback, T data)
		{
			if (callback != null)
			{
				global::GooglePlayGames.OurUtils.PlayGamesHelperObject.RunOnGameThread(delegate
				{
					global::GooglePlayGames.OurUtils.Logger.d("Invoking user callback on game thread");
					callback(data);
				});
			}
		}

		private void InitializeGameServices()
		{
			lock (GameServicesLock)
			{
				if (mServices != null)
				{
					return;
				}
				using (global::GooglePlayGames.Native.PInvoke.GameServicesBuilder gameServicesBuilder = global::GooglePlayGames.Native.PInvoke.GameServicesBuilder.Create())
				{
					using (global::GooglePlayGames.Native.PInvoke.PlatformConfiguration configRef = clientImpl.CreatePlatformConfiguration())
					{
						RegisterInvitationDelegate(mConfiguration.InvitationDelegate);
						gameServicesBuilder.SetOnAuthFinishedCallback(HandleAuthTransition);
						gameServicesBuilder.SetOnTurnBasedMatchEventCallback(delegate(global::GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent eventType, string matchId, global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch match)
						{
							mTurnBasedClient.HandleMatchEvent(eventType, matchId, match);
						});
						gameServicesBuilder.SetOnMultiplayerInvitationEventCallback(HandleInvitation);
						if (mConfiguration.EnableSavedGames)
						{
							gameServicesBuilder.EnableSnapshots();
						}
						if (mConfiguration.RequireGooglePlus)
						{
							gameServicesBuilder.RequireGooglePlus();
						}
						global::UnityEngine.Debug.Log("Building GPG services, implicitly attempts silent auth");
						mAuthState = global::GooglePlayGames.Native.NativeClient.AuthState.SilentPending;
						mServices = gameServicesBuilder.Build(configRef);
						mEventsClient = new global::GooglePlayGames.Native.NativeEventClient(new global::GooglePlayGames.Native.PInvoke.EventManager(mServices));
						mQuestsClient = new global::GooglePlayGames.Native.NativeQuestClient(new global::GooglePlayGames.Native.PInvoke.QuestManager(mServices));
						mTurnBasedClient = new global::GooglePlayGames.Native.NativeTurnBasedMultiplayerClient(this, new global::GooglePlayGames.Native.PInvoke.TurnBasedManager(mServices));
						mTurnBasedClient.RegisterMatchDelegate(mConfiguration.MatchDelegate);
						mRealTimeClient = new global::GooglePlayGames.Native.NativeRealtimeMultiplayerClient(this, new global::GooglePlayGames.Native.PInvoke.RealtimeManager(mServices));
						if (mConfiguration.EnableSavedGames)
						{
							mSavedGameClient = new global::GooglePlayGames.Native.NativeSavedGameClient(new global::GooglePlayGames.Native.PInvoke.SnapshotManager(mServices));
						}
						else
						{
							mSavedGameClient = new global::GooglePlayGames.Native.UnsupportedSavedGamesClient("You must enable saved games before it can be used. See PlayGamesClientConfiguration.Builder.EnableSavedGames.");
						}
						mAuthState = global::GooglePlayGames.Native.NativeClient.AuthState.SilentPending;
						mTokenClient = clientImpl.CreateTokenClient((mUser != null) ? mUser.id : null, false);
					}
				}
			}
		}

		internal void HandleInvitation(global::GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent eventType, string invitationId, global::GooglePlayGames.Native.PInvoke.MultiplayerInvitation invitation)
		{
			global::System.Action<global::GooglePlayGames.BasicApi.Multiplayer.Invitation, bool> currentHandler = mInvitationDelegate;
			if (currentHandler == null)
			{
				global::GooglePlayGames.OurUtils.Logger.d(string.Concat("Received ", eventType, " for invitation ", invitationId, " but no handler was registered."));
			}
			else if (eventType == global::GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent.REMOVED)
			{
				global::GooglePlayGames.OurUtils.Logger.d("Ignoring REMOVED for invitation " + invitationId);
			}
			else
			{
				bool shouldAutolaunch = eventType == global::GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent.UPDATED_FROM_APP_LAUNCH;
				global::GooglePlayGames.BasicApi.Multiplayer.Invitation invite = invitation.AsInvitation();
				global::GooglePlayGames.OurUtils.PlayGamesHelperObject.RunOnGameThread(delegate
				{
					currentHandler(invite, shouldAutolaunch);
				});
			}
		}

		public string GetUserEmail()
		{
			if (!IsAuthenticated())
			{
				global::UnityEngine.Debug.Log("Cannot get API client - not authenticated");
				return null;
			}
			mTokenClient.SetRationale(rationale);
			return mTokenClient.GetEmail();
		}

		public void GetUserEmail(global::System.Action<global::GooglePlayGames.BasicApi.CommonStatusCodes, string> callback)
		{
			if (!IsAuthenticated())
			{
				global::UnityEngine.Debug.Log("Cannot get API client - not authenticated");
				if (callback != null)
				{
					global::GooglePlayGames.OurUtils.PlayGamesHelperObject.RunOnGameThread(delegate
					{
						callback(global::GooglePlayGames.BasicApi.CommonStatusCodes.SignInRequired, null);
					});
					return;
				}
			}
			mTokenClient.SetRationale(rationale);
			mTokenClient.GetEmail(delegate(global::GooglePlayGames.BasicApi.CommonStatusCodes status, string email)
			{
				global::GooglePlayGames.OurUtils.PlayGamesHelperObject.RunOnGameThread(delegate
				{
					callback(status, email);
				});
			});
		}

		[global::System.Obsolete("Use GetServerAuthCode() then exchange it for a token")]
		public string GetAccessToken()
		{
			if (!IsAuthenticated())
			{
				global::UnityEngine.Debug.Log("Cannot get API client - not authenticated");
				return null;
			}
			if (!global::GooglePlayGames.GameInfo.WebClientIdInitialized())
			{
				if (noWebClientIdWarningCount++ % webclientWarningFreq == 0)
				{
					global::UnityEngine.Debug.LogError("Web client ID has not been set, cannot request access token.");
					noWebClientIdWarningCount = noWebClientIdWarningCount / webclientWarningFreq + 1;
				}
				return null;
			}
			mTokenClient.SetRationale(rationale);
			return mTokenClient.GetAccessToken();
		}

		[global::System.Obsolete("Use GetServerAuthCode() then exchange it for a token")]
		public void GetIdToken(global::System.Action<string> idTokenCallback)
		{
			if (!IsAuthenticated())
			{
				global::UnityEngine.Debug.Log("Cannot get API client - not authenticated");
				global::GooglePlayGames.OurUtils.PlayGamesHelperObject.RunOnGameThread(delegate
				{
					idTokenCallback(null);
				});
			}
			if (!global::GooglePlayGames.GameInfo.WebClientIdInitialized())
			{
				if (noWebClientIdWarningCount++ % webclientWarningFreq == 0)
				{
					global::UnityEngine.Debug.LogError("Web client ID has not been set, cannot request id token.");
					noWebClientIdWarningCount = noWebClientIdWarningCount / webclientWarningFreq + 1;
				}
				global::GooglePlayGames.OurUtils.PlayGamesHelperObject.RunOnGameThread(delegate
				{
					idTokenCallback(null);
				});
			}
			mTokenClient.SetRationale(rationale);
			mTokenClient.GetIdToken("247013943331-p01pqndt6uksnjbqv9vpv9qo2uj8ks7b.apps.googleusercontent.com", AsOnGameThreadCallback(idTokenCallback));
		}

		public void GetServerAuthCode(string serverClientId, global::System.Action<global::GooglePlayGames.BasicApi.CommonStatusCodes, string> callback)
		{
			mServices.FetchServerAuthCode(serverClientId, delegate(global::GooglePlayGames.Native.PInvoke.GameServices.FetchServerAuthCodeResponse serverAuthCodeResponse)
			{
				global::GooglePlayGames.BasicApi.CommonStatusCodes responseCode = global::GooglePlayGames.Native.ConversionUtils.ConvertResponseStatusToCommonStatus(serverAuthCodeResponse.Status());
				if (responseCode != global::GooglePlayGames.BasicApi.CommonStatusCodes.Success && responseCode != global::GooglePlayGames.BasicApi.CommonStatusCodes.SuccessCached)
				{
					global::GooglePlayGames.OurUtils.Logger.e("Error loading server auth code: " + serverAuthCodeResponse.Status());
				}
				if (callback != null)
				{
					string authCode = serverAuthCodeResponse.Code();
					global::GooglePlayGames.OurUtils.PlayGamesHelperObject.RunOnGameThread(delegate
					{
						callback(responseCode, authCode);
					});
				}
			});
		}

		public bool IsAuthenticated()
		{
			lock (AuthStateLock)
			{
				return mAuthState == global::GooglePlayGames.Native.NativeClient.AuthState.Authenticated;
			}
		}

		public void LoadFriends(global::System.Action<bool> callback)
		{
			if (!IsAuthenticated())
			{
				global::GooglePlayGames.OurUtils.Logger.d("Cannot loadFriends when not authenticated");
				global::GooglePlayGames.OurUtils.PlayGamesHelperObject.RunOnGameThread(delegate
				{
					callback(false);
				});
				return;
			}
			if (mFriends != null)
			{
				global::GooglePlayGames.OurUtils.PlayGamesHelperObject.RunOnGameThread(delegate
				{
					callback(true);
				});
				return;
			}
			mServices.PlayerManager().FetchFriends(delegate(global::GooglePlayGames.BasicApi.ResponseStatus status, global::System.Collections.Generic.List<global::GooglePlayGames.BasicApi.Multiplayer.Player> players)
			{
				if (status == global::GooglePlayGames.BasicApi.ResponseStatus.Success || status == global::GooglePlayGames.BasicApi.ResponseStatus.SuccessWithStale)
				{
					mFriends = players;
					global::GooglePlayGames.OurUtils.PlayGamesHelperObject.RunOnGameThread(delegate
					{
						callback(true);
					});
				}
				else
				{
					mFriends = new global::System.Collections.Generic.List<global::GooglePlayGames.BasicApi.Multiplayer.Player>();
					global::GooglePlayGames.OurUtils.Logger.e(string.Concat("Got ", status, " loading friends"));
					global::GooglePlayGames.OurUtils.PlayGamesHelperObject.RunOnGameThread(delegate
					{
						callback(false);
					});
				}
			});
		}

		public global::UnityEngine.SocialPlatforms.IUserProfile[] GetFriends()
		{
			if (mFriends == null && !friendsLoading)
			{
				global::GooglePlayGames.OurUtils.Logger.w("Getting friends before they are loaded!!!");
				friendsLoading = true;
				LoadFriends(delegate(bool ok)
				{
					global::GooglePlayGames.OurUtils.Logger.d("loading: " + ok + " mFriends = " + mFriends);
					if (!ok)
					{
						global::GooglePlayGames.OurUtils.Logger.e("Friends list did not load successfully.  Disabling loading until re-authenticated");
					}
					friendsLoading = !ok;
				});
			}
			return (mFriends != null) ? mFriends.ToArray() : new global::UnityEngine.SocialPlatforms.IUserProfile[0];
		}

		private void PopulateAchievements(uint authGeneration, global::GooglePlayGames.Native.PInvoke.AchievementManager.FetchAllResponse response)
		{
			if (authGeneration != mAuthGeneration)
			{
				global::GooglePlayGames.OurUtils.Logger.d("Received achievement callback after signout occurred, ignoring");
				return;
			}
			global::GooglePlayGames.OurUtils.Logger.d("Populating Achievements, status = " + response.Status());
			lock (AuthStateLock)
			{
				if (response.Status() != global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID && response.Status() != global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID_BUT_STALE)
				{
					global::GooglePlayGames.OurUtils.Logger.e("Error retrieving achievements - check the log for more information. Failing signin.");
					global::System.Action<bool> action = mPendingAuthCallbacks;
					mPendingAuthCallbacks = null;
					if (action != null)
					{
						InvokeCallbackOnGameThread(action, false);
					}
					SignOut();
					return;
				}
				global::System.Collections.Generic.Dictionary<string, global::GooglePlayGames.BasicApi.Achievement> dictionary = new global::System.Collections.Generic.Dictionary<string, global::GooglePlayGames.BasicApi.Achievement>();
				foreach (global::GooglePlayGames.Native.PInvoke.NativeAchievement item in response)
				{
					using (item)
					{
						dictionary[item.Id()] = item.AsAchievement();
					}
				}
				global::GooglePlayGames.OurUtils.Logger.d("Found " + dictionary.Count + " Achievements");
				mAchievements = dictionary;
			}
			global::GooglePlayGames.OurUtils.Logger.d("Maybe finish for Achievements");
			MaybeFinishAuthentication();
		}

		private void MaybeFinishAuthentication()
		{
			global::System.Action<bool> action = null;
			lock (AuthStateLock)
			{
				if (mUser == null || mAchievements == null)
				{
					global::GooglePlayGames.OurUtils.Logger.d(string.Concat("Auth not finished. User=", mUser, " achievements=", mAchievements));
					return;
				}
				global::GooglePlayGames.OurUtils.Logger.d("Auth finished. Proceeding.");
				action = mPendingAuthCallbacks;
				mPendingAuthCallbacks = null;
				mAuthState = global::GooglePlayGames.Native.NativeClient.AuthState.Authenticated;
			}
			if (action != null)
			{
				global::GooglePlayGames.OurUtils.Logger.d("Invoking Callbacks: " + action);
				InvokeCallbackOnGameThread(action, true);
			}
		}

		private void PopulateUser(uint authGeneration, global::GooglePlayGames.Native.PInvoke.PlayerManager.FetchSelfResponse response)
		{
			global::GooglePlayGames.OurUtils.Logger.d("Populating User");
			if (authGeneration != mAuthGeneration)
			{
				global::GooglePlayGames.OurUtils.Logger.d("Received user callback after signout occurred, ignoring");
				return;
			}
			lock (AuthStateLock)
			{
				if (response.Status() != global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID && response.Status() != global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID_BUT_STALE)
				{
					global::GooglePlayGames.OurUtils.Logger.e("Error retrieving user, signing out");
					global::System.Action<bool> action = mPendingAuthCallbacks;
					mPendingAuthCallbacks = null;
					if (action != null)
					{
						InvokeCallbackOnGameThread(action, false);
					}
					SignOut();
					return;
				}
				mUser = response.Self().AsPlayer();
				mFriends = null;
				mTokenClient = clientImpl.CreateTokenClient(mUser.id, true);
			}
			global::GooglePlayGames.OurUtils.Logger.d("Found User: " + mUser);
			global::GooglePlayGames.OurUtils.Logger.d("Maybe finish for User");
			MaybeFinishAuthentication();
		}

		private void HandleAuthTransition(global::GooglePlayGames.Native.Cwrapper.Types.AuthOperation operation, global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.AuthStatus status)
		{
			global::GooglePlayGames.OurUtils.Logger.d(string.Concat("Starting Auth Transition. Op: ", operation, " status: ", status));
			lock (AuthStateLock)
			{
				switch (operation)
				{
				case global::GooglePlayGames.Native.Cwrapper.Types.AuthOperation.SIGN_IN:
					if (status == global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.AuthStatus.VALID)
					{
						if (mSilentAuthCallbacks != null)
						{
							mPendingAuthCallbacks = (global::System.Action<bool>)global::System.Delegate.Combine(mPendingAuthCallbacks, mSilentAuthCallbacks);
							mSilentAuthCallbacks = null;
						}
						uint currentAuthGeneration = mAuthGeneration;
						mServices.AchievementManager().FetchAll(delegate(global::GooglePlayGames.Native.PInvoke.AchievementManager.FetchAllResponse results)
						{
							PopulateAchievements(currentAuthGeneration, results);
						});
						mServices.PlayerManager().FetchSelf(delegate(global::GooglePlayGames.Native.PInvoke.PlayerManager.FetchSelfResponse results)
						{
							PopulateUser(currentAuthGeneration, results);
						});
					}
					else if (mAuthState == global::GooglePlayGames.Native.NativeClient.AuthState.SilentPending)
					{
						mSilentAuthFailed = true;
						mAuthState = global::GooglePlayGames.Native.NativeClient.AuthState.Unauthenticated;
						global::System.Action<bool> callback = mSilentAuthCallbacks;
						mSilentAuthCallbacks = null;
						global::UnityEngine.Debug.Log("Invoking callbacks, AuthState changed from silentPending to Unauthenticated.");
						InvokeCallbackOnGameThread(callback, false);
						if (mPendingAuthCallbacks != null)
						{
							global::UnityEngine.Debug.Log("there are pending auth callbacks - starting AuthUI");
							GameServices().StartAuthorizationUI();
						}
					}
					else
					{
						global::UnityEngine.Debug.Log(string.Concat("AuthState == ", mAuthState, " calling auth callbacks with failure"));
						UnpauseUnityPlayer();
						global::System.Action<bool> callback2 = mPendingAuthCallbacks;
						mPendingAuthCallbacks = null;
						InvokeCallbackOnGameThread(callback2, false);
					}
					break;
				case global::GooglePlayGames.Native.Cwrapper.Types.AuthOperation.SIGN_OUT:
					ToUnauthenticated();
					break;
				default:
					global::GooglePlayGames.OurUtils.Logger.e("Unknown AuthOperation " + operation);
					break;
				}
			}
		}

		private void UnpauseUnityPlayer()
		{
		}

		private void ToUnauthenticated()
		{
			lock (AuthStateLock)
			{
				mUser = null;
				mFriends = null;
				mAchievements = null;
				mAuthState = global::GooglePlayGames.Native.NativeClient.AuthState.Unauthenticated;
				mTokenClient = clientImpl.CreateTokenClient(null, true);
				mAuthGeneration++;
			}
		}

		public void SignOut()
		{
			ToUnauthenticated();
			if (GameServices() != null)
			{
				GameServices().SignOut();
			}
		}

		public string GetUserId()
		{
			if (mUser == null)
			{
				return null;
			}
			return mUser.id;
		}

		public string GetUserDisplayName()
		{
			if (mUser == null)
			{
				return null;
			}
			return mUser.userName;
		}

		public string GetUserImageUrl()
		{
			if (mUser == null)
			{
				return null;
			}
			return mUser.AvatarURL;
		}

		public void GetPlayerStats(global::System.Action<global::GooglePlayGames.BasicApi.CommonStatusCodes, global::GooglePlayGames.BasicApi.PlayerStats> callback)
		{
			global::GooglePlayGames.OurUtils.PlayGamesHelperObject.RunOnGameThread(delegate
			{
				clientImpl.GetPlayerStats(GetApiClient(), callback);
			});
		}

		public void LoadUsers(string[] userIds, global::System.Action<global::UnityEngine.SocialPlatforms.IUserProfile[]> callback)
		{
			mServices.PlayerManager().FetchList(userIds, delegate(global::GooglePlayGames.Native.PInvoke.NativePlayer[] nativeUsers)
			{
				global::UnityEngine.SocialPlatforms.IUserProfile[] users = new global::UnityEngine.SocialPlatforms.IUserProfile[nativeUsers.Length];
				for (int i = 0; i < users.Length; i++)
				{
					users[i] = nativeUsers[i].AsPlayer();
				}
				global::GooglePlayGames.OurUtils.PlayGamesHelperObject.RunOnGameThread(delegate
				{
					callback(users);
				});
			});
		}

		public global::GooglePlayGames.BasicApi.Achievement GetAchievement(string achId)
		{
			if (mAchievements == null || !mAchievements.ContainsKey(achId))
			{
				return null;
			}
			return mAchievements[achId];
		}

		public void LoadAchievements(global::System.Action<global::GooglePlayGames.BasicApi.Achievement[]> callback)
		{
			global::GooglePlayGames.BasicApi.Achievement[] data = new global::GooglePlayGames.BasicApi.Achievement[mAchievements.Count];
			mAchievements.Values.CopyTo(data, 0);
			global::GooglePlayGames.OurUtils.PlayGamesHelperObject.RunOnGameThread(delegate
			{
				callback(data);
			});
		}

		public void UnlockAchievement(string achId, global::System.Action<bool> callback)
		{
			UpdateAchievement("Unlock", achId, callback, (global::GooglePlayGames.BasicApi.Achievement a) => a.IsUnlocked, delegate(global::GooglePlayGames.BasicApi.Achievement a)
			{
				a.IsUnlocked = true;
				GameServices().AchievementManager().Unlock(achId);
			});
		}

		public void RevealAchievement(string achId, global::System.Action<bool> callback)
		{
			UpdateAchievement("Reveal", achId, callback, (global::GooglePlayGames.BasicApi.Achievement a) => a.IsRevealed, delegate(global::GooglePlayGames.BasicApi.Achievement a)
			{
				a.IsRevealed = true;
				GameServices().AchievementManager().Reveal(achId);
			});
		}

		private void UpdateAchievement(string updateType, string achId, global::System.Action<bool> callback, global::System.Predicate<global::GooglePlayGames.BasicApi.Achievement> alreadyDone, global::System.Action<global::GooglePlayGames.BasicApi.Achievement> updateAchievment)
		{
			callback = AsOnGameThreadCallback(callback);
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(achId);
			InitializeGameServices();
			global::GooglePlayGames.BasicApi.Achievement achievement = GetAchievement(achId);
			if (achievement == null)
			{
				global::GooglePlayGames.OurUtils.Logger.d("Could not " + updateType + ", no achievement with ID " + achId);
				callback(false);
				return;
			}
			if (alreadyDone(achievement))
			{
				global::GooglePlayGames.OurUtils.Logger.d("Did not need to perform " + updateType + ": on achievement " + achId);
				callback(true);
				return;
			}
			global::GooglePlayGames.OurUtils.Logger.d("Performing " + updateType + " on " + achId);
			updateAchievment(achievement);
			GameServices().AchievementManager().Fetch(achId, delegate(global::GooglePlayGames.Native.PInvoke.AchievementManager.FetchResponse rsp)
			{
				if (rsp.Status() == global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID)
				{
					mAchievements.Remove(achId);
					mAchievements.Add(achId, rsp.Achievement().AsAchievement());
					callback(true);
				}
				else
				{
					global::GooglePlayGames.OurUtils.Logger.e("Cannot refresh achievement " + achId + ": " + rsp.Status());
					callback(false);
				}
			});
		}

		public void IncrementAchievement(string achId, int steps, global::System.Action<bool> callback)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(achId);
			callback = AsOnGameThreadCallback(callback);
			InitializeGameServices();
			global::GooglePlayGames.BasicApi.Achievement achievement = GetAchievement(achId);
			if (achievement == null)
			{
				global::GooglePlayGames.OurUtils.Logger.e("Could not increment, no achievement with ID " + achId);
				callback(false);
				return;
			}
			if (!achievement.IsIncremental)
			{
				global::GooglePlayGames.OurUtils.Logger.e("Could not increment, achievement with ID " + achId + " was not incremental");
				callback(false);
				return;
			}
			if (steps < 0)
			{
				global::GooglePlayGames.OurUtils.Logger.e("Attempted to increment by negative steps");
				callback(false);
				return;
			}
			GameServices().AchievementManager().Increment(achId, global::System.Convert.ToUInt32(steps));
			GameServices().AchievementManager().Fetch(achId, delegate(global::GooglePlayGames.Native.PInvoke.AchievementManager.FetchResponse rsp)
			{
				if (rsp.Status() == global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID)
				{
					mAchievements.Remove(achId);
					mAchievements.Add(achId, rsp.Achievement().AsAchievement());
					callback(true);
				}
				else
				{
					global::GooglePlayGames.OurUtils.Logger.e("Cannot refresh achievement " + achId + ": " + rsp.Status());
					callback(false);
				}
			});
		}

		public void SetStepsAtLeast(string achId, int steps, global::System.Action<bool> callback)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(achId);
			callback = AsOnGameThreadCallback(callback);
			InitializeGameServices();
			global::GooglePlayGames.BasicApi.Achievement achievement = GetAchievement(achId);
			if (achievement == null)
			{
				global::GooglePlayGames.OurUtils.Logger.e("Could not increment, no achievement with ID " + achId);
				callback(false);
				return;
			}
			if (!achievement.IsIncremental)
			{
				global::GooglePlayGames.OurUtils.Logger.e("Could not increment, achievement with ID " + achId + " is not incremental");
				callback(false);
				return;
			}
			if (steps < 0)
			{
				global::GooglePlayGames.OurUtils.Logger.e("Attempted to increment by negative steps");
				callback(false);
				return;
			}
			GameServices().AchievementManager().SetStepsAtLeast(achId, global::System.Convert.ToUInt32(steps));
			GameServices().AchievementManager().Fetch(achId, delegate(global::GooglePlayGames.Native.PInvoke.AchievementManager.FetchResponse rsp)
			{
				if (rsp.Status() == global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID)
				{
					mAchievements.Remove(achId);
					mAchievements.Add(achId, rsp.Achievement().AsAchievement());
					callback(true);
				}
				else
				{
					global::GooglePlayGames.OurUtils.Logger.e("Cannot refresh achievement " + achId + ": " + rsp.Status());
					callback(false);
				}
			});
		}

		public void ShowAchievementsUI(global::System.Action<global::GooglePlayGames.BasicApi.UIStatus> cb)
		{
			if (!IsAuthenticated())
			{
				return;
			}
			global::System.Action<global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus> callback = global::GooglePlayGames.Native.PInvoke.Callbacks.NoopUICallback;
			if (cb != null)
			{
				callback = delegate(global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus result)
				{
					cb((global::GooglePlayGames.BasicApi.UIStatus)result);
				};
			}
			callback = AsOnGameThreadCallback(callback);
			GameServices().AchievementManager().ShowAllUI(callback);
		}

		public int LeaderboardMaxResults()
		{
			return GameServices().LeaderboardManager().LeaderboardMaxResults;
		}

		public void ShowLeaderboardUI(string leaderboardId, global::GooglePlayGames.BasicApi.LeaderboardTimeSpan span, global::System.Action<global::GooglePlayGames.BasicApi.UIStatus> cb)
		{
			if (!IsAuthenticated())
			{
				return;
			}
			global::System.Action<global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus> callback = global::GooglePlayGames.Native.PInvoke.Callbacks.NoopUICallback;
			if (cb != null)
			{
				callback = delegate(global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus result)
				{
					cb((global::GooglePlayGames.BasicApi.UIStatus)result);
				};
			}
			callback = AsOnGameThreadCallback(callback);
			if (leaderboardId == null)
			{
				GameServices().LeaderboardManager().ShowAllUI(callback);
			}
			else
			{
				GameServices().LeaderboardManager().ShowUI(leaderboardId, span, callback);
			}
		}

		public void LoadScores(string leaderboardId, global::GooglePlayGames.BasicApi.LeaderboardStart start, int rowCount, global::GooglePlayGames.BasicApi.LeaderboardCollection collection, global::GooglePlayGames.BasicApi.LeaderboardTimeSpan timeSpan, global::System.Action<global::GooglePlayGames.BasicApi.LeaderboardScoreData> callback)
		{
			callback = AsOnGameThreadCallback(callback);
			GameServices().LeaderboardManager().LoadLeaderboardData(leaderboardId, start, rowCount, collection, timeSpan, mUser.id, callback);
		}

		public void LoadMoreScores(global::GooglePlayGames.BasicApi.ScorePageToken token, int rowCount, global::System.Action<global::GooglePlayGames.BasicApi.LeaderboardScoreData> callback)
		{
			callback = AsOnGameThreadCallback(callback);
			GameServices().LeaderboardManager().LoadScorePage(null, rowCount, token, callback);
		}

		public void SubmitScore(string leaderboardId, long score, global::System.Action<bool> callback)
		{
			callback = AsOnGameThreadCallback(callback);
			if (!IsAuthenticated())
			{
				callback(false);
			}
			InitializeGameServices();
			if (leaderboardId == null)
			{
				throw new global::System.ArgumentNullException("leaderboardId");
			}
			GameServices().LeaderboardManager().SubmitScore(leaderboardId, score, null);
			callback(true);
		}

		public void SubmitScore(string leaderboardId, long score, string metadata, global::System.Action<bool> callback)
		{
			callback = AsOnGameThreadCallback(callback);
			if (!IsAuthenticated())
			{
				callback(false);
			}
			InitializeGameServices();
			if (leaderboardId == null)
			{
				throw new global::System.ArgumentNullException("leaderboardId");
			}
			GameServices().LeaderboardManager().SubmitScore(leaderboardId, score, metadata);
			callback(true);
		}

		public global::GooglePlayGames.BasicApi.Multiplayer.IRealTimeMultiplayerClient GetRtmpClient()
		{
			if (!IsAuthenticated())
			{
				return null;
			}
			lock (GameServicesLock)
			{
				return mRealTimeClient;
			}
		}

		public global::GooglePlayGames.BasicApi.Multiplayer.ITurnBasedMultiplayerClient GetTbmpClient()
		{
			lock (GameServicesLock)
			{
				return mTurnBasedClient;
			}
		}

		public global::GooglePlayGames.BasicApi.SavedGame.ISavedGameClient GetSavedGameClient()
		{
			lock (GameServicesLock)
			{
				return mSavedGameClient;
			}
		}

		public global::GooglePlayGames.BasicApi.Events.IEventsClient GetEventsClient()
		{
			lock (GameServicesLock)
			{
				return mEventsClient;
			}
		}

		public global::GooglePlayGames.BasicApi.Quests.IQuestsClient GetQuestsClient()
		{
			lock (GameServicesLock)
			{
				return mQuestsClient;
			}
		}

		public void RegisterInvitationDelegate(global::GooglePlayGames.BasicApi.InvitationReceivedDelegate invitationDelegate)
		{
			if (invitationDelegate == null)
			{
				mInvitationDelegate = null;
				return;
			}
			mInvitationDelegate = global::GooglePlayGames.Native.PInvoke.Callbacks.AsOnGameThreadCallback(delegate(global::GooglePlayGames.BasicApi.Multiplayer.Invitation invitation, bool autoAccept)
			{
				invitationDelegate(invitation, autoAccept);
			});
		}

		public string GetToken()
		{
			if (mTokenClient != null)
			{
				return mTokenClient.GetAccessToken();
			}
			return null;
		}

		public global::System.IntPtr GetApiClient()
		{
			return global::GooglePlayGames.Native.Cwrapper.InternalHooks.InternalHooks_GetApiClient(mServices.AsHandle());
		}
	}
}
