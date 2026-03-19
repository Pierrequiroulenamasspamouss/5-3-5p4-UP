namespace Kampai.Common
{
	public class ShowSocialPartyFBConnectCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("ShowSocialPartyFBConnectCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::System.Action<bool> action { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal globalSFX { get; set; }

		[Inject(global::Kampai.Game.SocialServices.FACEBOOK)]
		public global::Kampai.Game.ISocialService facebookService { get; set; }

		[Inject]
		public global::Kampai.Common.ICoppaService coppaService { get; set; }

		public override void Execute()
		{
			logger.Info("Discord killswitch enabled = {0}", facebookService.isKillSwitchEnabled);
			if (facebookService.isKillSwitchEnabled || coppaService.Restricted())
			{
				action(false);
				return;
			}
			globalSFX.Dispatch("Play_menu_popUp_01");
			global::Kampai.UI.View.IGUICommand iGUICommand = null;
			iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Load, "popup_SocialParty_FBConnect");
			iGUICommand.Args.Add(typeof(global::System.Action<bool>), action);
			iGUICommand.skrimScreen = "StageSkrimFB";
			iGUICommand.singleSkrimClose = true;
			iGUICommand.darkSkrim = true;
			guiService.Execute(iGUICommand);
		}
	}
}
