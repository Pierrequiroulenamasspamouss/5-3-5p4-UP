namespace Kampai.Game.View
{
	public class SetLayerAction : global::Kampai.Game.View.KampaiAction
	{
		private global::Kampai.Game.View.ActionableObject obj;

		private int layer;

		private int frameCount;

		public SetLayerAction(global::Kampai.Game.View.ActionableObject obj, int layer, global::Kampai.Util.IKampaiLogger logger, int frameCount = 0)
			: base(logger)
		{
			this.obj = obj;
			this.layer = layer;
			this.frameCount = frameCount;
		}

		public override void Update()
		{
			if (!base.Done && frameCount-- < 1)
			{
				obj.SetRenderLayer(layer);
				base.Done = true;
			}
		}

		public override void Abort()
		{
			obj.SetRenderLayer(layer);
		}
	}
}
