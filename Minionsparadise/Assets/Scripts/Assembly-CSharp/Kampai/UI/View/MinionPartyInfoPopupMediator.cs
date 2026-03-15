namespace Kampai.UI.View
{
	public class MinionPartyInfoPopupMediator : global::Kampai.UI.View.AbstractGenericPopupMediator<global::Kampai.UI.View.MinionPartyInfoPopupView>
	{
		[Inject]
		public global::Kampai.UI.View.SetXPSignal setXPSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ExtendItemPopupSignal extendPopupSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			base.view.OnMenuClose.AddListener(OnMenuClose);
			setXPSignal.AddListener(SetPartyPoints);
			base.view.pointerDownSignal.AddListener(PointerDown);
			base.view.pointerUpSignal.AddListener(PointerUp);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			base.view.OnMenuClose.RemoveListener(OnMenuClose);
			setXPSignal.RemoveListener(SetPartyPoints);
			base.view.pointerDownSignal.RemoveListener(PointerDown);
			base.view.pointerUpSignal.RemoveListener(PointerUp);
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			base.soundFXSignal.Dispatch("Play_menu_popUp_02");
			global::UnityEngine.Vector3 itemCenter = args.Get<global::UnityEngine.Vector3>();
			SetPartyPoints();
			Register(null, itemCenter);
		}

		public override void Register(global::Kampai.Game.ItemDefinition itemDef, global::UnityEngine.Vector3 itemCenter)
		{
			base.view.Display(itemCenter);
			SetPartyPoints(false);
		}

		private void SetPartyPoints()
		{
			SetPartyPoints(true);
		}

		internal void SetPartyPoints(bool animate)
		{
			global::Kampai.Game.MinionParty minionPartyInstance = playerService.GetMinionPartyInstance();
			base.view.SetPartyPoints(minionPartyInstance.CurrentPartyPoints, minionPartyInstance.CurrentPartyPointsRequired, animate);
		}

		private void PointerDown(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			extendPopupSignal.Dispatch();
		}

		private void PointerUp(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
		}
	}
}
