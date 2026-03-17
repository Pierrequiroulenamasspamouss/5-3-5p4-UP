namespace Kampai.Game.View
{
	public class NamedCharacterManagerMediator : global::strange.extensions.mediation.impl.EventMediator
	{
		[Inject]
		public global::Kampai.Game.View.NamedCharacterManagerView view { get; set; }

		[Inject]
		public global::Kampai.Game.AddNamedCharacterSignal addSignal { get; set; }

		public override void OnRegister()
		{
			addSignal.AddListener(AddNamedCharacter);
		}

		public override void OnRemove()
		{
			addSignal.RemoveListener(AddNamedCharacter);
		}

		private void AddNamedCharacter(global::Kampai.Game.View.NamedCharacterObject character)
		{
			view.Add(character);
		}
	}
}
