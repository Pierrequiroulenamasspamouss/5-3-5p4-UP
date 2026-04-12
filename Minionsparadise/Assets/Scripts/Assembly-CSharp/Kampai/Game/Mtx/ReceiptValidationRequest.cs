namespace Kampai.Game.Mtx
{
	public class ReceiptValidationRequest
	{
		public string sku;

		[global::Newtonsoft.Json.JsonProperty("mtxTransactionId")]
		public string nimbleTransactionId;

		public string platformStoreTransactionId = string.Empty;

		public global::Kampai.Game.Mtx.IMtxReceipt receipt;

		public ReceiptValidationRequest(string sku, string nimbleTransactionId, string platformStoreTransactionId, global::Kampai.Game.Mtx.IMtxReceipt receipt)
		{
			this.sku = sku;
			this.nimbleTransactionId = nimbleTransactionId;
			this.platformStoreTransactionId = platformStoreTransactionId;
			this.receipt = receipt;
		}
	}
}
