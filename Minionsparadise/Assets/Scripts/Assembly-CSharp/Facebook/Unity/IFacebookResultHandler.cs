namespace Facebook.Unity
{
	internal interface IFacebookResultHandler
	{
		void OnInitComplete(global::Facebook.Unity.ResultContainer resultContainer);

		void OnLoginComplete(global::Facebook.Unity.ResultContainer resultContainer);

		void OnLogoutComplete(global::Facebook.Unity.ResultContainer resultContainer);

		void OnGetAppLinkComplete(global::Facebook.Unity.ResultContainer resultContainer);

		void OnGroupCreateComplete(global::Facebook.Unity.ResultContainer resultContainer);

		void OnGroupJoinComplete(global::Facebook.Unity.ResultContainer resultContainer);

		void OnAppRequestsComplete(global::Facebook.Unity.ResultContainer resultContainer);

		void OnShareLinkComplete(global::Facebook.Unity.ResultContainer resultContainer);
	}
}
