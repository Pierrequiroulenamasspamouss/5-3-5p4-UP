namespace Kampai.UI.View
{
	public class NotificationsMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.NotificationsView>
	{
		private bool autoClose;

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSkrimSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		public override void OnRegister()
		{
			playSFXSignal.Dispatch("Play_menu_popUp_01");
			base.OnRegister();
			base.view.OnMenuClose.AddListener(Close);
			base.view.confirmButton.ClickedSignal.AddListener(Close);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			base.view.OnMenuClose.RemoveListener(Close);
			base.view.confirmButton.ClickedSignal.RemoveListener(Close);
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			string message = args.Get<string>();
			autoClose = args.Get<bool>();
			base.view.Init(localService, message);
		}

		protected override void Close()
		{
			hideSkrimSignal.Dispatch("NotificationsSkrim");
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "popup_Notification");
		}

		protected override void OnCloseAllMenu(global::UnityEngine.GameObject exception)
		{
			if (autoClose)
			{
				base.OnCloseAllMenu(exception);
			}
		}
	}
}
