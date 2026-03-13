namespace Kampai.Game.View
{
	public class WaitForMecanimStateAction : global::Kampai.Game.View.KampaiAction
	{
		private global::Kampai.Game.View.ActionableObject target;

		private int StateHash;

		private int Layer;

		public WaitForMecanimStateAction(global::Kampai.Game.View.ActionableObject target, int stateHash, global::Kampai.Util.IKampaiLogger logger, int layer = 0)
			: base(logger)
		{
			this.target = target;
			StateHash = stateHash;
			Layer = layer;
		}

		public override void Abort()
		{
			base.Done = true;
		}

		public override void Update()
		{
			if (target.IsInAnimatorState(StateHash, Layer))
			{
				base.Done = true;
			}
		}
	}
}
