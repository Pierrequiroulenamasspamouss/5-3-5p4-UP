namespace Kampai.UI.View
{
	public class ResourcePlotUnlockRushDialogView : global::Kampai.UI.View.RushDialogView
	{
		public global::UnityEngine.UI.Text productionDescription;

		public global::UnityEngine.UI.Button leftButton;

		public global::UnityEngine.UI.Button rightButton;

		public global::Kampai.UI.View.KampaiImage resourceItem;

		public global::UnityEngine.UI.Text resourceItemAmt;

		internal void EnableArrows(bool enable)
		{
			leftButton.gameObject.SetActive(enable);
			rightButton.gameObject.SetActive(enable);
		}

		internal void SetProductionDescription(global::Kampai.Game.ItemDefinition itemDef, int itemAmount, string prodDesc)
		{
			productionDescription.text = prodDesc;
			resourceItem.sprite = UIUtils.LoadSpriteFromPath(itemDef.Image);
			resourceItem.maskSprite = UIUtils.LoadSpriteFromPath(itemDef.Mask);
			resourceItemAmt.text = itemAmount.ToString();
		}

		internal override void SetupItemCount(int count)
		{
		}
	}
}
