namespace Kampai.UI.View
{
	[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.UI.LayoutElement))]
	public class TSMQuantityItemView : global::UnityEngine.MonoBehaviour
	{
		public global::Kampai.UI.View.LocalizeView m_amount;

		public global::Kampai.UI.View.KampaiImage m_image;

		public global::Kampai.UI.View.TSMQuantityItemView Init(global::Kampai.Game.IDisplayableDefinition displayableDefinition, uint amount, global::Kampai.Util.IKampaiLogger logger, global::Kampai.Main.ILocalizationService locale)
		{
			UIUtils.SetItemIcon(m_image, displayableDefinition);
			if (amount <= 1)
			{
				m_amount.gameObject.SetActive(false);
				return this;
			}
			m_amount.logger = logger;
			m_amount.service = locale;
			m_amount.Format("QuantityItemFormat", amount);
			return this;
		}

		public void Disable()
		{
			if (!(m_image == null))
			{
				m_image.Desaturate = 1f;
			}
		}
	}
}
