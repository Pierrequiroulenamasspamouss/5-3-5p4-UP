namespace Discord.Unity.Editor
{
	internal class EditorWrapper : global::Discord.Unity.Editor.IEditorWrapper
	{
		private global::Discord.Unity.IFacebookCallbackHandler callbackHandler;

		public EditorWrapper(global::Discord.Unity.IFacebookCallbackHandler callbackHandler)
		{
			this.callbackHandler = callbackHandler;
		}

		public void Init()
		{
			callbackHandler.OnInitComplete(string.Empty);
		}

		public void ShowLoginMockDialog(global::Discord.Unity.Utilities.Callback<global::Discord.Unity.ResultContainer> callback, string callbackId, string permsisions)
		{
			global::Discord.Unity.Editor.Dialogs.MockLoginDialog component = global::Discord.Unity.ComponentFactory.GetComponent<global::Discord.Unity.Editor.Dialogs.MockLoginDialog>();
			component.Callback = callback;
			component.CallbackID = callbackId;
		}

		public void ShowAppRequestMockDialog(global::Discord.Unity.Utilities.Callback<global::Discord.Unity.ResultContainer> callback, string callbackId)
		{
			ShowEmptyMockDialog(callback, callbackId, "Mock App Request");
		}

		public void ShowGameGroupCreateMockDialog(global::Discord.Unity.Utilities.Callback<global::Discord.Unity.ResultContainer> callback, string callbackId)
		{
			ShowEmptyMockDialog(callback, callbackId, "Mock Game Group Create");
		}

		public void ShowGameGroupJoinMockDialog(global::Discord.Unity.Utilities.Callback<global::Discord.Unity.ResultContainer> callback, string callbackId)
		{
			ShowEmptyMockDialog(callback, callbackId, "Mock Game Group Join");
		}

		public void ShowAppInviteMockDialog(global::Discord.Unity.Utilities.Callback<global::Discord.Unity.ResultContainer> callback, string callbackId)
		{
			ShowEmptyMockDialog(callback, callbackId, "Mock App Invite");
		}

		public void ShowPayMockDialog(global::Discord.Unity.Utilities.Callback<global::Discord.Unity.ResultContainer> callback, string callbackId)
		{
			ShowEmptyMockDialog(callback, callbackId, "Mock Pay");
		}

		public void ShowMockShareDialog(global::Discord.Unity.Utilities.Callback<global::Discord.Unity.ResultContainer> callback, string subTitle, string callbackId)
		{
			global::Discord.Unity.Editor.Dialogs.MockShareDialog component = global::Discord.Unity.ComponentFactory.GetComponent<global::Discord.Unity.Editor.Dialogs.MockShareDialog>();
			component.SubTitle = subTitle;
			component.Callback = callback;
			component.CallbackID = callbackId;
		}

		private void ShowEmptyMockDialog(global::Discord.Unity.Utilities.Callback<global::Discord.Unity.ResultContainer> callback, string callbackId, string title)
		{
			global::Discord.Unity.Editor.Dialogs.EmptyMockDialog component = global::Discord.Unity.ComponentFactory.GetComponent<global::Discord.Unity.Editor.Dialogs.EmptyMockDialog>();
			component.Callback = callback;
			component.CallbackID = callbackId;
			component.EmptyDialogTitle = title;
		}
	}
}
