namespace Kampai.UI.View
{
	public class SalepackHUDMediator : global::strange.extensions.mediation.impl.EventMediator
	{
		private global::Kampai.Game.SalePackDefinition m_salePackDefinition;

		private bool m_isValidItem = true;

		[Inject]
		public global::Kampai.UI.View.SalepackHUDView view { get; set; }

		[Inject]
		public global::Kampai.UI.View.OpenUpSellModalSignal openUpSellModalSignal { get; set; }

		[Inject]
		public global::Kampai.Common.AppPauseSignal pauseSignal { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		[Inject]
		public global::Kampai.UI.View.RemoveSalePackSignal removeSalePackSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.Common.PickControllerModel pickModel { get; set; }

		[Inject]
		public global::Kampai.Game.IZoomCameraModel zoomCameraModel { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			view.Init();
			view.closeSignal.AddListener(Close);
			view.SalePackButton.ClickedSignal.AddListener(OnSalePackButtonClicked);
			pauseSignal.AddListener(OnPause);
			SetSalePackItemRoutines();
		}

		public override void OnRemove()
		{
			view.closeSignal.RemoveListener(Close);
			pauseSignal.RemoveListener(OnPause);
			view.SalePackButton.ClickedSignal.RemoveListener(OnSalePackButtonClicked);
			Close();
		}

		private void Close()
		{
			m_isValidItem = true;
		}

		private void OnPause()
		{
			Close();
		}

		private void OnEnable()
		{
			if (view != null)
			{
				SetSalePackItemRoutines();
			}
		}

		private void SetSalePackItemRoutines()
		{
			if (view.SalePackItem != null)
			{
				m_salePackDefinition = view.SalePackItem.Definition;
				StartCoroutine(UpdateSaleTime());
			}
		}

		internal void OnSalePackButtonClicked()
		{
			if (!pickModel.PanningCameraBlocked && !pickModel.ZoomingCameraBlocked && !zoomCameraModel.ZoomInProgress)
			{
				openUpSellModalSignal.Dispatch(m_salePackDefinition, "HUD", false);
			}
		}

		internal global::System.Collections.IEnumerator UpdateSaleTime()
		{
			while (m_isValidItem)
			{
				if (view == null || view.SalePackItem == null)
				{
					m_isValidItem = false;
					continue;
				}
				if (view.SalePackItem.Purchased || view.SalePackItem.Finished)
				{
					removeSalePackSignal.Dispatch(view.SalePackItem.ID);
					m_isValidItem = false;
				}
				int saleTime = timeEventService.GetTimeRemaining(view.SalePackItem.ID);
				if (saleTime > 0)
				{
					string saleTimeStr = UIUtils.FormatTime(saleTime, localizationService);
					view.ItemText.text = string.Format("{0}", saleTimeStr);
				}
				yield return new global::UnityEngine.WaitForSeconds(1f);
			}
		}
	}
}
