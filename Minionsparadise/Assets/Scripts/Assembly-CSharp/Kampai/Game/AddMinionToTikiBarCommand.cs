namespace Kampai.Game
{
	public class AddMinionToTikiBarCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.Character minionCharacter { get; set; }

		[Inject]
		public global::Kampai.Game.TikiBarBuilding tikiBar { get; set; }

		[Inject]
		public int openSlot { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService prestigeService { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.Minion minion = minionCharacter as global::Kampai.Game.Minion;
			if (minion != null)
			{
				AddCostumedMinionToTikiBar(minion);
				return;
			}
			global::Kampai.Game.NamedCharacter namedCharacter = minionCharacter as global::Kampai.Game.NamedCharacter;
			if (namedCharacter != null)
			{
				AddNamedCharacterToTikiBar(namedCharacter);
			}
		}

		private void AddCostumedMinionToTikiBar(global::Kampai.Game.Minion minion)
		{
			if (minion.State != global::Kampai.Game.MinionState.Tasking)
			{
				prestigeService.AddMinionToTikiBarSlot(minion, openSlot, tikiBar, true);
			}
		}

		private void AddNamedCharacterToTikiBar(global::Kampai.Game.NamedCharacter namedCharacter)
		{
			prestigeService.AddMinionToTikiBarSlot(namedCharacter, openSlot, tikiBar);
		}
	}
}
