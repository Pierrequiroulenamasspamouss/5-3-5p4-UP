public class AlligatorCameraTrigger : global::UnityEngine.MonoBehaviour
{
	public enum TriggerType
	{
		Alligator = 0,
		Minion = 1
	}

	public global::UnityEngine.Transform CameraTransform;

	public float Delay;

	public float TransitionDuration = 1f;

	public GoEaseType EaseType = GoEaseType.QuadInOut;

	public AlligatorCameraTrigger.TriggerType TriggerCause = AlligatorCameraTrigger.TriggerType.Minion;

	private AlligatorCameraController minionHardpoint;

	private void OnTriggerEnter(global::UnityEngine.Collider other)
	{
		if (TriggerCause == AlligatorCameraTrigger.TriggerType.Minion)
		{
			AlligatorCameraController component = other.gameObject.GetComponent<AlligatorCameraController>();
			if (component != null)
			{
				minionHardpoint = component;
				Invoke("MoveCamera", Delay);
			}
		}
		else
		{
			AlligatorAgent component2 = other.gameObject.GetComponent<AlligatorAgent>();
			if (component2 != null)
			{
				minionHardpoint = component2.MinionHardpointTransform.GetComponent<AlligatorCameraController>();
				Invoke("MoveCamera", Delay);
			}
		}
	}

	public void MoveCamera()
	{
		if (minionHardpoint != null)
		{
			minionHardpoint.AlignWithTransform(CameraTransform, TransitionDuration, EaseType);
			minionHardpoint = null;
		}
	}
}
