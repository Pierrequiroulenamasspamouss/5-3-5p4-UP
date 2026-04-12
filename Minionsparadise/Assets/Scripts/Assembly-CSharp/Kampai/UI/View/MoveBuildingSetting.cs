namespace Kampai.UI.View
{
	public class MoveBuildingSetting : global::Kampai.UI.View.WorldToGlassUISettings
	{
		public int Mask { get; private set; }

		public bool pulseAcceptButton { get; private set; }

		public bool ShowCostPanel { get; private set; }

		public MoveBuildingSetting(int trackedId, int Mask, bool pulseAccept, bool showCostPanel)
			: base(trackedId)
		{
			this.Mask = Mask;
			pulseAcceptButton = pulseAccept;
			ShowCostPanel = showCostPanel;
		}
	}
}
