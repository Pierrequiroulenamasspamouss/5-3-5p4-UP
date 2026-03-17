public class AlligatorVFXTrigger : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.GameObject VFXGameObject;

	public global::UnityEngine.Transform VFXParentTransform;

	public bool AttachToTransform;

	private global::UnityEngine.Transform vfxMarkerTransform;

	private global::UnityEngine.Transform vfxTransform;

	private void OnTriggerEnter(global::UnityEngine.Collider other)
	{
		global::Kampai.Game.Mignette.AlligatorSkiing.View.AlligatorSkiingMinionHardpointViewObject component = other.GetComponent<global::Kampai.Game.Mignette.AlligatorSkiing.View.AlligatorSkiingMinionHardpointViewObject>();
		if (component != null)
		{
			vfxTransform = global::UnityEngine.Object.Instantiate(VFXGameObject).transform;
			vfxTransform.SetParent(VFXParentTransform, false);
			vfxMarkerTransform = component.MinionTriggerVFXMarker;
		}
	}

	private void Update()
	{
		if (AttachToTransform && vfxTransform != null)
		{
			VFXParentTransform.position = vfxMarkerTransform.position;
		}
	}
}
