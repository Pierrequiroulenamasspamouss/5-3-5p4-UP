namespace Kampai.Game.View
{
	public abstract class ActionableObjectManagerView : global::Kampai.Util.KampaiView
	{
		public static global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.View.ActionableObject> allObjects = new global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.View.ActionableObject>();

		public static global::Kampai.Game.View.ActionableObject GetFromAllObjects(int objectid)
		{
			global::Kampai.Game.View.ActionableObject value;
			allObjects.TryGetValue(objectid, out value);
			return value;
		}

		public static void ClearAllObjects()
		{
			allObjects.Clear();
		}
	}
}
