namespace Kampai.UI.View
{
	public class MessagePopupMediator : global::Kampai.UI.View.KampaiMediator
	{
		[Inject]
		public global::Kampai.UI.View.MessagePopupView view { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseAllMessageDialogs closeAllDialogsSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.MessageDialogClosed messageDialogClosed { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		public override void OnRegister()
		{
			view.Init();
			closeAllDialogsSignal.AddListener(OnCloseDialogs);
			view.DialogClosedSignal.AddListener(OnDialogClosed);
		}

		public override void OnRemove()
		{
			closeAllDialogsSignal.RemoveListener(OnCloseDialogs);
			view.DialogClosedSignal.RemoveListener(OnDialogClosed);
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			string text = args.Get<string>();
			bool flag = args.Get<bool>();
			global::Kampai.UI.View.MessagePopUpAnchor anchor = ((!args.Contains<global::Kampai.UI.View.MessagePopUpAnchor>()) ? global::Kampai.UI.View.MessagePopUpAnchor.TOP_RIGHT : args.Get<global::Kampai.UI.View.MessagePopUpAnchor>());
			global::UnityEngine.Vector2 anchorPosition = ((!args.Contains<global::UnityEngine.Vector2>()) ? global::UnityEngine.Vector2.zero : args.Get<global::UnityEngine.Vector2>());
			global::Kampai.Util.Tuple<float, float> tuple = args.Get<global::Kampai.Util.Tuple<float, float>>();
			if (tuple != null)
			{
				view.SetCustomTiming(tuple.Item1, tuple.Item2);
			}
			view.AutoClose = !flag;
			view.Display(text, anchor, anchorPosition);
		}

		private void OnCloseDialogs()
		{
			view.Show(false);
		}

		private void OnDialogClosed()
		{
			messageDialogClosed.Dispatch();
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "popup_MessageBox");
		}
	}
}
