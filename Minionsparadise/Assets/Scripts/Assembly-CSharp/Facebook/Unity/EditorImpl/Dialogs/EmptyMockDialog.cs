namespace Discord.Unity.Editor.Dialogs
{
	internal class EmptyMockDialog : global::Discord.Unity.Editor.EditorFacebookMockDialog
	{
		public string EmptyDialogTitle { get; set; }

		protected override string DialogTitle
		{
			get
			{
				return EmptyDialogTitle;
			}
		}

		protected override void DoGui()
		{
		}

		protected override void SendSuccessResult()
		{
			global::System.Collections.Generic.Dictionary<string, object> dictionary = new global::System.Collections.Generic.Dictionary<string, object>();
			dictionary["did_complete"] = true;
			if (!string.IsNullOrEmpty(base.CallbackID))
			{
				dictionary["callback_id"] = base.CallbackID;
			}
			if (base.Callback != null)
			{
				base.Callback(new global::Discord.Unity.ResultContainer(dictionary));
			}
		}
	}
}
