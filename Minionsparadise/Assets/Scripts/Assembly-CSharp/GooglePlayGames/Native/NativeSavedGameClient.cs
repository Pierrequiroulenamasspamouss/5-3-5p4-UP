namespace GooglePlayGames.Native
{
	internal class NativeSavedGameClient : global::GooglePlayGames.BasicApi.SavedGame.ISavedGameClient
	{
		private class NativeConflictResolver : global::GooglePlayGames.BasicApi.SavedGame.IConflictResolver
		{
			private readonly global::GooglePlayGames.Native.PInvoke.SnapshotManager mManager;

			private readonly string mConflictId;

			private readonly global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadata mOriginal;

			private readonly global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadata mUnmerged;

			private readonly global::System.Action<global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus, global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata> mCompleteCallback;

			private readonly global::System.Action mRetryFileOpen;

			internal NativeConflictResolver(global::GooglePlayGames.Native.PInvoke.SnapshotManager manager, string conflictId, global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadata original, global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadata unmerged, global::System.Action<global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus, global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata> completeCallback, global::System.Action retryOpen)
			{
				mManager = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(manager);
				mConflictId = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(conflictId);
				mOriginal = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(original);
				mUnmerged = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(unmerged);
				mCompleteCallback = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(completeCallback);
				mRetryFileOpen = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(retryOpen);
			}

			public void ChooseMetadata(global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata chosenMetadata)
			{
				global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadata nativeSnapshotMetadata = chosenMetadata as global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadata;
				if (nativeSnapshotMetadata != mOriginal && nativeSnapshotMetadata != mUnmerged)
				{
					global::GooglePlayGames.OurUtils.Logger.e("Caller attempted to choose a version of the metadata that was not part of the conflict");
					mCompleteCallback(global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus.BadInputError, null);
					return;
				}
				mManager.Resolve(nativeSnapshotMetadata, new global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadataChange.Builder().Build(), mConflictId, delegate(global::GooglePlayGames.Native.PInvoke.SnapshotManager.CommitResponse response)
				{
					if (!response.RequestSucceeded())
					{
						mCompleteCallback(AsRequestStatus(response.ResponseStatus()), null);
					}
					else
					{
						mRetryFileOpen();
					}
				});
			}
		}

		private class Prefetcher
		{
			private readonly object mLock = new object();

			private bool mOriginalDataFetched;

			private byte[] mOriginalData;

			private bool mUnmergedDataFetched;

			private byte[] mUnmergedData;

			private global::System.Action<global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus, global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata> completedCallback;

			private readonly global::System.Action<byte[], byte[]> mDataFetchedCallback;

			internal Prefetcher(global::System.Action<byte[], byte[]> dataFetchedCallback, global::System.Action<global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus, global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata> completedCallback)
			{
				mDataFetchedCallback = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(dataFetchedCallback);
				this.completedCallback = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(completedCallback);
			}

			internal void OnOriginalDataRead(global::GooglePlayGames.Native.PInvoke.SnapshotManager.ReadResponse readResponse)
			{
				lock (mLock)
				{
					if (!readResponse.RequestSucceeded())
					{
						global::GooglePlayGames.OurUtils.Logger.e("Encountered error while prefetching original data.");
						completedCallback(AsRequestStatus(readResponse.ResponseStatus()), null);
						completedCallback = delegate
						{
						};
					}
					else
					{
						global::GooglePlayGames.OurUtils.Logger.d("Successfully fetched original data");
						mOriginalDataFetched = true;
						mOriginalData = readResponse.Data();
						MaybeProceed();
					}
				}
			}

			internal void OnUnmergedDataRead(global::GooglePlayGames.Native.PInvoke.SnapshotManager.ReadResponse readResponse)
			{
				lock (mLock)
				{
					if (!readResponse.RequestSucceeded())
					{
						global::GooglePlayGames.OurUtils.Logger.e("Encountered error while prefetching unmerged data.");
						completedCallback(AsRequestStatus(readResponse.ResponseStatus()), null);
						completedCallback = delegate
						{
						};
					}
					else
					{
						global::GooglePlayGames.OurUtils.Logger.d("Successfully fetched unmerged data");
						mUnmergedDataFetched = true;
						mUnmergedData = readResponse.Data();
						MaybeProceed();
					}
				}
			}

			private void MaybeProceed()
			{
				if (mOriginalDataFetched && mUnmergedDataFetched)
				{
					global::GooglePlayGames.OurUtils.Logger.d("Fetched data for original and unmerged, proceeding");
					mDataFetchedCallback(mOriginalData, mUnmergedData);
					return;
				}
				global::GooglePlayGames.OurUtils.Logger.d("Not all data fetched - original:" + mOriginalDataFetched + " unmerged:" + mUnmergedDataFetched);
			}
		}

		private static readonly global::System.Text.RegularExpressions.Regex ValidFilenameRegex = new global::System.Text.RegularExpressions.Regex("\\A[a-zA-Z0-9-._~]{1,100}\\Z");

		private readonly global::GooglePlayGames.Native.PInvoke.SnapshotManager mSnapshotManager;

		internal NativeSavedGameClient(global::GooglePlayGames.Native.PInvoke.SnapshotManager manager)
		{
			mSnapshotManager = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(manager);
		}

		public void OpenWithAutomaticConflictResolution(string filename, global::GooglePlayGames.BasicApi.DataSource source, global::GooglePlayGames.BasicApi.SavedGame.ConflictResolutionStrategy resolutionStrategy, global::System.Action<global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus, global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata> callback)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(filename);
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(callback);
			callback = ToOnGameThread(callback);
			if (!IsValidFilename(filename))
			{
				global::GooglePlayGames.OurUtils.Logger.e("Received invalid filename: " + filename);
				callback(global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus.BadInputError, null);
				return;
			}
			OpenWithManualConflictResolution(filename, source, false, delegate(global::GooglePlayGames.BasicApi.SavedGame.IConflictResolver resolver, global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata original, byte[] originalData, global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata unmerged, byte[] unmergedData)
			{
				switch (resolutionStrategy)
				{
				case global::GooglePlayGames.BasicApi.SavedGame.ConflictResolutionStrategy.UseOriginal:
					resolver.ChooseMetadata(original);
					break;
				case global::GooglePlayGames.BasicApi.SavedGame.ConflictResolutionStrategy.UseUnmerged:
					resolver.ChooseMetadata(unmerged);
					break;
				case global::GooglePlayGames.BasicApi.SavedGame.ConflictResolutionStrategy.UseLongestPlaytime:
					if (original.TotalTimePlayed >= unmerged.TotalTimePlayed)
					{
						resolver.ChooseMetadata(original);
					}
					else
					{
						resolver.ChooseMetadata(unmerged);
					}
					break;
				default:
					global::GooglePlayGames.OurUtils.Logger.e("Unhandled strategy " + resolutionStrategy);
					callback(global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus.InternalError, null);
					break;
				}
			}, callback);
		}

		private global::GooglePlayGames.BasicApi.SavedGame.ConflictCallback ToOnGameThread(global::GooglePlayGames.BasicApi.SavedGame.ConflictCallback conflictCallback)
		{
			return delegate(global::GooglePlayGames.BasicApi.SavedGame.IConflictResolver resolver, global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata original, byte[] originalData, global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata unmerged, byte[] unmergedData)
			{
				global::GooglePlayGames.OurUtils.Logger.d("Invoking conflict callback");
				global::GooglePlayGames.OurUtils.PlayGamesHelperObject.RunOnGameThread(delegate
				{
					conflictCallback(resolver, original, originalData, unmerged, unmergedData);
				});
			};
		}

		public void OpenWithManualConflictResolution(string filename, global::GooglePlayGames.BasicApi.DataSource source, bool prefetchDataOnConflict, global::GooglePlayGames.BasicApi.SavedGame.ConflictCallback conflictCallback, global::System.Action<global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus, global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata> completedCallback)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(filename);
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(conflictCallback);
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(completedCallback);
			conflictCallback = ToOnGameThread(conflictCallback);
			completedCallback = ToOnGameThread(completedCallback);
			if (!IsValidFilename(filename))
			{
				global::GooglePlayGames.OurUtils.Logger.e("Received invalid filename: " + filename);
				completedCallback(global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus.BadInputError, null);
			}
			else
			{
				InternalManualOpen(filename, source, prefetchDataOnConflict, conflictCallback, completedCallback);
			}
		}

		private void InternalManualOpen(string filename, global::GooglePlayGames.BasicApi.DataSource source, bool prefetchDataOnConflict, global::GooglePlayGames.BasicApi.SavedGame.ConflictCallback conflictCallback, global::System.Action<global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus, global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata> completedCallback)
		{
			mSnapshotManager.Open(filename, AsDataSource(source), global::GooglePlayGames.Native.Cwrapper.Types.SnapshotConflictPolicy.MANUAL, delegate(global::GooglePlayGames.Native.PInvoke.SnapshotManager.OpenResponse response)
			{
				if (!response.RequestSucceeded())
				{
					completedCallback(AsRequestStatus(response.ResponseStatus()), null);
				}
				else if (response.ResponseStatus() == global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.SnapshotOpenStatus.VALID)
				{
					completedCallback(global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus.Success, response.Data());
				}
				else if (response.ResponseStatus() == global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.SnapshotOpenStatus.VALID_WITH_CONFLICT)
				{
					global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadata original = response.ConflictOriginal();
					global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadata unmerged = response.ConflictUnmerged();
					global::GooglePlayGames.Native.NativeSavedGameClient.NativeConflictResolver resolver = new global::GooglePlayGames.Native.NativeSavedGameClient.NativeConflictResolver(mSnapshotManager, response.ConflictId(), original, unmerged, completedCallback, delegate
					{
						InternalManualOpen(filename, source, prefetchDataOnConflict, conflictCallback, completedCallback);
					});
					if (!prefetchDataOnConflict)
					{
						conflictCallback(resolver, original, null, unmerged, null);
					}
					else
					{
						global::GooglePlayGames.Native.NativeSavedGameClient.Prefetcher prefetcher = new global::GooglePlayGames.Native.NativeSavedGameClient.Prefetcher(delegate(byte[] originalData, byte[] unmergedData)
						{
							conflictCallback(resolver, original, originalData, unmerged, unmergedData);
						}, completedCallback);
						mSnapshotManager.Read(original, prefetcher.OnOriginalDataRead);
						mSnapshotManager.Read(unmerged, prefetcher.OnUnmergedDataRead);
					}
				}
				else
				{
					global::GooglePlayGames.OurUtils.Logger.e("Unhandled response status");
					completedCallback(global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus.InternalError, null);
				}
			});
		}

		public void ReadBinaryData(global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata metadata, global::System.Action<global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus, byte[]> completedCallback)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(metadata);
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(completedCallback);
			completedCallback = ToOnGameThread(completedCallback);
			global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadata nativeSnapshotMetadata = metadata as global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadata;
			if (nativeSnapshotMetadata == null)
			{
				global::GooglePlayGames.OurUtils.Logger.e("Encountered metadata that was not generated by this ISavedGameClient");
				completedCallback(global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus.BadInputError, null);
				return;
			}
			if (!nativeSnapshotMetadata.IsOpen)
			{
				global::GooglePlayGames.OurUtils.Logger.e("This method requires an open ISavedGameMetadata.");
				completedCallback(global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus.BadInputError, null);
				return;
			}
			mSnapshotManager.Read(nativeSnapshotMetadata, delegate(global::GooglePlayGames.Native.PInvoke.SnapshotManager.ReadResponse response)
			{
				if (!response.RequestSucceeded())
				{
					completedCallback(AsRequestStatus(response.ResponseStatus()), null);
				}
				else
				{
					completedCallback(global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus.Success, response.Data());
				}
			});
		}

		public void ShowSelectSavedGameUI(string uiTitle, uint maxDisplayedSavedGames, bool showCreateSaveUI, bool showDeleteSaveUI, global::System.Action<global::GooglePlayGames.BasicApi.SavedGame.SelectUIStatus, global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata> callback)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(uiTitle);
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(callback);
			callback = ToOnGameThread(callback);
			if (maxDisplayedSavedGames == 0)
			{
				global::GooglePlayGames.OurUtils.Logger.e("maxDisplayedSavedGames must be greater than 0");
				callback(global::GooglePlayGames.BasicApi.SavedGame.SelectUIStatus.BadInputError, null);
			}
			else
			{
				mSnapshotManager.SnapshotSelectUI(showCreateSaveUI, showDeleteSaveUI, maxDisplayedSavedGames, uiTitle, delegate(global::GooglePlayGames.Native.PInvoke.SnapshotManager.SnapshotSelectUIResponse response)
				{
					callback(AsUIStatus(response.RequestStatus()), (!response.RequestSucceeded()) ? null : response.Data());
				});
			}
		}

		public void CommitUpdate(global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata metadata, global::GooglePlayGames.BasicApi.SavedGame.SavedGameMetadataUpdate updateForMetadata, byte[] updatedBinaryData, global::System.Action<global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus, global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata> callback)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(metadata);
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(updatedBinaryData);
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(callback);
			callback = ToOnGameThread(callback);
			global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadata nativeSnapshotMetadata = metadata as global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadata;
			if (nativeSnapshotMetadata == null)
			{
				global::GooglePlayGames.OurUtils.Logger.e("Encountered metadata that was not generated by this ISavedGameClient");
				callback(global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus.BadInputError, null);
				return;
			}
			if (!nativeSnapshotMetadata.IsOpen)
			{
				global::GooglePlayGames.OurUtils.Logger.e("This method requires an open ISavedGameMetadata.");
				callback(global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus.BadInputError, null);
				return;
			}
			mSnapshotManager.Commit(nativeSnapshotMetadata, AsMetadataChange(updateForMetadata), updatedBinaryData, delegate(global::GooglePlayGames.Native.PInvoke.SnapshotManager.CommitResponse response)
			{
				if (!response.RequestSucceeded())
				{
					callback(AsRequestStatus(response.ResponseStatus()), null);
				}
				else
				{
					callback(global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus.Success, response.Data());
				}
			});
		}

		public void FetchAllSavedGames(global::GooglePlayGames.BasicApi.DataSource source, global::System.Action<global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus, global::System.Collections.Generic.List<global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata>> callback)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(callback);
			callback = ToOnGameThread(callback);
			mSnapshotManager.FetchAll(AsDataSource(source), delegate(global::GooglePlayGames.Native.PInvoke.SnapshotManager.FetchAllResponse response)
			{
				if (!response.RequestSucceeded())
				{
					callback(AsRequestStatus(response.ResponseStatus()), new global::System.Collections.Generic.List<global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata>());
				}
				else
				{
					callback(global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus.Success, global::System.Linq.Enumerable.ToList(global::System.Linq.Enumerable.Cast<global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata>(response.Data())));
				}
			});
		}

		public void Delete(global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata metadata)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(metadata);
			mSnapshotManager.Delete((global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadata)metadata);
		}

		internal static bool IsValidFilename(string filename)
		{
			if (filename == null)
			{
				return false;
			}
			return ValidFilenameRegex.IsMatch(filename);
		}

		private static global::GooglePlayGames.Native.Cwrapper.Types.SnapshotConflictPolicy AsConflictPolicy(global::GooglePlayGames.BasicApi.SavedGame.ConflictResolutionStrategy strategy)
		{
			switch (strategy)
			{
			case global::GooglePlayGames.BasicApi.SavedGame.ConflictResolutionStrategy.UseLongestPlaytime:
				return global::GooglePlayGames.Native.Cwrapper.Types.SnapshotConflictPolicy.LONGEST_PLAYTIME;
			case global::GooglePlayGames.BasicApi.SavedGame.ConflictResolutionStrategy.UseOriginal:
				return global::GooglePlayGames.Native.Cwrapper.Types.SnapshotConflictPolicy.LAST_KNOWN_GOOD;
			case global::GooglePlayGames.BasicApi.SavedGame.ConflictResolutionStrategy.UseUnmerged:
				return global::GooglePlayGames.Native.Cwrapper.Types.SnapshotConflictPolicy.MOST_RECENTLY_MODIFIED;
			default:
				throw new global::System.InvalidOperationException("Found unhandled strategy: " + strategy);
			}
		}

		private static global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus AsRequestStatus(global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.SnapshotOpenStatus status)
		{
			switch (status)
			{
			case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.SnapshotOpenStatus.VALID:
				return global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus.Success;
			case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.SnapshotOpenStatus.ERROR_NOT_AUTHORIZED:
				return global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus.AuthenticationError;
			case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.SnapshotOpenStatus.ERROR_TIMEOUT:
				return global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus.TimeoutError;
			default:
				global::GooglePlayGames.OurUtils.Logger.e("Encountered unknown status: " + status);
				return global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus.InternalError;
			}
		}

		private static global::GooglePlayGames.Native.Cwrapper.Types.DataSource AsDataSource(global::GooglePlayGames.BasicApi.DataSource source)
		{
			switch (source)
			{
			case global::GooglePlayGames.BasicApi.DataSource.ReadCacheOrNetwork:
				return global::GooglePlayGames.Native.Cwrapper.Types.DataSource.CACHE_OR_NETWORK;
			case global::GooglePlayGames.BasicApi.DataSource.ReadNetworkOnly:
				return global::GooglePlayGames.Native.Cwrapper.Types.DataSource.NETWORK_ONLY;
			default:
				throw new global::System.InvalidOperationException("Found unhandled DataSource: " + source);
			}
		}

		private static global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus AsRequestStatus(global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus status)
		{
			switch (status)
			{
			case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_INTERNAL:
				return global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus.InternalError;
			case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_LICENSE_CHECK_FAILED:
				global::GooglePlayGames.OurUtils.Logger.e("User attempted to use the game without a valid license.");
				return global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus.AuthenticationError;
			case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_NOT_AUTHORIZED:
				global::GooglePlayGames.OurUtils.Logger.e("User was not authorized (they were probably not logged in).");
				return global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus.AuthenticationError;
			case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_TIMEOUT:
				return global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus.TimeoutError;
			case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID:
			case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID_BUT_STALE:
				return global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus.Success;
			default:
				global::GooglePlayGames.OurUtils.Logger.e("Unknown status: " + status);
				return global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus.InternalError;
			}
		}

		private static global::GooglePlayGames.BasicApi.SavedGame.SelectUIStatus AsUIStatus(global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus uiStatus)
		{
			switch (uiStatus)
			{
			case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.VALID:
				return global::GooglePlayGames.BasicApi.SavedGame.SelectUIStatus.SavedGameSelected;
			case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.ERROR_CANCELED:
				return global::GooglePlayGames.BasicApi.SavedGame.SelectUIStatus.UserClosedUI;
			case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.ERROR_INTERNAL:
				return global::GooglePlayGames.BasicApi.SavedGame.SelectUIStatus.InternalError;
			case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.ERROR_NOT_AUTHORIZED:
				return global::GooglePlayGames.BasicApi.SavedGame.SelectUIStatus.AuthenticationError;
			case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.ERROR_TIMEOUT:
				return global::GooglePlayGames.BasicApi.SavedGame.SelectUIStatus.TimeoutError;
			default:
				global::GooglePlayGames.OurUtils.Logger.e("Encountered unknown UI Status: " + uiStatus);
				return global::GooglePlayGames.BasicApi.SavedGame.SelectUIStatus.InternalError;
			}
		}

		private static global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadataChange AsMetadataChange(global::GooglePlayGames.BasicApi.SavedGame.SavedGameMetadataUpdate update)
		{
			global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadataChange.Builder builder = new global::GooglePlayGames.Native.PInvoke.NativeSnapshotMetadataChange.Builder();
			if (update.IsCoverImageUpdated)
			{
				builder.SetCoverImageFromPngData(update.UpdatedPngCoverImage);
			}
			if (update.IsDescriptionUpdated)
			{
				builder.SetDescription(update.UpdatedDescription);
			}
			if (update.IsPlayedTimeUpdated)
			{
				builder.SetPlayedTime((ulong)update.UpdatedPlayedTime.Value.TotalMilliseconds);
			}
			return builder.Build();
		}

		private static global::System.Action<T1, T2> ToOnGameThread<T1, T2>(global::System.Action<T1, T2> toConvert)
		{
			return delegate(T1 val1, T2 val2)
			{
				global::GooglePlayGames.OurUtils.PlayGamesHelperObject.RunOnGameThread(delegate
				{
					toConvert(val1, val2);
				});
			};
		}
	}
}
