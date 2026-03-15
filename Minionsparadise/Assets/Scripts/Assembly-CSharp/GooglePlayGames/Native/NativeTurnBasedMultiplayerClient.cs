namespace GooglePlayGames.Native
{
	public class NativeTurnBasedMultiplayerClient : global::GooglePlayGames.BasicApi.Multiplayer.ITurnBasedMultiplayerClient
	{
		private readonly global::GooglePlayGames.Native.PInvoke.TurnBasedManager mTurnBasedManager;

		private readonly global::GooglePlayGames.Native.NativeClient mNativeClient;

		private volatile global::System.Action<global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch, bool> mMatchDelegate;

		internal NativeTurnBasedMultiplayerClient(global::GooglePlayGames.Native.NativeClient nativeClient, global::GooglePlayGames.Native.PInvoke.TurnBasedManager manager)
		{
			mTurnBasedManager = manager;
			mNativeClient = nativeClient;
		}

		public void CreateQuickMatch(uint minOpponents, uint maxOpponents, uint variant, global::System.Action<bool, global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback)
		{
			CreateQuickMatch(minOpponents, maxOpponents, variant, 0uL, callback);
		}

		public void CreateQuickMatch(uint minOpponents, uint maxOpponents, uint variant, ulong exclusiveBitmask, global::System.Action<bool, global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback)
		{
			callback = global::GooglePlayGames.Native.PInvoke.Callbacks.AsOnGameThreadCallback(callback);
			using (global::GooglePlayGames.Native.PInvoke.TurnBasedMatchConfigBuilder turnBasedMatchConfigBuilder = global::GooglePlayGames.Native.PInvoke.TurnBasedMatchConfigBuilder.Create())
			{
				turnBasedMatchConfigBuilder.SetVariant(variant).SetMinimumAutomatchingPlayers(minOpponents).SetMaximumAutomatchingPlayers(maxOpponents)
					.SetExclusiveBitMask(exclusiveBitmask);
				using (global::GooglePlayGames.Native.PInvoke.TurnBasedMatchConfig config = turnBasedMatchConfigBuilder.Build())
				{
					mTurnBasedManager.CreateMatch(config, BridgeMatchToUserCallback(delegate(global::GooglePlayGames.BasicApi.UIStatus status, global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match)
					{
						callback(status == global::GooglePlayGames.BasicApi.UIStatus.Valid, match);
					}));
				}
			}
		}

		public void CreateWithInvitationScreen(uint minOpponents, uint maxOpponents, uint variant, global::System.Action<bool, global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback)
		{
			CreateWithInvitationScreen(minOpponents, maxOpponents, variant, delegate(global::GooglePlayGames.BasicApi.UIStatus status, global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match)
			{
				callback(status == global::GooglePlayGames.BasicApi.UIStatus.Valid, match);
			});
		}

		public void CreateWithInvitationScreen(uint minOpponents, uint maxOpponents, uint variant, global::System.Action<global::GooglePlayGames.BasicApi.UIStatus, global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback)
		{
			callback = global::GooglePlayGames.Native.PInvoke.Callbacks.AsOnGameThreadCallback(callback);
			mTurnBasedManager.ShowPlayerSelectUI(minOpponents, maxOpponents, true, delegate(global::GooglePlayGames.Native.PInvoke.PlayerSelectUIResponse result)
			{
				if (result.Status() != global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.VALID)
				{
					callback((global::GooglePlayGames.BasicApi.UIStatus)result.Status(), null);
					return;
				}
				using (global::GooglePlayGames.Native.PInvoke.TurnBasedMatchConfigBuilder turnBasedMatchConfigBuilder = global::GooglePlayGames.Native.PInvoke.TurnBasedMatchConfigBuilder.Create())
				{
					turnBasedMatchConfigBuilder.PopulateFromUIResponse(result).SetVariant(variant);
					using (global::GooglePlayGames.Native.PInvoke.TurnBasedMatchConfig config = turnBasedMatchConfigBuilder.Build())
					{
						mTurnBasedManager.CreateMatch(config, BridgeMatchToUserCallback(callback));
					}
				}
			});
		}

		public void GetAllInvitations(global::System.Action<global::GooglePlayGames.BasicApi.Multiplayer.Invitation[]> callback)
		{
			mTurnBasedManager.GetAllTurnbasedMatches(delegate(global::GooglePlayGames.Native.PInvoke.TurnBasedManager.TurnBasedMatchesResponse allMatches)
			{
				global::GooglePlayGames.BasicApi.Multiplayer.Invitation[] array = new global::GooglePlayGames.BasicApi.Multiplayer.Invitation[allMatches.InvitationCount()];
				int num = 0;
				foreach (global::GooglePlayGames.Native.PInvoke.MultiplayerInvitation item in allMatches.Invitations())
				{
					array[num++] = item.AsInvitation();
				}
				callback(array);
			});
		}

		public void GetAllMatches(global::System.Action<global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch[]> callback)
		{
			mTurnBasedManager.GetAllTurnbasedMatches(delegate(global::GooglePlayGames.Native.PInvoke.TurnBasedManager.TurnBasedMatchesResponse allMatches)
			{
				int num = allMatches.MyTurnMatchesCount() + allMatches.TheirTurnMatchesCount() + allMatches.CompletedMatchesCount();
				global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch[] array = new global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch[num];
				int num2 = 0;
				foreach (global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch item in allMatches.MyTurnMatches())
				{
					array[num2++] = item.AsTurnBasedMatch(mNativeClient.GetUserId());
				}
				foreach (global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch item2 in allMatches.TheirTurnMatches())
				{
					array[num2++] = item2.AsTurnBasedMatch(mNativeClient.GetUserId());
				}
				foreach (global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch item3 in allMatches.CompletedMatches())
				{
					array[num2++] = item3.AsTurnBasedMatch(mNativeClient.GetUserId());
				}
				callback(array);
			});
		}

		private global::System.Action<global::GooglePlayGames.Native.PInvoke.TurnBasedManager.TurnBasedMatchResponse> BridgeMatchToUserCallback(global::System.Action<global::GooglePlayGames.BasicApi.UIStatus, global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> userCallback)
		{
			return delegate(global::GooglePlayGames.Native.PInvoke.TurnBasedManager.TurnBasedMatchResponse callbackResult)
			{
				using (global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch nativeTurnBasedMatch = callbackResult.Match())
				{
					if (nativeTurnBasedMatch == null)
					{
						global::GooglePlayGames.BasicApi.UIStatus arg = global::GooglePlayGames.BasicApi.UIStatus.InternalError;
						switch (callbackResult.ResponseStatus())
						{
						case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus.VALID:
							arg = global::GooglePlayGames.BasicApi.UIStatus.Valid;
							break;
						case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus.VALID_BUT_STALE:
							arg = global::GooglePlayGames.BasicApi.UIStatus.Valid;
							break;
						case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus.ERROR_INTERNAL:
							arg = global::GooglePlayGames.BasicApi.UIStatus.InternalError;
							break;
						case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus.ERROR_NOT_AUTHORIZED:
							arg = global::GooglePlayGames.BasicApi.UIStatus.NotAuthorized;
							break;
						case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus.ERROR_VERSION_UPDATE_REQUIRED:
							arg = global::GooglePlayGames.BasicApi.UIStatus.VersionUpdateRequired;
							break;
						case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus.ERROR_TIMEOUT:
							arg = global::GooglePlayGames.BasicApi.UIStatus.Timeout;
							break;
						}
						userCallback(arg, null);
					}
					else
					{
						global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch turnBasedMatch = nativeTurnBasedMatch.AsTurnBasedMatch(mNativeClient.GetUserId());
						global::GooglePlayGames.OurUtils.Logger.d("Passing converted match to user callback:" + turnBasedMatch);
						userCallback(global::GooglePlayGames.BasicApi.UIStatus.Valid, turnBasedMatch);
					}
				}
			};
		}

		public void AcceptFromInbox(global::System.Action<bool, global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback)
		{
			callback = global::GooglePlayGames.Native.PInvoke.Callbacks.AsOnGameThreadCallback(callback);
			mTurnBasedManager.ShowInboxUI(delegate(global::GooglePlayGames.Native.PInvoke.TurnBasedManager.MatchInboxUIResponse callbackResult)
			{
				using (global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch nativeTurnBasedMatch = callbackResult.Match())
				{
					if (nativeTurnBasedMatch == null)
					{
						callback(false, null);
					}
					else
					{
						global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch turnBasedMatch = nativeTurnBasedMatch.AsTurnBasedMatch(mNativeClient.GetUserId());
						global::GooglePlayGames.OurUtils.Logger.d("Passing converted match to user callback:" + turnBasedMatch);
						callback(true, turnBasedMatch);
					}
				}
			});
		}

		public void AcceptInvitation(string invitationId, global::System.Action<bool, global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback)
		{
			callback = global::GooglePlayGames.Native.PInvoke.Callbacks.AsOnGameThreadCallback(callback);
			FindInvitationWithId(invitationId, delegate(global::GooglePlayGames.Native.PInvoke.MultiplayerInvitation invitation)
			{
				if (invitation == null)
				{
					global::GooglePlayGames.OurUtils.Logger.e("Could not find invitation with id " + invitationId);
					callback(false, null);
				}
				else
				{
					mTurnBasedManager.AcceptInvitation(invitation, BridgeMatchToUserCallback(delegate(global::GooglePlayGames.BasicApi.UIStatus status, global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match)
					{
						callback(status == global::GooglePlayGames.BasicApi.UIStatus.Valid, match);
					}));
				}
			});
		}

		private void FindInvitationWithId(string invitationId, global::System.Action<global::GooglePlayGames.Native.PInvoke.MultiplayerInvitation> callback)
		{
			mTurnBasedManager.GetAllTurnbasedMatches(delegate(global::GooglePlayGames.Native.PInvoke.TurnBasedManager.TurnBasedMatchesResponse allMatches)
			{
				if (allMatches.Status() <= (global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus)0)
				{
					callback(null);
				}
				else
				{
					foreach (global::GooglePlayGames.Native.PInvoke.MultiplayerInvitation item in allMatches.Invitations())
					{
						using (item)
						{
							if (item.Id().Equals(invitationId))
							{
								callback(item);
								return;
							}
						}
					}
					callback(null);
				}
			});
		}

		public void RegisterMatchDelegate(global::GooglePlayGames.BasicApi.Multiplayer.MatchDelegate del)
		{
			if (del == null)
			{
				mMatchDelegate = null;
				return;
			}
			mMatchDelegate = global::GooglePlayGames.Native.PInvoke.Callbacks.AsOnGameThreadCallback(delegate(global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, bool autoLaunch)
			{
				del(match, autoLaunch);
			});
		}

		internal void HandleMatchEvent(global::GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent eventType, string matchId, global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch match)
		{
			global::System.Action<global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch, bool> currentDelegate = mMatchDelegate;
			if (currentDelegate == null)
			{
				return;
			}
			if (eventType == global::GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent.REMOVED)
			{
				global::GooglePlayGames.OurUtils.Logger.d("Ignoring REMOVE event for match " + matchId);
				return;
			}
			bool shouldAutolaunch = eventType == global::GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent.UPDATED_FROM_APP_LAUNCH;
			match.ReferToMe();
			global::GooglePlayGames.Native.PInvoke.Callbacks.AsCoroutine(WaitForLogin(delegate
			{
				currentDelegate(match.AsTurnBasedMatch(mNativeClient.GetUserId()), shouldAutolaunch);
				match.ForgetMe();
			}));
		}

		private global::System.Collections.IEnumerator WaitForLogin(global::System.Action method)
		{
			if (string.IsNullOrEmpty(mNativeClient.GetUserId()))
			{
				yield return null;
			}
			method();
		}

		public void TakeTurn(global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, byte[] data, string pendingParticipantId, global::System.Action<bool> callback)
		{
			global::GooglePlayGames.OurUtils.Logger.describe(data);
			callback = global::GooglePlayGames.Native.PInvoke.Callbacks.AsOnGameThreadCallback(callback);
			FindEqualVersionMatchWithParticipant(match, pendingParticipantId, callback, delegate(global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant pendingParticipant, global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch foundMatch)
			{
				mTurnBasedManager.TakeTurn(foundMatch, data, pendingParticipant, delegate(global::GooglePlayGames.Native.PInvoke.TurnBasedManager.TurnBasedMatchResponse result)
				{
					if (result.RequestSucceeded())
					{
						callback(true);
					}
					else
					{
						global::GooglePlayGames.OurUtils.Logger.d("Taking turn failed: " + result.ResponseStatus());
						callback(false);
					}
				});
			});
		}

		private void FindEqualVersionMatch(global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, global::System.Action<bool> onFailure, global::System.Action<global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch> onVersionMatch)
		{
			mTurnBasedManager.GetMatch(match.MatchId, delegate(global::GooglePlayGames.Native.PInvoke.TurnBasedManager.TurnBasedMatchResponse response)
			{
				using (global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch nativeTurnBasedMatch = response.Match())
				{
					if (nativeTurnBasedMatch == null)
					{
						global::GooglePlayGames.OurUtils.Logger.e(string.Format("Could not find match {0}", match.MatchId));
						onFailure(false);
					}
					else if (nativeTurnBasedMatch.Version() != match.Version)
					{
						global::GooglePlayGames.OurUtils.Logger.e(string.Format("Attempted to update a stale version of the match. Expected version was {0} but current version is {1}.", match.Version, nativeTurnBasedMatch.Version()));
						onFailure(false);
					}
					else
					{
						onVersionMatch(nativeTurnBasedMatch);
					}
				}
			});
		}

		private void FindEqualVersionMatchWithParticipant(global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, string participantId, global::System.Action<bool> onFailure, global::System.Action<global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant, global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch> onFoundParticipantAndMatch)
		{
			FindEqualVersionMatch(match, onFailure, delegate(global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch foundMatch)
			{
				if (participantId == null)
				{
					using (global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant arg = global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant.AutomatchingSentinel())
					{
						onFoundParticipantAndMatch(arg, foundMatch);
						return;
					}
				}
				using (global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant multiplayerParticipant = foundMatch.ParticipantWithId(participantId))
				{
					if (multiplayerParticipant == null)
					{
						global::GooglePlayGames.OurUtils.Logger.e(string.Format("Located match {0} but desired participant with ID {1} could not be found", match.MatchId, participantId));
						onFailure(false);
					}
					else
					{
						onFoundParticipantAndMatch(multiplayerParticipant, foundMatch);
					}
				}
			});
		}

		public int GetMaxMatchDataSize()
		{
			throw new global::System.NotImplementedException();
		}

		public void Finish(global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, byte[] data, global::GooglePlayGames.BasicApi.Multiplayer.MatchOutcome outcome, global::System.Action<bool> callback)
		{
			callback = global::GooglePlayGames.Native.PInvoke.Callbacks.AsOnGameThreadCallback(callback);
			FindEqualVersionMatch(match, callback, delegate(global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch foundMatch)
			{
				global::GooglePlayGames.Native.PInvoke.ParticipantResults participantResults = foundMatch.Results();
				foreach (string participantId in outcome.ParticipantIds)
				{
					global::GooglePlayGames.Native.Cwrapper.Types.MatchResult matchResult = ResultToMatchResult(outcome.GetResultFor(participantId));
					uint placementFor = outcome.GetPlacementFor(participantId);
					if (participantResults.HasResultsForParticipant(participantId))
					{
						global::GooglePlayGames.Native.Cwrapper.Types.MatchResult matchResult2 = participantResults.ResultsForParticipant(participantId);
						uint num = participantResults.PlacingForParticipant(participantId);
						if (matchResult != matchResult2 || placementFor != num)
						{
							global::GooglePlayGames.OurUtils.Logger.e(string.Format("Attempted to override existing results for participant {0}: Placing {1}, Result {2}", participantId, num, matchResult2));
							callback(false);
							return;
						}
					}
					else
					{
						global::GooglePlayGames.Native.PInvoke.ParticipantResults participantResults2 = participantResults;
						participantResults = participantResults2.WithResult(participantId, placementFor, matchResult);
						participantResults2.Dispose();
					}
				}
				mTurnBasedManager.FinishMatchDuringMyTurn(foundMatch, data, participantResults, delegate(global::GooglePlayGames.Native.PInvoke.TurnBasedManager.TurnBasedMatchResponse response)
				{
					callback(response.RequestSucceeded());
				});
			});
		}

		private static global::GooglePlayGames.Native.Cwrapper.Types.MatchResult ResultToMatchResult(global::GooglePlayGames.BasicApi.Multiplayer.MatchOutcome.ParticipantResult result)
		{
			switch (result)
			{
			case global::GooglePlayGames.BasicApi.Multiplayer.MatchOutcome.ParticipantResult.Loss:
				return global::GooglePlayGames.Native.Cwrapper.Types.MatchResult.LOSS;
			case global::GooglePlayGames.BasicApi.Multiplayer.MatchOutcome.ParticipantResult.None:
				return global::GooglePlayGames.Native.Cwrapper.Types.MatchResult.NONE;
			case global::GooglePlayGames.BasicApi.Multiplayer.MatchOutcome.ParticipantResult.Tie:
				return global::GooglePlayGames.Native.Cwrapper.Types.MatchResult.TIE;
			case global::GooglePlayGames.BasicApi.Multiplayer.MatchOutcome.ParticipantResult.Win:
				return global::GooglePlayGames.Native.Cwrapper.Types.MatchResult.WIN;
			default:
				global::GooglePlayGames.OurUtils.Logger.e("Received unknown ParticipantResult " + result);
				return global::GooglePlayGames.Native.Cwrapper.Types.MatchResult.NONE;
			}
		}

		public void AcknowledgeFinished(global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, global::System.Action<bool> callback)
		{
			callback = global::GooglePlayGames.Native.PInvoke.Callbacks.AsOnGameThreadCallback(callback);
			FindEqualVersionMatch(match, callback, delegate(global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch foundMatch)
			{
				mTurnBasedManager.ConfirmPendingCompletion(foundMatch, delegate(global::GooglePlayGames.Native.PInvoke.TurnBasedManager.TurnBasedMatchResponse response)
				{
					callback(response.RequestSucceeded());
				});
			});
		}

		public void Leave(global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, global::System.Action<bool> callback)
		{
			callback = global::GooglePlayGames.Native.PInvoke.Callbacks.AsOnGameThreadCallback(callback);
			FindEqualVersionMatch(match, callback, delegate(global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch foundMatch)
			{
				mTurnBasedManager.LeaveMatchDuringTheirTurn(foundMatch, delegate(global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus status)
				{
					callback(status > (global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus)0);
				});
			});
		}

		public void LeaveDuringTurn(global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, string pendingParticipantId, global::System.Action<bool> callback)
		{
			callback = global::GooglePlayGames.Native.PInvoke.Callbacks.AsOnGameThreadCallback(callback);
			FindEqualVersionMatchWithParticipant(match, pendingParticipantId, callback, delegate(global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant pendingParticipant, global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch foundMatch)
			{
				mTurnBasedManager.LeaveDuringMyTurn(foundMatch, pendingParticipant, delegate(global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus status)
				{
					callback(status > (global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus)0);
				});
			});
		}

		public void Cancel(global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, global::System.Action<bool> callback)
		{
			callback = global::GooglePlayGames.Native.PInvoke.Callbacks.AsOnGameThreadCallback(callback);
			FindEqualVersionMatch(match, callback, delegate(global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch foundMatch)
			{
				mTurnBasedManager.CancelMatch(foundMatch, delegate(global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus status)
				{
					callback(status > (global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus)0);
				});
			});
		}

		public void Rematch(global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, global::System.Action<bool, global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback)
		{
			callback = global::GooglePlayGames.Native.PInvoke.Callbacks.AsOnGameThreadCallback(callback);
			FindEqualVersionMatch(match, delegate
			{
				callback(false, null);
			}, delegate(global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch foundMatch)
			{
				mTurnBasedManager.Rematch(foundMatch, BridgeMatchToUserCallback(delegate(global::GooglePlayGames.BasicApi.UIStatus status, global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch m)
				{
					callback(status == global::GooglePlayGames.BasicApi.UIStatus.Valid, m);
				}));
			});
		}

		public void DeclineInvitation(string invitationId)
		{
			FindInvitationWithId(invitationId, delegate(global::GooglePlayGames.Native.PInvoke.MultiplayerInvitation invitation)
			{
				if (invitation != null)
				{
					mTurnBasedManager.DeclineInvitation(invitation);
				}
			});
		}
	}
}
