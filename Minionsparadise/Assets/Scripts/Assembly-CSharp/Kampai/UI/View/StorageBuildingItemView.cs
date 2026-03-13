namespace Kampai.UI.View
{
	public class StorageBuildingItemView : global::Kampai.Util.KampaiView
	{
		public global::Kampai.UI.View.PopupInfoButtonView InfoButtonView;

		public global::Kampai.UI.View.KampaiImage ItemIcon;

		public global::UnityEngine.UI.Text ItemQuantity;

		public global::Kampai.Game.Item StorageItem { get; set; }

		public global::UnityEngine.Vector2 OffsetMinDestination { get; set; }

		public global::UnityEngine.Vector2 OffsetMaxDestination { get; set; }

		public global::UnityEngine.Vector2 AnchorMinDestination { get; set; }

		public global::UnityEngine.Vector2 AnchorMaxDestination { get; set; }

		public void UpdatePos()
		{
			global::UnityEngine.RectTransform rectTransform = base.transform as global::UnityEngine.RectTransform;
			rectTransform.offsetMin = OffsetMinDestination;
			rectTransform.offsetMax = OffsetMaxDestination;
			rectTransform.anchorMin = AnchorMinDestination;
			rectTransform.anchorMax = AnchorMaxDestination;
		}

		public void MoveToAnchorOffset(float moveTime, global::UnityEngine.Vector2 newOffsetMin, global::UnityEngine.Vector2 newOffsetMax, global::UnityEngine.Vector2 newAnchorMin, global::UnityEngine.Vector2 newAnchorMax)
		{
			global::UnityEngine.RectTransform rectTransform = base.transform as global::UnityEngine.RectTransform;
			OffsetMinDestination = rectTransform.offsetMin;
			OffsetMaxDestination = rectTransform.offsetMax;
			AnchorMinDestination = rectTransform.anchorMin;
			AnchorMaxDestination = rectTransform.anchorMax;
			Go.to(this, moveTime, new GoTweenConfig().setEaseType(GoEaseType.Linear).vector2Prop("OffsetMinDestination", newOffsetMin).vector2Prop("OffsetMaxDestination", newOffsetMax)
				.vector2Prop("AnchorMinDestination", newAnchorMin)
				.vector2Prop("AnchorMaxDestination", newAnchorMax)
				.onUpdate(delegate
				{
					UpdatePos();
				})
				.onComplete(delegate(AbstractGoTween thisTween)
				{
					thisTween.destroy();
				}));
		}

		internal void SelectItem(bool isSelected)
		{
			if (isSelected)
			{
				global::UnityEngine.Vector3 originalScale = global::UnityEngine.Vector3.one;
				global::Kampai.Util.TweenUtil.Throb(ItemIcon.transform, 0.85f, 0.5f, out originalScale);
			}
			else
			{
				Go.killAllTweensWithTarget(ItemIcon.transform);
				ItemIcon.transform.localScale = global::UnityEngine.Vector3.one;
			}
		}
	}
}
