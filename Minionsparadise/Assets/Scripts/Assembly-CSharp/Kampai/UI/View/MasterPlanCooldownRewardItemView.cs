namespace Kampai.UI.View
{
	public class MasterPlanCooldownRewardItemView : global::Kampai.Util.KampaiView
	{
		public global::Kampai.UI.View.KampaiImage icon;

		public global::UnityEngine.UI.Text countText;

		public void SetCount(int count)
		{
			if (count <= 1)
			{
				countText.gameObject.SetActive(false);
			}
			else
			{
				countText.text = count.ToString();
			}
		}
	}
}
