namespace Kampai.Game.Mtx
{
	public interface IMtxReceiptValidationService
	{
		void AddPendingReceipt(string sku, string nimbleTransactionId, string platformStoreTransactionId, global::Kampai.Game.Mtx.IMtxReceipt receipt);

		void ValidatePendingReceipt();

		void ValidationResultCallback(global::Kampai.Game.Mtx.ReceiptValidationResult result);

		void RemovePendingReceipt(string nimbleTransactionId);

		bool HasPendingReceipts();
	}
}
