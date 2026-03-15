namespace Kampai.UI.View
{
	public class CreateSkrimCommand : global::strange.extensions.command.impl.Command
	{
		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("CreateSkrimCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public string Name { get; set; }

		[Inject]
		public global::strange.extensions.signal.impl.Signal<global::Kampai.Util.KampaiDisposable> Callback { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		public override void Execute()
		{
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Queue, "Skrim", Name);
			global::Kampai.UI.View.GUIArguments args = new global::Kampai.UI.View.GUIArguments(logger).Add(new global::Kampai.UI.View.SkrimCallback(Callback));
			iGUICommand.Args = args;
			guiService.Execute(iGUICommand);
		}
	}
}
