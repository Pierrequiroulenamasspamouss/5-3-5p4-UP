namespace Kampai.Game.View
{
	public class DCNBuildingObjectView : global::Kampai.Game.View.BuildingObject, global::strange.extensions.mediation.api.IView
	{
		private global::UnityEngine.MeshRenderer openScreen;

		private bool open;

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
			global::UnityEngine.GameObject gameObject = base.gameObject.FindChild("Unique_DCN:Unique_DCN_OpenTarp_Mesh");
			openScreen = gameObject.GetComponent<global::UnityEngine.MeshRenderer>();
			HideScreen();
		}

		internal void ShowScreen()
		{
			openScreen.enabled = true;
			open = true;
		}

		internal void HideScreen()
		{
			openScreen.enabled = false;
			open = false;
		}

		internal bool ScreenIsOpen()
		{
			return open;
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
