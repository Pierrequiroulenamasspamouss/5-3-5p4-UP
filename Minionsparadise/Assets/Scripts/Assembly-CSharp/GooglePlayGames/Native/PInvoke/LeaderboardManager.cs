namespace GooglePlayGames.Native.PInvoke
{
	internal class LeaderboardManager
	{
		private readonly global::GooglePlayGames.Native.PInvoke.GameServices mServices;

		internal int LeaderboardMaxResults
		{
			get
			{
				return 25;
			}
		}

		internal LeaderboardManager(global::GooglePlayGames.Native.PInvoke.GameServices services)
		{
			mServices = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(services);
		}

		internal void SubmitScore(string leaderboardId, long score, string metadata)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(leaderboardId, "leaderboardId");
			global::GooglePlayGames.OurUtils.Logger.d("Native Submitting score: " + score + " for lb " + leaderboardId + " with metadata: " + metadata);
			global::GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_SubmitScore(mServices.AsHandle(), leaderboardId, (ulong)score, metadata ?? string.Empty);
		}

		internal void ShowAllUI(global::System.Action<global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus> callback)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(callback);
			global::GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_ShowAllUI(mServices.AsHandle(), global::GooglePlayGames.Native.PInvoke.Callbacks.InternalShowUICallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback));
		}

		internal void ShowUI(string leaderboardId, global::GooglePlayGames.BasicApi.LeaderboardTimeSpan span, global::System.Action<global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus> callback)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(callback);
			global::GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_ShowUI(mServices.AsHandle(), leaderboardId, (global::GooglePlayGames.Native.Cwrapper.Types.LeaderboardTimeSpan)span, global::GooglePlayGames.Native.PInvoke.Callbacks.InternalShowUICallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback));
		}

		public void LoadLeaderboardData(string leaderboardId, global::GooglePlayGames.BasicApi.LeaderboardStart start, int rowCount, global::GooglePlayGames.BasicApi.LeaderboardCollection collection, global::GooglePlayGames.BasicApi.LeaderboardTimeSpan timeSpan, string playerId, global::System.Action<global::GooglePlayGames.BasicApi.LeaderboardScoreData> callback)
		{
			global::GooglePlayGames.Native.PInvoke.NativeScorePageToken internalObject = new global::GooglePlayGames.Native.PInvoke.NativeScorePageToken(global::GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_ScorePageToken(mServices.AsHandle(), leaderboardId, (global::GooglePlayGames.Native.Cwrapper.Types.LeaderboardStart)start, (global::GooglePlayGames.Native.Cwrapper.Types.LeaderboardTimeSpan)timeSpan, (global::GooglePlayGames.Native.Cwrapper.Types.LeaderboardCollection)collection));
			global::GooglePlayGames.BasicApi.ScorePageToken token = new global::GooglePlayGames.BasicApi.ScorePageToken(internalObject, leaderboardId, collection, timeSpan);
			global::GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_Fetch(mServices.AsHandle(), global::GooglePlayGames.Native.Cwrapper.Types.DataSource.CACHE_OR_NETWORK, leaderboardId, InternalFetchCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(delegate(global::GooglePlayGames.Native.PInvoke.FetchResponse rsp)
			{
				HandleFetch(token, rsp, playerId, rowCount, callback);
			}, global::GooglePlayGames.Native.PInvoke.FetchResponse.FromPointer));
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.LeaderboardManager.FetchCallback))]
		private static void InternalFetchCallback(global::System.IntPtr response, global::System.IntPtr data)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("LeaderboardManager#InternalFetchCallback", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Temporary, response, data);
		}

		internal void HandleFetch(global::GooglePlayGames.BasicApi.ScorePageToken token, global::GooglePlayGames.Native.PInvoke.FetchResponse response, string selfPlayerId, int maxResults, global::System.Action<global::GooglePlayGames.BasicApi.LeaderboardScoreData> callback)
		{
			global::GooglePlayGames.BasicApi.LeaderboardScoreData data = new global::GooglePlayGames.BasicApi.LeaderboardScoreData(token.LeaderboardId, (global::GooglePlayGames.BasicApi.ResponseStatus)response.GetStatus());
			if (response.GetStatus() != global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID && response.GetStatus() != global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID_BUT_STALE)
			{
				global::GooglePlayGames.OurUtils.Logger.w("Error returned from fetch: " + response.GetStatus());
				callback(data);
				return;
			}
			data.Title = response.Leaderboard().Title();
			data.Id = token.LeaderboardId;
			global::GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_FetchScoreSummary(mServices.AsHandle(), global::GooglePlayGames.Native.Cwrapper.Types.DataSource.CACHE_OR_NETWORK, token.LeaderboardId, (global::GooglePlayGames.Native.Cwrapper.Types.LeaderboardTimeSpan)token.TimeSpan, (global::GooglePlayGames.Native.Cwrapper.Types.LeaderboardCollection)token.Collection, InternalFetchSummaryCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(delegate(global::GooglePlayGames.Native.PInvoke.FetchScoreSummaryResponse rsp)
			{
				HandleFetchScoreSummary(data, rsp, selfPlayerId, maxResults, token, callback);
			}, global::GooglePlayGames.Native.PInvoke.FetchScoreSummaryResponse.FromPointer));
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.LeaderboardManager.FetchScoreSummaryCallback))]
		private static void InternalFetchSummaryCallback(global::System.IntPtr response, global::System.IntPtr data)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("LeaderboardManager#InternalFetchSummaryCallback", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Temporary, response, data);
		}

		internal void HandleFetchScoreSummary(global::GooglePlayGames.BasicApi.LeaderboardScoreData data, global::GooglePlayGames.Native.PInvoke.FetchScoreSummaryResponse response, string selfPlayerId, int maxResults, global::GooglePlayGames.BasicApi.ScorePageToken token, global::System.Action<global::GooglePlayGames.BasicApi.LeaderboardScoreData> callback)
		{
			if (response.GetStatus() != global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID && response.GetStatus() != global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID_BUT_STALE)
			{
				global::GooglePlayGames.OurUtils.Logger.w("Error returned from fetchScoreSummary: " + response);
				data.Status = (global::GooglePlayGames.BasicApi.ResponseStatus)response.GetStatus();
				callback(data);
				return;
			}
			global::GooglePlayGames.Native.PInvoke.NativeScoreSummary scoreSummary = response.GetScoreSummary();
			data.ApproximateCount = scoreSummary.ApproximateResults();
			data.PlayerScore = scoreSummary.LocalUserScore().AsScore(data.Id, selfPlayerId);
			if (maxResults <= 0)
			{
				callback(data);
			}
			else
			{
				LoadScorePage(data, maxResults, token, callback);
			}
		}

		public void LoadScorePage(global::GooglePlayGames.BasicApi.LeaderboardScoreData data, int maxResults, global::GooglePlayGames.BasicApi.ScorePageToken token, global::System.Action<global::GooglePlayGames.BasicApi.LeaderboardScoreData> callback)
		{
			if (data == null)
			{
				data = new global::GooglePlayGames.BasicApi.LeaderboardScoreData(token.LeaderboardId);
			}
			global::GooglePlayGames.Native.PInvoke.NativeScorePageToken nativeScorePageToken = (global::GooglePlayGames.Native.PInvoke.NativeScorePageToken)token.InternalObject;
			global::GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_FetchScorePage(mServices.AsHandle(), global::GooglePlayGames.Native.Cwrapper.Types.DataSource.CACHE_OR_NETWORK, nativeScorePageToken.AsPointer(), (uint)maxResults, InternalFetchScorePage, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(delegate(global::GooglePlayGames.Native.PInvoke.FetchScorePageResponse rsp)
			{
				HandleFetchScorePage(data, token, rsp, callback);
			}, global::GooglePlayGames.Native.PInvoke.FetchScorePageResponse.FromPointer));
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.LeaderboardManager.FetchScorePageCallback))]
		private static void InternalFetchScorePage(global::System.IntPtr response, global::System.IntPtr data)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("LeaderboardManager#InternalFetchScorePage", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Temporary, response, data);
		}

		internal void HandleFetchScorePage(global::GooglePlayGames.BasicApi.LeaderboardScoreData data, global::GooglePlayGames.BasicApi.ScorePageToken token, global::GooglePlayGames.Native.PInvoke.FetchScorePageResponse rsp, global::System.Action<global::GooglePlayGames.BasicApi.LeaderboardScoreData> callback)
		{
			data.Status = (global::GooglePlayGames.BasicApi.ResponseStatus)rsp.GetStatus();
			if (rsp.GetStatus() != global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID && rsp.GetStatus() != global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID_BUT_STALE)
			{
				callback(data);
			}
			global::GooglePlayGames.Native.PInvoke.NativeScorePage scorePage = rsp.GetScorePage();
			if (!scorePage.Valid())
			{
				callback(data);
			}
			if (scorePage.HasNextScorePage())
			{
				data.NextPageToken = new global::GooglePlayGames.BasicApi.ScorePageToken(scorePage.GetNextScorePageToken(), token.LeaderboardId, token.Collection, token.TimeSpan);
			}
			if (scorePage.HasPrevScorePage())
			{
				data.PrevPageToken = new global::GooglePlayGames.BasicApi.ScorePageToken(scorePage.GetPreviousScorePageToken(), token.LeaderboardId, token.Collection, token.TimeSpan);
			}
			foreach (global::GooglePlayGames.Native.PInvoke.NativeScoreEntry item in scorePage)
			{
				data.AddScore(item.AsScore(data.Id));
			}
			callback(data);
		}
	}
}
