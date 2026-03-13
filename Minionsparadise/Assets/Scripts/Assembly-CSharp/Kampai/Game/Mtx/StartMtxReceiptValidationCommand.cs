namespace Kampai.Game.Mtx
{
	public class StartMtxReceiptValidationCommand : global::strange.extensions.command.impl.Command
	{
		public const string VALIDATION_ENDPOINT_APPLE_APPSTORE = "/rest/transaction/verify/apple";

		public const string VALIDATION_ENDPOINT_GOOGLE_PLAY = "/rest/transaction/verify/google";

		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("StartMtxReceiptValidationCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.Mtx.ReceiptValidationRequest request { get; set; }

		[Inject]
		public global::Kampai.Game.Mtx.IMtxReceiptValidationService receiptValidationService { get; set; }

		[Inject]
		public global::Kampai.Game.IUserSessionService userSessionService { get; set; }

		[Inject]
		public ILocalPersistanceService localPersistService { get; set; }

		[Inject("game.server.host")]
		public string serverUrl { get; set; }

		[Inject]
		public global::Kampai.Splash.IDownloadService downloadService { get; set; }

		[Inject]
		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequestFactory requestFactory { get; set; }

		public override void Execute()
		{
			string userId = GetUserId();
			if (string.IsNullOrEmpty(userId))
			{
				logger.Error("[NCS] StartMtxReceiptValidationCommand.Execute(): unable to validate receipt: user is unknown at the moment.");
				global::Kampai.Game.Mtx.ReceiptValidationResult result = new global::Kampai.Game.Mtx.ReceiptValidationResult(request.sku, request.nimbleTransactionId, request.platformStoreTransactionId, global::Kampai.Game.Mtx.ReceiptValidationResult.Code.VALIDATION_UNAVAILABLE);
				receiptValidationService.ValidationResultCallback(result);
			}
			else if (!CreateAndSendHttpRequest(userId, request))
			{
				logger.Error("[NCS] StartMtxReceiptValidationCommand.Execute(): can't prepare validation request. Possible reason: unsupported receipt type(Amazon for example) or invalid receipt.");
				global::Kampai.Game.Mtx.ReceiptValidationResult result2 = new global::Kampai.Game.Mtx.ReceiptValidationResult(request.sku, request.nimbleTransactionId, request.platformStoreTransactionId, global::Kampai.Game.Mtx.ReceiptValidationResult.Code.RECEIPT_INVALID);
				receiptValidationService.ValidationResultCallback(result2);
			}
			else
			{
				Retain();
			}
		}

		private void OnHttpResponse(global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse response)
		{
			global::Kampai.Game.Mtx.ReceiptValidationResult.Code code = global::Kampai.Game.Mtx.ReceiptValidationResult.Code.VALIDATION_UNAVAILABLE;
			if (response.Success)
			{
				code = global::Kampai.Game.Mtx.ReceiptValidationResult.Code.SUCCESS;
			}
			else
			{
				logger.Error("[NCS] StartMtxReceiptValidationCommand.Execute(): validation request failure. Response status code: {0}, response error msg: {1}", response.Code, response.Body ?? "null");
				global::Kampai.Game.Mtx.ServerValidationError validationErrorFromHttpBody = GetValidationErrorFromHttpBody(response.Body);
				if (validationErrorFromHttpBody != null)
				{
					logger.Error("[NCS] StartMtxReceiptValidationCommand.Execute(): validation request failure. Logical server error: {0}", validationErrorFromHttpBody.code);
					switch (validationErrorFromHttpBody.code)
					{
					case global::Kampai.Game.Mtx.ServerValidationError.Code.RECEIPT_DUPLICATE:
						code = global::Kampai.Game.Mtx.ReceiptValidationResult.Code.RECEIPT_DUPLICATE;
						break;
					case global::Kampai.Game.Mtx.ServerValidationError.Code.RECEIPT_INVALID:
						code = global::Kampai.Game.Mtx.ReceiptValidationResult.Code.RECEIPT_INVALID;
						break;
					case global::Kampai.Game.Mtx.ServerValidationError.Code.VALIDATION_UNAVAILABLE:
						code = global::Kampai.Game.Mtx.ReceiptValidationResult.Code.VALIDATION_UNAVAILABLE;
						break;
					default:
						logger.Error("[NCS] StartMtxReceiptValidationCommand.Execute(): unsupported server error code: {0}", validationErrorFromHttpBody.code);
						code = global::Kampai.Game.Mtx.ReceiptValidationResult.Code.VALIDATION_UNAVAILABLE;
						break;
					}
				}
				else
				{
					logger.Error("[NCS] StartMtxReceiptValidationCommand.Execute(): can't extract logical server error from response body");
				}
			}
			global::Kampai.Game.Mtx.ReceiptValidationResult result = new global::Kampai.Game.Mtx.ReceiptValidationResult(request.sku, request.nimbleTransactionId, request.platformStoreTransactionId, code);
			receiptValidationService.ValidationResultCallback(result);
			Release();
		}

		private global::Kampai.Game.Mtx.ServerValidationError GetValidationErrorFromHttpBody(string body)
		{
			if (string.IsNullOrEmpty(body))
			{
				logger.Error("[NCS] StartMtxReceiptValidationCommand.GetValidationErrorFromHttpBody(): null http response body");
				return null;
			}
			global::Kampai.Game.Mtx.ServerValidationError result = null;
			try
			{
				global::Kampai.Game.Mtx.ServerValidationErrorResponse serverValidationErrorResponse = global::Newtonsoft.Json.JsonConvert.DeserializeObject<global::Kampai.Game.Mtx.ServerValidationErrorResponse>(body);
				result = ((serverValidationErrorResponse == null) ? null : serverValidationErrorResponse.error);
			}
			catch (global::Newtonsoft.Json.JsonSerializationException e)
			{
				HandleJsonException(e);
			}
			catch (global::Newtonsoft.Json.JsonReaderException e2)
			{
				HandleJsonException(e2);
			}
			return result;
		}

		private void HandleJsonException(global::System.Exception e)
		{
			logger.Error("[NCS] StartMtxReceiptValidationCommand.GetValidationErrorFromHttpBody(): ServerValidationError deserialization error {0}", e.Message);
		}

		private string GetUserId()
		{
			string text = null;
			global::Kampai.Game.UserSession userSession = userSessionService.UserSession;
			if (userSession != null && !string.IsNullOrEmpty(userSession.UserID))
			{
				text = userSession.UserID;
				logger.Info("[NCS] StartMtxReceiptValidationCommand.GetUserId(): use user ID from user session: {0}", text);
			}
			if (text == null)
			{
				string text2 = localPersistService.GetData("UserID");
				if (!string.IsNullOrEmpty(text2))
				{
					text = text2;
					logger.Info("[NCS] StartMtxReceiptValidationCommand.GetUserId(): use user ID from persistance: {0}.", text);
				}
			}
			return text;
		}

		private bool CreateAndSendHttpRequest(string userId, global::Kampai.Game.Mtx.ReceiptValidationRequest request)
		{
			global::Kampai.Game.Mtx.IMtxReceipt receipt = request.receipt;
			if (receipt == null)
			{
				logger.Error("[NCS] StartMtxReceiptValidationCommand.CreateHttpRequest(): null receipt");
				return false;
			}
			if (receipt is global::Kampai.Game.Mtx.GooglePlayReceipt)
			{
				global::Kampai.Game.Mtx.GooglePlayReceipt googlePlayReceipt = receipt as global::Kampai.Game.Mtx.GooglePlayReceipt;
				global::Kampai.Game.Mtx.GooglePlayReceiptValidationRequest googlePlayReceiptValidationRequest = new global::Kampai.Game.Mtx.GooglePlayReceiptValidationRequest();
				googlePlayReceiptValidationRequest.userId = userId;
				googlePlayReceiptValidationRequest.signedData = googlePlayReceipt.signedData;
				googlePlayReceiptValidationRequest.signature = googlePlayReceipt.signature;
				global::Kampai.Game.Mtx.GooglePlayReceiptValidationRequest googlePlayReceiptValidationRequest2 = googlePlayReceiptValidationRequest;
				string url = serverUrl + "/rest/transaction/verify/google";
				logger.Debug("[NCS] CreateAndSendHttpRequest(): userId: {0}, signedData: {1}, signature: {2}", googlePlayReceiptValidationRequest2.userId, googlePlayReceiptValidationRequest2.signedData, googlePlayReceiptValidationRequest2.signature);
				SendHttpRequest(url, googlePlayReceiptValidationRequest2);
				return true;
			}
			logger.Error("[NCS] StartMtxReceiptValidationCommand.CreateHttpRequest(): unsupported receipt type: {0}", receipt);
			return false;
		}

		private void SendHttpRequest(string url, object body)
		{
			global::Kampai.Splash.DownloadResponseSignal downloadResponseSignal = new global::Kampai.Splash.DownloadResponseSignal();
			downloadResponseSignal.AddListener(OnHttpResponse);
			downloadService.Perform(requestFactory.Resource(url).WithHeaderParam("user_id", userSessionService.UserSession.UserID).WithHeaderParam("session_key", userSessionService.UserSession.SessionID)
				.WithMethod("POST")
				.WithEntity(body)
				.WithContentType("application/json")
				.WithResponseSignal(downloadResponseSignal));
		}
	}
}
