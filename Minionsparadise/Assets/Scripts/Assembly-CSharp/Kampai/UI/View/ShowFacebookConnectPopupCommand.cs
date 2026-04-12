namespace Kampai.UI.View
{
	public class ShowFacebookConnectPopupCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject(global::Kampai.Game.SocialServices.FACEBOOK)]
		public global::Kampai.Game.ISocialService facebookService { get; set; }

		[Inject]
		public global::Kampai.Game.SocialLoginSignal socialLoginSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		public override void Execute()
		{
			if (!facebookService.isLoggedIn)
			{
				global::Kampai.Game.SocialSettingsDefinition socialSettingsDefinition = definitionService.Get<global::Kampai.Game.SocialSettingsDefinition>(1000009022);
				if (socialSettingsDefinition != null && socialSettingsDefinition.ShowFacebookConnectPopup)
				{
					global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Load, "popup_SocialParty_FBConnect");
					iGUICommand.skrimScreen = "StageSkrimFB";
					iGUICommand.darkSkrim = true;
					iGUICommand.singleSkrimClose = true;
					guiService.Execute(iGUICommand);
				}
				else
				{
					socialLoginSignal.Dispatch(facebookService, new global::Kampai.Util.Boxed<global::System.Action>(null));
				}
			}
		}
	}
}
