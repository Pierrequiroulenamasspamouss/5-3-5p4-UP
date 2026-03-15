namespace Kampai.UI.View
{
	public class HelpTipButtonView : global::Kampai.UI.View.ButtonView, global::UnityEngine.EventSystems.IPointerDownHandler, global::UnityEngine.EventSystems.IDragHandler, global::UnityEngine.EventSystems.IPointerUpHandler, global::UnityEngine.EventSystems.IEventSystemHandler
	{
		public int tipDefinitionId;

		private global::System.Collections.IEnumerator autoCloseCoroutine;

		public global::UnityEngine.RectTransform rectTransform;

		[Inject]
		public global::Kampai.UI.View.DisplayItemPopupSignal displayItemPopupSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideItemPopupSignal hideItemPopupSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IHelpTipTrackingService helpTipTrackingService { get; set; }

		protected override void Awake()
		{
			base.Awake();
			rectTransform = base.gameObject.GetComponent<global::UnityEngine.RectTransform>();
		}

		public override void OnClickEvent()
		{
			ClickedSignal.Dispatch();
		}

		public void OnPointerDown(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			if (autoCloseCoroutine != null)
			{
				StopCoroutine(autoCloseCoroutine);
				autoCloseCoroutine = null;
			}
			displayItemPopupSignal.Dispatch(tipDefinitionId, rectTransform, global::Kampai.UI.View.UIPopupType.HELPTIP);
			helpTipTrackingService.TrackHelpTipShown(tipDefinitionId);
		}

		public void OnPointerUp(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			autoCloseCoroutine = CloseTipPopup();
			StartCoroutine(autoCloseCoroutine);
		}

		private global::System.Collections.IEnumerator CloseTipPopup()
		{
			yield return new global::UnityEngine.WaitForSeconds(3f);
			hideItemPopupSignal.Dispatch();
			autoCloseCoroutine = null;
		}

		public void OnDrag(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
		}
	}
}
