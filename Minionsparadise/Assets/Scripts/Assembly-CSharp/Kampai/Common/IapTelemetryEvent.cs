namespace Kampai.Common
{
	public class IapTelemetryEvent
	{
		public string productId = string.Empty;

		public double productPrice;

		public string currency = string.Empty;

		public string appleReceipt = string.Empty;

		public string appleTransactionId = string.Empty;

		public string googlePurchaseData = string.Empty;

		public string googleDataSignature = string.Empty;

		public string googleOrderId = string.Empty;

		public uint nimbleMtxErrorCode;

		public override string ToString()
		{
			return string.Format("IapTelemetryEvent: productId = {0}, productPrice = {1}, currency = {2},\nappleReceipt = {3}, appleTransactionId = {4}, googlePurchaseData = {5}, googleDataSignature = {6}, nimbleMtxErrorCode = {7}", productId, productPrice, currency, appleReceipt, appleTransactionId, googlePurchaseData, googleDataSignature, nimbleMtxErrorCode);
		}
	}
}
