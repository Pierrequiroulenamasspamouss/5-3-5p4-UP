namespace Kampai.UI.View
{
	public static class GameObjectViewExtension
	{
		public static void SafeDestoryChildViews<T>(this global::UnityEngine.GameObject gameObject) where T : global::UnityEngine.MonoBehaviour
		{
			if (!(gameObject == null))
			{
				UIUtils.SafeDestoryViews(gameObject.GetComponentsInChildren<T>());
			}
		}

		public static void SafeDestoryChildViews<T>(this global::UnityEngine.RectTransform rectTransform) where T : global::UnityEngine.MonoBehaviour
		{
			if (!(rectTransform == null))
			{
				rectTransform.gameObject.SafeDestoryChildViews<T>();
			}
		}
	}
}
