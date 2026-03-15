public class ButterflyCatchMignetteNetViewObject : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Animator NetAnimator;

	public global::UnityEngine.Renderer NetTrailRenderer;

	public float SwipeDelay = 0.25f;

	public float CatchDelay = 0.25f;

	public float SwingLength = 0.767f;

	private bool animateSwipe;

	private float swipeTime;

	private float delayTimer;

	public void StartSwipe()
	{
		swipeTime = 0f;
		animateSwipe = true;
		delayTimer = SwipeDelay;
	}

	private void Update()
	{
		if (animateSwipe)
		{
			if (delayTimer > 0f)
			{
				delayTimer -= global::UnityEngine.Time.deltaTime;
			}
			else
			{
				swipeTime += global::UnityEngine.Time.deltaTime;
				if (swipeTime >= SwingLength)
				{
					animateSwipe = false;
					swipeTime = 0f;
				}
			}
		}
		float x = swipeTime / SwingLength;
		NetTrailRenderer.material.SetTextureOffset("_MainTex", new global::UnityEngine.Vector2(x, 0f));
	}
}
