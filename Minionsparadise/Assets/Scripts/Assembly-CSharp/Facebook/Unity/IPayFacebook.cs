namespace Facebook.Unity
{
	internal interface IPayFacebook
	{
		void Pay(string product, string action, int quantity, int? quantityMin, int? quantityMax, string requestId, string pricepointId, string testCurrency, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IPayResult> callback);
	}
}
