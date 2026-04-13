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
			
			float h = (buttonHeight > 0f) ? buttonHeight : 100f; // Sane default height for menu tabs
			float p = (padding >= 0f) ? padding : 5f;
			
			ScrollViewParent.offsetMin = new global::UnityEngine.Vector2(0f, (float)(-count) * h + ScrollViewParent.offsetMax.y);
			ScrollViewParent.offsetMax = global::UnityEngine.Vector2.zero;
			global::UnityEngine.RectTransform rectTransform = tabViews[count - 1].transform as global::UnityEngine.RectTransform;
			rectTransform.offsetMin = new global::UnityEngine.Vector2(p, (0f - h - p) * (float)count);
			rectTransform.offsetMax = new global::UnityEngine.Vector2(0f - p, (0f - h - p) * (float)(count - 1) - p);
			tabViews[count - 1].gameObject.SetActive(true);
		}

		internal void ClearTabs()
		{
			if (tabViews != null)
			{
				foreach (var tab in tabViews)
				{
					if (tab != null && tab.gameObject != null)
					{
						global::UnityEngine.Object.Destroy(tab.gameObject);
					}
				}
				tabViews.Clear();
			}
			count = 0;
			if (ScrollViewParent != null)
			{
				ScrollViewParent.offsetMin = global::UnityEngine.Vector2.zero;
				ScrollViewParent.offsetMax = global::UnityEngine.Vector2.zero;
			}
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
