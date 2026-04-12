public class AlligatorCheckpoint : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Material CheckpointPassedMat;

	public global::UnityEngine.MeshRenderer[] RenderersToSwap;

	private void OnTriggerEnter(global::UnityEngine.Collider other)
	{
		global::Kampai.Game.Mignette.AlligatorSkiing.View.AlligatorSkiingMinionHardpointViewObject component = other.GetComponent<global::Kampai.Game.Mignette.AlligatorSkiing.View.AlligatorSkiingMinionHardpointViewObject>();
		if (component != null)
		{
			global::UnityEngine.MeshRenderer[] renderersToSwap = RenderersToSwap;
			foreach (global::UnityEngine.MeshRenderer meshRenderer in renderersToSwap)
			{
				meshRenderer.material = CheckpointPassedMat;
			}
		}
	}
}
