namespace Kampai.UI.View
{
	public class ResourceIconModal : global::Kampai.UI.View.WorldToGlassUIModal
	{
		public global::Kampai.UI.View.KampaiImage TextBackground;

		public global::Kampai.UI.View.KampaiImage Image;

		public global::UnityEngine.UI.Text Text;

		public global::Kampai.UI.View.KampaiImage BackingGlow;

		public global::Kampai.UI.View.KampaiImage BackingPartyGlow;

		public global::Kampai.UI.View.KampaiImage Backing;

		public global::Kampai.UI.View.KampaiImage BackingStroke;

		public global::strange.extensions.signal.impl.Signal ClickedSignal = new global::strange.extensions.signal.impl.Signal();

		public void OnClickEvent()
		{
			ClickedSignal.Dispatch();
		}

		public void EnablePartyIcon()
		{
			if (BackingPartyGlow != null)
			{
				BackingPartyGlow.gameObject.SetActive(true);
			}
			if (BackingGlow != null)
			{
				BackingGlow.gameObject.SetActive(false);
			}
			if (Backing != null)
			{
				Backing.gameObject.SetActive(false);
			}
			if (BackingStroke != null)
			{
				BackingStroke.gameObject.SetActive(false);
			}
		}
	}
}
