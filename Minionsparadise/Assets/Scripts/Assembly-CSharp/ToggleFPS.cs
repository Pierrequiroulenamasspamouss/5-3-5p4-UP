public class ToggleFPS : global::UnityEngine.MonoBehaviour
{
	public void Toggle()
	{
		FPSGraphC component = global::UnityEngine.Camera.main.GetComponent<FPSGraphC>();
		if (component != null)
		{
			component.enabled = !component.enabled;
			return;
		}
		component = global::UnityEngine.Camera.main.gameObject.AddComponent<FPSGraphC>();
		component.showFPSNumber = true;
		component.showPerformanceOnClick = false;
	}
}
