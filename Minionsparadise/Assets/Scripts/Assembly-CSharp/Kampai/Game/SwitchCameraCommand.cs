namespace Kampai.Game
{
	internal sealed class SwitchCameraCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::UnityEngine.Camera camera { get; set; }

		[Inject(global::Kampai.Main.MainElement.CAMERA)]
		public global::UnityEngine.Camera mainCamera { get; set; }

		[Inject]
		public global::Kampai.Common.PickControllerModel pickControllerModel { get; set; }

		[Inject(global::Kampai.Main.MainElement.UI_WORLDCANVAS)]
		public global::UnityEngine.GameObject worldCanvas { get; set; }

		public override void Execute()
		{
			if (camera == mainCamera)
			{
				EnableUserInputToEnvironment();
			}
			else
			{
				DisableUserInputToEnvironment();
			}
		}

		private void DisableUserInputToEnvironment()
		{
			ToggleUserInputToEnvironment(false);
			global::UnityEngine.Canvas component = worldCanvas.GetComponent<global::UnityEngine.Canvas>();
			component.worldCamera = camera;
		}

		private void EnableUserInputToEnvironment()
		{
			ToggleUserInputToEnvironment(true);
			global::UnityEngine.Canvas component = worldCanvas.GetComponent<global::UnityEngine.Canvas>();
			component.worldCamera = mainCamera;
		}

		private void ToggleUserInputToEnvironment(bool enable)
		{
			pickControllerModel.ForceDisabled = !enable;
		}
	}
}
