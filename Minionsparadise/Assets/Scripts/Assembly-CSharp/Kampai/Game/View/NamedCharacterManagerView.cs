namespace Kampai.Game.View
{
	public class NamedCharacterManagerView : global::Kampai.Game.View.ObjectManagerView
	{
		public new global::Kampai.Game.View.NamedCharacterObject Get(int objectId)
		{
			global::Kampai.Game.View.ActionableObject value;
			objects.TryGetValue(objectId, out value);
			return value as global::Kampai.Game.View.NamedCharacterObject;
		}
	}
}
