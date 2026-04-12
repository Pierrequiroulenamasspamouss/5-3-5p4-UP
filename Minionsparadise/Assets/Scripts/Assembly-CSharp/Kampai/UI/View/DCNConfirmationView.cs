namespace Kampai.UI.View
{
	[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.Animator))]
	public class DCNConfirmationView : global::Kampai.UI.View.PopupMenuView
	{
		public global::Kampai.UI.View.ButtonView confirmButton;

		public global::Kampai.UI.View.ButtonView declineButton;

		public global::Kampai.UI.View.ToggleButtonView toggleButton;

		private global::strange.extensions.signal.impl.Signal<bool> CallbackSignal;

		private ILocalPersistanceService localPersistanceService;

		internal bool opened;

		public void Init(global::strange.extensions.signal.impl.Signal<bool> callback, ILocalPersistanceService localPersistanceService)
		{
			base.Init();
			this.localPersistanceService = localPersistanceService;
			CallbackSignal = callback;
			base.Open();
		}

		internal void SetupSignals()
		{
			confirmButton.ClickedSignal.AddListener(OnConfirmButtonClick);
			declineButton.ClickedSignal.AddListener(OnDeclineButtonClick);
		}

		internal void RemoveSignals()
		{
			confirmButton.ClickedSignal.RemoveListener(OnConfirmButtonClick);
			declineButton.ClickedSignal.RemoveListener(OnDeclineButtonClick);
		}

		private void OnConfirmButtonClick()
		{
			if (toggleButton.IsOn)
			{
				localPersistanceService.PutDataInt("DCNStoreDoNotShow", 1);
			}
			opened = true;
			CallbackSignal.Dispatch(opened);
			Close();
		}

		private void OnDeclineButtonClick()
		{
			CallbackSignal.Dispatch(opened);
			Close();
		}
	}
}
