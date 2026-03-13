public class WaterslideVFXManager : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.GameObject MinionWakeVFX;

	public global::UnityEngine.Transform MinionWakeHardpoint;

	public global::UnityEngine.GameObject ObstacleImpactVFX;

	private global::UnityEngine.GameObject minionWakeInstance;

	private void Start()
	{
		minionWakeInstance = global::UnityEngine.Object.Instantiate(MinionWakeVFX);
		minionWakeInstance.transform.parent = MinionWakeHardpoint;
		minionWakeInstance.transform.localPosition = global::UnityEngine.Vector3.zero;
	}

	public void DisplayMinionWake(bool display)
	{
		global::UnityEngine.ParticleSystem[] componentsInChildren = minionWakeInstance.GetComponentsInChildren<global::UnityEngine.ParticleSystem>();
		global::UnityEngine.ParticleSystem[] array = componentsInChildren;
		foreach (global::UnityEngine.ParticleSystem ps in array)
		{
			global::UnityEngine.UnityUtils.SetEmissionEnabled(ps, display);
		}
	}
}
