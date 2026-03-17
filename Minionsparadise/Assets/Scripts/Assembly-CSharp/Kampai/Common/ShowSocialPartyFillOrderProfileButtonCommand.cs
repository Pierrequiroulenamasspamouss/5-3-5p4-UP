namespace Kampai.Common
{
	public class ShowSocialPartyFillOrderProfileButtonCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public SocialPartyFillOrderProfileButtonMediator.SocialPartyFillOrderProfileButtonMediatorData mediatorData { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		public override void Execute()
		{
			global::Kampai.UI.View.IGUICommand iGUICommand = null;
			iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.LoadUntrackedInstance, "cmp_TeamPlayer");
			iGUICommand.Args.Add(typeof(SocialPartyFillOrderProfileButtonMediator.SocialPartyFillOrderProfileButtonMediatorData), mediatorData);
			guiService.Execute(iGUICommand);
		}
	}
}
