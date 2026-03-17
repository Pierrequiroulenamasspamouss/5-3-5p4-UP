namespace Kampai.Game.View
{
	internal sealed class CharacterIntroCompleteAction : global::Kampai.Game.View.KampaiAction
	{
		private int slotIndex;

		private global::Kampai.Game.View.CharacterObject characterObject;

		private global::UnityEngine.RuntimeAnimatorController stateMachine;

		private global::Kampai.Game.CharacterIntroCompleteSignal introCompleteSignal;

		public CharacterIntroCompleteAction(global::Kampai.Game.View.CharacterObject minionObj, int slotIndex, global::UnityEngine.RuntimeAnimatorController stateMachine, global::Kampai.Game.CharacterIntroCompleteSignal introCompleteSignal, global::Kampai.Util.IKampaiLogger logger)
			: base(logger)
		{
			this.slotIndex = slotIndex;
			characterObject = minionObj;
			this.stateMachine = stateMachine;
			this.introCompleteSignal = introCompleteSignal;
		}

		public override void Execute()
		{
			characterObject.MoveToPelvis();
			characterObject.SetAnimController(stateMachine);
			introCompleteSignal.Dispatch(characterObject, slotIndex);
			base.Done = true;
		}
	}
}
