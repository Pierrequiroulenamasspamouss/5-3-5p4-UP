namespace Kampai.Util.AnimatorStateInfo
{
	public class AnimatorStateView : global::strange.extensions.mediation.impl.View
	{
		private global::UnityEngine.Animator animator;

		private global::UnityEngine.UI.Text stateText;

		public void Initialize(global::UnityEngine.Animator animator)
		{
			this.animator = animator;
			stateText = GetComponent<global::UnityEngine.UI.Text>();
			global::UnityEngine.RectTransform component = GetComponent<global::UnityEngine.RectTransform>();
			component.localScale = global::UnityEngine.Vector3.one;
		}

		internal int? GetNameHash()
		{
			if (animator == null)
			{
				return null;
			}
			return animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
		}

		internal void UpdatePosition()
		{
			if (!(animator == null) && !(base.transform == null))
			{
				global::UnityEngine.RectTransform component = GetComponent<global::UnityEngine.RectTransform>();
				component.anchoredPosition = global::UnityEngine.Camera.main.WorldToScreenPoint(animator.transform.position);
			}
		}

		internal void UpdateStateName(string stateName)
		{
			if (!(stateText == null))
			{
				if (animator == null)
				{
					stateText.text = string.Empty;
				}
				else
				{
					stateText.text = stateName;
				}
			}
		}
	}
}
