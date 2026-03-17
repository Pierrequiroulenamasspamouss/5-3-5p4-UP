namespace Kampai.Game
{
	internal sealed class SavePlayerCommand : global::strange.extensions.command.impl.Command
	{
		public const string PLAYER_DATA_ENDPOINT = "/rest/gamestate/{0}";

		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SavePlayerCommand") as global::Kampai.Util.IKampaiLogger;

		private bool retried;

		private global::Kampai.Game.SaveLocation saveLocation;

		private string saveID;

		private bool saveImmediately;

		private global::System.DateTime saveTimestampUTC;

		[Inject]
		public global::Kampai.Util.Tuple<global::Kampai.Game.SaveLocation, string, bool> tuple { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerDurationService playerDurationService { get; set; }

		[Inject]
		public ILocalPersistanceService persistService { get; set; }

		[Inject]
		public global::Kampai.Game.IUserSessionService userSessionService { get; set; }

		[Inject("game.server.host")]
		public string ServerUrl { get; set; }

		[Inject]
		public global::Kampai.Splash.IDownloadService downloadService { get; set; }

		[Inject]
		public global::Kampai.Common.NetworkConnectionLostSignal networkConnectionLostSignal { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequestFactory requestFactory { get; set; }

		[Inject]
		public global::Kampai.Game.PlayerSavedSignal playerSavedSignal { get; set; }

		public override void Execute()
		{
			saveLocation = tuple.Item1;
			saveID = tuple.Item2;
			saveImmediately = tuple.Item3;
			saveTimestampUTC = global::System.DateTime.UtcNow;
			if (global::UnityEngine.PlayerPrefs.HasKey("Debug.StopSaving"))
			{
				OnSavingResult(false);
				return;
			}
			if (global::Kampai.Main.LoadState.Get() != global::Kampai.Main.LoadStateType.STARTED)
			{
				OnSavingResult(false);
				return;
			}
			if (persistService.HasKey("AutoSaveLock"))
			{
				if (!saveImmediately)
				{
					OnSavingResult(false);
					return;
				}
				persistService.DeleteKey("AutoSaveLock");
			}
			else if (!saveImmediately)
			{
				persistService.PutDataInt("AutoSaveLock", 1);
			}
			playerDurationService.UpdateGameplayDuration();
			byte[] array = playerService.Serialize();
			if (array == null || array.Length == 0)
			{
				OnSavingResult(false);
			}
			else if (saveImmediately)
			{
				ImmediateSanityCheck(array);
			}
			else
			{
				routineRunner.StartCoroutine(SanityCheck(array));
			}
		}

		private void OnSavingResult(bool success)
		{
			playerSavedSignal.Dispatch(saveLocation, saveID, saveTimestampUTC, success);
		}

		private global::Kampai.Game.Player.SanityCheckFailureReason DeepScan(global::Kampai.Game.Player prevSave)
		{
			return playerService.DeepScan(prevSave);
		}

		private global::System.Collections.IEnumerator SanityCheck(byte[] playerData)
		{
			yield return null;
			global::Kampai.Game.Player previousSave = playerService.LastSave;
			global::Kampai.Game.Player currentSave = playerService.LoadPlayerData(global::System.Text.Encoding.UTF8.GetString(playerData));
			yield return null;
			global::Kampai.Game.Player.SanityCheckFailureReason sanityCheck = SanityCheck(previousSave, currentSave);
			yield return null;
			if (sanityCheck == global::Kampai.Game.Player.SanityCheckFailureReason.Passed)
			{
				PassedSanityCheck(playerData, currentSave);
				if (persistService.HasKey("AutoSaveLock"))
				{
					persistService.DeleteKey("AutoSaveLock");
				}
			}
			else
			{
				if (persistService.HasKey("AutoSaveLock"))
				{
					persistService.DeleteKey("AutoSaveLock");
				}
				FailedSanityCheck(previousSave, currentSave, sanityCheck);
			}
		}

		private void ImmediateSanityCheck(byte[] playerData)
		{
			global::Kampai.Game.Player lastSave = playerService.LastSave;
			global::Kampai.Game.Player currentSave = playerService.LoadPlayerData(global::System.Text.Encoding.UTF8.GetString(playerData));
			global::Kampai.Game.Player.SanityCheckFailureReason sanityCheckFailureReason = SanityCheck(lastSave, currentSave);
			if (sanityCheckFailureReason == global::Kampai.Game.Player.SanityCheckFailureReason.Passed)
			{
				PassedSanityCheck(playerData, currentSave);
			}
			else
			{
				FailedSanityCheck(lastSave, currentSave, sanityCheckFailureReason);
			}
		}

		private global::Kampai.Game.Player.SanityCheckFailureReason SanityCheck(global::Kampai.Game.Player previousSave, global::Kampai.Game.Player currentSave)
		{
			global::Kampai.Game.Player.SanityCheckFailureReason result = global::Kampai.Game.Player.SanityCheckFailureReason.Passed;
			if (saveLocation != global::Kampai.Game.SaveLocation.REMOTE_NOSANITY)
			{
				result = currentSave.ValidateSaveData(previousSave);
			}
			return result;
		}

		private void PassedSanityCheck(byte[] playerData, global::Kampai.Game.Player currentSave)
		{
			playerService.LastSave = currentSave;
			if (saveLocation == global::Kampai.Game.SaveLocation.REMOTE || saveLocation == global::Kampai.Game.SaveLocation.REMOTE_NOSANITY)
			{
				if (saveImmediately || persistService.HasKey("AutoSaveLock"))
				{
					RemoteSavePlayerData(playerData, saveImmediately);
				}
				else
				{
					OnSavingResult(false);
				}
			}
			else
			{
				persistService.PutData("Player_" + saveID, global::System.Text.Encoding.UTF8.GetString(playerData));
				OnSavingResult(true);
			}
		}

		private void FailedSanityCheck(global::Kampai.Game.Player previousSave, global::Kampai.Game.Player currentSave, global::Kampai.Game.Player.SanityCheckFailureReason sanityCheck)
		{
			OnSavingResult(false);
			global::Kampai.Game.Player.SanityCheckFailureReason sanityCheckFailureReason = DeepScan(previousSave);
			if (sanityCheckFailureReason == global::Kampai.Game.Player.SanityCheckFailureReason.Passed)
			{
				string text = global::System.Text.Encoding.UTF8.GetString(playerService.SavePlayerData(previousSave));
				string text2 = global::System.Text.Encoding.UTF8.GetString(playerService.SavePlayerData(currentSave));
				logger.Fatal(global::Kampai.Util.FatalCode.PS_FAILED_SANITY_CHECK, (int)sanityCheck, "Failed sanity check on save because {0} \n{1}\n--------\n{2}", sanityCheck.ToString(), text, text2);
			}
			else
			{
				logger.Fatal(global::Kampai.Util.FatalCode.PS_FAILED_DEEP_SCAN, (int)sanityCheckFailureReason, "Failed deep scan because {0}\n", sanityCheckFailureReason.ToString());
			}
		}

		private void RemoteSavePlayerData(byte[] playerData, bool gameIsGoingToSleep)
		{
			global::Kampai.Game.UserSession userSession = userSessionService.UserSession;
			if (userSession != null)
			{
				string userID = userSession.UserID;
				global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> signal = new global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse>();
				signal.AddListener(OnPlayerSaved);
				global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest request = requestFactory.Resource(ServerUrl + string.Format("/rest/gamestate/{0}", userID)).WithHeaderParam("user_id", userSession.UserID).WithHeaderParam("session_key", userSession.SessionID)
					.WithContentType("text/plain")
					.WithMethod("POST")
					.WithRequestCount(playerService.getAndIncrementRequestCounter())
					.WithBody(playerData);
				global::Kampai.Splash.IDownloadService obj = downloadService;
				global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest request3;
				if (gameIsGoingToSleep)
				{
					global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest request2 = request;
					request3 = request2;
				}
				else
				{
					request3 = request.WithResponseSignal(signal);
				}
				obj.Perform(request3, gameIsGoingToSleep);
			}
			else
			{
				OnSavingResult(false);
				logger.Fatal(global::Kampai.Util.FatalCode.CMD_SAVE_PLAYER, "No user session");
			}
		}

		private void OnPlayerSaved(global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse response)
		{
			if (!response.Success)
			{
				if (response.Code == 409)
				{
					OnSavingResult(false);
					global::Kampai.Game.ErrorResponse errorResponse = global::Newtonsoft.Json.JsonConvert.DeserializeObject<global::Kampai.Game.ErrorResponse>(response.Body);
					logger.Fatal(global::Kampai.Util.FatalCode.GS_ERROR_CORRUPT_SAVE_DETECTED, errorResponse.Error.Message);
					return;
				}
				if (!retried)
				{
					retried = true;
					networkConnectionLostSignal.Dispatch();
				}
				logger.Error("Unable to save player to server: {0}", response.Code);
				OnSavingResult(false);
			}
			else
			{
				OnSavingResult(true);
				logger.Log(global::Kampai.Util.KampaiLogLevel.Debug, "User data saved to the server");
			}
		}
	}
}
