namespace Kampai.Fatal
{
	public class FatalView : global::Kampai.Util.KampaiView
	{
		[Inject]
		public global::Kampai.Common.ReInitializeGameSignal reInitializeGameSignal { get; set; }

		private void OnApplicationPause(bool isPausing)
		{
			if (!isPausing && reInitializeGameSignal != null)
			{
				reInitializeGameSignal.Dispatch(string.Empty);
			}
		}

		private void Update()
		{
			if (global::UnityEngine.Input.GetKeyDown(global::UnityEngine.KeyCode.Escape))
			{
				reInitializeGameSignal.Dispatch(string.Empty);
			}
		}
	}
}
