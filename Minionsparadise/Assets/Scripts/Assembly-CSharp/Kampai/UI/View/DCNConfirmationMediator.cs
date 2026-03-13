namespace Kampai.UI.View
{
	public class DCNConfirmationMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.DCNConfirmationView>
	{
		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSkrim { get; set; }

		[Inject]
		public ILocalPersistanceService localPersistanceService { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.UI.View.UIModel model { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			base.view.SetupSignals();
			base.view.OnMenuClose.AddListener(OnMenuClose);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			base.view.RemoveSignals();
			base.view.OnMenuClose.RemoveListener(OnMenuClose);
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			if (!model.StageUIOpen)
			{
				base.closeAllOtherMenuSignal.Dispatch(base.view.gameObject);
				base.view.Init(args.Get<global::strange.extensions.signal.impl.Signal<bool>>(), localPersistanceService);
			}
		}

		private void OnMenuClose()
		{
			SendTelemetry();
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "popup_DCNconfirmation");
			hideSkrim.Dispatch("ConfirmationSkrim");
		}

		private void SendTelemetry()
		{
			global::Kampai.Game.DCNService dCNService = gameContext.injectionBinder.GetInstance<global::Kampai.Game.IDCNService>() as global::Kampai.Game.DCNService;
			string launchURL = dCNService.GetLaunchURL();
			telemetryService.Send_Telemetry_EVT_DCN((!base.view.opened) ? "No" : "Yes", launchURL, dCNService.GetFeaturedContentId().ToString());
		}

		protected override void Close()
		{
			base.view.Close();
		}
	}
}
