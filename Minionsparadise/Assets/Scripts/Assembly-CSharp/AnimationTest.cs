public class AnimationTest : global::UnityEngine.MonoBehaviour
{
	private global::UnityEngine.Animator animator;

	public global::Kampai.Game.PhilCelebrateSignal celebrateSignal;

	public global::Kampai.Game.PhilGetAttentionSignal getAttentionSignal;

	public global::Kampai.Game.PhilPlayIntroSignal playIntroSignal;

	public global::Kampai.Game.PhilSitAtBarSignal sitAtBarSignal;

	public global::Kampai.Game.PhilActivateSignal activateSignal;

	public global::Kampai.Game.AnimatePhilSignal animatePhilSignal;

	public global::Kampai.Game.PhilGoToTikiBarSignal philGoToTikiBarSignal;

	public global::Kampai.Game.PhilEnableTikiBarControllerSignal enableTikiBarControllerSignal;

	private bool isGettingAttention;

	private bool isSittingAtBar;

	private bool isActive;

	private void Update()
	{
		if (!(animator != null))
		{
			global::UnityEngine.GameObject gameObject = global::UnityEngine.GameObject.Find("Phil");
			if ((bool)gameObject)
			{
				animator = gameObject.GetComponent<global::UnityEngine.Animator>();
				global::Kampai.Game.View.PhilMediator component = gameObject.GetComponent<global::Kampai.Game.View.PhilMediator>();
				celebrateSignal = component.celebrateSignal;
				getAttentionSignal = component.getAttentionSignal;
				playIntroSignal = component.playIntroSignal;
				sitAtBarSignal = component.sitAtBarSignal;
				activateSignal = component.activateSignal;
				enableTikiBarControllerSignal = component.enableTikiBarControllerSignal;
			}
		}
	}

	private void OnGUI()
	{
		if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(10f, 10f, 100f, 75f), "Celebrate"))
		{
			celebrateSignal.Dispatch();
		}
		if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(10f, 95f, 100f, 75f), "Get Attention"))
		{
			isGettingAttention = !isGettingAttention;
			getAttentionSignal.Dispatch(isGettingAttention);
		}
		if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(10f, 180f, 100f, 75f), "Play Intro"))
		{
			playIntroSignal.Dispatch(false);
		}
		if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(10f, 265f, 100f, 75f), "Sit at Bar"))
		{
			isSittingAtBar = !isSittingAtBar;
			sitAtBarSignal.Dispatch(isSittingAtBar);
		}
		if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(10f, 345f, 100f, 75f), "Activate"))
		{
			isActive = !isActive;
			activateSignal.Dispatch(isActive);
		}
		if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(10f, 425f, 100f, 75f), "Enable TikiBar Controller"))
		{
			enableTikiBarControllerSignal.Dispatch();
		}
	}
}
