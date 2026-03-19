namespace Discord.Unity
{
	internal interface IFacebookResultHandler
	{
		void OnInitComplete(global::Discord.Unity.ResultContainer resultContainer);

		void OnLoginComplete(global::Discord.Unity.ResultContainer resultContainer);

		void OnLogoutComplete(global::Discord.Unity.ResultContainer resultContainer);

		void OnGetAppLinkComplete(global::Discord.Unity.ResultContainer resultContainer);

		void OnGroupCreateComplete(global::Discord.Unity.ResultContainer resultContainer);

		void OnGroupJoinComplete(global::Discord.Unity.ResultContainer resultContainer);

		void OnAppRequestsComplete(global::Discord.Unity.ResultContainer resultContainer);

		void OnShareLinkComplete(global::Discord.Unity.ResultContainer resultContainer);
	}
}
