namespace Kampai.UI.View
{
	public class RewardedAdPickRewardModalView : global::Kampai.UI.View.PopupMenuView
	{
		public global::UnityEngine.UI.Text Headline;

		public global::UnityEngine.UI.Button CollectButton;

		public global::UnityEngine.UI.Text CollectButtonButtonText;

		public global::UnityEngine.UI.Button[] Boxes;

		private global::Kampai.Main.ILocalizationService localService;

		private global::Kampai.Game.IDefinitionService definitionService;

		private global::Kampai.Common.IRandomService randomService;

		internal void Init(global::Kampai.Main.ILocalizationService localService, global::Kampai.Game.IDefinitionService definitionService, global::Kampai.Common.IRandomService randomService)
		{
			base.Init();
			this.localService = localService;
			this.definitionService = definitionService;
			this.randomService = randomService;
			Localize();
			base.Open();
		}

		private void Localize()
		{
			Headline.text = localService.GetString("RewardAdPickRewardHeadline");
			CollectButtonButtonText.text = localService.GetString("Collect");
		}

		internal void Select(int index, global::Kampai.Util.QuantityItem[] items)
		{
			int num = 0;
			global::UnityEngine.UI.Button[] boxes = Boxes;
			foreach (global::UnityEngine.UI.Button button in boxes)
			{
				if (num++ == index)
				{
					button.interactable = false;
				}
				else
				{
					button.enabled = false;
				}
			}
			global::Kampai.Game.ItemDefinition rewardItem = definitionService.Get<global::Kampai.Game.ItemDefinition>(items[0].ID);
			int quantity = (int)items[0].Quantity;
			AddReward(Boxes[index].transform, rewardItem, quantity, true);
			StartCoroutine(ShowUnselectedBoxes(index, items));
		}

		private global::System.Collections.IEnumerator ShowUnselectedBoxes(int index, global::Kampai.Util.QuantityItem[] items)
		{
			yield return new global::UnityEngine.WaitForSeconds(1f);
			int j = 0;
			int[] itemsIndices = new int[2] { 1, 1 };
			itemsIndices[randomService.NextInt(2)]++;
			for (int i = 0; i < 3; i++)
			{
				if (i != index)
				{
					global::UnityEngine.UI.Button box = Boxes[i];
					box.enabled = true;
					box.interactable = false;
					int itemIndex = itemsIndices[j];
					AddReward(rewardItem: definitionService.Get<global::Kampai.Game.ItemDefinition>(items[itemIndex].ID), rewardAmount: (int)items[itemIndex].Quantity, parent: Boxes[i].transform, playVFX: false);
					j++;
				}
			}
			yield return new global::UnityEngine.WaitForSeconds(1f);
			CollectButton.gameObject.SetActive(true);
		}

		private void AddReward(global::UnityEngine.Transform parent, global::Kampai.Game.ItemDefinition rewardItem, int rewardAmount, bool playVFX)
		{
			global::UnityEngine.GameObject original = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.GameObject>("rewardedAdReward");
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(original);
			gameObject.transform.SetParent(parent, false);
			global::Kampai.UI.View.RewardedAdRewardView component = gameObject.GetComponent<global::Kampai.UI.View.RewardedAdRewardView>();
			component.Init(rewardItem, rewardAmount, playVFX);
		}
	}
}
