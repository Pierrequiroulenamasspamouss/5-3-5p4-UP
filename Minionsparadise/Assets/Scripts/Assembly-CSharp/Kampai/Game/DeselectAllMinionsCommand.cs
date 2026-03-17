namespace Kampai.Game
{
	public class DeselectAllMinionsCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Common.DeselectMinionSignal deselectSignal { get; set; }

		[Inject]
		public global::Kampai.Common.PickControllerModel model { get; set; }

		public override void Execute()
		{
			DeselectAll();
		}

		private void DeselectAll()
		{
			global::System.Collections.Generic.List<int> list = new global::System.Collections.Generic.List<int>();
			foreach (int key in model.SelectedMinions.Keys)
			{
				list.Add(key);
			}
			foreach (int item in list)
			{
				deselectSignal.Dispatch(item);
			}
			list.Clear();
			list = null;
			model.SelectedMinions.Clear();
			model.HasPlayedGacha = false;
		}
	}
}
