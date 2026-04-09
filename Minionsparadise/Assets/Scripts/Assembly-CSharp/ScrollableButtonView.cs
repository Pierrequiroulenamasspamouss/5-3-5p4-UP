using Kampai.Util;
public class ScrollableButtonView : global::Kampai.Util.KampaiView, global::UnityEngine.EventSystems.IPointerDownHandler, global::UnityEngine.EventSystems.IDragHandler, global::UnityEngine.EventSystems.IPointerUpHandler, global::UnityEngine.EventSystems.IEventSystemHandler, global::Kampai.UI.View.IDoubleConfirmHandler
{
	public global::strange.extensions.signal.impl.Signal ClickedSignal = new global::strange.extensions.signal.impl.Signal();

	private global::System.Collections.IEnumerator waitEnumerator;

	private int tapCount;

	protected bool doubleTap;

	private bool isInConfirmState;

	internal global::UnityEngine.Animator animator;

	[global::UnityEngine.SerializeField]
	private bool ignoreAnimator;

	private global::UnityEngine.UI.ScrollRect myScrollView;

	private float currentOffset;

	private global::UnityEngine.Vector2 startPos;

	[Inject]
	public ILocalPersistanceService localPersistService { get; set; }

	[Inject]
	public global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal { get; set; }

	private float MaxClickOffset
	{
		get
		{
			float dpi = global::UnityEngine.Screen.dpi;
			if (dpi <= 0)
			{
				dpi = 96f;
			}
			return 0.15f * dpi;
		}
	}

	private global::UnityEngine.UI.ScrollRect scrollRect
	{
		get
		{
			if (myScrollView == null)
			{
				myScrollView = base.transform.GetComponentInParent<global::UnityEngine.UI.ScrollRect>();
			}
			return myScrollView;
		}
	}

	public virtual void OnPointerDown(global::UnityEngine.EventSystems.PointerEventData eventData)
	{
		startPos = eventData.position;
		if (scrollRect != null)
		{
			myScrollView.OnBeginDrag(eventData);
		}
		if (waitEnumerator != null)
		{
			StopCoroutine(waitEnumerator);
			waitEnumerator = null;
		}
	}

	public virtual void OnDrag(global::UnityEngine.EventSystems.PointerEventData eventData)
	{
		if (myScrollView != null)
		{
			myScrollView.OnDrag(eventData);
		}
	}

	public virtual void OnPointerUp(global::UnityEngine.EventSystems.PointerEventData eventData)
	{
		currentOffset = (eventData.position - startPos).magnitude;
		if (myScrollView != null)
		{
			myScrollView.OnEndDrag(eventData);
		}
		global::UnityEngine.UI.Button component = GetComponent<global::UnityEngine.UI.Button>();
		bool interactable = component == null || component.interactable;
		
		if (interactable)
		{
			if (currentOffset < MaxClickOffset)
			{
				ButtonClicked();
			}
			else
			{
				global::UnityEngine.Debug.Log(string.Format("ANTIGRAVITY: ScrollableButtonView - Click rejected as drag. Offset: {0}, Max: {1}", currentOffset, MaxClickOffset));
				ResetAnim();
			}
		}
		else
		{
			global::UnityEngine.Debug.Log("ANTIGRAVITY: ScrollableButtonView - Click rejected: not interactable");
		}
		
		if (component != null)
		{
			global::Kampai.UI.View.KampaiButton kampaiButton = component as global::Kampai.UI.View.KampaiButton;
			if (kampaiButton != null)
			{
				kampaiButton.ChangeToNormalState();
			}
		}
		if (base.gameObject.activeInHierarchy && isInConfirmState && waitEnumerator == null)
		{
			waitEnumerator = Wait();
			StartCoroutine(waitEnumerator);
		}
	}

	public virtual void ButtonClicked()
	{
		updateTapCount();
		ClickedSignal.Dispatch();
		if (!isDoubleConfirmed())
		{
			playSFXSignal.Dispatch("Play_button_click_01");
			ShowConfirmMessage();
		}
		else
		{
			ResetAnim();
		}
	}

	public void ResetTapState()
	{
		tapCount = 0;
		if (animator.IsReady() && animator.HasParameter("Pressed_Confirm"))
		{
			animator.SetBool("Pressed_Confirm", false);
		}
		isInConfirmState = false;
	}

	public void updateTapCount()
	{
		if (doubleTap)
		{
			if (tapCount < 2)
			{
				tapCount++;
			}
			else
			{
				tapCount = 1;
			}
		}
	}

	public virtual void EnableDoubleConfirm()
	{
		doubleTap = true;
	}

	public void DisableDoubleConfirm()
	{
		doubleTap = false;
	}

	public virtual void ShowConfirmMessage()
	{
		bool flag = doubleTap && localPersistService.GetDataIntPlayer("DoublePurchaseConfirm") != 0;
		if (animator.IsReady() && animator.HasParameter("Pressed_Confirm") && flag)
		{
			animator.SetBool("Pressed_Confirm", flag);
			isInConfirmState = flag;
			if (base.gameObject.activeSelf && waitEnumerator != null)
			{
				StopCoroutine(waitEnumerator);
			}
			if (base.gameObject.activeSelf && animator.isActiveAndEnabled)
			{
				waitEnumerator = Wait();
				StartCoroutine(Wait());
			}
		}
	}

	public bool isDoubleConfirmed()
	{
		if (doubleTap)
		{
			if (localPersistService.GetDataIntPlayer("DoublePurchaseConfirm") != 0)
			{
				return tapCount == 2;
			}
			return true;
		}
		return true;
	}

	public virtual void ResetAnim()
	{
		ResetTapState();
		if (animator != null)
		{
			animator.Play("Normal", 0, 0f);
		}
	}

	public virtual void Disable()
	{
		AddAnimator();
		if (animator != null)
		{
			animator.Play("Disabled");
		}
	}

	private global::System.Collections.IEnumerator Wait()
	{
		yield return new global::UnityEngine.WaitForSeconds(2.5f);
		if (waitEnumerator != null)
		{
			ResetTapState();
			waitEnumerator = null;
		}
	}

	protected override void Start()
	{
		base.Start();
		AddAnimator();
	}

	private void AddAnimator()
	{
		if (animator == null && !ignoreAnimator)
		{
			animator = base.gameObject.GetComponent<global::UnityEngine.Animator>();
			if (!(animator == null))
			{
				animator.applyRootMotion = false;
			}
		}
	}
}
