namespace Kampai.Game.View
{
	public class ConstantSpeedPathAction : global::Kampai.Game.View.PathAction
	{
		public float Speed { get; private set; }

		public ConstantSpeedPathAction(global::Kampai.Game.View.ActionableObject obj, global::System.Collections.Generic.IList<global::UnityEngine.Vector3> path, float speed, global::Kampai.Util.IKampaiLogger logger)
			: base(obj, path, 1f, logger)
		{
			base.obj = obj;
			base.path = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>(path);
			Speed = speed;
		}

		public override float Duration()
		{
			return EstimatePathLength() / Speed;
		}

		protected float EstimatePathLength()
		{
			float num = 0f;
			for (int i = 0; i < path.Count - 1; i++)
			{
				num += global::UnityEngine.Vector3.Distance(path[i], path[i + 1]);
			}
			return num;
		}

		public override void LateUpdate()
		{
			obj.SetAnimBool("isMoving", true);
			obj.SetAnimFloat("speed", Speed);
		}
	}
}
