namespace Kampai.Game.View
{
	internal sealed class CharacterDrinkingCompleteAction : global::Kampai.Game.View.KampaiAction
	{
		private global::Kampai.Game.View.CharacterObject characterObject;

		private global::Kampai.Game.CharacterDrinkingCompleteSignal drinkingCompleteSignal;

		public CharacterDrinkingCompleteAction(global::Kampai.Game.View.CharacterObject minionObj, global::Kampai.Game.CharacterDrinkingCompleteSignal drinkingCompleteSignal, global::Kampai.Util.IKampaiLogger logger)
			: base(logger)
		{
			characterObject = minionObj;
			this.drinkingCompleteSignal = drinkingCompleteSignal;
		}

		public override void Execute()
		{
			drinkingCompleteSignal.Dispatch(characterObject.ID);
			base.Done = true;
		}
	}
}
