namespace Kampai.Game
{
	public class UserSessionService : global::Kampai.Game.IUserSessionService
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("UserSessionService") as global::Kampai.Util.IKampaiLogger;

		private global::Kampai.Game.UserSession Session;

		private global::strange.extensions.signal.impl.Signal loginCallback;

		[Inject]
		public global::Kampai.Game.UserRegisteredSignal userRegisteredSignal { get; set; }

		[Inject]
		public global::Kampai.Util.IInvokerService invoker { get; set; }

		[Inject]
		public global::Kampai.Main.SetupHockeyAppUserSignal setupHockeyAppUser { get; set; }

		[Inject]
		public ILocalPersistanceService LocalPersistService { get; set; }

		[Inject]
		public global::Kampai.Game.IDLCService dlcService { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.Splash.IDownloadService downloadService { get; set; }

		[Inject]
		public global::Kampai.Main.SetupSwrveSignal setupSwrveSignal { get; set; }

		[Inject]
		public global::Kampai.Main.SetupSupersonicSignal setupSupersonicSignal { get; set; }

		[Inject]
		public global::Kampai.Game.UpdateUserSignal updateUserSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ISynergyService synergyService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.UserSessionGrantedSignal userSessionGrantedSignal { get; set; }

		public global::Kampai.Game.UserSession UserSession
		{
			get
			{
				return Session;
			}
			set
			{
				Session = value;
			}
		}

		public void LoginRequestCallback(global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse response)
		{
			global::Kampai.Util.TimeProfiler.EndSection("login");
			if (response.Success)
			{
				string body = response.Body;
				global::UnityEngine.Debug.Log(string.Format("<color=cyan>[DEBUG] Login Response Body:</color> {0}", body));
				global::Kampai.Game.UserSession userSession = (UserSession = global::Newtonsoft.Json.JsonConvert.DeserializeObject<global::Kampai.Game.UserSession>(body));
				userSessionGrantedSignal.Dispatch();
				LocalPersistService.PutData("LoadMode", "remote");
				updateSynergyId(userSession);
				string empty = string.Empty;
				if (response.Headers.ContainsKey("X-Kampai-Remote-IP-Address"))
				{
					empty = response.Headers["X-Kampai-Remote-IP-Address"];
					logger.Info("Client IP address reported as {0}", empty);
				}
				if (loginCallback != null)
				{
					loginCallback.Dispatch();
					return;
				}
				telemetryService.Send_Telemetry_EVT_USER_GAME_LOAD_FUNNEL("70 - User Login", playerService.SWRVEGroup, dlcService.GetDownloadQualityLevel());
				logger.Log(global::Kampai.Util.KampaiLogLevel.Info, "User's session ID: {0}", UserSession.SessionID);
				setupSwrveSignal.Dispatch(userSession.UserID);
				setupSupersonicSignal.Dispatch();
				global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> signal = new global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse>();
				signal.AddListener(CatchAuthenticationErrorResponse);
				downloadService.AddGlobalResponseListener(signal);
			}
			else
			{
				invoker.Add(delegate
				{
					logger.Fatal(global::Kampai.Util.FatalCode.GS_ERROR_LOGIN, "Response code {0}", response.Code);
				});
			}
		}

		public void RegisterRequestCallback(global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse response)
		{
			global::Kampai.Util.TimeProfiler.EndSection("register");
			if (response.Success)
			{
				string body = response.Body;
				global::UnityEngine.Debug.Log(string.Format("<color=cyan>[DEBUG] Register Response Body:</color> {0}", body));
				global::Kampai.Game.UserIdentity userIdentity = global::Newtonsoft.Json.JsonConvert.DeserializeObject<global::Kampai.Game.UserIdentity>(body);
				setupHockeyAppUser.Dispatch(userIdentity.UserID);
				userRegisteredSignal.Dispatch(userIdentity);
				return;
			}
			logger.Log(global::Kampai.Util.KampaiLogLevel.Error, "RegisterUserCommand error with URL : {0}", response.Request.Uri);
			invoker.Add(delegate
			{
				logger.Fatal(global::Kampai.Util.FatalCode.GS_ERROR_LOGIN_3, "Response code {0}", response.Code);
			});
		}

		public void UserUpdateRequestCallback(string synergyID, global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse response)
		{
			if (response.Success)
			{
				Session.SynergyID = synergyID;
				return;
			}
			logger.Log(global::Kampai.Util.KampaiLogLevel.Error, "Failed to update user {0} with synergy ID {1}", UserSession.UserID, synergyID);
		}

		public void setLoginCallback(global::strange.extensions.signal.impl.Signal a)
		{
			loginCallback = a;
		}

		public void OpenURL(string url)
		{
			global::UnityEngine.Application.OpenURL(url);
		}

		private void CatchAuthenticationErrorResponse(global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse response)
		{
			if (response.Code != 401 || response.Body == null)
			{
				return;
			}
			try
			{
				global::Kampai.Game.ErrorResponse errorResponse = global::Newtonsoft.Json.JsonConvert.DeserializeObject<global::Kampai.Game.ErrorResponse>(response.Body);
				if (errorResponse.Error.Message != null && errorResponse.Error.Message.Equals("Invalid Session"))
				{
					invoker.Add(delegate
					{
						logger.Fatal(global::Kampai.Util.FatalCode.SESSION_INVALID);
					});
				}
			}
			catch (global::Newtonsoft.Json.JsonSerializationException)
			{
				logger.Debug("UserSessionService:CatchAuthenticationErrorResponse - JsonSerializationException");
			}
			catch (global::Newtonsoft.Json.JsonReaderException)
			{
				logger.Debug("UserSessionService:CatchAuthenticationErrorResponse - JsonReaderException");
			}
		}

		private void updateSynergyId(global::Kampai.Game.UserSession session)
		{
			string userID = synergyService.userID;
			string synergyID = session.SynergyID;
			if (string.IsNullOrEmpty(synergyID) && !string.IsNullOrEmpty(userID))
			{
				updateUserSignal.Dispatch(userID);
			}
			if (!string.IsNullOrEmpty(synergyID) && !synergyID.Equals(userID))
			{
				logger.Debug("SynergyIds don't match oops, changing them, old SynergyID = {0}  new Synergy ID = {1} ", NimbleBridge_SynergyIdManager.GetComponent().GetSynergyId(), synergyID);
				using (NimbleBridge_SynergyIdManager.GetComponent().Login(synergyID, session.UserID))
				{
				}
			}
		}
	}
}
