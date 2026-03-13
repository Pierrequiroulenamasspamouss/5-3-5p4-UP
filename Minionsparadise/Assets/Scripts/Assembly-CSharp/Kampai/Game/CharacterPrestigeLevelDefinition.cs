namespace Kampai.Game
{
	public class CharacterPrestigeLevelDefinition
	{
		public uint UnlockLevel { get; set; }

		public int UnlockQuestID { get; set; }

		public uint PointsNeeded { get; set; }

		public int AttachedQuestID { get; set; }

		public string WelcomePanelMessageLocalizedKey { get; set; }

		public string FarewellPanelMessageLocalizedKey { get; set; }
	}
}
