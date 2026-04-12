namespace Kampai.UI.View.UpSell
{
	public class UpSellMessagingModalView : global::Kampai.UI.View.PopupMenuView
	{
		[global::UnityEngine.Header("Base Upsell Message")]
		public global::Kampai.UI.View.LocalizeView Title;

		public global::Kampai.UI.View.LocalizeView Description;

		public global::Kampai.UI.View.ButtonView GoToButton;

		public global::UnityEngine.GameObject GoToImage;

		public global::UnityEngine.GameObject GoToText;

		public global::UnityEngine.GameObject GotItText;

		public global::Kampai.UI.View.ButtonView backGroundButton;

		[global::UnityEngine.Header("UpSell Image")]
		public global::Kampai.UI.View.KampaiImage UpsellMessageImage;

		protected global::Kampai.Game.SalePackDefinition salePackDefinition;

		internal void Init(global::Kampai.Game.SalePackDefinition sale)
		{
			base.Init();
			salePackDefinition = sale;
			if (salePackDefinition == null)
			{
				logger.Error("Sale Pack Definition is null for Upsell Messaging");
				Close(true);
			}
			else
			{
				LoadSaleInfo();
			}
		}

		protected void LoadSaleInfo()
		{
			SetupLocale();
			bool flag = salePackDefinition.MessageLinkType == global::Kampai.Game.SalePackMessageLinkType.None;
			GotItText.SetActive(flag);
			GoToImage.SetActive(!flag);
			GoToText.SetActive(!flag);
			if (salePackDefinition.MessageType == global::Kampai.Game.SalePackMessageType.Image)
			{
				SetupImageMessage();
			}
		}

		private void SetupLocale()
		{
			if (!(Title == null) && !string.IsNullOrEmpty(salePackDefinition.LocalizedKey))
			{
				Title.LocKey = salePackDefinition.LocalizedKey;
				if (!(Description == null) && !string.IsNullOrEmpty(salePackDefinition.Description))
				{
					Description.LocKey = salePackDefinition.Description;
				}
			}
		}

		private void SetupImageMessage()
		{
			UpsellMessageImage.sprite = UIUtils.LoadSpriteFromPath(salePackDefinition.MessageImage);
			UpsellMessageImage.maskSprite = UIUtils.LoadSpriteFromPath(salePackDefinition.MessageMask);
		}
	}
}
