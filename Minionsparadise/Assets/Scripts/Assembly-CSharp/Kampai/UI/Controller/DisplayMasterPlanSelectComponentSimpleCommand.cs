namespace Kampai.UI.Controller
{
	public class DisplayMasterPlanSelectComponentSimpleCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseAllMessageDialogs closeAllDialogsSignal { get; set; }

		public override void Execute()
		{
			closeAllDialogsSignal.Dispatch();
			OpenModal();
		}

		private void OpenModal()
		{
			global::Kampai.UI.View.IGUICommand command = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Queue, "screen_MasterplanSelectComponent");
			guiService.Execute(command);
		}
	}
}
