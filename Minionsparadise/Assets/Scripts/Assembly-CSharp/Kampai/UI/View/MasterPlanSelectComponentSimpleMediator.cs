namespace Kampai.UI.View
{
	public class MasterPlanSelectComponentSimpleMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.MasterPlanSelectComponentSimpleView>
	{
		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseAllMessageDialogs closeAllMessageDialogs { get; set; }

		[Inject]
		public global::Kampai.UI.IGhostComponentService ghostService { get; set; }

		public override void OnRegister()
		{
			base.view.Init();
			closeAllMessageDialogs.AddListener(Close);
			base.view.OnMenuClose.AddListener(OnMenuClose);
			ghostService.DisplayAllSelectablePlanComponents();
			base.OnRegister();
		}

		public override void OnRemove()
		{
			closeAllMessageDialogs.RemoveListener(Close);
			base.view.OnMenuClose.RemoveListener(OnMenuClose);
			base.OnRemove();
		}

		protected override void Close()
		{
			ghostService.ClearGhostComponentBuildings();
			base.view.Close();
		}

		private void OnMenuClose()
		{
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "screen_MasterplanSelectComponent");
		}
	}
}
