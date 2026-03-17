namespace Kampai.Game
{
	public class ShowDialogCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public string key { get; set; }

		[Inject]
		public global::Kampai.Game.QuestDialogSetting settings { get; set; }

		[Inject]
		public global::Kampai.Util.Tuple<int, int> questIds { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		public override void Execute()
		{
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Queue, "screen_Dialog");
			global::Kampai.UI.View.GUIArguments args = iGUICommand.Args;
			args.Add(key);
			args.Add(settings);
			args.Add(questIds);
			guiService.Execute(iGUICommand);
		}
	}
}
