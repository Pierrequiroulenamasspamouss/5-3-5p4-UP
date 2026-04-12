namespace Kampai.UI.View
{
	public class DeepLinkHandler : global::UnityEngine.MonoBehaviour
	{
		private bool waitingToProcessLink;

		public global::Kampai.Util.IKampaiLogger logger { get; set; }

		public global::Kampai.UI.View.MoveBuildMenuSignal moveBuildMenuSignal { get; set; }

		public global::Kampai.UI.View.ShowMTXStoreSignal showMTXStoreSignal { get; set; }

		public global::Kampai.Game.CloneUserFromEnvSignal cloneUserFromEnvSignal { get; set; }
		
		[Inject]
		public global::Kampai.Game.ITimedSocialEventService timedSocialEventService { get; set; }

		private void Awake()
		{
			waitingToProcessLink = true;
			StartCoroutine(WaitToProcessLink());
		}

		public virtual void OnDeepLink(string uriString)
		{
			if (!waitingToProcessLink)
			{
				waitingToProcessLink = true;
				StartCoroutine(WaitToProcessLink());
			}
		}

		private global::System.Collections.IEnumerator WaitToProcessLink()
		{
			yield return new global::UnityEngine.WaitForSeconds(0.5f);
			ProcessDeepLink();
			waitingToProcessLink = false;
		}

		private void RemoveLinkFromPrefs()
		{
			global::UnityEngine.PlayerPrefs.DeleteKey("DeepLink");
			global::UnityEngine.PlayerPrefs.Save();
		}

		internal void ProcessDeepLink()
		{
			string text = global::UnityEngine.PlayerPrefs.GetString("DeepLink");
			if (text.Length == 0)
			{
				RemoveLinkFromPrefs();
				return;
			}
			global::System.Uri uri = new global::System.Uri(text);
			logger.Debug("uri.Host: {0}", uri.Host);
			if (uri.Host != "deeplink")
			{
				logger.Error("Not a deeplink url: {0}", text);
				RemoveLinkFromPrefs();
				return;
			}
			string absolutePath = uri.AbsolutePath;
			string[] array = absolutePath.Split('/');
			if (array.Length < 3)
			{
				logger.Error("Incorrect deeplink url: {0}", text);
				RemoveLinkFromPrefs();
				return;
			}
			string text2 = array[1];
			logger.Debug("action = {0}", text2);
			switch (text2)
			{
			case "clone":
				if (global::Kampai.Util.GameConstants.StaticConfig.DEBUG_ENABLED && array.Length == 4)
				{
					cloneUserFromEnvSignal.Dispatch(array[2], global::System.Convert.ToInt64(array[3]));
					break;
				}
				logger.Error("Incorrect deeplink url: {0}", text);
				break;
			case "join":
				if (array.Length == 3)
				{
					long teamId = global::System.Convert.ToInt64(array[2]);
					global::Kampai.Game.TimedSocialEventDefinition currentSocialEvent = timedSocialEventService.GetCurrentSocialEvent();
					if (currentSocialEvent != null)
					{
						logger.Debug("Joining team {0} for event {1}", teamId, currentSocialEvent.ID);
						timedSocialEventService.JoinSocialTeam(currentSocialEvent.ID, teamId, null);
					}
					else
					{
						logger.Error("No active social event found to join team {0}", teamId);
					}
				}
				break;
			default:
				logger.Error("Unsupported action: {0}", text2);
				break;
			case "view":
			{
				string text3 = array[2];
				logger.Debug("target = {0}", text3);
				switch (text3)
				{
				case "build_menu":
					moveBuildMenuSignal.Dispatch(true);
					break;
				case "grind_store":
					showMTXStoreSignal.Dispatch(new global::Kampai.Util.Tuple<int, int>(800001, 0));
					break;
				case "premium_store":
					showMTXStoreSignal.Dispatch(new global::Kampai.Util.Tuple<int, int>(800002, 0));
					break;
				default:
					logger.Error("Unsupported target: {0}", text3);
					break;
				}
				break;
			}
			}
			RemoveLinkFromPrefs();
		}
	}
}
