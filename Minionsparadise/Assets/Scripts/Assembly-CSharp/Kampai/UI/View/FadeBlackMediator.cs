namespace Kampai.UI.View
{
	public class FadeBlackMediator : global::Kampai.UI.View.KampaiMediator
	{
		[Inject]
		public global::Kampai.UI.View.FadeBlackView view { get; set; }

		[Inject]
		public global::Kampai.UI.View.FadeBlackSignal fadeBlackSignal { get; set; }

		public override void OnRegister()
		{
			fadeBlackSignal.AddListener(view.Fade);
		}

		public override void OnRemove()
		{
			fadeBlackSignal.RemoveListener(view.Fade);
		}
	}
}
