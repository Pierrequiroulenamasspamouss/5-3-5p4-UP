namespace Kampai.Game.View
{
	public abstract class NamedMinionView : global::Kampai.Game.View.NamedCharacterObject, global::strange.extensions.mediation.api.IView
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
