namespace Kampai.UI.View
{
	[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.Animator))]
	[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.UI.Button))]
	public class UpsellButtonView : global::Kampai.UI.View.DoubleConfirmButtonView
	{
		public enum Type
		{
			Free = 0,
			ItemInput = 1,
			Cash = 2
		}

		public global::Kampai.UI.View.UpsellButtonView.Type type;

		public global::UnityEngine.UI.Button button;

		public global::Kampai.UI.View.KampaiImage inputItemIcon;

		public global::Kampai.UI.View.LocalizeView costText;

		[global::UnityEngine.Header("Animators")]
		[global::UnityEngine.Tooltip("If this is not set asm_buttonClick_Tertiary is used by default")]
		public global::UnityEngine.RuntimeAnimatorController freeCollectButtonAnimator;

		protected global::Kampai.Game.ICurrencyService currencyService;

		protected global::Kampai.Game.IDefinitionService definitionService;

		protected global::Kampai.Main.ILocalizationService localizationService;

		protected global::Kampai.Util.IKampaiLogger logger;

		protected global::Kampai.Game.IPlayerService playerService;

		protected global::Kampai.Game.IUpsellService upsellService;

		private global::Kampai.Game.Transaction.TransactionInstance transactionInstance;

		protected override void Awake()
		{
			base.Awake();
			button = GetComponent<global::UnityEngine.UI.Button>();
		}

		internal void Init(global::Kampai.Game.IUpsellService upsellService, global::Kampai.Game.IPlayerService playerService, global::Kampai.Game.ICurrencyService currencyService, global::Kampai.Game.IDefinitionService defService, global::Kampai.Main.ILocalizationService locService)
		{
			this.upsellService = upsellService;
			this.currencyService = currencyService;
			definitionService = defService;
			localizationService = locService;
			this.playerService = playerService;
			DisableDoubleConfirm();
		}

		public override void Disable()
		{
			ResetTapState();
			button.interactable = false;
		}

		public void SetupButton(global::Kampai.Game.Transaction.TransactionInstance transactionInstance, string SKU, int percentagePer100, bool isFree, string buttonLocKey)
		{
			this.transactionInstance = transactionInstance;
			string text = SetPrice(SKU, percentagePer100, isFree, buttonLocKey);
			if (type != global::Kampai.UI.View.UpsellButtonView.Type.Cash || string.Compare(text, localizationService.GetString("StoreBuy"), global::System.StringComparison.Ordinal) == 0)
			{
				costText.text = text;
				return;
			}
			costText.Format(costText.text, text);
		}

		public string SetPrice(string SKU, int PercentagePer100, bool isFree, string buttonLocKey)
		{
			EnableInputIcon(false);
			bool flag = !string.IsNullOrEmpty(SKU);
			bool flag2 = PercentagePer100 > 0;
			bool flag3 = global::Kampai.Game.Transaction.TransactionDataExtension.GetInputCount(transactionInstance) > 0;
			if (isFree)
			{
				if (string.IsNullOrEmpty(buttonLocKey))
				{
					buttonLocKey = "Collect";
				}
				SetupFreeItemButton(buttonLocKey, freeCollectButtonAnimator);
				type = global::Kampai.UI.View.UpsellButtonView.Type.Free;
				return localizationService.GetString(buttonLocKey);
			}
			if (flag3)
			{
				global::Kampai.Util.QuantityItem inputItem = global::Kampai.Game.Transaction.TransactionDataExtension.GetInputItem(transactionInstance, 0);
				type = global::Kampai.UI.View.UpsellButtonView.Type.ItemInput;
				if (inputItem == null)
				{
					return string.Empty;
				}
				if (inputItem.ID == 1)
				{
					EnableDoubleConfirm();
				}
				if (flag2)
				{
					int transactionCurrencyCost = global::Kampai.Game.Transaction.TransactionUtil.GetTransactionCurrencyCost(transactionInstance.ToDefinition(), definitionService, playerService, (global::Kampai.Game.StaticItem)inputItem.ID);
					SetupBuyButton(inputItem.ID);
					return ((int)((float)transactionCurrencyCost * (1f - (float)PercentagePer100 / 100f))).ToString();
				}
				SetupBuyButton(inputItem.ID);
				return upsellService.SumOutput(transactionInstance.Inputs, inputItem.ID).ToString();
			}
			if (flag)
			{
				type = global::Kampai.UI.View.UpsellButtonView.Type.Cash;
				return currencyService.GetPriceWithCurrencyAndFormat(SKU);
			}
			return string.Empty;
		}

		private void EnableInputIcon(bool isEnabled)
		{
			if (inputItemIcon != null && inputItemIcon.gameObject != null)
			{
				inputItemIcon.gameObject.SetActive(isEnabled);
			}
		}

		public void SetupBuyButton(int itemId)
		{
			global::Kampai.Game.ItemDefinition itemDefinition = definitionService.Get<global::Kampai.Game.ItemDefinition>(itemId);
			if (itemDefinition == null)
			{
				logger.Error(string.Format("Item input is not an item definition for quantity item: {0}", itemId));
			}
			else
			{
				SetInputIcon(itemDefinition, inputItemIcon);
			}
		}

		private static void SetInputIcon(global::Kampai.Game.ItemDefinition itemDefinition, global::Kampai.UI.View.KampaiImage inputItemIcon)
		{
			inputItemIcon.gameObject.SetActive(true);
			UIUtils.SetItemIcon(inputItemIcon, itemDefinition);
		}

		public void SetupFreeItemButton(string buttonLocKey, global::UnityEngine.RuntimeAnimatorController freeCollectButtonAnimator)
		{
			if (freeCollectButtonAnimator == null)
			{
				freeCollectButtonAnimator = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>("asm_buttonClick_Tertiary");
			}
			SetCostText(buttonLocKey, logger);
			if (animator == null || freeCollectButtonAnimator == null)
			{
				logger.Error(string.Format("Button animator is null: {0}", this));
			}
			else
			{
				animator.runtimeAnimatorController = freeCollectButtonAnimator;
			}
		}

		public void SetCostText(string buttonLocKey, global::Kampai.Util.IKampaiLogger logger)
		{
			if (costText == null)
			{
				if (costText == null)
				{
					logger.Error("Purchase Button Cost GameObject is null");
				}
			}
			else
			{
				costText.LocKey = buttonLocKey;
			}
		}
	}
}
