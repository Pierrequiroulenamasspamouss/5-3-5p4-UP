namespace Discord.Unity.Canvas
{
	internal interface ICanvasFacebookCallbackHandler : global::Discord.Unity.IFacebookCallbackHandler
	{
		void OnPayComplete(string message);

		void OnFacebookAuthResponseChange(string message);

		void OnUrlResponse(string message);

		void OnHideUnity(bool hide);
	}
}
