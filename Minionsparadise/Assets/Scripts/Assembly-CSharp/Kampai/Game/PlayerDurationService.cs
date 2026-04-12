namespace Kampai.Game
{
	public class PlayerDurationService : global::Kampai.Game.IPlayerDurationService
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("PlayerDurationService") as global::Kampai.Util.IKampaiLogger;

		private int lastGameplaySecondsUpdate;

		private int gameplayDeltaSinceLastSave;

		private int accumulatedPauseTimePerAppSeesion;

		private int lastPauseUTC;

		private int SESSION_COUNTER_MAX_APP_PAUSE_DURATION_SEC = 120;

		int global::Kampai.Game.IPlayerDurationService.TotalSecondsSinceLevelUp
		{
			get
			{
				logger.Debug("LEVELUP TELEMETRY: TotalSecondsSinceLevelUp");
				logger.Debug(string.Format("LEVELUP TELEMETRY: ActualUTC: {0}", timeService.CurrentTime()));
				logger.Debug(string.Format("LEVELUP TELEMETRY: LevelUpUTC: {0}", playerService.LevelUpUTC));
				return timeService.CurrentTime() - playerService.LevelUpUTC;
			}
		}

		int global::Kampai.Game.IPlayerDurationService.GameplaySecondsSinceLevelUp
		{
			get
			{
				logger.Debug("LEVELUP TELEMETRY: GameplaySecondsSinceLevelUp");
				int num = timeService.AppTime() - lastGameplaySecondsUpdate;
				logger.Debug(string.Format("LEVELUP TELEMETRY: AppTime: {0}", timeService.AppTime()));
				logger.Debug(string.Format("LEVELUP TELEMETRY: lastGameplaySecondsUpdate: {0}", lastGameplaySecondsUpdate));
				logger.Debug(string.Format("LEVELUP TELEMETRY: GameplaySecondsSinceLevelUp: {0}", playerService.GameplaySecondsSinceLevelUp));
				logger.Debug(string.Format("LEVELUP TELEMETRY: currentGameplaySeconds: {0}", num));
				return playerService.GameplaySecondsSinceLevelUp + num;
			}
		}

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Game.PlayerSessionCountUpdatedSignal playerSessionCountUpdatedSignal { get; set; }

		public int TotalGamePlaySeconds
		{
			get
			{
				return playerService.AccumulatedGameplayDuration + timeService.AppTime() - gameplayDeltaSinceLastSave - accumulatedPauseTimePerAppSeesion;
			}
		}

		void global::Kampai.Game.IPlayerDurationService.InitLevelUpUTC()
		{
			logger.Debug("LEVELUP TELEMETRY: InitLevelUpUTC");
			logger.Debug(string.Format("LEVELUP TELEMETRY: AppTime: {0}", timeService.AppTime()));
			if (playerService.LevelUpUTC == 0)
			{
				logger.Debug(string.Format("LEVELUP TELEMETRY: Initializing LevelUpUTC: {0}", timeService.CurrentTime()));
				playerService.LevelUpUTC = timeService.CurrentTime();
			}
			lastGameplaySecondsUpdate = timeService.AppTime();
			gameplayDeltaSinceLastSave = 0;
		}

		void global::Kampai.Game.IPlayerDurationService.MarkLevelUpUTC()
		{
			logger.Debug("LEVELUP TELEMETRY: MarkLevelUpUTC");
			playerService.LevelUpUTC = timeService.CurrentTime();
			playerService.GameplaySecondsSinceLevelUp = 0;
			lastGameplaySecondsUpdate = timeService.AppTime();
		}

		void global::Kampai.Game.IPlayerDurationService.SetLastGameStartUTC()
		{
			UpdateLastTimePlayed();
			playerService.LastPlayedUTC = timeService.CurrentTime();
			if (playerService.FirstGameStartUTC == 0)
			{
				playerService.FirstGameStartUTC = playerService.LastPlayedUTC;
			}
			playerService.AlterQuantity(global::Kampai.Game.StaticItem.PLAYER_SESSION_COUNT, 1);
			playerSessionCountUpdatedSignal.Dispatch();
		}

		void global::Kampai.Game.IPlayerDurationService.UpdateGameplayDuration()
		{
			logger.Debug("LEVELUP TELEMETRY: UpdateGameplayDuration");
			logger.Debug(string.Format("LEVELUP TELEMETRY: lastGameplaySecondsUpdate: {0}", lastGameplaySecondsUpdate));
			logger.Debug(string.Format("LEVELUP TELEMETRY: AppTime: {0}", timeService.AppTime()));
			int num = timeService.AppTime() - lastGameplaySecondsUpdate;
			logger.Debug(string.Format("LEVELUP TELEMETRY: gameplaySecondsSinceLastUpdate: {0}", num));
			playerService.GameplaySecondsSinceLevelUp += num;
			playerService.AccumulatedGameplayDuration += timeService.AppTime() - gameplayDeltaSinceLastSave - accumulatedPauseTimePerAppSeesion;
			logger.Debug(string.Format("LEVELUP TELEMETRY: GameplaySecondsSinceLevelUp: {0}", playerService.GameplaySecondsSinceLevelUp));
			lastGameplaySecondsUpdate = timeService.AppTime();
			gameplayDeltaSinceLastSave = timeService.AppTime() - accumulatedPauseTimePerAppSeesion;
		}

		void global::Kampai.Game.IPlayerDurationService.OnAppPause()
		{
			UpdateLastTimePlayed();
			lastPauseUTC = timeService.CurrentTime();
		}

		void global::Kampai.Game.IPlayerDurationService.OnAppResume()
		{
			UpdateLastTimePlayed();
			if (lastPauseUTC > 0)
			{
				int num = timeService.CurrentTime() - lastPauseUTC;
				accumulatedPauseTimePerAppSeesion += num;
				if (num > SESSION_COUNTER_MAX_APP_PAUSE_DURATION_SEC)
				{
					playerService.AlterQuantity(global::Kampai.Game.StaticItem.PLAYER_SESSION_COUNT, 1);
					playerSessionCountUpdatedSignal.Dispatch();
				}
			}
		}

		int global::Kampai.Game.IPlayerDurationService.GetGameTimeDuration(global::Kampai.Game.IGameTimeTracker gameTimeTracker)
		{
			return TotalGamePlaySeconds - gameTimeTracker.StartGameTime;
		}

		private bool SameDate(global::System.DateTime first, global::System.DateTime second)
		{
			if (first.Year == second.Year && first.Month == second.Month && first.Day == second.Day)
			{
				return true;
			}
			return false;
		}

		private void SetConsecutiveDays(int previousSeconds, int currentSeconds)
		{
			global::System.DateTime first = new global::System.DateTime(1970, 1, 1, 0, 0, 0, 0, global::System.DateTimeKind.Utc).AddSeconds(currentSeconds);
			global::System.DateTime second = new global::System.DateTime(1970, 1, 1, 0, 0, 0, 0, global::System.DateTimeKind.Utc).AddSeconds(previousSeconds);
			if (!SameDate(first, second))
			{
				second = second.AddDays(1.0);
				if (SameDate(first, second))
				{
					playerService.AlterQuantity(global::Kampai.Game.StaticItem.CONSECUTIVE_DAYS_COUNT, 1);
				}
				else
				{
					playerService.SetQuantity(global::Kampai.Game.StaticItem.CONSECUTIVE_DAYS_COUNT, 1);
				}
			}
		}

		private void UpdateLastTimePlayed()
		{
			int num = timeService.CurrentTime();
			SetConsecutiveDays(playerService.LastPlayedUTC, num);
			playerService.LastPlayedUTC = num;
		}
	}
}
