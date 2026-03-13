namespace Kampai.Game.View
{
	public class PlayMecanimStateAction : global::Kampai.Game.View.KampaiAction
	{
		private global::Kampai.Game.View.ActionableObject target;

		private int StateHash;

		private int Layer;

		public PlayMecanimStateAction(global::Kampai.Game.View.ActionableObject target, int stateHash, global::Kampai.Util.IKampaiLogger logger, int layer = 0)
			: base(logger)
		{
			this.target = target;
			StateHash = stateHash;
			Layer = layer;
		}

		public override void Execute()
		{
			target.PlayAnimation(StateHash, Layer, 0f);
			base.Done = true;
		}
	}
}
