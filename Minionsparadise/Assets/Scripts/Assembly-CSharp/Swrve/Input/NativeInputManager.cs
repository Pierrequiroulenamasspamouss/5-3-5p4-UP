namespace Swrve.Input
{
	public class NativeInputManager : global::Swrve.Input.IInputManager
	{
		private static global::Swrve.Input.NativeInputManager instance;

		public static global::Swrve.Input.NativeInputManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new global::Swrve.Input.NativeInputManager();
				}
				return instance;
			}
		}

		private NativeInputManager()
		{
		}

		bool global::Swrve.Input.IInputManager.GetMouseButtonUp(int buttonId)
		{
			return !global::UnityEngine.Input.GetMouseButton(buttonId);
		}

		bool global::Swrve.Input.IInputManager.GetMouseButtonDown(int buttonId)
		{
			return global::UnityEngine.Input.GetMouseButton(buttonId);
		}

		global::UnityEngine.Vector3 global::Swrve.Input.IInputManager.GetMousePosition()
		{
			global::UnityEngine.Vector3 mousePosition = global::UnityEngine.Input.mousePosition;
			mousePosition.y = (float)global::UnityEngine.Screen.height - mousePosition.y;
			return mousePosition;
		}
	}
}
