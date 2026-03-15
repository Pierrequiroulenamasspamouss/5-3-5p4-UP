namespace Kampai.UI.View
{
	public static class WorldToGlassUIBuilder
	{
		public static T Build<T>(string prefab, global::UnityEngine.Transform i_parent, global::Kampai.UI.View.WorldToGlassUISettings settings, global::Kampai.Util.IKampaiLogger logger) where T : global::Kampai.UI.View.WorldToGlassView
		{
			if (string.IsNullOrEmpty(prefab) || i_parent == null)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.EX_NULL_ARG);
			}
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(global::Kampai.Util.KampaiResources.Load(prefab)) as global::UnityEngine.GameObject;
			gameObject.transform.SetParent(i_parent, false);
			gameObject.transform.SetAsFirstSibling();
			gameObject.SetActive(true);
			global::Kampai.UI.View.WorldToGlassUIModal component = gameObject.GetComponent<global::Kampai.UI.View.WorldToGlassUIModal>();
			component.Settings = settings;
			return gameObject.AddComponent<T>();
		}
	}
}
