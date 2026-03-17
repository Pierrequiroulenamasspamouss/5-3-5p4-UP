namespace Kampai.Game.View
{
	public class VillainManagerView : global::Kampai.Game.View.ObjectManagerView
	{
		public new global::Kampai.Game.View.VillainView Get(int objectId)
		{
			global::Kampai.Game.View.ActionableObject value;
			objects.TryGetValue(objectId, out value);
			return value as global::Kampai.Game.View.VillainView;
		}
	}
}
