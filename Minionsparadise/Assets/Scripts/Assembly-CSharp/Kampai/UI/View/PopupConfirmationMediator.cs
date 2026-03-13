namespace Kampai.UI.View
{
	public class PopupConfirmationMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.PopupConfirmationView>
	{
		private bool result;

		private global::strange.extensions.signal.impl.Signal<bool> CallbackSignal;

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.Game.CloseConfirmationSignal closeConfirmationSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSkrim { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			closeConfirmationSignal.AddListener(Cancel);
			base.view.Decline.ClickedSignal.AddListener(Cancel);
			base.view.Accept.ClickedSignal.AddListener(Proceed);
			base.view.OnMenuClose.AddListener(OnMenuClose);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			closeConfirmationSignal.RemoveListener(Cancel);
			base.view.Decline.ClickedSignal.RemoveListener(Cancel);
			base.view.Accept.ClickedSignal.RemoveListener(Proceed);
			base.view.OnMenuClose.RemoveListener(OnMenuClose);
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			global::Kampai.UI.View.PopupConfirmationSetting setting = args.Get<global::Kampai.UI.View.PopupConfirmationSetting>();
			Init(setting);
		}

		protected override void OnCloseAllMenu(global::UnityEngine.GameObject exception)
		{
		}

		protected override void Close()
		{
			soundFXSignal.Dispatch("Play_main_menu_close_01");
			base.view.Close();
		}

		private void Cancel()
		{
			result = false;
			Close();
		}

		private void Proceed()
		{
			result = true;
			Close();
		}

		private void Init(global::Kampai.UI.View.PopupConfirmationSetting setting)
		{
			base.view.title.text = localService.GetString(setting.TitleKey);
			if (setting.DescriptionAlreadyTranslated)
			{
				base.view.description.text = setting.DescriptionKey;
			}
			else
			{
				base.view.description.text = localService.GetString(setting.DescriptionKey);
			}
			if (!string.IsNullOrEmpty(setting.LeftButtonText))
			{
				base.view.LeftButton.LocKey = setting.LeftButtonText;
			}
			if (!string.IsNullOrEmpty(setting.RightButtonText))
			{
				base.view.RightButton.LocKey = setting.RightButtonText;
			}
			string imagePath = setting.ImagePath;
			if (imagePath == null || string.IsNullOrEmpty(setting.ImagePath))
			{
				imagePath = "img_char_Min_FeedbackPositive01";
			}
			CallbackSignal = setting.ConfirmationCallback;
			base.closeAllOtherMenuSignal.Dispatch(base.gameObject);
			base.view.Init();
		}

		private void OnMenuClose()
		{
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "popup_Confirmation");
			hideSkrim.Dispatch("ConfirmationSkrim");
			CallbackSignal.Dispatch(result);
			result = false;
		}
	}
}
