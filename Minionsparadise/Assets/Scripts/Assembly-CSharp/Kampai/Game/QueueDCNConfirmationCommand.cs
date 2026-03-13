namespace Kampai.Game
{
	public class QueueDCNConfirmationCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::strange.extensions.signal.impl.Signal<bool> callback { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public ILocalPersistanceService localPersistanceService { get; set; }

		[Inject]
		public global::Kampai.Game.IDCNService dcnService { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		public override void Execute()
		{
			if (localPersistanceService.HasKey("DCNStoreDoNotShow"))
			{
				callback.Dispatch(true);
				telemetryService.Send_Telemetry_EVT_DCN("Yes", dcnService.GetLaunchURL(), dcnService.GetFeaturedContentId().ToString());
				return;
			}
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Queue, "popup_DCNconfirmation");
			iGUICommand.Args.Add(callback);
			iGUICommand.skrimScreen = "ConfirmationSkrim";
			iGUICommand.darkSkrim = true;
			guiService.Execute(iGUICommand);
		}
	}
}
