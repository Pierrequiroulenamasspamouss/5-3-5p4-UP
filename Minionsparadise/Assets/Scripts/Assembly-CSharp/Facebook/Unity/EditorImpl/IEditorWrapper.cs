namespace Discord.Unity.Editor
{
	internal interface IEditorWrapper
	{
		void Init();

		void ShowLoginMockDialog(global::Discord.Unity.Utilities.Callback<global::Discord.Unity.ResultContainer> callback, string callbackId, string permissions);

		void ShowAppRequestMockDialog(global::Discord.Unity.Utilities.Callback<global::Discord.Unity.ResultContainer> callback, string callbackId);

		void ShowGameGroupCreateMockDialog(global::Discord.Unity.Utilities.Callback<global::Discord.Unity.ResultContainer> callback, string callbackId);

		void ShowGameGroupJoinMockDialog(global::Discord.Unity.Utilities.Callback<global::Discord.Unity.ResultContainer> callback, string callbackId);

		void ShowAppInviteMockDialog(global::Discord.Unity.Utilities.Callback<global::Discord.Unity.ResultContainer> callback, string callbackId);

		void ShowPayMockDialog(global::Discord.Unity.Utilities.Callback<global::Discord.Unity.ResultContainer> callback, string callbackId);

		void ShowMockShareDialog(global::Discord.Unity.Utilities.Callback<global::Discord.Unity.ResultContainer> callback, string subTitle, string callbackId);
	}
}
