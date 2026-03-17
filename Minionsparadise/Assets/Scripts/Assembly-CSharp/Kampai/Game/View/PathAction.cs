namespace Kampai.Game.View
{
	public class PathAction : global::Kampai.Game.View.KampaiAction
	{
		protected global::Kampai.Game.View.ActionableObject obj;

		protected global::System.Collections.Generic.IList<global::UnityEngine.Vector3> path;

		private float time = 1f;

		private float startTime = -1f;

		private GoTween tween;

		private global::UnityEngine.Vector3 lastPosition;

		public global::UnityEngine.Vector3 GoalPosition
		{
			get
			{
				return path[path.Count - 1];
			}
		}

		public PathAction(global::Kampai.Game.View.ActionableObject obj, global::System.Collections.Generic.IList<global::UnityEngine.Vector3> path, float time, global::Kampai.Util.IKampaiLogger logger)
			: base(logger)
		{
			this.obj = obj;
			this.path = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>(path);
			this.time = time;
		}

		public override void Abort()
		{
			if (tween != null)
			{
				tween.destroy();
			}
			base.Done = true;
			obj.SetAnimBool("isMoving", false);
		}

		public virtual float Duration()
		{
			return time;
		}

		public virtual float RemainingTime()
		{
			return global::UnityEngine.Mathf.Max(time - (global::UnityEngine.Time.realtimeSinceStartup - startTime), 0f);
		}

		public override void Execute()
		{
			startTime = global::UnityEngine.Time.realtimeSinceStartup;
			int num = path.Count;
			if (num > 1)
			{
				if (num > 2)
				{
					path.Insert(0, path[0]);
					num++;
					path.Add(path[num - 1]);
				}
				float sqrMagnitude = (path[0] - path[num - 1]).sqrMagnitude;
				if (sqrMagnitude < 0.0001f)
				{
					base.Done = true;
					return;
				}
				GoSpline goSpline = new GoSpline(path as global::System.Collections.Generic.List<global::UnityEngine.Vector3>);
				lastPosition = obj.transform.position;
				obj.SetAnimBool("isMoving", true);
				tween = Go.to(obj.transform, Duration(), new GoTweenConfig().setEaseType(GoEaseType.Linear).positionPath(goSpline, false, GoLookAtType.NextPathNode).onComplete(delegate(AbstractGoTween thisTween)
				{
					thisTween.destroy();
					obj.SetAnimBool("isMoving", false);
					obj.StopLocalAudio();
					PathFinished();
				}));
			}
			else
			{
				if (num == 1)
				{
					obj.gameObject.transform.position = path[0];
				}
				PathFinished();
			}
		}

		protected virtual void PathFinished()
		{
			base.Done = true;
		}

		public override void LateUpdate()
		{
			global::UnityEngine.Vector3 position = obj.transform.position;
			float num = global::UnityEngine.Vector3.Distance(lastPosition, position);
			float a = num / global::UnityEngine.Time.deltaTime;
			obj.SetAnimBool("isMoving", true);
			obj.SetAnimFloat("speed", global::UnityEngine.Mathf.Min(a, 4.5f));
			lastPosition = position;
		}

		public override string ToString()
		{
			return string.Format("{0} : {1}", base.ToString(), VectorUtils.PathToString(path));
		}
	}
}
