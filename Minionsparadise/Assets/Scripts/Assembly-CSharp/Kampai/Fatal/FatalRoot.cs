namespace Kampai.Fatal
{
	public class FatalRoot : global::strange.extensions.context.impl.ContextView
	{
		private void Awake()
		{
			global::UnityEngine.Screen.sleepTimeout = -2;
		}

		private void Start()
		{
			context = new global::Kampai.Fatal.FatalContext(this, false);
			context.Start();
		}
	}
}
