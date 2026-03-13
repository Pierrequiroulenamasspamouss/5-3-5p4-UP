namespace Kampai.UI.View
{
	public class CurrencyStoreCategoryButtonView : ScrollableButtonView
	{
		public global::Kampai.UI.View.StoreBadgeView BadgeView;

		[global::UnityEngine.Header("Selected Button")]
		public global::UnityEngine.GameObject PanelForSelected;

		public global::UnityEngine.UI.Text SelectedCategoryTitle;

		public global::Kampai.UI.View.KampaiImage SelectedCategoryIcon;

		[global::UnityEngine.Header("Deselected Button")]
		public global::UnityEngine.UI.Text CategoryTitle;

		public global::Kampai.UI.View.KampaiImage CategoryIcon;

		public new global::strange.extensions.signal.impl.Signal<global::Kampai.Game.CurrencyStoreCategoryDefinition> ClickedSignal = new global::strange.extensions.signal.impl.Signal<global::Kampai.Game.CurrencyStoreCategoryDefinition>();

		public global::Kampai.Game.CurrencyStoreCategoryDefinition categoryDefintiion { get; set; }

		public void Init(global::Kampai.Game.CurrencyStoreCategoryDefinition categoryDefinition, global::Kampai.Main.ILocalizationService localizationService)
		{
			categoryDefintiion = categoryDefinition;
			global::UnityEngine.UI.Text categoryTitle = CategoryTitle;
			string text = localizationService.GetString(categoryDefinition.LocalizedKey);
			SelectedCategoryTitle.text = text;
			categoryTitle.text = text;
			global::Kampai.UI.View.KampaiImage categoryIcon = CategoryIcon;
			global::UnityEngine.Sprite sprite = UIUtils.LoadSpriteFromPath(categoryDefinition.Image);
			SelectedCategoryIcon.sprite = sprite;
			categoryIcon.sprite = sprite;
			global::Kampai.UI.View.KampaiImage categoryIcon2 = CategoryIcon;
			sprite = UIUtils.LoadSpriteFromPath(categoryDefinition.Mask);
			SelectedCategoryIcon.maskSprite = sprite;
			categoryIcon2.maskSprite = sprite;
			PanelForSelected.SetActive(false);
		}

		public void SetBadgeCount(int badgeCount)
		{
			BadgeView.SetNewUnlockCounter(badgeCount);
		}

		public void MarkAsViewed()
		{
			BadgeView.SetNewUnlockCounter(0);
		}

		public override void ButtonClicked()
		{
			base.playSFXSignal.Dispatch("Play_button_click_01");
			MarkAsViewed();
			ClickedSignal.Dispatch(categoryDefintiion);
			MarkAsSelected();
		}

		public void MarkAsDeselected()
		{
			PanelForSelected.SetActive(false);
		}

		public void MarkAsSelected()
		{
			PanelForSelected.SetActive(true);
		}
	}
}
