namespace Kampai.UI.View
{
	[global::UnityEngine.AddComponentMenu("UI/KampaiButton")]
	public class KampaiButton : global::UnityEngine.UI.Button
	{
		public void ChangeToNormalState()
		{
			DoStateTransition(global::UnityEngine.UI.Selectable.SelectionState.Normal, false);
		}
	}
}
