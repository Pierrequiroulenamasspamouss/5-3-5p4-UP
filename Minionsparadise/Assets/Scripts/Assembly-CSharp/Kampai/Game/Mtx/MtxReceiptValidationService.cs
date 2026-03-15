namespace Kampai.Game.Mtx
{
	public class MtxReceiptValidationService : global::Kampai.Game.Mtx.IMtxReceiptValidationService
	{
		private string TAG = "[NCS] ";

		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("MtxReceiptValidationService") as global::Kampai.Util.IKampaiLogger;

		private global::System.Collections.Generic.List<global::Kampai.Game.Mtx.ReceiptValidationRequest> pendingReceiptValidationRequests;

		private global::Kampai.Game.Mtx.ReceiptValidationRequest requestInProgress;

		[Inject]
		public global::Kampai.Game.StartMtxReceiptValidationSignal startMtxReceiptValidationSignal { get; set; }

		[Inject]
		public global::Kampai.Game.FinishMtxReceiptValidationSignal finishMtxReceiptValidationSignal { get; set; }

		[Inject]
		public ILocalPersistanceService localPersistence { get; set; }

		[PostConstruct]
		public void PostConstruct()
		{
			pendingReceiptValidationRequests = LoadFromPersistence();
		}

		public void AddPendingReceipt(string sku, string nimbleTransactionId, string platformStoreTransactionId, global::Kampai.Game.Mtx.IMtxReceipt receipt)
		{
			if (pendingReceiptValidationRequests.Find((global::Kampai.Game.Mtx.ReceiptValidationRequest r) => r.nimbleTransactionId.Equals(nimbleTransactionId)) != null)
			{
				logger.Warning("{0}MtxReceiptValidationService.AddPendingReceipt() receipt for tr-n Id is already exist. No-op for sku {1}, nimble tr-n ID {2}", TAG, sku, nimbleTransactionId);
			}
			else
			{
				logger.Debug("{0}MtxReceiptValidationService.AddPendingReceipt: sku {1}, nimble tr-n ID {2}", TAG, sku, nimbleTransactionId);
				pendingReceiptValidationRequests.Add(new global::Kampai.Game.Mtx.ReceiptValidationRequest(sku, nimbleTransactionId, platformStoreTransactionId, receipt));
				SaveToPersistence(pendingReceiptValidationRequests);
			}
		}

		public void ValidatePendingReceipt()
		{
			if (requestInProgress == null && pendingReceiptValidationRequests.Count > 0)
			{
				requestInProgress = pendingReceiptValidationRequests[0];
				logger.Debug("{0}MtxReceiptValidationService.ValidatePendingReceipt for sku {1}", TAG, requestInProgress.sku);
				startMtxReceiptValidationSignal.Dispatch(requestInProgress);
			}
		}

		public void ValidationResultCallback(global::Kampai.Game.Mtx.ReceiptValidationResult result)
		{
			logger.Debug("{0}MtxReceiptValidationService.ValidationResultCallback for sku {1}", TAG, requestInProgress.sku);
			requestInProgress = null;
			finishMtxReceiptValidationSignal.Dispatch(result);
		}

		public void RemovePendingReceipt(string nimbleTransactionId)
		{
			logger.Debug("{0}MtxReceiptValidationService.RemovePendingReceipt: nimble tr-n ID {1}", TAG, nimbleTransactionId);
			pendingReceiptValidationRequests.RemoveAll((global::Kampai.Game.Mtx.ReceiptValidationRequest r) => r.nimbleTransactionId.Equals(nimbleTransactionId));
			SaveToPersistence(pendingReceiptValidationRequests);
		}

		public bool HasPendingReceipts()
		{
			return pendingReceiptValidationRequests.Count > 0;
		}

		private void SaveToPersistence(global::System.Collections.Generic.List<global::Kampai.Game.Mtx.ReceiptValidationRequest> pendingReceiptValidationRequests)
		{
			try
			{
				string data = global::Newtonsoft.Json.JsonConvert.SerializeObject(pendingReceiptValidationRequests);
				localPersistence.PutData("MtxPendingReceipts", data);
			}
			catch (global::Newtonsoft.Json.JsonSerializationException ex)
			{
				logger.Error("{0}SaveToPersistence(): Json Parse Err: {1}", TAG, ex);
			}
			catch (global::System.Exception ex2)
			{
				logger.Error("{0}SaveToPersistence(): error: {1}", TAG, ex2);
			}
		}

		private global::System.Collections.Generic.List<global::Kampai.Game.Mtx.ReceiptValidationRequest> LoadFromPersistence()
		{
			global::System.Collections.Generic.List<global::Kampai.Game.Mtx.ReceiptValidationRequest> list = null;
			string data = localPersistence.GetData("MtxPendingReceipts");
			if (data != null)
			{
				try
				{
					list = global::Newtonsoft.Json.JsonConvert.DeserializeObject<global::System.Collections.Generic.List<global::Kampai.Game.Mtx.ReceiptValidationRequest>>(data, new global::Newtonsoft.Json.JsonConverter[1]
					{
						new global::Kampai.Game.MtxReceiptConverter()
					});
				}
				catch (global::Newtonsoft.Json.JsonSerializationException e)
				{
					HandleLoadJsonException(e);
				}
				catch (global::Newtonsoft.Json.JsonReaderException e2)
				{
					HandleLoadJsonException(e2);
				}
				catch (global::System.Exception ex)
				{
					logger.Error("{0}SaveToPersistence(): error: {1}", TAG, ex);
				}
			}
			return list ?? new global::System.Collections.Generic.List<global::Kampai.Game.Mtx.ReceiptValidationRequest>();
		}

		private void HandleLoadJsonException(global::System.Exception e)
		{
			logger.Error("{0}LoadFromPersistence(): Json Parse Err: {1}", TAG, e);
		}
	}
}
