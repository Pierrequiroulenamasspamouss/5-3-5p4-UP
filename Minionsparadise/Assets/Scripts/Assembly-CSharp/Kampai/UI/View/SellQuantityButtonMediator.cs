namespace Kampai.UI.View
{
	public class SellQuantityButtonMediator : global::strange.extensions.mediation.impl.Mediator
	{
		private float buttonHeldTime;

		private int prevValue;

		[Inject]
		public global::Kampai.UI.View.SellQuantityButtonView view { get; set; }

		[Inject]
		public global::Kampai.Common.AppPauseSignal pauseSignal { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			view.OnPointerDownSignal.AddListener(OnPointerDown);
			view.OnPointerUpSignal.AddListener(OnPointerUp);
			pauseSignal.AddListener(OnPause);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			view.OnPointerDownSignal.RemoveListener(OnPointerDown);
			view.OnPointerUpSignal.RemoveListener(OnPointerUp);
			pauseSignal.RemoveListener(OnPause);
		}

		public void OnPointerDown(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			StartCoroutine(InitWait());
		}

		public void OnPointerUp(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			StopCoroutine(Wait());
			StopCoroutine(InitWait());
		}

		private global::System.Collections.IEnumerator InitWait()
		{
			if (view.IsPriceButton)
			{
				StartCoroutine(WaitToPlaySound(view.PRICE_INIT_WAIT_TIME));
				yield return new global::UnityEngine.WaitForSeconds(view.PRICE_INIT_WAIT_TIME);
				buttonHeldTime = 0f;
				prevValue = view.MinValue;
			}
			else
			{
				StartCoroutine(WaitToPlaySound(view.COUNT_WAIT_TIME));
			}
			view.heldDownSignal.Dispatch(1);
			if (view.IsHeldDown)
			{
				StartCoroutine(Wait());
			}
		}

		private global::System.Collections.IEnumerator Wait()
		{
			global::UnityEngine.UI.Button btn = view.GetComponentInParent<global::UnityEngine.UI.Button>();
			if (btn == null || !btn.interactable)
			{
				yield break;
			}
			if (view.IsPriceButton)
			{
				yield return null;
				if (view.IsHeldDown)
				{
					buttonHeldTime += global::UnityEngine.Time.deltaTime;
					int value = global::UnityEngine.Mathf.FloorToInt(global::UnityEngine.Mathf.Lerp(view.MinValue, view.MaxValue, buttonHeldTime / view.PRICE_MAX_WAIT_TIME));
					if (value != prevValue)
					{
						view.heldDownSignal.Dispatch(value - prevValue);
						prevValue = value;
					}
					StartCoroutine(Wait());
				}
			}
			else
			{
				yield return new global::UnityEngine.WaitForSeconds(view.COUNT_WAIT_TIME);
				if (view.IsHeldDown)
				{
					view.heldDownSignal.Dispatch(1);
					StartCoroutine(Wait());
				}
			}
		}

		private global::System.Collections.IEnumerator WaitToPlaySound(float timeSpacing)
		{
			global::UnityEngine.UI.Button btn = view.GetComponentInParent<global::UnityEngine.UI.Button>();
			if (btn == null || !btn.interactable)
			{
				yield break;
			}
			do
			{
				if (view.IsHeldDown)
				{
					view.playSFXSignal.Dispatch("Play_button_click_01");
				}
				yield return new global::UnityEngine.WaitForSeconds(timeSpacing);
			}
			while (view.IsHeldDown && btn.interactable);
		}

		private void OnPause()
		{
			if (view.IsHeldDown)
			{
				StopCoroutine(WaitToPlaySound(0f));
			}
			view.IsHeldDown = false;
		}
	}
}
