namespace Facebook.Unity.Editor
{
	internal class EditorWrapper : global::Facebook.Unity.Editor.IEditorWrapper
	{
		private global::Facebook.Unity.IFacebookCallbackHandler callbackHandler;

		public EditorWrapper(global::Facebook.Unity.IFacebookCallbackHandler callbackHandler)
		{
			this.callbackHandler = callbackHandler;
		}

		public void Init()
		{
			callbackHandler.OnInitComplete(string.Empty);
		}

		public void ShowLoginMockDialog(global::Facebook.Unity.Utilities.Callback<global::Facebook.Unity.ResultContainer> callback, string callbackId, string permsisions)
		{
			global::Facebook.Unity.Editor.Dialogs.MockLoginDialog component = global::Facebook.Unity.ComponentFactory.GetComponent<global::Facebook.Unity.Editor.Dialogs.MockLoginDialog>();
			component.Callback = callback;
			component.CallbackID = callbackId;
		}

		public void ShowAppRequestMockDialog(global::Facebook.Unity.Utilities.Callback<global::Facebook.Unity.ResultContainer> callback, string callbackId)
		{
			ShowEmptyMockDialog(callback, callbackId, "Mock App Request");
		}

		public void ShowGameGroupCreateMockDialog(global::Facebook.Unity.Utilities.Callback<global::Facebook.Unity.ResultContainer> callback, string callbackId)
		{
			ShowEmptyMockDialog(callback, callbackId, "Mock Game Group Create");
		}

		public void ShowGameGroupJoinMockDialog(global::Facebook.Unity.Utilities.Callback<global::Facebook.Unity.ResultContainer> callback, string callbackId)
		{
			ShowEmptyMockDialog(callback, callbackId, "Mock Game Group Join");
		}

		public void ShowAppInviteMockDialog(global::Facebook.Unity.Utilities.Callback<global::Facebook.Unity.ResultContainer> callback, string callbackId)
		{
			ShowEmptyMockDialog(callback, callbackId, "Mock App Invite");
		}

		public void ShowPayMockDialog(global::Facebook.Unity.Utilities.Callback<global::Facebook.Unity.ResultContainer> callback, string callbackId)
		{
			ShowEmptyMockDialog(callback, callbackId, "Mock Pay");
		}

		public void ShowMockShareDialog(global::Facebook.Unity.Utilities.Callback<global::Facebook.Unity.ResultContainer> callback, string subTitle, string callbackId)
		{
			global::Facebook.Unity.Editor.Dialogs.MockShareDialog component = global::Facebook.Unity.ComponentFactory.GetComponent<global::Facebook.Unity.Editor.Dialogs.MockShareDialog>();
			component.SubTitle = subTitle;
			component.Callback = callback;
			component.CallbackID = callbackId;
		}

		private void ShowEmptyMockDialog(global::Facebook.Unity.Utilities.Callback<global::Facebook.Unity.ResultContainer> callback, string callbackId, string title)
		{
			global::Facebook.Unity.Editor.Dialogs.EmptyMockDialog component = global::Facebook.Unity.ComponentFactory.GetComponent<global::Facebook.Unity.Editor.Dialogs.EmptyMockDialog>();
			component.Callback = callback;
			component.CallbackID = callbackId;
			component.EmptyDialogTitle = title;
		}
	}
}
