namespace Kampai.UI.View
{
	public class DisplayCurrencyStoreCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Util.Tuple<int, int> categorySettings { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		public override void Execute()
		{
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Queue, "screen_Store");
			global::Kampai.UI.View.GUIArguments args = iGUICommand.Args;
			args.Add(categorySettings);
			guiService.Execute(iGUICommand);
		}
	}
}
