namespace Prime31
{
	public class LifecycleHelper : global::UnityEngine.MonoBehaviour
	{
		public event global::System.Action<bool> onApplicationPausedEvent;

		private void OnApplicationPause(bool paused)
		{
			if (this.onApplicationPausedEvent != null)
			{
				this.onApplicationPausedEvent(paused);
			}
		}
	}
}
