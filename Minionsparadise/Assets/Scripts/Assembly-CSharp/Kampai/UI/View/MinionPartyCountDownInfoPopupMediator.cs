namespace Kampai.UI.View
{
	public class MinionPartyCountDownInfoPopupMediator : global::Kampai.UI.View.AbstractGenericPopupMediator<global::Kampai.UI.View.MinionPartyCountDownInfoPopupView>
	{
		private global::System.Collections.IEnumerator updateRoutine;

		private bool _isCooldownActive;

		private global::Kampai.Game.MinionParty minionParty;

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IGuestOfHonorService guestService { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			base.view.OnMenuClose.AddListener(OnMenuClose);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			_isCooldownActive = false;
			if (updateRoutine != null)
			{
				StopCoroutine(updateRoutine);
				updateRoutine = null;
			}
			base.view.OnMenuClose.RemoveListener(OnMenuClose);
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			base.soundFXSignal.Dispatch("Play_menu_popUp_02");
			global::UnityEngine.Vector3 itemCenter = args.Get<global::UnityEngine.Vector3>();
			Register(null, itemCenter);
		}

		public override void Register(global::Kampai.Game.ItemDefinition itemDef, global::UnityEngine.Vector3 itemCenter)
		{
			base.view.Display(itemCenter);
			minionParty = playerService.GetMinionPartyInstance();
			_isCooldownActive = true;
			updateRoutine = UpdateCooldownTime();
			StartCoroutine(updateRoutine);
		}

		internal global::System.Collections.IEnumerator UpdateCooldownTime()
		{
			while (_isCooldownActive)
			{
				if (base.view == null)
				{
					_isCooldownActive = false;
					continue;
				}
				int timeRemaining = guestService.GetBuffRemainingTime(minionParty);
				if (timeRemaining <= 0)
				{
					_isCooldownActive = false;
					break;
				}
				base.view.UpdateCountDownText(string.Format("{0}", UIUtils.FormatTime(timeRemaining, base.localizationService)));
				yield return new global::UnityEngine.WaitForSeconds(1f);
			}
		}
	}
}
