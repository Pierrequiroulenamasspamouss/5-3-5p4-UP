namespace Kampai.UI.View
{
	public class ShowNeedXMinionsCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int numMinionsNeeded { get; set; }

		[Inject]
		public global::Kampai.UI.View.PopupMessageSignal messageSignal { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		public override void Execute()
		{
			string type = localService.GetString("NeedsXMinions*", numMinionsNeeded);
			messageSignal.Dispatch(type, global::Kampai.UI.View.PopupMessageType.NORMAL);
		}
	}
}
