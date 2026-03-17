public class ForceAxisPosition : global::UnityEngine.MonoBehaviour
{
	public enum AxisForce
	{
		X_AXIS = 0,
		Y_AXIS = 1,
		Z_AXIS = 2,
		XZ_AXIS = 3,
		XY_AXIS = 4,
		YZ_AXIS = 5,
		XYZ_AXIS = 6
	}

	public global::UnityEngine.Vector3 forcedPosition = global::UnityEngine.Vector3.zero;

	public ForceAxisPosition.AxisForce limitToAxis;

	private global::UnityEngine.Vector3 pos = global::UnityEngine.Vector3.zero;

	private void Start()
	{
	}

	private void Update()
	{
		pos = base.transform.position;
		switch (limitToAxis)
		{
		case ForceAxisPosition.AxisForce.X_AXIS:
			pos.x = forcedPosition.x;
			break;
		case ForceAxisPosition.AxisForce.Y_AXIS:
			pos.y = forcedPosition.y;
			break;
		case ForceAxisPosition.AxisForce.Z_AXIS:
			pos.z = forcedPosition.z;
			break;
		case ForceAxisPosition.AxisForce.XZ_AXIS:
			pos.x = forcedPosition.x;
			pos.z = forcedPosition.z;
			break;
		case ForceAxisPosition.AxisForce.XY_AXIS:
			pos.x = forcedPosition.x;
			pos.y = forcedPosition.y;
			break;
		case ForceAxisPosition.AxisForce.YZ_AXIS:
			pos.y = forcedPosition.y;
			pos.z = forcedPosition.z;
			break;
		case ForceAxisPosition.AxisForce.XYZ_AXIS:
			pos.x = forcedPosition.x;
			pos.y = forcedPosition.y;
			pos.z = forcedPosition.z;
			break;
		}
		base.transform.position = pos;
	}
}
