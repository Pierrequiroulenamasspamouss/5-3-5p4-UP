namespace Kampai.UI.View
{
	public class SalepackHUDView : global::Kampai.Util.KampaiView
	{
		public global::Kampai.UI.View.ButtonView SalePackButton;

		public global::UnityEngine.Transform VFXRoot;

		public global::UnityEngine.UI.Text ItemText;

		public global::Kampai.UI.View.KampaiImage ItemIcon;

		public global::strange.extensions.signal.impl.Signal closeSignal = new global::strange.extensions.signal.impl.Signal();

		public global::Kampai.Game.Sale SalePackItem { get; set; }

		public void Init()
		{
		}

		public void SetupIcon(global::Kampai.Game.SalePackDefinition salePackDefinition)
		{
			if (!string.IsNullOrEmpty(salePackDefinition.GlassIconImage) && !string.IsNullOrEmpty(salePackDefinition.GlassIconMask))
			{
				ItemIcon.sprite = UIUtils.LoadSpriteFromPath(salePackDefinition.GlassIconImage);
				ItemIcon.maskSprite = UIUtils.LoadSpriteFromPath(salePackDefinition.GlassIconMask);
			}
		}

		public void Close()
		{
			closeSignal.Dispatch();
		}
	}
}
