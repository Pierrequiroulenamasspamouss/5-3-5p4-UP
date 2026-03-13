namespace Kampai.Game.View
{
	public class CancelablePathAction : global::Kampai.Game.View.PathAction
	{
		private global::strange.extensions.signal.impl.Signal cancelPathActionSignal;

		private float maximumDeviationFromDestination;

		public CancelablePathAction(global::strange.extensions.signal.impl.Signal cancelPathActionSignal, float maximumDeviationFromDestination, global::Kampai.Game.View.ActionableObject obj, global::System.Collections.Generic.IList<global::UnityEngine.Vector3> path, float time, global::Kampai.Util.IKampaiLogger logger)
			: base(obj, path, time, logger)
		{
			this.cancelPathActionSignal = cancelPathActionSignal;
			this.maximumDeviationFromDestination = maximumDeviationFromDestination;
		}

		protected override void PathFinished()
		{
			global::UnityEngine.Vector3 a = path[path.Count - 1];
			global::UnityEngine.Vector3 position = obj.transform.position;
			float num = global::UnityEngine.Vector3.Distance(a, position);
			if (num > maximumDeviationFromDestination)
			{
				cancelPathActionSignal.Dispatch();
			}
			else
			{
				base.PathFinished();
			}
		}
	}
}
