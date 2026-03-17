public class AlligatorSkiingRotateTrigger : global::UnityEngine.MonoBehaviour
{
	public enum TriggerType
	{
		Alligator = 0,
		Minion = 1
	}

	public global::UnityEngine.Vector3 Rotation = global::UnityEngine.Vector3.zero;

	public AlligatorSkiingRotateTrigger.TriggerType TriggerCause = AlligatorSkiingRotateTrigger.TriggerType.Minion;

	private void OnTriggerEnter(global::UnityEngine.Collider other)
	{
		if (TriggerCause == AlligatorSkiingRotateTrigger.TriggerType.Minion)
		{
			global::Kampai.Game.Mignette.AlligatorSkiing.View.AlligatorSkiingMinionHardpointViewObject component = other.GetComponent<global::Kampai.Game.Mignette.AlligatorSkiing.View.AlligatorSkiingMinionHardpointViewObject>();
			if (component != null)
			{
				component.Agent.OnRotateMinion(global::UnityEngine.Quaternion.Euler(Rotation));
			}
		}
		else
		{
			AlligatorAgent component2 = other.GetComponent<AlligatorAgent>();
			if (component2 != null)
			{
				component2.OnRotateMinion(global::UnityEngine.Quaternion.Euler(Rotation));
			}
		}
	}
}
