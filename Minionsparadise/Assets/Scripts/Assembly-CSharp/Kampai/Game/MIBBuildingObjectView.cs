namespace Kampai.Game
{
	public class MIBBuildingObjectView : global::Kampai.Game.View.BuildingObject, global::strange.extensions.mediation.api.IView
	{
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

		protected void Awake()
		{
			global::Kampai.Util.KampaiView.BubbleToContextOnAwake(this, ref currentContext);
		}

		protected void Start()
		{
			global::Kampai.Util.KampaiView.BubbleToContextOnStart(this, ref currentContext);
		}

		protected void OnDestroy()
		{
			global::Kampai.Util.KampaiView.BubbleToContextOnDestroy(this, ref currentContext);
		}
	}
}
