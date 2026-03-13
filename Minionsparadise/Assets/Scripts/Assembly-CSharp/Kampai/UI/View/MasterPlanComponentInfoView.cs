namespace Kampai.UI.View
{
	public class MasterPlanComponentInfoView : global::Kampai.Util.KampaiView
	{
		public enum ItemType
		{
			Requires = 0,
			Rewards = 1
		}

		public bool enabledTextOnRequires;

		public global::Kampai.UI.View.LocalizeView titleText;

		public global::Kampai.UI.View.KampaiImage itemPanelBackground;

		public global::UnityEngine.Transform itemPanel;

		[global::UnityEngine.Header("Colors")]
		public global::UnityEngine.Color requiresPanelBackgroundColor;

		public global::UnityEngine.Color requiresTitleColor;

		public global::UnityEngine.Color rewardPanelBackgroundColor;

		public global::UnityEngine.Color rewardTitleColor;

		[global::UnityEngine.Header("External Headers")]
		public global::UnityEngine.GameObject itemViewPrefab;

		private readonly global::System.Collections.Generic.IList<global::UnityEngine.MonoBehaviour> itemIconList = new global::System.Collections.Generic.List<global::UnityEngine.MonoBehaviour>();

		internal readonly global::strange.extensions.signal.impl.Signal<global::Kampai.Game.MasterPlanComponentTask, global::Kampai.UI.View.KampaiImage> setTaskIconSignal = new global::strange.extensions.signal.impl.Signal<global::Kampai.Game.MasterPlanComponentTask, global::Kampai.UI.View.KampaiImage>();

		internal readonly global::strange.extensions.signal.impl.Signal<global::Kampai.Util.QuantityItem, global::Kampai.UI.View.KampaiImage> setRewardIconSignal = new global::strange.extensions.signal.impl.Signal<global::Kampai.Util.QuantityItem, global::Kampai.UI.View.KampaiImage>();

		internal readonly global::strange.extensions.signal.impl.Signal updateItemsSignal = new global::strange.extensions.signal.impl.Signal();

		public global::Kampai.UI.View.MasterPlanComponentInfoView.ItemType itemType { get; internal set; }

		public int index { get; private set; }

		internal void Init(global::Kampai.UI.View.MasterPlanComponentInfoView.ItemType itemType, int index)
		{
			this.itemType = itemType;
			this.index = index;
			switch (this.itemType)
			{
			case global::Kampai.UI.View.MasterPlanComponentInfoView.ItemType.Requires:
				titleText.LocKey = "MasterPlanRequires";
				titleText.color = requiresTitleColor;
				itemPanelBackground.color = requiresPanelBackgroundColor;
				break;
			case global::Kampai.UI.View.MasterPlanComponentInfoView.ItemType.Rewards:
				titleText.LocKey = "MasterPlanReward";
				titleText.color = rewardTitleColor;
				itemPanelBackground.color = rewardPanelBackgroundColor;
				break;
			}
			updateItemsSignal.Dispatch();
		}

		internal void Refresh(global::System.Type type, int componentIndex)
		{
			if (type == GetType())
			{
				index = componentIndex;
				updateItemsSignal.Dispatch();
			}
		}

		internal void SetupRewardIcons(global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> rewardItems)
		{
			if (rewardItems == null)
			{
				return;
			}
			int count = rewardItems.Count;
			int num = UpdateImages(count);
			for (int i = 0; i < num; i++)
			{
				global::Kampai.UI.View.MasterPlanComponentItemView masterPlanComponentItemView = itemIconList[i] as global::Kampai.UI.View.MasterPlanComponentItemView;
				if (!(masterPlanComponentItemView == null))
				{
					if (i > count)
					{
						masterPlanComponentItemView.gameObject.SetActive(false);
						continue;
					}
					masterPlanComponentItemView.gameObject.SetActive(true);
					global::Kampai.Util.QuantityItem quantityItem = rewardItems[i];
					masterPlanComponentItemView.quantityText.text = UIUtils.FormatLargeNumber((int)quantityItem.Quantity);
					setRewardIconSignal.Dispatch(quantityItem, masterPlanComponentItemView.icon);
				}
			}
		}

		internal void SetupTasksIcons(global::System.Collections.Generic.IList<global::Kampai.Game.MasterPlanComponentTask> tasks)
		{
			if (tasks == null)
			{
				return;
			}
			int count = tasks.Count;
			int num = UpdateImages(count);
			for (int i = 0; i < num; i++)
			{
				global::Kampai.UI.View.MasterPlanComponentItemView masterPlanComponentItemView = itemIconList[i] as global::Kampai.UI.View.MasterPlanComponentItemView;
				if (masterPlanComponentItemView == null)
				{
					continue;
				}
				if (i >= count)
				{
					masterPlanComponentItemView.gameObject.SetActive(false);
					continue;
				}
				masterPlanComponentItemView.gameObject.SetActive(true);
				global::Kampai.Game.MasterPlanComponentTask masterPlanComponentTask = tasks[i];
				if (masterPlanComponentTask != null)
				{
					setTaskIconSignal.Dispatch(masterPlanComponentTask, masterPlanComponentItemView.icon);
					masterPlanComponentItemView.quantityText.gameObject.SetActive(enabledTextOnRequires);
				}
			}
		}

		private int UpdateImages(int taskCount)
		{
			int count = itemIconList.Count;
			if (count == taskCount)
			{
				return count;
			}
			UIUtils.SafeDestoryViews(itemIconList);
			for (int i = 0; i < taskCount; i++)
			{
				global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(itemViewPrefab, global::UnityEngine.Vector3.zero, global::UnityEngine.Quaternion.identity) as global::UnityEngine.GameObject;
				if (!(gameObject == null))
				{
					global::Kampai.UI.View.MasterPlanComponentItemView component = gameObject.GetComponent<global::Kampai.UI.View.MasterPlanComponentItemView>();
					if (!(component == null))
					{
						gameObject.transform.SetParent(itemPanel, false);
						itemIconList.Add(component);
					}
				}
			}
			return itemIconList.Count;
		}
	}
}
