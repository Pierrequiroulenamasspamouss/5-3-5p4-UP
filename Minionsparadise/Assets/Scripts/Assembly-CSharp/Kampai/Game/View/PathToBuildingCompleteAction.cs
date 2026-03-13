namespace Kampai.Game.View
{
	internal sealed class PathToBuildingCompleteAction : global::Kampai.Game.View.KampaiAction
	{
		private global::Kampai.Game.View.CharacterObject characterObject;

		private int slotIndex;

		private global::strange.extensions.signal.impl.Signal<global::Kampai.Game.View.CharacterObject, int> addToBuildingSignal;

		public PathToBuildingCompleteAction(global::Kampai.Game.View.CharacterObject minionObj, int slotIndex, global::strange.extensions.signal.impl.Signal<global::Kampai.Game.View.CharacterObject, int> addSignal, global::Kampai.Util.IKampaiLogger logger)
			: base(logger)
		{
			characterObject = minionObj;
			this.slotIndex = slotIndex;
			addToBuildingSignal = addSignal;
		}

		public override void Execute()
		{
			addToBuildingSignal.Dispatch(characterObject, slotIndex);
			base.Done = true;
		}
	}
}
