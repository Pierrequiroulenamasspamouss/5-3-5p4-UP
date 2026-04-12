namespace Kampai.UI.View
{
	public class DebrisModalView : global::Kampai.UI.View.PopupMenuView
	{
		public global::UnityEngine.UI.Text TitleText;

		public global::UnityEngine.UI.Text MinionsCountText;

		public global::UnityEngine.UI.Text AvailableText;

		public global::UnityEngine.GameObject DebrisModalItemPrefab;

		public global::UnityEngine.Transform DebrisModalItemContainer;

		private global::Kampai.UI.View.DebrisModalItemView modalItemView;

		private string itemImage;

		private string itemMask;

		private int itemsRequired;

		public int MinionsAvailable { get; private set; }

		internal void Init(int minionsAvailable, int itemsAvailable, int itemsRequired, string image, string mask, global::Kampai.Main.ILocalizationService localService, global::Kampai.Game.DebrisBuildingDefinition definition)
		{
			base.Init();
			this.itemsRequired = itemsRequired;
			UpdateAvailableMinions(minionsAvailable);
			string text = localService.GetString(definition.LocalizedKey);
			string text2 = localService.GetString("ClearX?", text);
			TitleText.text = text2;
			AvailableText.text = localService.GetString("ResourceAvailable");
			itemImage = image;
			itemMask = mask;
			CreateItem(itemsAvailable);
			base.Open();
		}

		internal void UpdateAvailableMinions(int minionsAvailable)
		{
			MinionsAvailable = minionsAvailable;
			MinionsCountText.text = string.Format("{0}", minionsAvailable);
		}

		internal void OnDragItemOverDropArea(global::Kampai.UI.View.DragDropItemView dragDropItemView, bool success)
		{
			global::Kampai.UI.View.DebrisModalItemView component = dragDropItemView.GetComponent<global::Kampai.UI.View.DebrisModalItemView>();
			component.Highlight(success);
		}

		internal void OnDropItemOverDropArea(global::Kampai.UI.View.DragDropItemView dragDropItemView, bool success)
		{
			global::Kampai.UI.View.DebrisModalItemView component = dragDropItemView.GetComponent<global::Kampai.UI.View.DebrisModalItemView>();
			component.Highlight(false);
			component.ResetPosition(!success);
		}

		private void CreateItem(int itemsAvailable)
		{
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(DebrisModalItemPrefab);
			gameObject.transform.SetParent(DebrisModalItemContainer, false);
			modalItemView = gameObject.GetComponent<global::Kampai.UI.View.DebrisModalItemView>();
			modalItemView.Init(itemImage, itemMask, itemsAvailable, itemsRequired);
		}
	}
}
