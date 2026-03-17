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
		public global::Kampai.Game.NetworkLostCloseSignal closeSignal { get; set; }

		public override void OnRegister()
		{
			view.retryButton.ClickedSignal.AddListener(OnRetry);
			view.title.text = locService.GetString("OfflineTitle");
			view.description.text = locService.GetString("OfflineDescription");
			view.retryButtonText.text = locService.GetString("OfflineRetry");
			view.OnMenuClose.AddListener(OnMenuClose);
			view.Init();
			view.Open();
			button = view.retryButton.GetComponent<global::UnityEngine.UI.Button>();
			openSignal.Dispatch();
		}

		public override void OnRemove()
		{
			view.retryButton.ClickedSignal.RemoveListener(OnRetry);
			view.OnMenuClose.RemoveListener(OnMenuClose);
			closeSignal.Dispatch();
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
