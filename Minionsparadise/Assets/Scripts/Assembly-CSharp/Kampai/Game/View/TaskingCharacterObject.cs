namespace Kampai.Game.View
{
	public class TaskingCharacterObject
	{
		private global::Kampai.Game.View.CharacterObject characterObject;

		public int RoutingIndex;

		public int ID
		{
			get
			{
				return characterObject.ID;
			}
		}

		public global::Kampai.Game.View.CharacterObject Character
		{
			get
			{
				return characterObject;
			}
		}

		public TaskingCharacterObject(global::Kampai.Game.View.CharacterObject delegateCharacterObject, int routingIndex)
		{
			characterObject = delegateCharacterObject;
			RoutingIndex = routingIndex;
		}
	}
}
