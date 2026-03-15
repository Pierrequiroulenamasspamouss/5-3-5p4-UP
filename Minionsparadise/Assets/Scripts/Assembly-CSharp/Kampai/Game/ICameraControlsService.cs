namespace Kampai.Game
{
	public interface ICameraControlsService
	{
		void RegisterListener(global::System.Action<global::UnityEngine.Vector3, int> obj);

		void UnregisterListener(global::System.Action<global::UnityEngine.Vector3, int> obj);

		void OnGameInput(global::UnityEngine.Vector3 position, int input);
	}
}
