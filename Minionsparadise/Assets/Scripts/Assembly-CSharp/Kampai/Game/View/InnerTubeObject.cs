namespace Kampai.Game.View
{
	public class InnerTubeObject : global::UnityEngine.MonoBehaviour
	{
		public global::System.Collections.Generic.List<global::UnityEngine.Vector3> tubeStartLocations;

		public global::System.Collections.Generic.List<float> tubeRotations;

		public global::System.Collections.Generic.List<float> tubeWaitBeforeFadeoutSeconds;

		public global::System.Collections.Generic.List<float> tubeFadeoutSeconds;

		public global::System.Collections.Generic.List<global::UnityEngine.Renderer> availableRenderers;

		public global::System.Collections.Generic.List<global::UnityEngine.Texture> availableTextures;

		public global::UnityEngine.Animator tubeAnimator;

		private float waitSeconds = 15f;

		private float fadeSeconds = 5f;

		private GoTween tubeTween;

		private float m_fadeAlpha;

		private global::Kampai.Util.Graphics.MaterialModifier materialModifier;

		public float FadeAlpha
		{
			get
			{
				return global::Kampai.Util.Graphics.MaterialModifierExtensions.GetFadeAlpha(materialModifier);
			}
			set
			{
				global::Kampai.Util.Graphics.MaterialModifierExtensions.SetFadeAlpha(materialModifier, value);
			}
		}

		private void Awake()
		{
		}

		public void SetTubeNumberAndFloatAway(int routeNumber, global::Kampai.Util.IRoutineRunner routineRunner, global::Kampai.Common.IRandomService randomService)
		{
			int index = 0;
			if (routeNumber < tubeStartLocations.Count && routeNumber < tubeRotations.Count && routeNumber < tubeWaitBeforeFadeoutSeconds.Count && routeNumber < tubeFadeoutSeconds.Count)
			{
				index = routeNumber;
			}
			waitSeconds = tubeWaitBeforeFadeoutSeconds[index];
			fadeSeconds = tubeFadeoutSeconds[index];
			base.gameObject.transform.position = tubeStartLocations[index];
			base.gameObject.transform.rotation = global::UnityEngine.Quaternion.AngleAxis(tubeRotations[index], global::UnityEngine.Vector3.up);
			StartAnimation(routineRunner, randomService);
		}

		public void StartAnimation(global::Kampai.Util.IRoutineRunner rout, global::Kampai.Common.IRandomService rand)
		{
			int index = rand.NextInt(availableTextures.Count);
			availableRenderers[0].material.mainTexture = availableTextures[index];
			tubeAnimator.applyRootMotion = true;
			tubeAnimator.cullingMode = global::UnityEngine.AnimatorCullingMode.AlwaysAnimate;
			tubeAnimator.Play("FloatAway");
			FadeTubeOut(rout);
		}

		private void FadeTubeOut(global::Kampai.Util.IRoutineRunner rout)
		{
			if (tubeTween != null && tubeTween.isValid())
			{
				tubeTween.destroy();
			}
			materialModifier = materialModifier ?? new global::Kampai.Util.Graphics.MaterialModifier(availableRenderers);
			global::Kampai.Util.Graphics.MaterialModifierExtensions.SetFadeAlpha(materialModifier, 1f);
			tubeTween = Go.to(this, fadeSeconds, new GoTweenConfig().setDelay(waitSeconds).floatProp("FadeAlpha", 0f).onComplete(delegate(AbstractGoTween tubeTweenArg)
			{
				tubeTweenArg.destroy();
				if (rout != null)
				{
					rout.StartCoroutine(Suicide());
				}
			}));
		}

		private global::System.Collections.IEnumerator Suicide()
		{
			yield return new global::UnityEngine.WaitForSeconds(1f);
			global::UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
