namespace Kampai.Game
{
	public class CameraControlsService : global::Kampai.Game.ICameraControlsService
	{
		private global::System.Action<global::UnityEngine.Vector3, int> controlEvent;

		public void RegisterListener(global::System.Action<global::UnityEngine.Vector3, int> obj)
		{
			controlEvent = (global::System.Action<global::UnityEngine.Vector3, int>)global::System.Delegate.Combine(controlEvent, obj);
		}

		public void UnregisterListener(global::System.Action<global::UnityEngine.Vector3, int> obj)
		{
			controlEvent = (global::System.Action<global::UnityEngine.Vector3, int>)global::System.Delegate.Remove(controlEvent, obj);
		}

		public void OnGameInput(global::UnityEngine.Vector3 position, int input)
		{
			if (controlEvent != null)
			{
				controlEvent(position, input);
			}
		}
	}
}
