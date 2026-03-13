namespace Kampai.UI.View.UpSell
{
	public class UpSellMessagingModalMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.UpSell.UpSellMessagingModalView>
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("UpSellMessagingModalMediator") as global::Kampai.Util.IKampaiLogger;

		private global::Kampai.Game.SalePackDefinition m_salePackDefinition;

		private string m_prefabName;

		private int m_salePackInstanceID;

		private bool needImpression = true;

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSignal { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.UI.View.PurchaseSalePackSignal purchaseSalePackSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.FinishPurchasingSalePackSignal finishPurchaseSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseAllOtherMenuSignal closeOtherMenuSignal { get; set; }

		[Inject]
		public global::Kampai.Game.UpdateSaleBadgeSignal updateSaleBadgeSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.MoveBuildMenuSignal moveBuildMenuSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowMTXStoreSignal showMTXStoreSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ProcessUpSellImpressionSignal processUpSellImpressionSignal { get; set; }

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			m_salePackDefinition = args.Get<global::Kampai.Game.PackDefinition>() as global::Kampai.Game.SalePackDefinition;
			base.view.Init(m_salePackDefinition);
			m_prefabName = args.Get<string>();
			base.view.Open();
			closeOtherMenuSignal.Dispatch(base.gameObject);
			telemetryService.Send_Telemetry_EVT_UPSELL(m_salePackDefinition.SKU, global::Kampai.Common.Service.Telemetry.UpsellStatus.Viewed);
			UpdateStoreBadge();
		}

		private void UpdateStoreBadge()
		{
			global::Kampai.Game.Sale firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Sale>(m_salePackDefinition.ID);
			if (firstInstanceByDefinitionId != null)
			{
				m_salePackInstanceID = firstInstanceByDefinitionId.ID;
				if (firstInstanceByDefinitionId.Started && !firstInstanceByDefinitionId.Viewed)
				{
					firstInstanceByDefinitionId.Viewed = true;
					updateSaleBadgeSignal.Dispatch();
				}
			}
		}

		public override void OnRegister()
		{
			base.OnRegister();
			base.view.OnMenuClose.AddListener(OnMenuClose);
			base.view.GoToButton.ClickedSignal.AddListener(OnGotoButtonClicked);
			base.view.backGroundButton.ClickedSignal.AddListener(Close);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			base.view.OnMenuClose.RemoveListener(OnMenuClose);
			base.view.GoToButton.ClickedSignal.RemoveListener(OnGotoButtonClicked);
			base.view.backGroundButton.ClickedSignal.RemoveListener(Close);
		}

		protected override void Close()
		{
			if (needImpression)
			{
				processUpSellImpressionSignal.Dispatch(m_salePackInstanceID);
			}
			base.view.Close();
		}

		private void OnMenuClose()
		{
			hideSignal.Dispatch("UpSellModalSkrim");
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, m_prefabName);
		}

		private void OnGotoButtonClicked()
		{
			int iD = m_salePackDefinition.ID;
			if (m_salePackDefinition.TransactionDefinition != null && m_salePackDefinition.TransactionDefinition.Outputs != null)
			{
				logger.Info("Processing Transaction...");
				purchaseSalePackSignal.Dispatch(iD);
			}
			else
			{
				finishPurchaseSignal.Dispatch(iD);
			}
			switch (m_salePackDefinition.MessageLinkType)
			{
			case global::Kampai.Game.SalePackMessageLinkType.HTML:
				logger.Info("Launching Sale Message Link {0}", m_salePackDefinition.MessageUrl);
				global::UnityEngine.Application.OpenURL(m_salePackDefinition.MessageUrl);
				break;
			case global::Kampai.Game.SalePackMessageLinkType.Store:
				showMTXStoreSignal.Dispatch(new global::Kampai.Util.Tuple<int, int>(800003, 0));
				break;
			case global::Kampai.Game.SalePackMessageLinkType.BuildMenu:
				moveBuildMenuSignal.Dispatch(true);
				break;
			default:
				logger.Error("Unknown SalePackMessageLinkType {0}", m_salePackDefinition.MessageLinkType);
				break;
			case global::Kampai.Game.SalePackMessageLinkType.None:
				break;
			}
			needImpression = false;
			Close();
			telemetryService.Send_Telemetry_EVT_UPSELL(m_salePackDefinition.SKU, global::Kampai.Common.Service.Telemetry.UpsellStatus.Clicked);
		}
	}
}
