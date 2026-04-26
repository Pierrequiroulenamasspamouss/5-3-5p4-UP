namespace Kampai.Game
{
	public class KeyboardInput : global::Kampai.Game.IInput
	{
		private bool previousState;

		private bool pressed;

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Common.IPickService pickService { get; set; }

		[Inject]
		public global::Kampai.Game.Mignette.IMignetteService mignetteService { get; set; }

		[Inject]
		public global::Kampai.Game.DebugKeyHitSignal debugKeyHitSignal { get; set; }

		[PostConstruct]
		public void PostConstruct()
		{
			routineRunner.StartCoroutine(Update());
		}

		private global::System.Collections.IEnumerator Update()
		{
			while (true)
			{
				bool currentState = global::UnityEngine.Input.GetMouseButton(0);
				int input = 0;
				if (currentState)
				{
					input |= 1;
				}
				if (global::UnityEngine.Mathf.Abs(global::UnityEngine.Input.GetAxis("Mouse ScrollWheel")) > 0f)
				{
					input |= 2;
				}
				if (!previousState && currentState)
				{
					pressed = true;
				}
				else if (previousState && !currentState)
				{
					pressed = false;
				}
				pickService.OnGameInput(global::UnityEngine.Input.mousePosition, input, pressed);
				mignetteService.OnGameInput(global::UnityEngine.Input.mousePosition, input, pressed);
				previousState = currentState;
				yield return null;
			}
		}
	}
}
