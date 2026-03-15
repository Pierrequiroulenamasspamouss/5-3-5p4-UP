namespace Kampai.UI.View
{
	public class ShowBuddyWelcomePanelUICommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Util.Boxed<global::Kampai.Game.Prestige> prestige { get; set; }

		[Inject]
		public global::Kampai.UI.View.CharacterWelcomeState state { get; set; }

		[Inject]
		public int minionCount { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.UI.View.UIModel uiModel { get; set; }

		public override void Execute()
		{
			uiModel.WelcomeBuddyOpen = true;
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Queue, "popup_CharacterState");
			global::Kampai.UI.View.GUIArguments args = iGUICommand.Args;
			if (prestige.Value != null)
			{
				args.Add(prestige.Value);
			}
			args.Add(state);
			args.Add(minionCount);
			guiService.Execute(iGUICommand);
		}
	}
}
