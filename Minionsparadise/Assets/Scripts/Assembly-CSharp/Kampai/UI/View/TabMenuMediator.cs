namespace Kampai.UI.View
{
	public class TabMenuMediator : global::strange.extensions.mediation.impl.EventMediator
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("TabMenuMediator") as global::Kampai.Util.IKampaiLogger;

		private global::Kampai.Game.StoreItemType lastTabClicked = global::Kampai.Game.StoreItemType.GrindCurrency;

		[Inject]
		public global::Kampai.UI.View.TabMenuView view { get; set; }

		[Inject]
		public global::Kampai.UI.View.AddStoreTabSignal addTabSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.OnTabClickedSignal tabClickSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.MoveTabMenuSignal moveTabSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetBadgeForStoreTabSignal setBadgeForTabSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetNewUnlockForStoreTabSignal setNewUnlockForTabSignal { get; set; }

		[Inject]
		public global::Kampai.UI.IBuildMenuService buildMenuService { get; set; }

		[Inject]
		public global::Kampai.UI.View.ClearNewBuildTabCount clearNewBuildTabCount { get; set; }

		[Inject]
		public global::Kampai.UI.View.ToggleStoreTabSignal toggleStoreTabSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.RemoveUnlockForBuildMenuSignal removeUnlockForBuildMenuSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetNewUnlockForBuildMenuSignal setNewUnlockForBuildMenuSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.UIModel uiModel { get; set; }

		public override void OnRegister()
		{
			view.Init(setNewUnlockForBuildMenuSignal, removeUnlockForBuildMenuSignal);
			addTabSignal.AddListener(AddStoreTab);
			moveTabSignal.AddListener(ShowMenu);
			setBadgeForTabSignal.AddListener(SetBadgeForTab);
			setNewUnlockForTabSignal.AddListener(SetUnlockForTab);
			clearNewBuildTabCount.AddListener(SetClearForTab);
			toggleStoreTabSignal.AddListener(ToggleStoreTab);
		}

		public override void OnRemove()
		{
			addTabSignal.RemoveListener(AddStoreTab);
			moveTabSignal.RemoveListener(ShowMenu);
			setBadgeForTabSignal.RemoveListener(SetBadgeForTab);
			setNewUnlockForTabSignal.RemoveListener(SetUnlockForTab);
			clearNewBuildTabCount.RemoveListener(SetClearForTab);
			toggleStoreTabSignal.RemoveListener(ToggleStoreTab);
		}

		internal void AddStoreTab(global::Kampai.UI.View.StoreTab tab)
		{
			global::Kampai.UI.View.StoreTabView storeTabView = global::Kampai.UI.View.StoreTabBuilder.Build(tab, view.ScrollViewParent.transform, logger);
			storeTabView.ClickedSignal.AddListener(OnTabClicked);
			global::UnityEngine.RectTransform rectTransform = storeTabView.transform as global::UnityEngine.RectTransform;
			view.AddStoreTab(storeTabView, rectTransform.sizeDelta.y, storeTabView.PaddingInPixel);
		}

		private void ToggleStoreTab(global::Kampai.Game.StoreItemType type, bool show)
		{
			view.ToggleStoreTab(type, show);
		}

		internal void SetBadgeForTab(global::Kampai.Game.StoreItemType type, int badgeCount)
		{
			view.SetBadgeForStoreTab(type, badgeCount);
		}

		internal void SetUnlockForTab(global::Kampai.Game.StoreItemType type, int badgeCount)
		{
			view.SetUnlockForTab(type, badgeCount);
		}

		internal void SetClearForTab(global::Kampai.Game.StoreItemType type)
		{
			view.ClearUnlockForTab(type);
		}

		internal void ShowMenu(bool show)
		{
			if (show)
			{
				lastTabClicked = global::Kampai.Game.StoreItemType.GrindCurrency;
			}
			view.ShowMenu(show);
		}

		internal void OnTabClicked(global::Kampai.Game.StoreItemType type, string localizedTitle)
		{
			if (lastTabClicked != type)
			{
				lastTabClicked = type;
				tabClickSignal.Dispatch(type, localizedTitle);
				view.HideBadge(type);
				buildMenuService.ClearTab(type);
				uiModel.GoToInEffect = false;
			}
		}
	}
}
