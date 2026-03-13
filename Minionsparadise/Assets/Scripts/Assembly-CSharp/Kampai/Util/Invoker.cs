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

		private void Update()
		{
			if (isInitialized)
			{
				invokerService.Update();
			}
		}
	}
}
