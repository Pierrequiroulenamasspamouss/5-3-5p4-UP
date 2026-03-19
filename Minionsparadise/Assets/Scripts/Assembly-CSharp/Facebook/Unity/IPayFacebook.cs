namespace Discord.Unity
{
	internal interface IPayFacebook
	{
		void Pay(string product, string action, int quantity, int? quantityMin, int? quantityMax, string requestId, string pricepointId, string testCurrency, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IPayResult> callback);
	}
}
