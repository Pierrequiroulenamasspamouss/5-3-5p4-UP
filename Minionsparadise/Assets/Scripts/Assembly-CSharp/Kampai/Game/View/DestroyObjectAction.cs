namespace Kampai.Game.View
{
	public class DestroyObjectAction : global::Kampai.Game.View.KampaiAction
	{
		private global::UnityEngine.Object target;

		public DestroyObjectAction(global::UnityEngine.Object target, global::Kampai.Util.IKampaiLogger logger)
			: base(logger)
		{
			this.target = target;
		}

		public override void Execute()
		{
			if (!base.Done)
			{
				global::UnityEngine.Object.Destroy(target);
			}
			base.Done = true;
		}

		public override void Abort()
		{
			Execute();
		}
	}
}
