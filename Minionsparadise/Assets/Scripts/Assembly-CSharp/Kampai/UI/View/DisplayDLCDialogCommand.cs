namespace Kampai.UI.View
{
	public class DisplayDLCDialogCommand : global::strange.extensions.command.impl.Command
	{
		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject(global::Kampai.Main.MainElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable mainContext { get; set; }

		[Inject]
		public global::Kampai.Game.IDLCService dlcService { get; set; }

		[Inject]
		public global::Kampai.Game.SavePlayerSignal savePlayerSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSoundFXSignal { get; set; }

		public override void Execute()
		{
			global::strange.extensions.signal.impl.Signal<bool> signal = new global::strange.extensions.signal.impl.Signal<bool>();
			signal.AddListener(delegate(bool result)
			{
				ConfirmationCallback(result);
			});
			global::Kampai.UI.View.PopupConfirmationSetting type = new global::Kampai.UI.View.PopupConfirmationSetting("popupConfirmationDefaultTitle", "DLCConfirmationDialog", "img_char_Min_FeedbackChecklist01", signal);
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.QueueConfirmationSignal>().Dispatch(type);
			playSoundFXSignal.Dispatch("Play_training_popUp_01");
		}

		public void ConfirmationCallback(bool result)
		{
			if (result)
			{
				savePlayerSignal.Dispatch(new global::Kampai.Util.Tuple<global::Kampai.Game.SaveLocation, string, bool>(global::Kampai.Game.SaveLocation.REMOTE, string.Empty, true));
				string displayQualityLevel = dlcService.GetDisplayQualityLevel();
				if (displayQualityLevel.Equals("DLCHDPack"))
				{
					dlcService.SetDisplayQualityLevel("DLCSDPack");
				}
				else
				{
					dlcService.SetDisplayQualityLevel("DLCHDPack");
				}
				mainContext.injectionBinder.GetInstance<global::Kampai.Main.ReloadGameSignal>().Dispatch();
			}
		}
	}
}
