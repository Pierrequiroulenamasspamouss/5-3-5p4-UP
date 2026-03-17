namespace Kampai.Game.View
{
	public class SetCullingModeAction : global::Kampai.Game.View.KampaiAction
	{
		private global::Kampai.Game.View.Animatable obj;

		private global::UnityEngine.AnimatorCullingMode mode;

		public SetCullingModeAction(global::Kampai.Game.View.Animatable animatable, global::UnityEngine.AnimatorCullingMode mode, global::Kampai.Util.IKampaiLogger logger)
			: base(logger)
		{
			obj = animatable;
			this.mode = mode;
		}

		public override void Execute()
		{
			obj.SetAnimatorCullingMode(mode);
			base.Done = true;
		}

		public override void Abort()
		{
			Execute();
		}
	}
}
