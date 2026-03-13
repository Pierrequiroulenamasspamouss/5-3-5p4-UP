namespace Kampai.UI.View.UpSell
{
	public class UpsellPriceMarkdownView : global::UnityEngine.MonoBehaviour
	{
		public global::Kampai.UI.View.LocalizeView originalPriceText;

		public global::Kampai.UI.View.LocalizeView percentOffText;

		public global::Kampai.UI.View.LocalizeView percentOffBanner;

		private global::Kampai.Game.PackDefinition m_packDefinition;

		private global::Kampai.Game.IDefinitionService m_definitionService;

		private global::Kampai.Game.ICurrencyService m_currencyService;

		private global::Kampai.Game.PremiumCurrencyItemDefinition m_currencyItemDefinition;

		public void Init(global::Kampai.Game.PackDefinition packDefinition, global::Kampai.Game.IDefinitionService definitionService, global::Kampai.Game.ICurrencyService currencyService)
		{
			m_packDefinition = packDefinition;
			m_definitionService = definitionService;
			m_currencyService = currencyService;
			SetupMTXDescription();
		}

		private void SetupMTXDescription()
		{
			if (percentOffText == null)
			{
				return;
			}
			if (originalPriceText != null && m_packDefinition.TransactionType == global::Kampai.Game.UpsellTransactionType.Cash)
			{
				if (m_definitionService.TryGet<global::Kampai.Game.PremiumCurrencyItemDefinition>(m_packDefinition.CurrencyImageID, out m_currencyItemDefinition))
				{
					originalPriceText.text = m_currencyService.GetPriceWithCurrencyAndFormat(m_currencyItemDefinition.SKU);
				}
				else
				{
					originalPriceText.gameObject.SetActive(false);
				}
				percentOffText.text = m_currencyService.GetPriceWithCurrencyAndFormat(m_packDefinition.SKU);
			}
			percentOffBanner.text = string.Format("{0}%", m_packDefinition.PercentagePer100);
		}
	}
}
