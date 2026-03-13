namespace Kampai.UI.View
{
	public class StartPartyPopupView : global::Kampai.UI.View.PopupMenuView
	{
		public global::Kampai.UI.View.ButtonView accept;

		public global::UnityEngine.RectTransform congratsHeaderTransform;

		public override void Init()
		{
			base.Init();
			base.Open();
		}

		internal void PulseStartButton()
		{
			global::UnityEngine.Animator component = accept.GetComponent<global::UnityEngine.Animator>();
			if (!(component == null))
			{
				component.SetBool("Pulse", true);
			}
		}

		internal void CenterHeader()
		{
			congratsHeaderTransform.anchoredPosition = new global::UnityEngine.Vector2(0.5f, 0.5f);
		}
	}
}
