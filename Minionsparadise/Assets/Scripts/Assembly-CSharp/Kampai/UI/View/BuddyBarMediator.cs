namespace Kampai.UI.View
{
	public class BuddyBarMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.BuddyBarView>
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("BuddyBarMediator") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IPrestigeService prestigeService { get; set; }

		[Inject]
		public global::Kampai.UI.View.LoadBuddyBarSignal loadSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			base.view.Init();
			base.view.SkrimButtonView.ClickedSignal.AddListener(Close);
			loadSignal.AddListener(Load);
			base.gameObject.SetActive(false);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			base.view.SkrimButtonView.ClickedSignal.RemoveListener(Close);
			loadSignal.RemoveListener(Load);
		}

		private void Load()
		{
			if (base.view.IsOpen())
			{
				return;
			}
			playSFXSignal.Dispatch("Play_menu_popUp_01");
			global::System.Collections.Generic.IList<global::Kampai.Game.Prestige> buddyPrestiges = prestigeService.GetBuddyPrestiges();
			if (buddyPrestiges != null)
			{
				int count = buddyPrestiges.Count;
				base.view.SetupRowCount(count);
				for (int i = 0; i < count; i++)
				{
					global::Kampai.UI.View.BuddyAvatarView buddyAvatarView = global::Kampai.UI.View.BuddyAvatarBuilder.Build(buddyPrestiges[i], prestigeService, logger);
					base.view.AddItem(buddyAvatarView, i);
				}
				base.view.InitScrollView(count);
			}
		}

		protected override void Close()
		{
			playSFXSignal.Dispatch("Play_button_click_01");
			base.view.Close();
		}
	}
}
