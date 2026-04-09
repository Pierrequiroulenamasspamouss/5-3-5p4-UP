namespace Kampai.UI.View
{
	public class OfflineMediator : global::strange.extensions.mediation.impl.Mediator
	{
		private global::UnityEngine.UI.Button button;

		[Inject]
		public global::Kampai.UI.View.OfflineView view { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService locService { get; set; }

		[Inject]
		public global::Kampai.Common.NetworkModel networkModel { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowOfflinePopupSignal showOfflinePopupSignal { get; set; }

		[Inject]
		public global::Kampai.Common.ResumeNetworkOperationSignal resumeNetworkOperationSignal { get; set; }

		[Inject]
		public global::Kampai.Game.NetworkLostOpenSignal openSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.TransitionToOfflineModeSignal transitionToOfflineModeSignal { get; set; }

		[Inject]
		public global::Kampai.Game.NetworkLostCloseSignal closeSignal { get; set; }

		public override void OnRegister()
		{
			view.retryButton.ClickedSignal.AddListener(OnRetry);
			
			// Resolve the playOfflineButton instance to ensure we are listening to the correct one
			// We iterate over children to find the button named "btn_playOffline" that is actually active or preferred
			foreach (global::Kampai.UI.View.ButtonView b in view.GetComponentsInChildren<global::Kampai.UI.View.ButtonView>(true))
			{
				if (b.name == "btn_playOffline")
				{
					view.playOfflineButton = b;
					break;
				}
			}

			view.title.text = locService.GetString("OfflineTitle");
			view.description.text = locService.GetString("OfflineDescription");
			view.retryButtonText.text = locService.GetString("OfflineRetry");
			
			if (view.playOfflineButton != null)
			{
				UnityEngine.Debug.Log(string.Format("[OfflineMode] OfflineMediator: Linked to playOfflineButton (Hash: {0}, Name: {1})", view.playOfflineButton.GetHashCode(), view.playOfflineButton.name));
				view.playOfflineButton.ClickedSignal.AddListener(OnPlayOffline);
				view.playOfflineButtonText.text = locService.GetString("OfflinePlayOffline");
			}
			else
			{
				UnityEngine.Debug.LogError("[OfflineMode] OfflineMediator: Could not find btn_playOffline in children!");
			}
			
			view.OnMenuClose.AddListener(OnMenuClose);
			view.Init();
			view.Open();
			button = view.retryButton.GetComponent<global::UnityEngine.UI.Button>();
			openSignal.Dispatch();
		}

		public override void OnRemove()
		{
			if (view != null)
			{
				if (view.retryButton != null && view.retryButton.ClickedSignal != null)
				{
					view.retryButton.ClickedSignal.RemoveListener(OnRetry);
				}
				if (view.playOfflineButton != null && view.playOfflineButton.ClickedSignal != null)
				{
					view.playOfflineButton.ClickedSignal.RemoveListener(OnPlayOffline);
				}
				view.OnMenuClose.RemoveListener(OnMenuClose);
			}
			if (closeSignal != null)
			{
				closeSignal.Dispatch();
			}
		}

		private void OnRetry()
		{
			button.interactable = false;
			StartCoroutine(WaitForRetry());
			networkModel.isConnectionLost = !global::Kampai.Util.NetworkUtil.IsConnected();
			if (!networkModel.isConnectionLost)
			{
				Close();
				resumeNetworkOperationSignal.Dispatch();
			}
		}

		private void OnPlayOffline()
		{
			UnityEngine.Debug.Log("[OfflineMode] OfflineMediator OnPlayOffline triggered.");
			transitionToOfflineModeSignal.Dispatch();
		}

		private global::System.Collections.IEnumerator WaitForRetry()
		{
			yield return new global::UnityEngine.WaitForSeconds(2f);
			if (view.retryButton != null)
			{
				button.interactable = true;
			}
		}

		private void OnMenuClose()
		{
			if (!networkModel.isConnectionLost)
			{
				showOfflinePopupSignal.Dispatch(false);
			}
			else
			{
				view.Open();
			}
		}

		private void Close()
		{
			view.Close();
		}
	}
}
