namespace Kampai.Game.View
{
	public class OrderBoardBuildingObjectView : global::Kampai.Game.View.BuildingObject, global::strange.extensions.mediation.api.IView
	{
		public global::Kampai.Game.OrderBoard orderBoard;

		private OrderBoardBuildingTicketsView ticketViews;

		private bool ticketsReady;

		private bool _requiresContext = true;

		protected bool registerWithContext = true;

		private global::strange.extensions.context.api.IContext currentContext;

		public bool requiresContext
		{
			get
			{
				return _requiresContext;
			}
			set
			{
				_requiresContext = value;
			}
		}

		public bool registeredWithContext { get; set; }

		public virtual bool autoRegisterWithContext
		{
			get
			{
				return registerWithContext;
			}
			set
			{
				registerWithContext = value;
			}
		}

		internal override void Init(global::Kampai.Game.Building building, global::Kampai.Util.IKampaiLogger logger, global::System.Collections.Generic.IDictionary<string, global::UnityEngine.RuntimeAnimatorController> controllers, global::Kampai.Game.IDefinitionService definitionService)
		{
			base.Init(building, logger, controllers, definitionService);
			orderBoard = building as global::Kampai.Game.OrderBoard;
			ticketViews = base.gameObject.GetComponent<OrderBoardBuildingTicketsView>();
			ticketsReady = false;
		}

		internal void ClearBoard()
		{
			if (ticketViews == null)
			{
				return;
			}
			if (!ticketsReady)
			{
				ticketsReady = ticketViews.IsOrderboardSetupCorrectly();
				if (!ticketsReady)
				{
					logger.Error("Tickets are not assign on the prefab. Please make sure that they do.");
					return;
				}
			}
			ticketViews.DisableTickets();
		}

		public void ToggleHitbox(bool enable)
		{
			global::UnityEngine.Collider[] components = GetComponents<global::UnityEngine.Collider>();
			foreach (global::UnityEngine.Collider collider in components)
			{
				collider.enabled = enable;
			}
		}

		internal void SetTicketState(int ticketIndex, global::Kampai.Game.OrderBoardTicketState state)
		{
			if (ticketViews == null)
			{
				return;
			}
			if (!ticketsReady)
			{
				ticketsReady = ticketViews.IsOrderboardSetupCorrectly();
				if (!ticketsReady)
				{
					logger.Error("Tickets are not assign on the prefab. Please make sure that they do.");
					return;
				}
			}
			ticketViews.SetTicketState(ticketIndex, state);
		}

		protected void Awake()
		{
			if (autoRegisterWithContext && !registeredWithContext)
			{
				global::Kampai.Util.KampaiView.BubbleToContext(this, true, false, ref currentContext);
			}
		}

		protected void Start()
		{
			if (autoRegisterWithContext && !registeredWithContext)
			{
				global::Kampai.Util.KampaiView.BubbleToContext(this, true, true, ref currentContext);
			}
		}

		protected void OnDestroy()
		{
			global::Kampai.Util.KampaiView.BubbleToContext(this, false, false, ref currentContext);
		}
	}
}
