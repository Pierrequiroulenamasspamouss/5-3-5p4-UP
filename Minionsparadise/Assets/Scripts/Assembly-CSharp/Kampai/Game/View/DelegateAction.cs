namespace Kampai.Game.View
{
	public class DelegateAction : global::Kampai.Game.View.KampaiAction
	{
		private global::System.Action Once;

		public DelegateAction(global::System.Action once, global::Kampai.Util.IKampaiLogger logger)
			: base(logger)
		{
			Once = once;
		}

		public override void Execute()
		{
			if (!base.Done)
			{
				Once();
				base.Done = true;
			}
		}
	}
}
