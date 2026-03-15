namespace Kampai.Game.View
{
	public class DuelParameterizedDelegateAction : global::Kampai.Game.View.KampaiAction
	{
		private global::System.Action<object, object> Once;

		private object Param1;

		private object Param2;

		public DuelParameterizedDelegateAction(global::System.Action<object, object> once, object param1, object param2, global::Kampai.Util.IKampaiLogger logger)
			: base(logger)
		{
			Once = once;
			Param1 = param1;
			Param2 = param2;
		}

		public override void Execute()
		{
			if (!base.Done)
			{
				Once(Param1, Param2);
				base.Done = true;
			}
		}
	}
}
