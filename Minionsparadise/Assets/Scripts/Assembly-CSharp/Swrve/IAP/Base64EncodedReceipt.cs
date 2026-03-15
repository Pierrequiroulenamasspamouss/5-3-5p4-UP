namespace Swrve.IAP
{
	public class Base64EncodedReceipt : global::Swrve.IAP.IapReceipt
	{
		private string base64encodedreceipt;

		private Base64EncodedReceipt(string r)
		{
			base64encodedreceipt = r;
		}

		public static global::Swrve.IAP.IapReceipt FromString(string r)
		{
			return new global::Swrve.IAP.Base64EncodedReceipt(r);
		}

		public string GetBase64EncodedReceipt()
		{
			return base64encodedreceipt;
		}
	}
}
