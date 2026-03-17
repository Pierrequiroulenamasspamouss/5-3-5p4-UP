namespace Kampai.Game.View
{
	public abstract class KampaiAction
	{
		protected readonly global::Kampai.Util.IKampaiLogger logger;

		protected bool instant;

		public bool Done { get; protected set; }

		public KampaiAction(global::Kampai.Util.IKampaiLogger logger)
		{
			this.logger = logger;
		}

		public virtual void Execute()
		{
		}

		public virtual void Abort()
		{
			Done = true;
		}

		public virtual void Update()
		{
		}

		public virtual void LateUpdate()
		{
		}

		public override string ToString()
		{
			return string.Format("{0}:{1}", base.ToString(), Done);
		}

		public global::Kampai.Game.View.KampaiAction SetInstant()
		{
			instant = true;
			return this;
		}

		public bool IsInstant()
		{
			return instant;
		}
	}
}
