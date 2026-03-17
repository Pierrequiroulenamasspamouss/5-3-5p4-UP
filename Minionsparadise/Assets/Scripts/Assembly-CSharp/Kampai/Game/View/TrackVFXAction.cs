namespace Kampai.Game.View
{
	public class TrackVFXAction : global::Kampai.Game.View.KampaiAction
	{
		private global::Kampai.Util.VFXScript vfxScript;

		private global::Kampai.Game.View.MinionObject minion;

		public TrackVFXAction(global::Kampai.Game.View.MinionObject minion, global::Kampai.Util.VFXScript vfxScript, global::Kampai.Util.IKampaiLogger logger)
			: base(logger)
		{
			this.vfxScript = vfxScript;
			this.minion = minion;
		}

		public override void Execute()
		{
			minion.TrackVFX(vfxScript);
			base.Done = true;
		}

		public override void Abort()
		{
			minion.UntrackVFX();
		}
	}
}
