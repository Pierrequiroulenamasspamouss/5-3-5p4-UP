namespace Kampai.Game.Mignette.EdwardMinionHands.View
{
	public class EdwardMinionHandsCollectableViewObject : global::UnityEngine.MonoBehaviour
	{
		public global::Kampai.Game.Mignette.View.MignetteCollectableViewObject CollectableViewObject;

		private int pointValue;

		private float timeoutTimer;

		private global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsMignetteManagerView parentView;

		private GoSpline spline;

		private bool isFlashing;

		private float flashTimer;

		private float travelPct;

		public void StartCollectable(global::UnityEngine.Vector3 targetPos, int points, float time, global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsMignetteManagerView parent)
		{
			global::System.Collections.Generic.List<global::UnityEngine.Vector3> list = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>();
			list.Add(base.transform.position);
			global::UnityEngine.Vector3 item = (base.transform.position + targetPos) / 2f;
			item.y += 2f;
			list.Add(item);
			list.Add(targetPos);
			spline = new GoSpline(list);
			spline.buildPath();
			travelPct = 0f;
			pointValue = points;
			timeoutTimer = time;
			parentView = parent;
			CollectableViewObject.GetComponent<global::UnityEngine.Collider>().enabled = false;
			CollectableViewObject.UpdateMaterialForPointValue(points);
			CollectableViewObject.SetState(global::Kampai.Game.Mignette.View.MignetteCollectableViewObject.CollectableStates.Spawn, parentView.mignetteCamera);
		}

		public void Update()
		{
			if (parentView == null || parentView.IsPaused)
			{
				return;
			}
			if (travelPct < 1f)
			{
				travelPct += global::UnityEngine.Time.deltaTime * 1f;
				if (travelPct >= 1f)
				{
					travelPct = 1f;
					CollectableViewObject.GetComponent<global::UnityEngine.Collider>().enabled = true;
					CollectableViewObject.SetState(global::Kampai.Game.Mignette.View.MignetteCollectableViewObject.CollectableStates.Bounce, parentView.mignetteCamera);
				}
				global::UnityEngine.Vector3 pointOnPath = spline.getPointOnPath(travelPct);
				base.transform.position = pointOnPath;
			}
			timeoutTimer -= global::UnityEngine.Time.deltaTime;
			if (timeoutTimer <= 1f)
			{
				isFlashing = true;
			}
			if (timeoutTimer <= 0f)
			{
				parentView.CollectableHasTimedOut(this);
			}
			else if (isFlashing)
			{
				flashTimer -= global::UnityEngine.Time.deltaTime;
				if (flashTimer <= 0f)
				{
					CollectableViewObject.ToggleModel();
					flashTimer = 0.1f;
				}
			}
		}

		public bool WasTapped()
		{
			if (CollectableViewObject.CollectableState == global::Kampai.Game.Mignette.View.MignetteCollectableViewObject.CollectableStates.Collected)
			{
				return false;
			}
			CollectableViewObject.SetState(global::Kampai.Game.Mignette.View.MignetteCollectableViewObject.CollectableStates.Collected, parentView.mignetteCamera);
			return true;
		}

		public int GetPointValue()
		{
			return pointValue;
		}
	}
}
