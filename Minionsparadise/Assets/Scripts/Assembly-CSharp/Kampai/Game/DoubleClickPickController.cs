namespace Kampai.Game
{
	public class DoubleClickPickController : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Common.PickControllerModel model { get; set; }

		[Inject]
		public global::Kampai.Common.DeselectAllMinionsSignal deselectAllSignal { get; set; }

		public override void Execute()
		{
			if (model.SelectedMinions.Count > 0)
			{
				deselectAllSignal.Dispatch();
			}
		}
	}
}
