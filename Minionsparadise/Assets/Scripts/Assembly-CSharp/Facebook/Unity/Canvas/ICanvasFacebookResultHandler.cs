namespace Discord.Unity.Canvas
{
	internal interface ICanvasFacebookResultHandler : global::Discord.Unity.IFacebookResultHandler
	{
		void OnPayComplete(global::Discord.Unity.ResultContainer resultContainer);

		void OnFacebookAuthResponseChange(global::Discord.Unity.ResultContainer resultContainer);

		void OnUrlResponse(string message);

		void OnHideUnity(bool hide);
	}
}
