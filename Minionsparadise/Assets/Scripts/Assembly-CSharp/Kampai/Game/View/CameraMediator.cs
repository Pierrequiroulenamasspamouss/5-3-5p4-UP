namespace Kampai.Game.View
{
	public interface CameraMediator
	{
		void OnGameInput(global::UnityEngine.Vector3 position, int input);

		void OnDisableBehaviour(int behaviour);

		void OnEnableBehaviour(int behaviour);
	}
}
