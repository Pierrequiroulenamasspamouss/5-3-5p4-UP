namespace Kampai.Game
{
	public class UnleashCharacterAtShoreCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("UnleashCharacterAtShoreCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.Character minionCharacter { get; set; }

		[Inject]
		public int openSlot { get; set; }

		[Inject(global::Kampai.Game.GameElement.NAMED_CHARACTER_MANAGER)]
		public global::UnityEngine.GameObject namedCharacterManager { get; set; }

		[Inject(global::Kampai.Game.GameElement.MINION_MANAGER)]
		public global::UnityEngine.GameObject minionManager { get; set; }

		[Inject]
		public global::Kampai.Game.PhilPlayIntroSignal playIntroSignal { get; set; }

		[Inject]
		public global::Kampai.Game.PopUnleashedCharacterToTikiBarSignal popCustomerToTikiBarSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.View.CharacterObject characterObject = null;
			bool type = false;
			if (minionCharacter is global::Kampai.Game.Minion)
			{
				global::Kampai.Game.View.MinionManagerView component = minionManager.GetComponent<global::Kampai.Game.View.MinionManagerView>();
				characterObject = component.Get(minionCharacter.ID);
				global::Kampai.Game.Minion minion = minionCharacter as global::Kampai.Game.Minion;
				if (minion.HasPrestige)
				{
					type = true;
				}
			}
			else if (minionCharacter is global::Kampai.Game.NamedCharacter)
			{
				global::Kampai.Game.View.NamedCharacterManagerView component2 = namedCharacterManager.GetComponent<global::Kampai.Game.View.NamedCharacterManagerView>();
				characterObject = component2.Get(minionCharacter.ID);
				type = true;
			}
			if (characterObject == null)
			{
				logger.Error("AddMinionToTikiBarSlot: ao as MinionObject and NamedCharacterObject == null");
				return;
			}
			popCustomerToTikiBarSignal.Dispatch(characterObject, openSlot);
			playIntroSignal.Dispatch(type);
		}
	}
}
