namespace Kampai.UI.View
{
	public class UIRoot : global::strange.extensions.context.impl.ContextView
	{
		private void Awake()
		{
			UnityEngine.Debug.Log("ANTIGRAVITY: UIRoot.Awake() called");
			context = new global::Kampai.UI.View.UIContext(this, true);
			UnityEngine.Debug.Log("ANTIGRAVITY: UIContext created, calling Start()");
			context.Start();
			UnityEngine.Debug.Log("ANTIGRAVITY: UIContext.Start() returned");
		}
	}
}
