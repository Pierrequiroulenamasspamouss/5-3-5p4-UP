namespace Kampai.UI.View
{
	public class LevelUpButtonView : global::Kampai.UI.View.ButtonView, global::UnityEngine.EventSystems.IPointerDownHandler, global::UnityEngine.EventSystems.IPointerUpHandler, global::UnityEngine.EventSystems.IEventSystemHandler
	{
		private bool isDragging;

		private float currentOffset;

		private global::UnityEngine.Vector2 startPos;

		private float MaxClickOffset
		{
			get
			{
				return 0.15f * global::UnityEngine.Screen.dpi;
			}
		}

		public override void OnClickEvent()
		{
			if (PlaySoundOnClick)
			{
				base.playSFXSignal.Dispatch("Play_button_click_01");
			}
			if (!isDragging)
			{
				ClickedSignal.Dispatch();
			}
		}

		public virtual void OnPointerDown(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			startPos = eventData.position;
		}

		public virtual void OnPointerUp(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			currentOffset = (eventData.position - startPos).magnitude;
			if (currentOffset < MaxClickOffset)
			{
				isDragging = false;
			}
			else
			{
				isDragging = true;
			}
		}
	}
}
