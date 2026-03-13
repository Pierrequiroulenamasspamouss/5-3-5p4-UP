namespace Kampai.Game
{
	public class SelectionCompleteCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::UnityEngine.Vector3 centerPoint { get; set; }

		[Inject]
		public global::Kampai.Game.StartGroupGachaSignal startGroupGachaSignal { get; set; }

		[Inject]
		public global::Kampai.Common.PickControllerModel model { get; set; }

		public override void Execute()
		{
			if (model.SelectedMinions.Count > 0 && !model.HasPlayedGacha)
			{
				global::System.Collections.Generic.HashSet<int> minionIds = new global::System.Collections.Generic.HashSet<int>(model.SelectedMinions.Keys);
				startGroupGachaSignal.Dispatch(new global::Kampai.Game.MinionAnimationInstructions(minionIds, new global::Kampai.Util.Boxed<global::UnityEngine.Vector3>(centerPoint)));
				model.HasPlayedGacha = true;
			}
		}
	}
}
