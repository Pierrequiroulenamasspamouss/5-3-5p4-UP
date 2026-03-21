namespace Kampai.Main
{
	public class PostDownloadManifestCommand : global::strange.extensions.command.impl.Command
	{
		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("PostDownloadManifestCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Common.SetupManifestSignal setupManifestSignal { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Common.ReconcileDLCSignal reconcileDLCSignal { get; set; }

		[Inject]
		public global::Kampai.Splash.DLCModel dlcModel { get; set; }

		[Inject]
		public global::Kampai.Game.IDLCService dlcService { get; set; }

		[Inject]
		public global::Kampai.Common.IManifestService manifestService { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Common.CheckAvailableStorageSignal checkAvailableStorageSignal { get; set; }

		[Inject]
		public global::Kampai.Game.LoginUserSignal loginSignal { get; set; }

		[Inject]
		public global::Kampai.Splash.LaunchDownloadSignal launchDownloadSignal { get; set; }

		[Inject]
		public global::Kampai.Common.IVideoService videoService { get; set; }

		[Inject]
		public global::Kampai.Splash.IBackgroundDownloadDlcService backgroundDownloadDlcService { get; set; }

		[Inject]
		public global::Kampai.Game.IConfigurationsService configurationsService { get; set; }

		[Inject]
		public global::Kampai.Main.IAssetsPreloadService assetsPreloadService { get; set; }

		public override void Execute()
		{
			global::Kampai.Util.TimeProfiler.EndSection("retrieve manifest");
			logger.Info("PostDownloadManifestCommand setup manifest");
			telemetryService.Send_Telemetry_EVT_USER_GAME_LOAD_FUNNEL("30 - Loaded DLC Manifest", playerService.SWRVEGroup, dlcService.GetDownloadQualityLevel());
			try
			{
				logger.Info("PostDownloadManifestCommand dispatching setupManifestSignal");
				setupManifestSignal.Dispatch();
				logger.Info("PostDownloadManifestCommand setupManifestSignal dispatched");
			}
			catch (global::Kampai.Util.FatalException ex)
			{
				logger.FatalNoThrow(ex.FatalCode, ex.ReferencedId, "Message: {0}, Reason: {1}", ex.Message, (ex.InnerException == null) ? ex.ToString() : ex.InnerException.ToString());
				return;
			}
			global::System.Collections.Generic.IList<global::Kampai.Util.BundleInfo> unstreamablePackagedBundlesList = manifestService.GetUnstreamablePackagedBundlesList();
			logger.Info("PostDownloadManifestCommand unstreamablePackagedBundlesList count: " + unstreamablePackagedBundlesList.Count);
			if (unstreamablePackagedBundlesList.Count > 0)
			{
				logger.Info("PostDownloadManifestCommand starting CopyStreamingAssets");
				routineRunner.StartCoroutine(CopyStreamingAssets(unstreamablePackagedBundlesList));
			}
			else
			{
				logger.Info("PostDownloadManifestCommand calling CompleteManifestDownload");
				CompleteManifestDownload();
			}
		}

		private void CompleteManifestDownload()
		{
			logger.Debug("[Manifest] CompleteManifestDownload");
			routineRunner.StartCoroutine(WaitAFrame(delegate
			{
				reconcileDLCSignal.Dispatch(true);
				ulong num = dlcModel.TotalSize;
				if (ShouldPlayVideo() && !videoService.IsIntroCached())
				{
					num += 5242880;
				}
				if (num != 0L)
				{
					checkAvailableStorageSignal.Dispatch(global::Kampai.Util.GameConstants.PERSISTENT_DATA_PATH, num, TryPlayVideoStartDownloadDLC);
				}
				else
				{
					TryPlayVideoStartDownloadDLC();
				}
			}));
		}

		private void TryPlayVideoStartDownloadDLC()
		{
			logger.Debug("[Manifest] TryPlayVideoStartDownloadDLC");
			bool flag = dlcModel.NeededBundles.Count == 0;
			// Force flag to true to skip DLC download and proceed to login
			flag = true; 
			if (ShouldPlayVideo())
			{
				PlayVideo((!flag) ? new global::System.Action(VideoStartedPlayingCallback) : null);
			}
			else if (!flag)
			{
				DownloadDlcInForeground();
			}
			if (flag)
			{
				assetsPreloadService.PreloadAllAssets();
				loginSignal.Dispatch();
			}
		}

		private void VideoStartedPlayingCallback()
		{
			logger.Info("[Manifest] Video playing; starting background DLC");
			backgroundDownloadDlcService.Start();
			logger.Info("[Manifest] Waiting for video to finish");
			routineRunner.StartCoroutine(StopBackgroundDlcDownloading(6));
		}

		private void DownloadDlcInForeground()
		{
			logger.Info("[Manifest] Downloading DLC in foreground");
			launchDownloadSignal.Dispatch(false);
		}

		private global::System.Collections.IEnumerator StopBackgroundDlcDownloading(int frames)
		{
			while (frames-- > 0)
			{
				yield return new global::UnityEngine.WaitForEndOfFrame();
			}
			global::Kampai.Main.LoadState.Set(global::Kampai.Main.LoadStateType.BOOTING);
			logger.Info("[Manifest]: stopping downloading and wait until it finished");
			backgroundDownloadDlcService.Stop();
			while (!backgroundDownloadDlcService.Stopped)
			{
				logger.Info("[Manifest]: waiting");
				yield return new global::UnityEngine.WaitForSeconds(0.1f);
			}
			logger.Info("[Manifest]: background downloading finished, reconcile DLC again.");
			telemetryService.Send_Telemetry_EVT_USER_GAME_LOAD_FUNNEL("50 - Played Intro Video", "anyVariant", dlcService.GetDownloadQualityLevel());
			CompleteManifestDownload();
		}

		private bool ShouldPlayVideo()
		{
			if (!global::UnityEngine.PlayerPrefs.HasKey("intro_video_played"))
			{
				logger.Info("[Manifest] PostDownloadManifestCommand.ShouldPlayVideo: {0}", true);
				return true;
			}
			int num = global::UnityEngine.PlayerPrefs.GetInt("intro_video_played");
			logger.Info("[Manifest] PostDownloadManifestCommand.ShouldPlayVideo: {0}", num == 0);
			return num == 0;
		}

		private void PlayVideo(global::System.Action callback)
		{
			bool dEBUG_ENABLED = global::Kampai.Util.GameConstants.StaticConfig.DEBUG_ENABLED;
			logger.Info("[Manifest] PostDownloadManifestCommand.PlayVideo skippable: {0}", dEBUG_ENABLED);
			videoService.playIntro(false, dEBUG_ENABLED, callback, configurationsService.GetConfigurations().videoUri);
		}

		private global::System.Collections.IEnumerator WaitAFrame(global::System.Action a)
		{
			yield return null;
			a();
		}

		private global::System.Collections.IEnumerator CopyStreamingAssets(global::System.Collections.Generic.IList<global::Kampai.Util.BundleInfo> unstreambleBundles)
		{
			logger.Info("[Manifest] Copying streaming assets: {0}", unstreambleBundles.Count.ToString());
			string StreamingAssetsDLCPath = global::System.IO.Path.Combine(global::UnityEngine.Application.streamingAssetsPath, "DLC");
#if !UNITY_WEBPLAYER
			if (!global::System.IO.Directory.Exists(global::Kampai.Util.GameConstants.DLC_PATH))
			{
				global::System.IO.Directory.CreateDirectory(global::Kampai.Util.GameConstants.DLC_PATH);
			}
#endif
			byte[] buffer = new byte[4096];
			foreach (global::Kampai.Util.BundleInfo bundle in unstreambleBundles)
			{
				string dlcPathName = global::System.IO.Path.Combine(global::Kampai.Util.GameConstants.DLC_PATH, bundle.name + ".unity3d");
				bool validDlcFound = false;
#if !UNITY_WEBPLAYER
#if !UNITY_WEBPLAYER
				bool dlcFileExists = global::System.IO.File.Exists(dlcPathName);
#else
				bool dlcFileExists = false;
#endif
				validDlcFound = dlcFileExists && new global::System.IO.FileInfo(dlcPathName).Length == (long)bundle.size;
				if (dlcFileExists && !validDlcFound)
				{
					global::System.IO.File.Delete(dlcPathName);
				}
#else
				validDlcFound = false;
#endif
				if (!validDlcFound)
				{
					byte[] results = null;
					string filePath = global::System.IO.Path.Combine(StreamingAssetsDLCPath, bundle.originalName);
					logger.Debug("[Manifest] Copying: {0}", filePath);
					if (filePath.Contains("://"))
					{
						global::UnityEngine.WWW www = new global::UnityEngine.WWW(filePath);
						yield return www;
						if (www.error == null)
						{
							results = www.bytes;
						}
						else
						{
							logger.Info("[Manifest] Error copying {0} from WWW", filePath);
						}
					}
#if !UNITY_WEBPLAYER
					else if (global::System.IO.File.Exists(filePath))
#else
					else if (false)
#endif
					{
#if !UNITY_WEBPLAYER
						results = global::System.IO.File.ReadAllBytes(filePath);
#endif
					}
					if (results != null)
					{
						logger.Info("[Manifest] Saving {0} to {1} ({2} bytes)", bundle.originalName, dlcPathName, results.Length);
#if !UNITY_WEBPLAYER
						using (global::System.IO.FileStream file = new global::System.IO.FileStream(dlcPathName, global::System.IO.FileMode.Create, global::System.IO.FileAccess.Write))
						{
							using (global::System.IO.MemoryStream ms = new global::System.IO.MemoryStream(results))
							{
								global::System.IO.Stream inputStream = ((!bundle.isZipped) ? ((global::System.IO.Stream)ms) : ((global::System.IO.Stream)new global::ICSharpCode.SharpZipLib.GZip.GZipInputStream(ms)));
								try
								{
									int totalBytesRead = 0;
									for (int bytesRead = inputStream.Read(buffer, 0, buffer.Length); bytesRead > 0; bytesRead = inputStream.Read(buffer, 0, buffer.Length))
									{
										totalBytesRead += bytesRead;
										logger.Info("[Manifest] {0} - {1}", bundle.originalName, totalBytesRead);
										file.Write(buffer, 0, bytesRead);
									}
								}
								finally
								{
									if (bundle.isZipped)
									{
										inputStream.Dispose();
									}
								}
							}
						}
#endif
						yield return null;
					}
					else
					{
						logger.Info("[Manifest] No data for {0}", bundle.originalName);
					}
				}
				else
				{
					logger.Info("[Manifest] {0} already exists.", bundle.name);
				}
			}
			CompleteManifestDownload();
		}
	}
}
