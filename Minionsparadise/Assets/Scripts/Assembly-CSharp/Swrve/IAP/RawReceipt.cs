namespace Swrve.IAP
{
	public class RawReceipt : global::Swrve.IAP.IapReceipt
	{
		private string base64encodedreceipt;

		private RawReceipt(string r)
		{
			base64encodedreceipt = global::System.Convert.ToBase64String(global::System.Text.Encoding.UTF8.GetBytes(r));
		}

		public static global::Swrve.IAP.IapReceipt FromString(string r)
		{
			return new global::Swrve.IAP.RawReceipt(r);
		}

		public string GetBase64EncodedReceipt()
		{
			return base64encodedreceipt;
		}
	}
}
