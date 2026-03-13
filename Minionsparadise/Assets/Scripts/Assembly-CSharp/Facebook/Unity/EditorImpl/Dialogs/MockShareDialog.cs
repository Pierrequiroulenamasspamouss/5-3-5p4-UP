namespace Facebook.Unity.Editor.Dialogs
{
	internal class MockShareDialog : global::Facebook.Unity.Editor.EditorFacebookMockDialog
	{
		public string SubTitle { private get; set; }

		protected override string DialogTitle
		{
			get
			{
				return "Mock " + SubTitle + " Dialog";
			}
		}

		protected override void DoGui()
		{
		}

		protected override void SendSuccessResult()
		{
			global::System.Collections.Generic.Dictionary<string, object> dictionary = new global::System.Collections.Generic.Dictionary<string, object>();
			if (global::Facebook.Unity.FB.IsLoggedIn)
			{
				dictionary["postId"] = GenerateFakePostID();
			}
			else
			{
				dictionary["did_complete"] = true;
			}
			if (!string.IsNullOrEmpty(base.CallbackID))
			{
				dictionary["callback_id"] = base.CallbackID;
			}
			if (base.Callback != null)
			{
				base.Callback(new global::Facebook.Unity.ResultContainer(dictionary));
			}
		}

		protected override void SendCancelResult()
		{
			global::System.Collections.Generic.Dictionary<string, object> dictionary = new global::System.Collections.Generic.Dictionary<string, object>();
			dictionary["cancelled"] = "true";
			if (!string.IsNullOrEmpty(base.CallbackID))
			{
				dictionary["callback_id"] = base.CallbackID;
			}
			base.Callback(new global::Facebook.Unity.ResultContainer(dictionary));
		}

		private string GenerateFakePostID()
		{
			global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
			stringBuilder.Append(global::Facebook.Unity.AccessToken.CurrentAccessToken.UserId);
			stringBuilder.Append('_');
			for (int i = 0; i < 17; i++)
			{
				stringBuilder.Append(global::UnityEngine.Random.Range(0, 10));
			}
			return stringBuilder.ToString();
		}
	}
}
