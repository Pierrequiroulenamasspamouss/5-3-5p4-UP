namespace Facebook.Unity.Editor
{
	internal interface IEditorWrapper
	{
		void Init();

		void ShowLoginMockDialog(global::Facebook.Unity.Utilities.Callback<global::Facebook.Unity.ResultContainer> callback, string callbackId, string permissions);

		void ShowAppRequestMockDialog(global::Facebook.Unity.Utilities.Callback<global::Facebook.Unity.ResultContainer> callback, string callbackId);

		void ShowGameGroupCreateMockDialog(global::Facebook.Unity.Utilities.Callback<global::Facebook.Unity.ResultContainer> callback, string callbackId);

		void ShowGameGroupJoinMockDialog(global::Facebook.Unity.Utilities.Callback<global::Facebook.Unity.ResultContainer> callback, string callbackId);

		void ShowAppInviteMockDialog(global::Facebook.Unity.Utilities.Callback<global::Facebook.Unity.ResultContainer> callback, string callbackId);

		void ShowPayMockDialog(global::Facebook.Unity.Utilities.Callback<global::Facebook.Unity.ResultContainer> callback, string callbackId);

		void ShowMockShareDialog(global::Facebook.Unity.Utilities.Callback<global::Facebook.Unity.ResultContainer> callback, string subTitle, string callbackId);
	}
}
