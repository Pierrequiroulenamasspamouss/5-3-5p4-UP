namespace Kampai.UI.View
{
	public class BuffInfoPopupMediator : global::Kampai.UI.View.AbstractGenericPopupMediator<global::Kampai.UI.View.BuffInfoPopupView>
	{
		private global::Kampai.Game.View.DummyCharacterObject dummyCharacter;

		private global::System.Collections.IEnumerator closePopupCoroutine;

		[Inject]
		public global::Kampai.Game.IGuestOfHonorService guestOfHonorService { get; set; }

		[Inject]
		public global::Kampai.UI.IFancyUIService fancyUIService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSkrimSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.BuffInfoPopupClosedSignal buffPopupClosedSignal { get; set; }

		[Inject(global::Kampai.Main.MainElement.UI_GLASSCANVAS)]
		public global::UnityEngine.GameObject glassCanvas { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			base.view.OnMenuClose.AddListener(OnMenuClose);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			base.view.OnMenuClose.RemoveListener(OnMenuClose);
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			int num = playerService.GetMinionPartyInstance().lastGuestsOfHonorPrestigeIDs[0];
			global::Kampai.Game.BuffDefinition recentBuffDefinition = guestOfHonorService.GetRecentBuffDefinition();
			if (num != 0 && recentBuffDefinition != null && dummyCharacter == null)
			{
				global::Kampai.UI.DummyCharacterType characterType = fancyUIService.GetCharacterType(num);
				dummyCharacter = fancyUIService.CreateCharacter(characterType, global::Kampai.UI.DummyCharacterAnimationState.Idle, base.view.MinionSlot.transform, base.view.MinionSlot.VillainScale, base.view.MinionSlot.VillainPositionOffset, num);
				global::Kampai.Game.PrestigeDefinition prestigeDefinition = definitionService.Get<global::Kampai.Game.PrestigeDefinition>(num);
				base.view.SetGuestName(prestigeDefinition.LocalizedKey);
				base.view.SetBuff(recentBuffDefinition, guestOfHonorService.GetCurrentBuffMultipler());
				base.soundFXSignal.Dispatch("Play_menu_popUp_02");
				global::UnityEngine.Vector3 itemCenter = args.Get<global::UnityEngine.Vector3>();
				float yInput = args.Get<float>();
				base.view.SetOffset(yInput, glassCanvas, itemCenter);
				base.view.Open();
				closePopupCoroutine = DelayClose();
				StartCoroutine(closePopupCoroutine);
			}
			else
			{
				hideSkrimSignal.Dispatch("GenericPopup");
				Close();
			}
		}

		internal global::System.Collections.IEnumerator DelayClose()
		{
			yield return new global::UnityEngine.WaitForSeconds(base.view.Duration);
			hideSkrimSignal.Dispatch("GenericPopup");
			Close();
		}

		public override void Close()
		{
			buffPopupClosedSignal.Dispatch();
			base.Close();
			if (closePopupCoroutine != null)
			{
				StopCoroutine(closePopupCoroutine);
			}
			global::Kampai.UI.View.IGUICommand command = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Unload, "screen_BuffPopup");
			guiService.Execute(command);
		}
	}
}
