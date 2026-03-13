namespace Facebook.Unity.Canvas
{
	internal interface ICanvasFacebookResultHandler : global::Facebook.Unity.IFacebookResultHandler
	{
		void OnPayComplete(global::Facebook.Unity.ResultContainer resultContainer);

		void OnFacebookAuthResponseChange(global::Facebook.Unity.ResultContainer resultContainer);

		void OnUrlResponse(string message);

		void OnHideUnity(bool hide);
	}
}
