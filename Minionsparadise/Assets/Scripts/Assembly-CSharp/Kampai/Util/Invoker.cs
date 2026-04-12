namespace Kampai.Util
{
	public class Invoker : global::UnityEngine.MonoBehaviour
	{
		private global::Kampai.Util.InvokerService invokerService;

		private bool isInitialized;

		public void Initialize(global::Kampai.Util.InvokerService invokerService)
		{
			this.invokerService = invokerService;
			isInitialized = true;
		}

		private int frameCounter = 0;

		private void Update()
		{
			if (isInitialized && invokerService != null)
			{
				invokerService.Update();
				frameCounter++;
				if (frameCounter >= 300)
				{
					// global::UnityEngine.Debug.Log("Invoker: Main thread pulse alive.");
					frameCounter = 0;
				}
			}
		}
	}
}
