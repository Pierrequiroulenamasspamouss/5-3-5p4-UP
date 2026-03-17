namespace Kampai.UI.View
{
	public interface IGUIService
	{
		global::UnityEngine.GameObject Execute(global::Kampai.UI.View.IGUICommand command);

		global::UnityEngine.GameObject Execute(global::Kampai.UI.View.GUIOperation operation, string prefab);

		global::UnityEngine.GameObject Execute(global::Kampai.UI.View.GUIOperation operation, global::Kampai.UI.View.GUIPriority priority, string prefab);

		global::Kampai.UI.View.IGUICommand BuildCommand(global::Kampai.UI.View.GUIOperation operation, string prefab);

		global::Kampai.UI.View.IGUICommand BuildCommand(global::Kampai.UI.View.GUIOperation operation, string prefab, string guiLabel);

		global::Kampai.UI.View.IGUICommand BuildCommand(global::Kampai.UI.View.GUIOperation operation, global::Kampai.UI.View.GUIPriority priority, string prefab);

		void AddToArguments(object arg);

		void RemoveFromArguments(global::System.Type arg);
	}
}
