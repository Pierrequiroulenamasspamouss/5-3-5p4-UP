public class Rotater : global::UnityEngine.MonoBehaviour
{
	private global::UnityEngine.Vector3 rotationSession = default(global::UnityEngine.Vector3);

	private global::UnityEngine.Vector3 targetRotation = default(global::UnityEngine.Vector3);

	private bool shouldRotate = true;

	private bool shouldTrackRotation;

	public float XRate;

	public float YRate;

	public float ZRate = 30f;

	public event global::System.EventHandler RotationSessionCompleteCallback;

	private void FixedUpdate()
	{
		if (shouldRotate)
		{
			global::UnityEngine.Vector3 zero = global::UnityEngine.Vector3.zero;
			zero.x = global::UnityEngine.Time.deltaTime * XRate;
			zero.y = global::UnityEngine.Time.deltaTime * YRate;
			zero.z = global::UnityEngine.Time.deltaTime * ZRate;
			CheckRotationSession(zero);
			base.transform.Rotate(zero);
		}
	}

	public void SetTargetRotation(global::UnityEngine.Vector3 targetAccumulation)
	{
		shouldRotate = true;
		shouldTrackRotation = true;
		targetRotation = targetAccumulation;
	}

	private void CheckRotationSession(global::UnityEngine.Vector3 rot)
	{
		if (shouldTrackRotation)
		{
			rotationSession += rot;
			if ((rotationSession - targetRotation).magnitude < 0.1f)
			{
				shouldRotate = false;
				rotationSession = global::UnityEngine.Vector3.zero;
				OnRotationComplete();
			}
		}
	}

	private void OnRotationComplete()
	{
		global::System.EventHandler rotationSessionCompleteCallback = this.RotationSessionCompleteCallback;
		if (rotationSessionCompleteCallback != null)
		{
			rotationSessionCompleteCallback(this, global::System.EventArgs.Empty);
		}
	}
}
