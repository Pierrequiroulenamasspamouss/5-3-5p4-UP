public class AlligatorVFXManager : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.GameObject AlligatorWakeVFX;

	public global::UnityEngine.Transform AlligatorWakeHardpoint;

	public global::UnityEngine.GameObject MinionWakeVFX;

	public global::UnityEngine.Transform MinionWakeHardpoint;

	public global::UnityEngine.GameObject ObstacleImpactVFX;

	private global::UnityEngine.GameObject alligatorWakeInstance;

	private global::UnityEngine.GameObject minionWakeInstance;

	private void Start()
	{
		alligatorWakeInstance = global::UnityEngine.Object.Instantiate(AlligatorWakeVFX);
		alligatorWakeInstance.transform.parent = AlligatorWakeHardpoint;
		alligatorWakeInstance.transform.localPosition = global::UnityEngine.Vector3.zero;
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

	public void PlayImpactVfx(global::UnityEngine.Vector3 pos)
	{
		global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(ObstacleImpactVFX);
		gameObject.transform.position = pos;
		gameObject.SetActive(true);
	}

	public void DisplayAlligatorWake(bool display)
	{
		alligatorWakeInstance.SetActive(display);
	}
}
