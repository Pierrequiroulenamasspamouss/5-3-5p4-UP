namespace Kampai.Common
{
	public class ShowSocialPartyFillOrderButtonCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.UI.View.SocialPartyFillOrderButtonMediator.SocialPartyFillOrderButtonMediatorData mediatorData { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public int row { get; set; }

		public override void Execute()
		{
			global::Kampai.UI.View.IGUICommand iGUICommand = null;
			iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.LoadUntrackedInstance, "cmp_SocialFillOrder");
			iGUICommand.Args.Add(typeof(global::Kampai.UI.View.SocialPartyFillOrderButtonMediator.SocialPartyFillOrderButtonMediatorData), mediatorData);
			iGUICommand.Args.Add(typeof(int), row);
			guiService.Execute(iGUICommand);
		}
	}
}
