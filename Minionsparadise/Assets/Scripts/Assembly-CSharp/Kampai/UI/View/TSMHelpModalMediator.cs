namespace Kampai.UI.View
{
	public class TSMHelpModalMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.TSMHelpModalView>
	{
		[Inject]
		public global::Kampai.UI.IFancyUIService fancyUIService { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseAllOtherMenuSignal closeSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSkrimSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Main.MoveAudioListenerSignal moveAudioListenerSignal { get; set; }

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			base.Initialize(args);
			base.view.InitializeView(fancyUIService, args.Get<global::Kampai.UI.View.TSMHelpModalArguments>(), moveAudioListenerSignal);
		}

		protected override void Close()
		{
			moveAudioListenerSignal.Dispatch(true, null);
			playSFXSignal.Dispatch("Play_menu_disappear_01");
			base.view.Close();
		}

		public override void OnRegister()
		{
			closeSignal.Dispatch(null);
			base.OnRegister();
			base.view.OnMenuClose.AddListener(OnMenuClose);
			base.view.Button.ClickedSignal.AddListener(OnOkClicked);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			CleanupListeners();
		}

		private void CleanupListeners()
		{
			base.view.OnMenuClose.RemoveListener(OnMenuClose);
			base.view.Button.ClickedSignal.RemoveListener(OnOkClicked);
		}

		private void OnMenuClose()
		{
			hideSkrimSignal.Dispatch("ProceduralTaskSkrim");
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "popup_TSM_Help");
		}

		private void OnOkClicked()
		{
			CleanupListeners();
			closeSignal.Dispatch(null);
		}
	}
}
