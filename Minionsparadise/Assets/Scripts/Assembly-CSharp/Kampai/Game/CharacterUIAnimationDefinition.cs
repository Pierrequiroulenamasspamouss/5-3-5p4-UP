namespace Kampai.Game
{
	public class CharacterUIAnimationDefinition
	{
		public string StateMachine { get; set; }

		public int IdleWeightedAnimationID { get; set; }

		public int IdleCount { get; set; }

		public int HappyWeightedAnimationID { get; set; }

		public int HappyCount { get; set; }

		public int SelectedWeightedAnimationID { get; set; }

		public int SelectedCount { get; set; }

		public bool UseLegacy { get; set; }
	}
}
