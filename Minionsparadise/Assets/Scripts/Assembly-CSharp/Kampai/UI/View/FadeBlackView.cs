namespace Kampai.UI.View
{
	public class FadeBlackView : global::Kampai.Util.KampaiView
	{
		public global::UnityEngine.UI.Image FadeBlackImage;

		private global::System.Collections.Generic.IList<global::System.Action> actionQueue;

		public float alpha
		{
			get
			{
				return FadeBlackImage.color.a;
			}
			set
			{
				FadeBlackImage.color = new global::UnityEngine.Color(0f, 0f, 0f, value);
			}
		}

		public void Fade(bool fadeIn, global::System.Collections.Generic.IList<global::System.Action> actionQueue)
		{
			this.actionQueue = actionQueue;
			if (fadeIn)
			{
				FadeIn();
			}
			else
			{
				FadeOut();
			}
		}

		private void FadeIn()
		{
			FadeBlackImage.enabled = true;
			GoTweenConfig goTweenConfig = new GoTweenConfig();
			goTweenConfig.floatProp("alpha", 1f, true);
			Go.to(this, 0.25f, goTweenConfig).setOnCompleteHandler(FadeInComplete);
		}

		private void FadeOut()
		{
			GoTweenConfig goTweenConfig = new GoTweenConfig();
			goTweenConfig.floatProp("alpha", -1f, true);
			Go.to(this, 0.25f, goTweenConfig).setOnCompleteHandler(FadeOutComplete);
		}

		private void FadeInComplete(AbstractGoTween tween)
		{
			ProcessActions();
		}

		private void FadeOutComplete(AbstractGoTween tween)
		{
			FadeBlackImage.enabled = false;
			ProcessActions();
		}

		private void ProcessActions()
		{
			if (actionQueue == null)
			{
				return;
			}
			foreach (global::System.Action item in actionQueue)
			{
				item();
			}
			actionQueue.Clear();
		}
	}
}
