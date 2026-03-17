namespace Kampai.UI.View
{
	public class MinionPartyInfoPopupView : global::Kampai.UI.View.PopupMenuView, global::UnityEngine.EventSystems.IPointerDownHandler, global::UnityEngine.EventSystems.IPointerUpHandler, global::UnityEngine.EventSystems.IEventSystemHandler, IGenericPopupView
	{
		public global::UnityEngine.UI.Text partyPointsText;

		public float offsetValue = -7f;

		public global::UnityEngine.RectTransform FillImage;

		public global::strange.extensions.signal.impl.Signal<global::UnityEngine.EventSystems.PointerEventData> pointerDownSignal = new global::strange.extensions.signal.impl.Signal<global::UnityEngine.EventSystems.PointerEventData>();

		public global::strange.extensions.signal.impl.Signal<global::UnityEngine.EventSystems.PointerEventData> pointerUpSignal = new global::strange.extensions.signal.impl.Signal<global::UnityEngine.EventSystems.PointerEventData>();

		public int partyPointsTweenCount { get; set; }

		public void Init(global::Kampai.Main.ILocalizationService localizationService)
		{
			base.Init();
		}

		public void Display(global::UnityEngine.Vector3 itemCenter)
		{
			base.Init();
			global::UnityEngine.RectTransform rectTransform = base.transform as global::UnityEngine.RectTransform;
			global::UnityEngine.Vector3 anchoredPosition3D = rectTransform.sizeDelta / offsetValue;
			anchoredPosition3D.x = 0f;
			rectTransform.anchoredPosition3D = anchoredPosition3D;
			rectTransform.anchorMin = itemCenter;
			rectTransform.anchorMax = itemCenter;
			base.Open();
		}

		public void OnPointerDown(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			pointerDownSignal.Dispatch(eventData);
		}

		public void OnPointerUp(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			pointerUpSignal.Dispatch(eventData);
		}

		public void SetPartyPoints(uint partyPoints, uint maxPartyPoints, bool animate = true)
		{
			if (!animate)
			{
				SetPartyPointsText(partyPoints, maxPartyPoints);
				FillImage.anchorMax = new global::UnityEngine.Vector2((float)partyPoints / (float)maxPartyPoints, 1f);
				return;
			}
			SetPartyPointsText(partyPoints, maxPartyPoints);
			Go.to(FillImage, 1f, new GoTweenConfig().vector2Prop("anchorMax", new global::UnityEngine.Vector2((float)partyPoints / (float)maxPartyPoints, 1f)).onComplete(delegate
			{
				if (partyPoints > maxPartyPoints)
				{
					partyPoints = maxPartyPoints;
				}
			}));
		}

		public void SetPartyPointsText(uint partyPoints, uint maxPartyPoints)
		{
			partyPointsText.text = string.Format("{0}/{1}", partyPoints, maxPartyPoints);
		}
	}
}
