namespace Kampai.UI.View
{
	public class TabMenuView : global::Kampai.Util.KampaiView
	{
		public global::UnityEngine.UI.Text StoreTitle;

		public global::UnityEngine.RectTransform ScrollViewParent;

		private int count;

		private global::System.Collections.Generic.List<global::Kampai.UI.View.StoreTabView> tabViews;

		private global::UnityEngine.Animator animator;

		private global::System.Collections.Generic.Dictionary<global::Kampai.Game.StoreItemType, int> oldBadgeCount;

		private global::Kampai.UI.View.RemoveUnlockForBuildMenuSignal removeUnlockForBuildMenuSignal;

		private global::Kampai.UI.View.SetNewUnlockForBuildMenuSignal setNewUnlockForBuildMenuSignal;

		public void Init(global::Kampai.UI.View.SetNewUnlockForBuildMenuSignal setNewUnlockForBuildMenuSignal, global::Kampai.UI.View.RemoveUnlockForBuildMenuSignal removeUnlockForBuildMenuSignal)
		{
			this.removeUnlockForBuildMenuSignal = removeUnlockForBuildMenuSignal;
			this.setNewUnlockForBuildMenuSignal = setNewUnlockForBuildMenuSignal;
			animator = base.transform.GetComponentInParent<global::UnityEngine.Animator>();
			StoreTitle.rectTransform.offsetMin = global::UnityEngine.Vector2.zero;
			StoreTitle.rectTransform.offsetMax = global::UnityEngine.Vector2.zero;
			tabViews = new global::System.Collections.Generic.List<global::Kampai.UI.View.StoreTabView>();
			oldBadgeCount = new global::System.Collections.Generic.Dictionary<global::Kampai.Game.StoreItemType, int>();
		}

		private global::Kampai.UI.View.StoreTabView GetStoreTabView(global::Kampai.Game.StoreItemType type)
		{
			foreach (global::Kampai.UI.View.StoreTabView tabView in tabViews)
			{
				if (tabView.Type == type)
				{
					return tabView;
				}
			}
			return null;
		}

		internal void SetBadgeForStoreTab(global::Kampai.Game.StoreItemType type, int badgeCount)
		{
			global::Kampai.UI.View.StoreTabView storeTabView = GetStoreTabView(type);
			if (storeTabView != null)
			{
				storeTabView.SetBadgeCount(badgeCount);
				oldBadgeCount[type] = badgeCount;
			}
		}

		internal void SetUnlockForTab(global::Kampai.Game.StoreItemType type, int badgeCount)
		{
			global::Kampai.UI.View.StoreTabView storeTabView = GetStoreTabView(type);
			if (storeTabView != null)
			{
				storeTabView.SetNewUnlockState(badgeCount);
				oldBadgeCount[type] = badgeCount;
			}
		}

		internal void ClearUnlockForTab(global::Kampai.Game.StoreItemType type)
		{
			global::Kampai.UI.View.StoreTabView storeTabView = GetStoreTabView(type);
			if (storeTabView != null)
			{
				storeTabView.SetNewUnlockState(0);
				oldBadgeCount[type] = 0;
				removeUnlockForBuildMenuSignal.Dispatch(oldBadgeCount[type]);
			}
		}

		public global::UnityEngine.GameObject GetStoreTabObject(global::Kampai.Game.StoreItemType type)
		{
			global::Kampai.UI.View.StoreTabView storeTabView = GetStoreTabView(type);
			if (storeTabView != null)
			{
				return storeTabView.gameObject;
			}
			return null;
		}

		internal void AddStoreTab(global::Kampai.UI.View.StoreTabView tabView, float buttonHeight, float padding)
		{
			tabViews.Add(tabView);
			count = tabViews.Count;
			ScrollViewParent.offsetMin = new global::UnityEngine.Vector2(0f, (float)(-count) * buttonHeight + ScrollViewParent.offsetMax.y);
			ScrollViewParent.offsetMax = global::UnityEngine.Vector2.zero;
			global::UnityEngine.RectTransform rectTransform = tabViews[count - 1].transform as global::UnityEngine.RectTransform;
			rectTransform.offsetMin = new global::UnityEngine.Vector2(padding, (0f - buttonHeight - padding) * (float)count);
			rectTransform.offsetMax = new global::UnityEngine.Vector2(0f - padding, (0f - buttonHeight - padding) * (float)(count - 1) - padding);
			tabViews[count - 1].gameObject.SetActive(true);
		}

		internal void ToggleStoreTab(global::Kampai.Game.StoreItemType type, bool show)
		{
			foreach (global::Kampai.UI.View.StoreTabView tabView in tabViews)
			{
				if (tabView.Type == type)
				{
					tabView.gameObject.SetActive(show);
					break;
				}
			}
		}

		internal void HideBadge(global::Kampai.Game.StoreItemType type)
		{
			SetBadgeForStoreTab(type, 0);
			if (oldBadgeCount.ContainsKey(type))
			{
				removeUnlockForBuildMenuSignal.Dispatch(oldBadgeCount[type]);
				oldBadgeCount[type] = 0;
			}
			SetUnlockForTab(type, 0);
		}

		internal void ShowMenu(bool show)
		{
			if (show)
			{
				int num = 0;
				foreach (global::System.Collections.Generic.KeyValuePair<global::Kampai.Game.StoreItemType, int> item in oldBadgeCount)
				{
					num += item.Value;
				}
				setNewUnlockForBuildMenuSignal.Dispatch(num);
			}
			animator.SetBool("OnOpenSubMenu", !show);
		}
	}
}
