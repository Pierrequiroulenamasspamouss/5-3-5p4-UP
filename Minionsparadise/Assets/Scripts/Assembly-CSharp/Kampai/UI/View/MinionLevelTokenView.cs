namespace Kampai.UI.View
{
	public class MinionLevelTokenView : global::Kampai.UI.View.WorldToGlassView
	{
		public global::Kampai.UI.View.ButtonView HarvestButton;

		protected override string UIName
		{
			get
			{
				return "MinionLevelToken";
			}
		}

		internal void SetTokenCount(uint tokenCount)
		{
			base.gameObject.SetActive(tokenCount != 0);
		}

		protected override void LoadModalData(global::Kampai.UI.View.WorldToGlassUIModal modal)
		{
		}
	}
}
