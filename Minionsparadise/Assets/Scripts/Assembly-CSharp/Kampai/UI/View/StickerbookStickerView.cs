namespace Kampai.UI.View
{
	public class StickerbookStickerView : global::Kampai.Util.KampaiView
	{
		public global::Kampai.UI.View.PopupInfoButtonView buttonView;

		public global::Kampai.UI.View.KampaiImage image;

		public global::Kampai.UI.View.KampaiImage lockImage;

		public bool locked;

		public int stickerDefinitionID;

		internal global::Kampai.Game.StickerDefinition stickerDefinition;

		internal void Init(global::Kampai.Game.IDefinitionService definitionService)
		{
			stickerDefinition = definitionService.Get<global::Kampai.Game.StickerDefinition>(stickerDefinitionID);
			if (locked)
			{
				lockImage.gameObject.SetActive(true);
				image.sprite = UIUtils.LoadSpriteFromPath("img_fill_128");
			}
			else
			{
				image.color = global::UnityEngine.Color.white;
				lockImage.gameObject.SetActive(false);
				image.sprite = UIUtils.LoadSpriteFromPath(stickerDefinition.Image);
			}
			image.maskSprite = UIUtils.LoadSpriteFromPath(stickerDefinition.Mask);
		}
	}
}
