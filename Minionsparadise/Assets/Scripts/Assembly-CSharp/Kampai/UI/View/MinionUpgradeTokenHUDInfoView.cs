namespace Kampai.UI.View
{
	[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.Animator))]
	public class MinionUpgradeTokenHUDInfoView : global::Kampai.Util.KampaiView
	{
		public global::UnityEngine.UI.Text tokenText;

		internal global::UnityEngine.Animator animator;

		internal void Init()
		{
			animator = GetComponent<global::UnityEngine.Animator>();
		}

		internal void SetText(string text)
		{
			tokenText.text = text;
		}
	}
}
