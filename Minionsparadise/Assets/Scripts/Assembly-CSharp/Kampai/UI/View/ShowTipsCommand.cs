namespace Kampai.UI.View
{
	public class ShowTipsCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public string localizedKey { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public ILocalPersistanceService localPersistService { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		public override void Execute()
		{
			if (!localPersistService.HasKey(localizedKey))
			{
				global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Queue, "popup_Tip");
				iGUICommand.skrimScreen = "DidYouKnowSkrim";
				iGUICommand.darkSkrim = false;
				string value = localService.GetString(localizedKey);
				global::Kampai.UI.View.GUIArguments args = iGUICommand.Args;
				args.Add(value);
				guiService.Execute(iGUICommand);
			}
		}
	}
}
