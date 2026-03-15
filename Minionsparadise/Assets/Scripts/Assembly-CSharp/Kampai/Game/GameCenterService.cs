namespace Kampai.Game
{
	public class GameCenterService : global::Kampai.Game.ISocialService, global::Kampai.Game.ISynergyService
	{
		private global::strange.extensions.signal.impl.Signal<global::Kampai.Game.ISocialService> successSignal;

		private global::strange.extensions.signal.impl.Signal<global::Kampai.Game.ISocialService> failureSignal;

		private global::Kampai.Game.GameCenterAuthToken token;

		private bool killSwitchFlag;

		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("GameCenterService") as global::Kampai.Util.IKampaiLogger;

		[Inject(global::Kampai.Main.MainElement.MANAGER_PARENT)]
		public global::UnityEngine.GameObject managers { get; set; }

		[Inject]
		public global::Kampai.Game.GameCenterAuthTokenCompleteSignal authCompleteSignal { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.Game.IConfigurationsService configurationsService { get; set; }

		public string LoginSource { get; set; }

		public string userID
		{
			get
			{
				return string.Empty;
			}
		}

		public global::Kampai.Game.SocialServices type
		{
			get
			{
				return global::Kampai.Game.SocialServices.GAMECENTER;
			}
		}

		public bool isLoggedIn
		{
			get
			{
				return false;
			}
		}

		public bool isKillSwitchEnabled
		{
			get
			{
				return killSwitchFlag;
			}
		}

		public string accessToken
		{
			get
			{
				logger.Debug("accessToken = {0}", global::Newtonsoft.Json.JsonConvert.SerializeObject(token));
				return global::Newtonsoft.Json.JsonConvert.SerializeObject(token);
			}
		}

		public string userName
		{
			get
			{
				return string.Empty;
			}
		}

		public global::System.DateTime tokenExpiry
		{
			get
			{
				return default(global::System.DateTime);
			}
		}

		public string locKey
		{
			get
			{
				return "AccountTypeGameCenter";
			}
		}

		public void Init(global::strange.extensions.signal.impl.Signal<global::Kampai.Game.ISocialService> successSignal, global::strange.extensions.signal.impl.Signal<global::Kampai.Game.ISocialService> failureSignal)
		{
			logger.Debug("Game Center Login");
			updateKillSwitchFlag();
			this.successSignal = successSignal;
			this.failureSignal = failureSignal;
			if (isKillSwitchEnabled)
			{
				failureSignal.Dispatch(this);
			}
		}

		private void AuthenticationFail(string message)
		{
			RemoveAuthSubscribers();
			logger.Debug("Game Center Login Failed: " + message);
			failureSignal.Dispatch(this);
		}

		private void AuthenticationSuccess()
		{
		}

		private void GenerateIdentityFail(string message)
		{
			RemoveIdentitySubscribers();
			logger.Debug("Game Center Generate Identity Failed: " + message);
		}

		private void GenerateIdentitySuccess(global::System.Collections.Generic.Dictionary<string, string> identity)
		{
			logger.Debug("Game Center Generate Identity Success");
			global::UnityEngine.GameObject gameObject = managers.FindChild("GameCenterAuthManager");
			if (gameObject != null)
			{
				authCompleteSignal.AddOnce(delegate
				{
					global::Kampai.Game.GameCenterAuthToken gameCenterAuthToken = new global::Kampai.Game.GameCenterAuthToken
					{
						publicKeyUrl = identity["publicKeyUrl"],
						signature = identity["signature"],
						salt = identity["salt"],
						timestamp = identity["timestamp"],
						bundleId = global::Kampai.Util.Native.BundleIdentifier,
						playerId = string.Empty
					};
					token = gameCenterAuthToken;
					successSignal.Dispatch(this);
				});
			}
		}

		private void RemoveAuthSubscribers()
		{
		}

		private void RemoveIdentitySubscribers()
		{
		}

		public void Login(global::strange.extensions.signal.impl.Signal<global::Kampai.Game.ISocialService> successSignal, global::strange.extensions.signal.impl.Signal<global::Kampai.Game.ISocialService> failureSignal, global::System.Action callback)
		{
			Init(successSignal, failureSignal);
		}

		public void Logout()
		{
		}

		public void updateKillSwitchFlag()
		{
			killSwitchFlag = configurationsService.isKillSwitchOn(global::Kampai.Game.KillSwitch.GAMECENTER);
			logger.Info("Game Center killswitch {0}", killSwitchFlag);
		}

		public void SendLoginTelemetry(string loginLocation)
		{
			telemetryService.Send_Telemetry_EVT_EBISU_LOGIN_GAMECENTER(loginLocation);
		}

		public void incrementAchievement(string achievementID, float percentComplete)
		{
		}

		public void ShowAchievements()
		{
		}
	}
}
