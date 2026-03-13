namespace Kampai.Common
{
	public class ShowSocialPartyFillOrderCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int forceFillOrderId { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal globalSFX { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseAllOtherMenuSignal closeAllOtherMenuSignal { get; set; }

		public override void Execute()
		{
			globalSFX.Dispatch("Play_menu_popUp_01");
			closeAllOtherMenuSignal.Dispatch(null);
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Load, "SocialPartyFillOrderScreen");
			iGUICommand.skrimScreen = "SocialSkrim";
			iGUICommand.darkSkrim = true;
			if (forceFillOrderId > 0)
			{
				global::Kampai.UI.View.GUIAutoAction<int> gUIAutoAction = new global::Kampai.UI.View.GUIAutoAction<int>();
				gUIAutoAction.value = forceFillOrderId;
				iGUICommand.Args.Add(gUIAutoAction);
			}
			guiService.Execute(iGUICommand);
		}
	}
}
