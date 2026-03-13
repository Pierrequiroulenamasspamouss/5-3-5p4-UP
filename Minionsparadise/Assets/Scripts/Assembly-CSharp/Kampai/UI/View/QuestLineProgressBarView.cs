namespace Kampai.UI.View
{
	public class QuestLineProgressBarView : global::Kampai.Util.KampaiView
	{
		public global::UnityEngine.UI.Text QuestLineTitleText;

		public global::UnityEngine.UI.Text QuestLineProgressText;

		public global::UnityEngine.UI.Image FillImage;

		internal void SetTitle(string title)
		{
			QuestLineTitleText.text = title;
		}

		internal void UpdateProgress(int CompletedCount, int TotalCount)
		{
			QuestLineProgressText.text = string.Format("{0}/{1}", CompletedCount, TotalCount);
			float x = (float)CompletedCount / (float)TotalCount;
			FillImage.rectTransform.anchorMax = new global::UnityEngine.Vector2(x, 1f);
		}
	}
}
