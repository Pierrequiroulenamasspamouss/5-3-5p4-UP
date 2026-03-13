namespace Kampai.UI.View
{
	[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.Animator))]
	public class DiscoGlobeView : global::strange.extensions.mediation.impl.View
	{
		public global::UnityEngine.GameObject DiscoGlobeMesh;

		public global::UnityEngine.GameObject Disco3DElementsParent;

		public global::UnityEngine.GameObject EffectsParentObject;

		public global::UnityEngine.GameObject CameraControlsPanel;

		private global::UnityEngine.Animator animator;

		protected override void Start()
		{
			if (global::UnityEngine.Application.isPlaying)
			{
				base.Start();
			}
			animator = GetComponent<global::UnityEngine.Animator>();
		}

		internal void ShowDiscoBallAwesomeness()
		{
			Disco3DElementsParent.SetActive(true);
			if (animator != null)
			{
				animator.Play("anim_DiscoIntro");
			}
		}

		public void AnimationDoneCallback(string animationState, global::System.Action callback)
		{
			StartCoroutine(CheckAnimationComplete(animationState, callback));
		}

		public bool IsAnimationPlaying(string animationState)
		{
			global::UnityEngine.AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
			return (currentAnimatorStateInfo.IsName(animationState) && currentAnimatorStateInfo.normalizedTime < 1f) || currentAnimatorStateInfo.loop || currentAnimatorStateInfo.normalizedTime < 1f;
		}

		internal global::System.Collections.IEnumerator CheckAnimationComplete(string animationState, global::System.Action callback)
		{
			yield return null;
			while (animator != null && IsAnimationPlaying(animationState))
			{
				yield return null;
			}
			if (callback != null)
			{
				callback();
			}
		}

		internal void RemoveDiscoBallAwesomeness(global::System.Action onFinishCallback)
		{
			if (animator != null)
			{
				animator.Play("anim_DiscoOutro");
				AnimationDoneCallback("anim_DiscoOutro", onFinishCallback);
			}
			else if (onFinishCallback != null)
			{
				onFinishCallback();
			}
		}

		internal void DisplayEffects(bool display)
		{
			EffectsParentObject.SetActive(display);
		}

		internal void ShowCameraControlsPanel(bool display)
		{
			CameraControlsPanel.SetActive(display);
			if (display)
			{
				global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(global::Kampai.Util.KampaiResources.Load<global::UnityEngine.GameObject>("cmp_CameraControls"));
				gameObject.transform.SetParent(CameraControlsPanel.transform, false);
			}
		}

		internal void DisplayDisco3DElements(bool display)
		{
			Disco3DElementsParent.SetActive(display);
		}
	}
}
