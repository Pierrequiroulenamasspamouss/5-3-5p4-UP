public class StartGameTrigger : global::UnityEngine.MonoBehaviour
{
	public void OnTriggerEnter(global::UnityEngine.Collider other)
	{
		AlligatorAgent component = other.GetComponent<AlligatorAgent>();
		if (component != null)
		{
			component.OnStartGame();
		}
	}
}
