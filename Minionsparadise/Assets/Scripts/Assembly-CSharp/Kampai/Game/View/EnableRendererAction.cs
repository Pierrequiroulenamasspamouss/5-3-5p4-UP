namespace Kampai.Game.View
{
	public class EnableRendererAction : global::Kampai.Game.View.KampaiAction
	{
		private global::Kampai.Game.View.MinionObject obj;

		private bool enable;

		public EnableRendererAction(global::Kampai.Game.View.MinionObject obj, bool enable, global::Kampai.Util.IKampaiLogger logger)
			: base(logger)
		{
			this.obj = obj;
			this.enable = enable;
		}

		public override void Abort()
		{
			base.Done = true;
		}

		public override void Execute()
		{
			obj.EnableRenderers(enable);
			base.Done = true;
		}
	}
}
