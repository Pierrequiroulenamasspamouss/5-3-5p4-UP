public class AlligatorSkiingSpeedTrigger : global::UnityEngine.MonoBehaviour
{
	public enum TriggerType
	{
		Alligator = 0,
		Minion = 1
	}

	public float Speed;

	public float acceleration;

	public AlligatorSkiingSpeedTrigger.TriggerType TriggerCause;

	private void OnTriggerEnter(global::UnityEngine.Collider other)
	{
		if (TriggerCause == AlligatorSkiingSpeedTrigger.TriggerType.Minion)
		{
			global::Kampai.Game.Mignette.AlligatorSkiing.View.AlligatorSkiingMinionHardpointViewObject component = other.GetComponent<global::Kampai.Game.Mignette.AlligatorSkiing.View.AlligatorSkiingMinionHardpointViewObject>();
			if (component != null)
			{
				component.Agent.ChangeSpeed(Speed, acceleration);
			}
		}
		else
		{
			AlligatorAgent component2 = other.GetComponent<AlligatorAgent>();
			if (component2 != null)
			{
				component2.ChangeSpeed(Speed, acceleration);
			}
		}
	}
}
