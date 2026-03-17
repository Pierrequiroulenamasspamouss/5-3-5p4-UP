public class EnvironmentAudioManagerView : global::strange.extensions.mediation.impl.View
{
	public global::strange.extensions.signal.impl.Signal<global::UnityEngine.Vector3> checkHit = new global::strange.extensions.signal.impl.Signal<global::UnityEngine.Vector3>();

	public global::UnityEngine.Camera mainCamera { get; set; }

	private void Update()
	{
		if (!(mainCamera == null))
		{
			global::UnityEngine.Plane plane = new global::UnityEngine.Plane(global::UnityEngine.Vector3.up, global::UnityEngine.Vector3.zero);
			global::UnityEngine.Ray ray = new global::UnityEngine.Ray(mainCamera.transform.position, mainCamera.transform.forward);
			float enter;
			if (plane.Raycast(ray, out enter))
			{
				global::UnityEngine.Vector3 point = ray.GetPoint(enter);
				base.transform.position = point;
				checkHit.Dispatch(point);
			}
		}
	}
}
