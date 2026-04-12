namespace Swrve.Input
{
	public interface IInputManager
	{
		bool GetMouseButtonUp(int buttonId);

		bool GetMouseButtonDown(int buttonId);

		global::UnityEngine.Vector3 GetMousePosition();
	}
}
