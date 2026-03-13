namespace Kampai.UI.View
{
	public class CollectionRewardIndicator : global::UnityEngine.MonoBehaviour
	{
		public global::UnityEngine.UI.Text RewardCountLabel;

		public global::Kampai.UI.View.KampaiImage RewardImage;

		public global::UnityEngine.UI.Text RewardLocLabel;

		public void PlayCollectAnimation()
		{
			GetComponent<global::UnityEngine.Animator>().Play("Collect", 0, 0f);
		}
	}
}
