namespace Kampai.UI.View
{
	public class CreateNewSellView : global::Kampai.Util.KampaiView
	{
		internal const int MIN_ITEM_COUNT = 1;

		internal const int MAX_ITEM_COUNT = 6;

		internal const int MIN_ITEM_PRICE = 10;

		internal const int MAX_ITEM_PRICE = 300;

		internal const float MOVE_TIME = 0.15f;

		internal readonly global::UnityEngine.Vector3 DIRECTION_UP = new global::UnityEngine.Vector3(0f, 0f, 90f);

		internal readonly global::UnityEngine.Vector3 DIRECTION_DOWN = new global::UnityEngine.Vector3(0f, 0f, -90f);

		internal readonly global::UnityEngine.Color COLOR_HIDDEN = new global::UnityEngine.Color(0f, 0f, 0f, 0f);

		public global::Kampai.UI.View.SellQuantityButtonView AddItemCountButton;

		public global::Kampai.UI.View.SellQuantityButtonView MinusItemCountButton;

		public global::Kampai.UI.View.SellQuantityButtonView AddPriceButton;

		public global::Kampai.UI.View.SellQuantityButtonView MinusPriceButton;

		public global::Kampai.UI.View.ButtonView PutOnSaleButton;

		public global::Kampai.UI.View.ButtonView ResetPriceButton;

		public global::UnityEngine.UI.Text ItemCountText;

		public global::UnityEngine.UI.Text PriceText;

		public global::UnityEngine.Animator maxoutFXAnimator;

		public global::UnityEngine.Color sellPriceUpBackgroundColor = new global::UnityEngine.Color(73f / 85f, 50f / 51f, 0.8156863f);

		public global::UnityEngine.Color sellPriceUpTextColor = new global::UnityEngine.Color(2f / 51f, 29f / 51f, 2f / 51f);

		public global::UnityEngine.Color sellPriceDownBackgroundColor = new global::UnityEngine.Color(0.9843137f, 0.83137256f, 0.8509804f);

		public global::UnityEngine.Color sellPriceDownTextColor = new global::UnityEngine.Color(48f / 85f, 2f / 15f, 2f / 15f);

		public global::Kampai.UI.View.KampaiImage ItemIconImage;

		public global::Kampai.UI.View.KampaiImage CurrencyImage;

		public global::Kampai.UI.View.KampaiImage MarketValueImage;

		public global::Kampai.UI.View.KampaiImage MarketValueArrowImage;

		public global::Kampai.UI.View.KampaiImage QuantityImage;

		internal global::Kampai.Game.Item item;

		internal global::Kampai.Game.MarketplaceItemDefinition marketplaceDef;

		internal global::strange.extensions.signal.impl.Signal ClosePanelSignal = new global::strange.extensions.signal.impl.Signal();

		private int m_itemCount = 1;

		private int m_price = 100;

		private int m_maxItemCount;

		private int m_minPrice;

		private int m_maxPrice;

		internal bool IsOpen { get; private set; }

		internal int MaxPrice
		{
			get
			{
				return m_maxPrice * m_itemCount;
			}
		}

		internal int MinPrice
		{
			get
			{
				return m_minPrice * m_itemCount;
			}
		}

		internal int ItemCount
		{
			get
			{
				return m_itemCount;
			}
			set
			{
				m_price /= m_itemCount;
				if (m_maxItemCount == 1)
				{
					SetButtonIteractable(AddItemCountButton, false);
					SetButtonIteractable(MinusItemCountButton, false);
					m_itemCount = 1;
				}
				else if (value <= 1)
				{
					m_itemCount = 1;
					SetButtonIteractable(AddItemCountButton, true);
					SetButtonIteractable(MinusItemCountButton, false);
				}
				else if (value >= m_maxItemCount)
				{
					m_itemCount = m_maxItemCount;
					SetButtonIteractable(AddItemCountButton, false);
					SetButtonIteractable(MinusItemCountButton, true);
				}
				else
				{
					m_itemCount = value;
					SetButtonIteractable(AddItemCountButton, true);
					SetButtonIteractable(MinusItemCountButton, true);
				}
				m_price *= m_itemCount;
				SetItemCountText();
				UpdatePriceRange();
				UpdateCountRange();
				Price = m_price;
			}
		}

		internal int Price
		{
			get
			{
				return m_price;
			}
			set
			{
				float num = global::UnityEngine.Mathf.Floor(value);
				if (MinPrice == MaxPrice)
				{
					SetButtonIteractable(AddPriceButton, false);
					SetButtonIteractable(MinusPriceButton, false);
					m_price = MinPrice;
				}
				else if (num <= (float)MinPrice)
				{
					m_price = MinPrice;
					SetButtonIteractable(AddPriceButton, true);
					SetButtonIteractable(MinusPriceButton, false);
				}
				else if (num >= (float)MaxPrice)
				{
					m_price = MaxPrice;
					SetButtonIteractable(AddPriceButton, false);
					SetButtonIteractable(MinusPriceButton, true);
				}
				else
				{
					m_price = value;
					SetButtonIteractable(AddPriceButton, true);
					SetButtonIteractable(MinusPriceButton, true);
				}
				SetSellPriceText();
			}
		}

		private void ChangeMarketValueDisplay(global::UnityEngine.Color backgroundColor, global::UnityEngine.Color arrowColor, global::UnityEngine.Color textColor, global::UnityEngine.Vector3 direction)
		{
			MarketValueImage.color = global::UnityEngine.Color.white;
			global::UnityEngine.UI.Button component = MarketValueImage.GetComponent<global::UnityEngine.UI.Button>();
			if (component != null)
			{
				global::UnityEngine.UI.ColorBlock colors = component.colors;
				colors.normalColor = backgroundColor;
				colors.highlightedColor = backgroundColor;
				component.colors = colors;
			}
			MarketValueArrowImage.color = arrowColor;
			MarketValueArrowImage.transform.localEulerAngles = direction;
			PriceText.color = textColor;
		}

		internal void CheckAmountMaxOutState(int newAmount)
		{
			if (newAmount < 1 || newAmount > m_maxItemCount)
			{
				maxoutFXAnimator.SetTrigger("maxoutQty");
			}
		}

		internal void CheckPriceMaxOutState(int newPrice)
		{
			if (newPrice < MinPrice || newPrice > MaxPrice)
			{
				maxoutFXAnimator.SetTrigger("maxoutPrice");
			}
		}

		private void OnEnable()
		{
			Init();
		}

		internal void Init()
		{
			global::UnityEngine.RectTransform rectTransform = MarketValueImage.transform as global::UnityEngine.RectTransform;
			if (!(rectTransform == null))
			{
				AddItemCountButton.SetSize(rectTransform.rect.height);
				AddPriceButton.SetSize(rectTransform.rect.height);
				MinusItemCountButton.SetSize(rectTransform.rect.height);
				MinusPriceButton.SetSize(rectTransform.rect.height);
				UpdatePriceRange();
				UpdateCountRange();
			}
		}

		internal void UpdatePriceRange()
		{
			global::Kampai.UI.View.SellQuantityButtonView addPriceButton = AddPriceButton;
			addPriceButton.MaxValue = MaxPrice;
			addPriceButton.MinValue = MinPrice;
			addPriceButton.IsPriceButton = true;
			addPriceButton = MinusPriceButton;
			addPriceButton.MaxValue = MaxPrice;
			addPriceButton.MinValue = MinPrice;
			addPriceButton.IsPriceButton = true;
		}

		internal void UpdateCountRange()
		{
			global::Kampai.UI.View.SellQuantityButtonView addItemCountButton = AddItemCountButton;
			addItemCountButton.MaxValue = m_maxItemCount;
			addItemCountButton.MinValue = 1;
			addItemCountButton.IsPriceButton = false;
			addItemCountButton = MinusItemCountButton;
			addItemCountButton.MaxValue = m_maxItemCount;
			addItemCountButton.MinValue = 1;
			addItemCountButton.IsPriceButton = false;
		}

		internal void SetForSaleItem(global::Kampai.Game.MarketplaceItemDefinition itemDefinition, global::Kampai.Game.Item itemInstance, int maxSellQuantity, bool resetItem, bool coppaRestricted)
		{
			if (item != null && item.ID == itemInstance.ID && !resetItem)
			{
				return;
			}
			item = itemInstance;
			marketplaceDef = itemDefinition;
			if (maxSellQuantity == 0)
			{
				maxSellQuantity = 6;
			}
			if (item == null)
			{
				m_maxItemCount = maxSellQuantity;
				ItemCount = 0;
				SetButtonIteractable(MinusItemCountButton, false);
				SetButtonIteractable(AddItemCountButton, false);
				SetButtonIteractable(MinusPriceButton, false);
				SetButtonIteractable(AddPriceButton, false);
				return;
			}
			m_maxItemCount = global::System.Math.Min((int)item.Quantity, maxSellQuantity);
			ItemCount = (int)item.Quantity / 2;
			if (marketplaceDef != null)
			{
				m_minPrice = marketplaceDef.MinStrikePrice;
				m_maxPrice = marketplaceDef.MaxStrikePrice;
				Price = marketplaceDef.StartingStrikePrice * m_itemCount;
				SetMarketplaceValue((!coppaRestricted) ? itemDefinition.PriceTrend : 0);
			}
			else
			{
				m_minPrice = 10;
				m_maxPrice = 300;
				Price = 100;
				SetMarketplaceValue(0);
			}
			if (!resetItem)
			{
				global::UnityEngine.Sprite sprite = UIUtils.LoadSpriteFromPath(item.Definition.Image);
				ItemIconImage.sprite = sprite;
				global::UnityEngine.Sprite maskSprite = UIUtils.LoadSpriteFromPath(item.Definition.Mask);
				ItemIconImage.maskSprite = maskSprite;
			}
		}

		private void SetButtonIteractable(global::Kampai.UI.View.ButtonView buttonView, bool interactable)
		{
			global::UnityEngine.UI.Button component = buttonView.GetComponent<global::UnityEngine.UI.Button>();
			if (component != null)
			{
				component.interactable = interactable;
			}
		}

		internal void SetItemCountText()
		{
			if (!(ItemCountText == null))
			{
				ItemCountText.text = string.Format("{0}", m_itemCount);
			}
		}

		internal void SetSellPriceText()
		{
			if (!(PriceText == null))
			{
				int number = global::UnityEngine.Mathf.FloorToInt(m_price);
				PriceText.text = UIUtils.FormatLargeNumber(number);
			}
		}

		internal void OpenPanel()
		{
			IsOpen = true;
			base.gameObject.SetActive(true);
		}

		internal void ClosePanel()
		{
			IsOpen = false;
			marketplaceDef = null;
			item = null;
		}

		private void SetMarketplaceValue(int marketplaceValue)
		{
			global::UnityEngine.Color color;
			global::UnityEngine.Color textColor;
			global::UnityEngine.Color cOLOR_HIDDEN;
			global::UnityEngine.Vector3 direction;
			if (marketplaceValue > 0)
			{
				color = sellPriceUpBackgroundColor;
				textColor = (cOLOR_HIDDEN = sellPriceUpTextColor);
				direction = DIRECTION_UP;
			}
			else if (marketplaceValue < 0)
			{
				color = sellPriceDownBackgroundColor;
				textColor = (cOLOR_HIDDEN = sellPriceDownTextColor);
				direction = DIRECTION_DOWN;
			}
			else
			{
				color = QuantityImage.color;
				textColor = global::UnityEngine.Color.white;
				cOLOR_HIDDEN = COLOR_HIDDEN;
				direction = DIRECTION_UP;
			}
			ChangeMarketValueDisplay(color, cOLOR_HIDDEN, textColor, direction);
		}
	}
}
