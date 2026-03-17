namespace Kampai.Game.View
{
	public class DCNBuildingObjectMediator : global::strange.extensions.mediation.impl.EventMediator
	{
		private global::strange.extensions.signal.impl.Signal<bool> callbackSignal = new global::strange.extensions.signal.impl.Signal<bool>();

		[Inject]
		public global::Kampai.Game.View.DCNBuildingObjectView view { get; set; }

		[Inject]
		public global::Kampai.Game.ShowDCNScreenSignal showDCNScreenSignal { get; set; }

		[Inject]
		public global::Kampai.Game.DCNMaybeShowContentSignal dcnMaybeShowContentSignal { get; set; }

		[Inject]
		public global::Kampai.Game.DCNShowFeaturedContentSignal dcnShowFeaturedContentSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IDCNService dcnService { get; set; }

		[Inject]
		public global::Kampai.Game.QueueDCNConfirmationSignal queueConfirmationSignal { get; set; }

		public override void OnRegister()
		{
			showDCNScreenSignal.AddListener(ShowScreen);
			dcnShowFeaturedContentSignal.AddListener(ShowContent);
			callbackSignal.AddListener(OpenContentCallback);
			StartCoroutine(DCNViewIsReady());
		}

		public override void OnRemove()
		{
			showDCNScreenSignal.RemoveListener(ShowScreen);
			dcnShowFeaturedContentSignal.RemoveListener(ShowContent);
			callbackSignal.RemoveListener(OpenContentCallback);
		}

		private void ShowScreen(bool show)
		{
			if (show)
			{
				view.ShowScreen();
			}
			else
			{
				view.HideScreen();
			}
		}

		private global::System.Collections.IEnumerator DCNViewIsReady()
		{
			yield return new global::UnityEngine.WaitForEndOfFrame();
			yield return new global::UnityEngine.WaitForEndOfFrame();
			dcnMaybeShowContentSignal.Dispatch();
		}

		private void ShowContent()
		{
			if (view.ScreenIsOpen())
			{
				queueConfirmationSignal.Dispatch(callbackSignal);
			}
		}

		private void OpenContentCallback(bool result)
		{
			if (result)
			{
				view.HideScreen();
			}
			dcnService.OpenFeaturedContent(result);
		}
	}
}
