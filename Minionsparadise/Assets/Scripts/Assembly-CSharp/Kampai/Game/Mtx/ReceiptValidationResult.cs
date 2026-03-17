namespace Kampai.Game.Mtx
{
	public class ReceiptValidationResult
	{
		public enum Code
		{
			SUCCESS = 0,
			RECEIPT_INVALID = 1,
			RECEIPT_DUPLICATE = 2,
			VALIDATION_UNAVAILABLE = 3
		}

		public string sku;

		public string nimbleTransactionId;

		public string platformStoreTransactionId;

		public global::Kampai.Game.Mtx.ReceiptValidationResult.Code code;

		public ReceiptValidationResult(string sku, string nimbleTransactionId, string platformStoreTransactionId, global::Kampai.Game.Mtx.ReceiptValidationResult.Code code)
		{
			this.sku = sku;
			this.nimbleTransactionId = nimbleTransactionId;
			this.platformStoreTransactionId = platformStoreTransactionId;
			this.code = code;
		}
	}
}
