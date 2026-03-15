public class WaterslideSpinnerViewObject : global::UnityEngine.MonoBehaviour
{
	public enum ParticleState
	{
		None = 0,
		Intro = 1,
		Spin = 2,
		Land = 3,
		Invalid = 4
	}

	private const string INTRO_ANIM_TRIGGER_NAME = "PlayIntro";

	private const string DIVE_ANIM_INT_NAME = "DiveSelected";

	public global::UnityEngine.Animator SpinnerAnimator;

	private global::Kampai.Main.PlayGlobalSoundFXSignal playGlobalAudioSignal;

	public global::UnityEngine.GameObject IntroVfxGameObject;

	public global::UnityEngine.GameObject SpinVfxGameObject;

	public global::UnityEngine.GameObject LandVfxGameObject;

	private WaterslideSpinnerViewObject.ParticleState lastState = WaterslideSpinnerViewObject.ParticleState.Invalid;

	public void Start()
	{
		if (lastState == WaterslideSpinnerViewObject.ParticleState.Invalid)
		{
			SetParticleState(WaterslideSpinnerViewObject.ParticleState.None);
		}
	}

	public void SetParticleState(WaterslideSpinnerViewObject.ParticleState state)
	{
		if (lastState != state)
		{
			lastState = state;
			if (IntroVfxGameObject != null)
			{
				IntroVfxGameObject.SetActive(state == WaterslideSpinnerViewObject.ParticleState.Intro);
			}
			if (SpinVfxGameObject != null)
			{
				SpinVfxGameObject.SetActive(state == WaterslideSpinnerViewObject.ParticleState.Spin);
			}
			if (LandVfxGameObject != null)
			{
				LandVfxGameObject.SetActive(state == WaterslideSpinnerViewObject.ParticleState.Land);
			}
		}
	}

	public void StartIntro(global::Kampai.Main.PlayGlobalSoundFXSignal audioSignal)
	{
		playGlobalAudioSignal = audioSignal;
		SpinnerAnimator.SetTrigger("PlayIntro");
		SetParticleState(WaterslideSpinnerViewObject.ParticleState.Intro);
	}

	public void SelectDive(int diveIndex)
	{
		SetParticleState(WaterslideSpinnerViewObject.ParticleState.Land);
		if (playGlobalAudioSignal != null)
		{
			playGlobalAudioSignal.Dispatch("Play_poseSelect_01");
		}
		SpinnerAnimator.SetInteger("DiveSelected", diveIndex);
	}

	public void OnSpinLoopAnimEvent()
	{
		if (lastState != WaterslideSpinnerViewObject.ParticleState.Land)
		{
			SetParticleState(WaterslideSpinnerViewObject.ParticleState.Spin);
		}
		if (playGlobalAudioSignal != null)
		{
			playGlobalAudioSignal.Dispatch("Play_poseShuffle_01");
		}
	}

	public float GetAnimationPct()
	{
		return SpinnerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f;
	}
}
