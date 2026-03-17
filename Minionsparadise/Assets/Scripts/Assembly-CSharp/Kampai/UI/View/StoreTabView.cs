namespace Kampai.UI.View
{
	public class StoreTabView : ScrollableButtonView
	{
		public global::UnityEngine.UI.Text TabName;

		public global::Kampai.UI.View.StoreBadgeView TabBadgeCount;

		public global::Kampai.UI.View.KampaiImage TabIcon;

		public float PaddingInPixel;

		public new global::strange.extensions.signal.impl.Signal<global::Kampai.Game.StoreItemType, string> ClickedSignal = new global::strange.extensions.signal.impl.Signal<global::Kampai.Game.StoreItemType, string>();

		public global::Kampai.Game.StoreItemType Type { get; set; }

		public override void ButtonClicked()
		{
			ClickedSignal.Dispatch(Type, TabName.text);
		}

		internal void SetBadgeCount(int badgeCount)
		{
			TabBadgeCount.SetBadgeCount(global::UnityEngine.Mathf.Abs(TabBadgeCount.CurrentBadgeCount - badgeCount));
		}

		internal void SetNewUnlockState(int badgeCount)
		{
			TabBadgeCount.SetNewUnlockCounter(badgeCount);
		}
	}
}
