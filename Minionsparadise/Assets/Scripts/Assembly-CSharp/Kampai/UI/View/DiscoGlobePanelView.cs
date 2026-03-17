namespace Kampai.UI.View
{
	public class DiscoGlobePanelView : global::strange.extensions.mediation.impl.View
	{
		private global::Kampai.UI.View.DiscoGlobeView discoGlobeView;

		internal void PreLoadDiscoGlobe()
		{
			if (!(discoGlobeView != null))
			{
				global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(global::Kampai.Util.KampaiResources.Load<global::UnityEngine.GameObject>("screen_DiscoBall"));
				gameObject.transform.SetParent(base.transform, false);
				gameObject.transform.SetAsFirstSibling();
				discoGlobeView = gameObject.GetComponent<global::Kampai.UI.View.DiscoGlobeView>();
			}
		}

		internal void DisplayDiscoGlobe(bool display, global::Kampai.Game.MinionParty party)
		{
			if (display)
			{
				DisplayDiscoGlobeView();
			}
			else
			{
				RemoveDiscoGlobeView(party);
			}
		}

		private void DisplayDiscoGlobeView()
		{
			if (discoGlobeView == null)
			{
				PreLoadDiscoGlobe();
			}
			StartCoroutine(WaitAFrameDisplayGlobe());
		}

		private global::System.Collections.IEnumerator WaitAFrameDisplayGlobe()
		{
			yield return null;
			global::Kampai.UI.View.DiscoGlobeMediator discoGlobeMediator = discoGlobeView.GetComponent<global::Kampai.UI.View.DiscoGlobeMediator>();
			discoGlobeMediator.DisplayDiscoGlobe();
		}

		private void RemoveDiscoGlobeView(global::Kampai.Game.MinionParty party)
		{
			if (!(discoGlobeView == null))
			{
				if (party.PartyPreSkip)
				{
					DestroyDiscoGlobeView();
				}
				else
				{
					discoGlobeView.RemoveDiscoBallAwesomeness(DestroyDiscoGlobeView);
				}
			}
		}

		internal void DestroyDiscoGlobeView()
		{
			if (!(discoGlobeView == null))
			{
				global::UnityEngine.Object.Destroy(discoGlobeView.gameObject);
				discoGlobeView = null;
			}
		}
	}
}
