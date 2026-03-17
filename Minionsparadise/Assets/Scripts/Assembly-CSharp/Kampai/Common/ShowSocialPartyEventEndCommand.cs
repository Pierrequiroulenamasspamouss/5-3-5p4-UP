namespace Kampai.Common
{
	public class ShowSocialPartyEventEndCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal globalSFX { get; set; }

		public override void Execute()
		{
			globalSFX.Dispatch("Play_menu_popUp_01");
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Load, "popup_SocialParty_End");
		}
	}
}
