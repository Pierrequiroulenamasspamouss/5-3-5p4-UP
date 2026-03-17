namespace Kampai.UI.View
{
	public class LoadGUICommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("LoadGUICommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.UI.View.CreateFunMeterSignal createXPBar { get; set; }

		[Inject]
		public global::Kampai.UI.View.CreatePartyMeterSignal createPartyMeter { get; set; }

		public override void Execute()
		{
			logger.EventStart("LoadGUICommand.Execute");
			global::UnityEngine.GameObject o = guiService.Execute(global::Kampai.UI.View.GUIOperation.LoadStatic, "screen_HUD");
			base.injectionBinder.Bind<global::UnityEngine.GameObject>().ToValue(o).ToName(global::Kampai.UI.View.UIElement.HUD);
			createXPBar.Dispatch();
			createPartyMeter.Dispatch();
			logger.EventStart("LoadGUICommand.Execute");
		}
	}
}
