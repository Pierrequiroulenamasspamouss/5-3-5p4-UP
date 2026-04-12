namespace Kampai.BuildingsSizeToolbox
{
	public class BuildingsSizeToolkitValueUpdateButton : global::UnityEngine.MonoBehaviour, global::UnityEngine.EventSystems.IPointerDownHandler, global::UnityEngine.EventSystems.IPointerUpHandler, global::UnityEngine.EventSystems.IEventSystemHandler
	{
		public float Sign = 1f;

		public float WaitBeforeRepeat = 0.5f;

		public float RepeatRate = 60f;

		public global::strange.extensions.signal.impl.Signal<float> UpdateValueSignal = new global::strange.extensions.signal.impl.Signal<float>();

		private global::UnityEngine.Coroutine repeatCoroutine;

		void global::UnityEngine.EventSystems.IPointerDownHandler.OnPointerDown(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			if (repeatCoroutine == null)
			{
				repeatCoroutine = StartCoroutine(buttonPressedCoroutine());
			}
		}

		void global::UnityEngine.EventSystems.IPointerUpHandler.OnPointerUp(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			if (repeatCoroutine != null)
			{
				StopCoroutine(repeatCoroutine);
				repeatCoroutine = null;
			}
		}

		private global::System.Collections.IEnumerator buttonPressedCoroutine()
		{
			UpdateValueSignal.Dispatch(Sign);
			yield return new global::UnityEngine.WaitForSeconds(WaitBeforeRepeat);
			UpdateValueSignal.Dispatch(Sign);
			while (true)
			{
				yield return new global::UnityEngine.WaitForSeconds(1f / RepeatRate);
				UpdateValueSignal.Dispatch(Sign);
			}
		}
	}
}
