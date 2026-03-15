namespace Kampai.Game.View
{
	public class StopWalkingAction : global::Kampai.Game.View.SetAnimatorAction
	{
		private static readonly global::System.Collections.Generic.Dictionary<string, object> stopWalkingArgs = new global::System.Collections.Generic.Dictionary<string, object>();

		public StopWalkingAction(global::Kampai.Game.View.ActionableObject obj, global::Kampai.Util.IKampaiLogger logger)
			: base(obj, null, logger, stopWalkingArgs)
		{
			if (stopWalkingArgs.Count == 0)
			{
				stopWalkingArgs.Add("isMoving", false);
			}
		}

		public override void Execute()
		{
			if (obj.GetDefaultAnimControllerName().Equals(obj.GetCurrentAnimControllerName()))
			{
				base.Execute();
			}
			base.Done = true;
		}
	}
}
