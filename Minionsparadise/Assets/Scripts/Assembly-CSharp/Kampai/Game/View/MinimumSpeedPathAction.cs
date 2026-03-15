namespace Kampai.Game.View
{
	public class MinimumSpeedPathAction : global::Kampai.Game.View.ConstantSpeedPathAction
	{
		private float MinimumSpeed;

		private float TimeToArrive;

		public MinimumSpeedPathAction(global::Kampai.Game.View.MinionObject obj, global::System.Collections.Generic.IList<global::UnityEngine.Vector3> path, float timeToArrive, float minimumSpeed, global::Kampai.Util.IKampaiLogger logger)
			: base(obj, path, minimumSpeed, logger)
		{
			base.obj = obj;
			base.path = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>(path);
			MinimumSpeed = minimumSpeed;
			TimeToArrive = timeToArrive;
		}

		public override float Duration()
		{
			float num = EstimatePathLength();
			float num2 = num / TimeToArrive;
			float result = TimeToArrive;
			if (num2 < MinimumSpeed)
			{
				result = num / MinimumSpeed;
			}
			return result;
		}

		public override void LateUpdate()
		{
			obj.SetAnimBool("isMoving", true);
			obj.SetAnimFloat("speed", base.Speed);
		}
	}
}
