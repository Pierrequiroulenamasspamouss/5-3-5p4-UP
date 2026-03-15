namespace Kampai.UI.View
{
	public class SocialPartyFBConnectView : global::Kampai.UI.View.PopupMenuView
	{
		public global::Kampai.UI.View.ButtonView connectButton;

		public global::Kampai.UI.View.ButtonView quitButton;

		public global::UnityEngine.UI.Text connectButtonText;

		public global::UnityEngine.UI.Text txtHeadline;

		public global::UnityEngine.UI.Text txtDescription;

		public global::System.Collections.Generic.List<global::UnityEngine.Animator> objectiveAnimators;

		public float animationDelay = 0.5f;

		public override void Init()
		{
			base.Init();
			base.Open();
			StartCoroutine(DisplayObjectives());
		}

		private global::System.Collections.IEnumerator DisplayObjectives()
		{
			yield return new global::UnityEngine.WaitForSeconds(animationDelay);
			for (int i = 0; i < objectiveAnimators.Count; i++)
			{
				objectiveAnimators[i].Play("Open");
				yield return new global::UnityEngine.WaitForSeconds(animationDelay);
			}
		}
	}
}
