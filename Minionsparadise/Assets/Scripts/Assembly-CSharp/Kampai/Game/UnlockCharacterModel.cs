namespace Kampai.Game
{
	public class UnlockCharacterModel
	{
		public bool stuartFirstTimeHonor { get; set; }

		public int routeIndex { get; set; }

		public global::System.Collections.Generic.IList<global::Kampai.Game.Character> minionUnlocks { get; set; }

		public global::System.Collections.Generic.IList<global::Kampai.Game.Character> characterUnlocks { get; set; }

		public global::System.Collections.Generic.IList<global::Kampai.Game.QuestDialogSetting> dialogQueue { get; set; }

		public UnlockCharacterModel()
		{
			stuartFirstTimeHonor = false;
			routeIndex = -1;
			minionUnlocks = new global::System.Collections.Generic.List<global::Kampai.Game.Character>();
			characterUnlocks = new global::System.Collections.Generic.List<global::Kampai.Game.Character>();
			dialogQueue = new global::System.Collections.Generic.List<global::Kampai.Game.QuestDialogSetting>();
		}
	}
}
