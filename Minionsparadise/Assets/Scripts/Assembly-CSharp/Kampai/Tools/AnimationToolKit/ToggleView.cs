namespace Kampai.Tools.AnimationToolKit
{
	public class ToggleView : global::Kampai.Util.KampaiView
	{
		internal void SetLabel(string labelText)
		{
			global::UnityEngine.UI.Text componentInChildren = GetComponentInChildren<global::UnityEngine.UI.Text>();
			componentInChildren.text = labelText;
		}

		internal void SetPosition(global::UnityEngine.Vector3 position)
		{
			global::UnityEngine.RectTransform rectTransform = base.transform as global::UnityEngine.RectTransform;
			rectTransform.anchoredPosition = global::UnityEngine.Vector2.zero;
			rectTransform.localPosition = position;
		}
	}
}
