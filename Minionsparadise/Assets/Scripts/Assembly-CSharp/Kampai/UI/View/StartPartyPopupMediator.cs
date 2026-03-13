namespace Kampai.UI.View
{
	public class StartPartyPopupMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.StartPartyPopupView>
	{
		private bool haveAPrestigeCharacter;

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSkrim { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Game.IGuestOfHonorService guestOfHonorService { get; set; }

		[Inject]
		public global::Kampai.UI.View.StopAutopanSignal stopPan { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowMinionPartySkipButtonSignal showSkipButtonSignal { get; set; }

		public override void OnRegister()
		{
			base.view.accept.ClickedSignal.AddListener(Proceed);
			base.view.OnMenuClose.AddListener(OnMenuClose);
			if (!guestOfHonorService.PartyShouldProduceBuff())
			{
				base.view.PulseStartButton();
				return;
			}
			haveAPrestigeCharacter = true;
			base.view.accept.gameObject.SetActive(false);
			base.view.CenterHeader();
			StartCoroutine(TransitionOut());
		}

		public override void OnRemove()
		{
			base.view.accept.ClickedSignal.RemoveListener(Proceed);
			base.view.OnMenuClose.RemoveListener(OnMenuClose);
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			Init();
			soundFXSignal.Dispatch("Play_main_menu_open_01");
			soundFXSignal.Dispatch("Play_startParty_popUp_01");
		}

		protected override void OnCloseAllMenu(global::UnityEngine.GameObject exception)
		{
		}

		protected override void Close()
		{
		}

		private void Proceed()
		{
			soundFXSignal.Dispatch("Play_main_menu_close_01");
			base.view.Close();
			guestOfHonorService.SelectGuestOfHonor(0);
			StartMinionParty();
		}

		private void StartMinionParty()
		{
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.StartMinionPartyIntroSignal>().Dispatch();
		}

		private void Init()
		{
			stopPan.Dispatch();
			base.closeAllOtherMenuSignal.Dispatch(base.gameObject);
			base.view.Init();
		}

		private global::System.Collections.IEnumerator TransitionOut()
		{
			yield return new global::UnityEngine.WaitForSeconds(3f);
			global::Kampai.UI.View.IGUICommand command = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Queue, "screen_GuestOfHonorSelection");
			guiService.Execute(command);
			base.view.Close();
		}

		private void OnMenuClose()
		{
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "screen_StartPartyPopup");
			if (!haveAPrestigeCharacter)
			{
				hideSkrim.Dispatch("StartPartySkirm");
				showSkipButtonSignal.Dispatch(true);
			}
		}
	}
}
