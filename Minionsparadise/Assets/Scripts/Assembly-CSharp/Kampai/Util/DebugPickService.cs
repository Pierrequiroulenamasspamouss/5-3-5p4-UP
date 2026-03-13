namespace Kampai.Util
{
	public class DebugPickService
	{
		private global::UnityEngine.RaycastHit hit;

		private global::UnityEngine.Vector3 inputPosition;

		private global::UnityEngine.GameObject startHitObject;

		private bool prevPress;

		[Inject(global::Kampai.Main.MainElement.CAMERA)]
		public global::UnityEngine.Camera camera { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowDebugVisualizerSignal showDebugVisualizerSignal { get; set; }

		public void OnGameInput(global::UnityEngine.Vector3 inputPosition, int input, bool pressed)
		{
			this.inputPosition = inputPosition;
			if (!prevPress && pressed)
			{
				TouchStart();
			}
			else if (prevPress && pressed)
			{
				TouchHold();
			}
			else if (prevPress && !pressed)
			{
				TouchEnd();
			}
			prevPress = pressed;
		}

		private void TouchStart()
		{
			global::UnityEngine.Ray ray = camera.ScreenPointToRay(inputPosition);
			if (global::UnityEngine.Physics.Raycast(ray, out hit))
			{
				startHitObject = hit.collider.gameObject;
				showDebugVisualizerSignal.Dispatch(startHitObject, -1, 0f);
			}
			if (startHitObject != null)
			{
				switch (startHitObject.layer)
				{
				case 8:
				case 9:
				case 11:
				case 12:
				case 14:
				case 15:
				case 17:
					break;
				case 10:
				case 13:
				case 16:
					break;
				}
			}
		}

		private void TouchHold()
		{
		}

		private void TouchEnd()
		{
			startHitObject = null;
		}
	}
}
